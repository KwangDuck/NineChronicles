using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bencodex.Types;
using Lib9c.Formatters;
using MessagePack;
using MessagePack.Resolvers;
using Nekoyume.Action;
using Nekoyume.BlockChain;
using Nekoyume.Game.Controller;
using Nekoyume.Game.VFX;
using Nekoyume.Helper;
using Nekoyume.L10n;
using Nekoyume.Model.State;
using Nekoyume.Pattern;
using Nekoyume.State;
using Nekoyume.UI;
using UnityEngine;
using Menu = Nekoyume.UI.Menu;

namespace Nekoyume.Game
{
    using Libplanet.Crypto;
    using UniRx;

    //[RequireComponent(typeof(Agent), typeof(RPCAgent))]
    [RequireComponent(typeof(DummyAgent))]
    public class Game : MonoSingleton<Game>
    {
        [SerializeField]
        private Stage stage = null;

        [SerializeField]
        private bool useSystemLanguage = true;

        [SerializeField]
        private LanguageType languageType = default;

        [SerializeField]
        private Prologue prologue = null;

        public States States { get; private set; }

        public LocalLayer LocalLayer { get; private set; }
        
        public LocalLayerActions LocalLayerActions { get; private set; }

        public IAgent Agent { get; private set; }
        
        public Analyzer Analyzer { get; private set; }

        public Stage Stage => stage;

        // FIXME Action.PatchTableSheet.Execute()에 의해서만 갱신됩니다.
        // 액션 실행 여부와 상관 없이 최신 상태를 반영하게끔 수정해야합니다.
        public TableSheets TableSheets { get; private set; }

        public ActionManager ActionManager { get; private set; }

        public bool IsInitialized { get; private set; }

        public Prologue Prologue => prologue;

        public const string AddressableAssetsContainerPath = nameof(AddressableAssetsContainer);

        private CommandLineOptions _options;

        private string _msg;

        private static readonly string CommandLineOptionsJsonPath =
            Path.Combine(Application.streamingAssetsPath, "clo.json");

        #region Mono & Initialization

        protected override void Awake()
        {
            Debug.Log("[Game] Awake() invoked");

            Application.targetFrameRate = 60;
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
            base.Awake();

            _options = CommandLineOptions.Load(
                CommandLineOptionsJsonPath
            );

            Debug.Log("[Game] Awake() CommandLineOptions loaded");

            //if (_options.RpcClient)
            //{
            //    Agent = GetComponent<RPCAgent>();
            //    SubscribeRPCAgent();
            //}
            //else
            //{
            //    Agent = GetComponent<Agent>();
            //}

            Agent = GetComponent<DummyAgent>();

            States = new States();
            LocalLayer = new LocalLayer();
            LocalLayerActions = new LocalLayerActions();
            MainCanvas.instance.InitializeIntro();
        }

        private IEnumerator Start()
        {
            Debug.Log("[Game] Start() invoked");
            var resolver = MessagePack.Resolvers.CompositeResolver.Create(
                NineChroniclesResolver.Instance,
                StandardResolver.Instance
            );
            var options = MessagePackSerializerOptions.Standard.WithResolver(resolver);
            MessagePackSerializer.DefaultOptions = options;

#if UNITY_EDITOR
            if (useSystemLanguage)
            {
                yield return L10nManager.Initialize().ToYieldInstruction();
            }
            else
            {
                yield return L10nManager.Initialize(languageType).ToYieldInstruction();
            }
#else
            yield return L10nManager.Initialize(LanguageTypeMapper.ISO396(_options.Language)).ToYieldInstruction();
#endif
            Debug.Log("[Game] Start() L10nManager initialized");

            // Initialize MainCanvas first
            MainCanvas.instance.InitializeFirst();
            // Initialize TableSheets. This should be done before initialize the Agent.
            yield return StartCoroutine(CoInitializeTableSheets());
            Debug.Log("[Game] Start() TableSheets initialized");
            yield return StartCoroutine(ResourcesHelper.CoInitialize());
            Debug.Log("[Game] Start() ResourcesHelper initialized");
            AudioController.instance.Initialize();
            Debug.Log("[Game] Start() AudioController initialized");
            yield return null;

            // Initialize Agent
            var agentInitialized = false;
            var agentInitializeSucceed = false;
            yield return StartCoroutine(
                CoLogin(
                    succeed =>
                    {
                        Debug.Log($"Agent initialized. {succeed}");
                        agentInitialized = true;
                        agentInitializeSucceed = succeed;
                    }
                )
            );

            yield return new WaitUntil(() => agentInitialized);
            Analyzer = new Analyzer().Initialize(Agent.Address.ToString());
            Analyzer.Track("Unity/Started");
            // NOTE: Create ActionManager after Agent initialized.
            ActionManager = new ActionManager(Agent);
            yield return StartCoroutine(CoSyncTableSheets());
            Debug.Log("[Game] Start() TableSheets synchronized");
            // Initialize MainCanvas second
            yield return StartCoroutine(MainCanvas.instance.InitializeSecond());
            // Initialize Rank.SharedModel
            RankPopup.UpdateSharedModel();
            // Initialize Stage
            Stage.Initialize();

            Widget.Find<VersionSystem>().SetVersion(Agent.AppProtocolVersion);

            ShowNext(agentInitializeSucceed);
            StartCoroutine(CoUpdate());
        }

        protected override void OnDestroy()
        {
            ActionManager.Dispose();
            base.OnDestroy();
        }

        
        private void QuitWithAgentConnectionError()
        {
            var screen = Widget.Find<BlockSyncLoadingScreen>();
            if (screen.IsActive())
            {
                screen.Close();
            }

            // FIXME 콜백 인자를 구조화 하면 타입 쿼리 없앨 수 있을 것 같네요.
            var popup = Widget.Find<IconAndButtonSystem>();
            popup.Show("UI_ERROR", "UI_ERROR_RPC_CONNECTION", "UI_QUIT");
            popup.SetCancelCallbackToExit();
        }

        // FIXME: Leave one between this or CoSyncTableSheets()
        private IEnumerator CoInitializeTableSheets()
        {
            yield return null;
            var request =
                Resources.LoadAsync<AddressableAssetsContainer>(AddressableAssetsContainerPath);
            yield return request;
            if (!(request.asset is AddressableAssetsContainer addressableAssetsContainer))
            {
                throw new FailedToLoadResourceException<AddressableAssetsContainer>(
                    AddressableAssetsContainerPath);
            }

            List<TextAsset> csvAssets = addressableAssetsContainer.tableCsvAssets;
            var csv = new Dictionary<string, string>();
            foreach (var asset in csvAssets)
            {
                csv[asset.name] = asset.text;
            }
            TableSheets = new TableSheets(csv);
        }

        // FIXME: Leave one between this or CoInitializeTableSheets()
        private IEnumerator CoSyncTableSheets()
        {
            yield return null;
            var request =
                Resources.LoadAsync<AddressableAssetsContainer>(AddressableAssetsContainerPath);
            yield return request;
            if (!(request.asset is AddressableAssetsContainer addressableAssetsContainer))
            {
                throw new FailedToLoadResourceException<AddressableAssetsContainer>(
                    AddressableAssetsContainerPath);
            }

            var task = Task.Run(() =>
            {
                List<TextAsset> csvAssets = addressableAssetsContainer.tableCsvAssets;
                var csv = new ConcurrentDictionary<string, string>();
                Parallel.ForEach(csvAssets, asset =>
                {
                    if (Agent.GetState(Addresses.TableSheet.Derive(asset.name)) is Text tableCsv)
                    {
                        var table = tableCsv.ToDotnetString();
                        csv[asset.name] = table;
                    }
                });
                TableSheets = new TableSheets(csv);
            });
            yield return new WaitUntil(() => task.IsCompleted);
        }

        public static IDictionary<string, string> GetTableCsvAssets()
        {
            var container =
                Resources.Load<AddressableAssetsContainer>(AddressableAssetsContainerPath);
            return container.tableCsvAssets.ToDictionary(asset => asset.name, asset => asset.text);
        }

        private void ShowNext(bool succeed)
        {
            Debug.Log($"[Game]ShowNext({succeed}) invoked");
            if (succeed)
            {
                IsInitialized = true;
                var intro = Widget.Find<IntroScreen>();
                intro.Close();
                Widget.Find<PreloadingScreen>().Show();
                StartCoroutine(ClosePreloadingScene(4));
            }
            else
            {
                QuitWithAgentConnectionError();
            }
        }

        private IEnumerator ClosePreloadingScene(float time)
        {
            yield return new WaitForSeconds(time);
            Widget.Find<PreloadingScreen>().Close();
        }

        #endregion

        protected override void OnApplicationQuit()
        {
            if (Analyzer.Instance != null)
            {
                Analyzer.Instance.Track("Unity/Player Quit");
                Analyzer.Instance.Flush();   
            }
        }

        private IEnumerator CoUpdate()
        {
            while (enabled)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    PlayMouseOnClickVFX(Input.mousePosition);
                }

                if (Input.GetKeyDown(KeyCode.Escape) &&
                    !Widget.IsOpenAnyPopup())
                {
                    Quit();
                }

                yield return null;
            }
        }

        public static void BackToMain(bool showLoadingScreen, Exception exc)
        {
            Debug.LogException(exc);

            var (key, code, errorMsg) = ErrorCode.GetErrorCode(exc);
            Event.OnRoomEnter.Invoke(showLoadingScreen);
            instance.Stage.OnRoomEnterEnd
                .First()
                .Subscribe(_ => PopupError(key, code, errorMsg));

            MainCanvas.instance.InitWidgetInMain();
        }

        public static void PopupError(Exception exc)
        {
            Debug.LogException(exc);
            var (key, code, errorMsg) = ErrorCode.GetErrorCode(exc);
            PopupError(key, code, errorMsg);
        }

        private static void PopupError(string key, string code, string errorMsg)
        {
            errorMsg = errorMsg == string.Empty
                ? string.Format(
                    L10nManager.Localize("UI_ERROR_RETRY_FORMAT"),
                    L10nManager.Localize(key),
                    code)
                : errorMsg;
            var popup = Widget.Find<IconAndButtonSystem>();
            popup.Show(L10nManager.Localize("UI_ERROR"), errorMsg,
                L10nManager.Localize("UI_OK"), false);
            popup.SetCancelCallbackToExit();
        }

        public static void Quit()
        {
            var popup = Widget.Find<QuitSystem>();
            if (popup.gameObject.activeSelf)
            {
                popup.Close();
                return;
            }

            popup.Show();
        }

        private static void PlayMouseOnClickVFX(Vector3 position)
        {
            position = ActionCamera.instance.Cam.ScreenToWorldPoint(position);
            var vfx = VFXController.instance.CreateAndChaseCam<MouseClickVFX>(position);
            vfx.Play();
        }

        private IEnumerator CoLogin(Action<bool> callback)
        {
            if (_options.Maintenance)
            {
                var w = Widget.Create<IconAndButtonSystem>();
                w.CancelCallback = () =>
                {
                    Application.OpenURL(GameConfig.DiscordLink);
#if UNITY_EDITOR
                    UnityEditor.EditorApplication.ExitPlaymode();
#else
                    Application.Quit();
#endif
                };
                w.Show(
                    "UI_MAINTENANCE",
                    "UI_MAINTENANCE_CONTENT",
                    "UI_OK",
                    true,
                    IconAndButtonSystem.SystemType.Information
                );
                yield break;
            }

            if (_options.TestEnd)
            {
                var w = Widget.Find<ConfirmPopup>();
                w.CloseCallback = result =>
                {
                    if (result == ConfirmResult.Yes)
                    {
                        Application.OpenURL(GameConfig.DiscordLink);
                    }

#if UNITY_EDITOR
                    UnityEditor.EditorApplication.ExitPlaymode();
#else
                    Application.Quit();
#endif
                };
                w.Show("UI_TEST_END", "UI_TEST_END_CONTENT", "UI_GO_DISCORD", "UI_QUIT");

                yield break;
            }

            var settings = Widget.Find<UI.SettingPopup>();
            settings.UpdateSoundSettings();
            settings.UpdatePrivateKey(_options.PrivateKey);

            //var loginPopup = Widget.Find<LoginSystem>();

            //if (Application.isBatchMode)
            //{
            //    loginPopup.Show(_options.KeyStorePath, _options.PrivateKey);
            //}
            //else
            //{
            //    var intro = Widget.Find<IntroScreen>();
            //    intro.Show(_options.KeyStorePath, _options.PrivateKey);
            //    yield return new WaitUntil(() => loginPopup.Login);
            //}

            var privateKey = new PrivateKey();

            yield return Agent.Initialize(
                _options,
                privateKey,
                callback
            );
        }

        public void ResetStore()
        {
            var confirm = Widget.Find<ConfirmPopup>();
            var storagePath = _options.StoragePath ?? BlockChain.Agent.DefaultStoragePath;
            confirm.CloseCallback = result =>
            {
                if (result == ConfirmResult.No)
                {
                    return;
                }

                StoreUtils.ResetStore(storagePath);

#if UNITY_EDITOR
                UnityEditor.EditorApplication.ExitPlaymode();
#else
                Application.Quit();
#endif
            };
            confirm.Show("UI_CONFIRM_RESET_STORE_TITLE", "UI_CONFIRM_RESET_STORE_CONTENT");
        }

        public void ResetKeyStore()
        {
            var confirm = Widget.Find<ConfirmPopup>();
            confirm.CloseCallback = result =>
            {
                if (result == ConfirmResult.No)
                {
                    return;
                }

                var keyPath = _options.KeyStorePath;
                if (Directory.Exists(keyPath))
                {
                    Directory.Delete(keyPath, true);
                }

#if UNITY_EDITOR
                UnityEditor.EditorApplication.ExitPlaymode();
#else
                Application.Quit();
#endif
            };

            confirm.Show("UI_CONFIRM_RESET_KEYSTORE_TITLE", "UI_CONFIRM_RESET_KEYSTORE_CONTENT");
        }
    }
}
