// #define TEST_LOG

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CsvHelper;
using UniRx;
using UnityEngine;
using System.Threading.Tasks;

namespace Nekoyume.L10n
{
    public static class L10nManager
    {
        public enum State
        {
            None,
            InInitializing,
            Initialized,
            InLanguageChanging,
        }

        public const string SettingsAssetPathInResources = "L10nSettings/L10nSettings";

        public static readonly string CsvFilesRootDirectoryPath =
            Path.Combine(Application.streamingAssetsPath, "Localization");

        private static IReadOnlyDictionary<string, string> _dictionary =
            new Dictionary<string, string>();

        public static State CurrentState { get; private set; } = State.None;

        #region Language

        public static LanguageType CurrentLanguage { get; private set; } = SystemLanguage;

        private static LanguageType SystemLanguage
        {
            get
            {
                switch (Application.systemLanguage)
                {
                    case UnityEngine.SystemLanguage.Chinese:
                    case UnityEngine.SystemLanguage.ChineseSimplified:
                    case UnityEngine.SystemLanguage.ChineseTraditional:
                        return LanguageType.ChineseSimplified;
                    case UnityEngine.SystemLanguage.Portuguese:
                        return LanguageType.PortugueseBrazil;
                }

                var systemLang = Application.systemLanguage.ToString();
                return !Enum.TryParse<LanguageType>(systemLang, out var languageType)
                    ? default
                    : languageType;
            }
        }

        #endregion

        #region Settings

        private static L10nSettings _settings;

        private static LanguageTypeSettings? _currentLanguageTypeSettingsCache;

        public static LanguageTypeSettings CurrentLanguageTypeSettings =>
            _currentLanguageTypeSettingsCache.HasValue &&
            _currentLanguageTypeSettingsCache.Value.languageType == CurrentLanguage
                ? _currentLanguageTypeSettingsCache.Value
                : (_currentLanguageTypeSettingsCache = _settings.FontAssets
                    .First(asset => asset.languageType.Equals(CurrentLanguage))).Value;

        #endregion

        #region Event

        private static readonly ISubject<LanguageType> OnInitializeSubject =
            new Subject<LanguageType>();

        private static readonly ISubject<LanguageType> OnLanguageChangeSubject =
            new Subject<LanguageType>();

        public static IObservable<LanguageType> OnInitialize => OnInitializeSubject;

        public static IObservable<LanguageType> OnLanguageChange => OnLanguageChangeSubject;

        public static IObservable<LanguageTypeSettings> OnLanguageTypeSettingsChange =>
            OnLanguageChange.Select(_ => CurrentLanguageTypeSettings);

        #endregion

        private static List<string> streamingFiles = new List<string>() {
            "buff_description.csv",
            "buff_name.csv",
            "character_name.csv",
            "common.csv",
            "elemental_type.csv",
            "enum_type.csv",
            "error.csv",
            "help_popup.csv",
            "item_description.csv",
            "item_name.csv",
            "notification.csv",
            "quest.csv",
            "skill_name.csv",
            "stage_description.csv",
            "tutorial.csv",
            "world_name.csv",
        };

        #region Control

        public static async Task<IObservable<LanguageType>> Initialize()
        {
            return await Initialize(CurrentLanguage);
        }

        public static async Task<IObservable<LanguageType>> Initialize(LanguageType languageType)
        {
            Debug.Log($"{nameof(L10nManager)}.{nameof(Initialize)}() state is {CurrentState.ToString()}, LanguageType is {languageType.ToString()}");
#if TEST_LOG
            Debug.Log($"{nameof(L10nManager)}.{nameof(Initialize)}() called.");
#endif
            switch (CurrentState)
            {
                case State.InInitializing:
                    Debug.LogWarning($"[{nameof(L10nManager)}] Already in initializing now.");
                    return OnInitialize;
                case State.InLanguageChanging:
                    Debug.LogWarning(
                        $"[{nameof(L10nManager)}] Already initialized and in changing language now.");
                    return OnLanguageChange;
                case State.Initialized:
                    Debug.LogWarning(
                        $"[{nameof(L10nManager)}] Already initialized as {CurrentLanguage}.");
                    return Observable.Empty(CurrentLanguage);
            }

            CurrentState = State.InInitializing;
            await InitializeInternal(languageType);

            Debug.Log($"{nameof(L10nManager)}.{nameof(Initialize)}() return");

            return CurrentState == State.Initialized
                ? Observable.Empty(CurrentLanguage)
                : OnInitialize;
        }

        private static async Task InitializeInternal(LanguageType languageType)
        {
            Debug.Log($"{nameof(L10nManager)}.{nameof(InitializeInternal)}()");
            _dictionary = await GetDictionary(languageType);
            CurrentLanguage = languageType;
            _settings = Resources.Load<L10nSettings>(SettingsAssetPathInResources);
            CurrentState = State.Initialized;
            OnInitializeSubject.OnNext(CurrentLanguage);
            Debug.Log($"{nameof(L10nManager)}.{nameof(InitializeInternal)}() end");
        }

        public static IObservable<LanguageType> SetLanguage(LanguageType languageType)
        {
#if TEST_LOG
            Debug.Log($"{nameof(L10nManager)}.{nameof(SetLanguage)}({languageType}) called.");
#endif
            if (languageType == CurrentLanguage)
            {
                return Observable.Empty(CurrentLanguage);
            }

            switch (CurrentState)
            {
                case State.None:
                case State.InInitializing:
                    var subject = new Subject<LanguageType>();
                    subject.OnError(new L10nNotInitializedException());
                    return subject;
                case State.InLanguageChanging:
                    Debug.LogWarning($"[{nameof(L10nManager)}] Already in changing language now.");
                    return OnLanguageChange;
            }

            CurrentState = State.InLanguageChanging;
            SetLanguageInternal(languageType);

            return CurrentState == State.Initialized
                ? Observable.Empty(CurrentLanguage)
                : OnLanguageChange;
        }

        private static async void SetLanguageInternal(LanguageType languageType)
        {
            _dictionary = await GetDictionary(languageType);
            CurrentLanguage = languageType;
            CurrentState = State.Initialized;
            OnLanguageChangeSubject.OnNext(CurrentLanguage);
        }

        #endregion

        public static async Task<IReadOnlyDictionary<string, string>> GetDictionary(LanguageType languageType)
        {
            var dictionary = new Dictionary<string, string>();

            #if UNITY_EDITOR
            if (!Directory.Exists(CsvFilesRootDirectoryPath))
            {
                throw new DirectoryNotFoundException(CsvFilesRootDirectoryPath);
            }

            var csvFileInfos = new DirectoryInfo(CsvFilesRootDirectoryPath).GetFiles("*.csv");
            foreach (var csvFileInfo in csvFileInfos)
            {
                using (var streamReader = new StreamReader(csvFileInfo.FullName))
                using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    csvReader.Configuration.PrepareHeaderForMatch =
                        (header, index) => header.ToLower();
                    var records = csvReader.GetRecords<L10nCsvModel>();
                    var recordsIndex = 0;
                    try
                    {
                        foreach (var record in records)
                        {
#if TEST_LOG
                        Debug.Log($"{csvFileInfo.Name}: {recordsIndex}");
#endif
                            var key = record.Key;
                            if (string.IsNullOrEmpty(key))
                            {
                                recordsIndex++;
                                continue;
                            }

                            var value = (string) typeof(L10nCsvModel)
                                .GetProperty(languageType.ToString())?
                                .GetValue(record);

                            if (string.IsNullOrEmpty(value))
                            {
                                value = record.English;
                            }

                            if (dictionary.ContainsKey(key))
                            {
                                throw new L10nAlreadyContainsKeyException(
                                    $"key: {key}, recordsIndex: {recordsIndex}, csvFileInfo: {csvFileInfo.FullName}");
                            }

                            dictionary.Add(key, value);
                            recordsIndex++;
                        }
                    }
                    catch (CsvHelper.MissingFieldException e)
                    {
                        Debug.LogWarning($"`{csvFileInfo.Name}` file has empty field.\n{e}");
                    }
                }
            }
            #elif UNITY_ANDROID
            foreach (var fileName in streamingFiles)
            {
                var filePath = Path.Combine(CsvFilesRootDirectoryPath, fileName);

                WWW fileData = new WWW(filePath);
                while(!fileData.isDone) await Task.Yield();
                var text = fileData.text;

                byte[] byteArray = Encoding.UTF8.GetBytes(text);
                MemoryStream stream = new MemoryStream(byteArray);
                
                using (var streamReader = new StreamReader(stream))
                using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    csvReader.Configuration.PrepareHeaderForMatch =
                        (header, index) => header.ToLower();
                    var records = csvReader.GetRecords<L10nCsvModel>();
                    var recordsIndex = 0;
                    try
                    {
                        foreach (var record in records)
                        {
//#if TEST_LOG
                        Debug.Log($"{fileName}: {recordsIndex}");
//#endif
                            var key = record.Key;
                            if (string.IsNullOrEmpty(key))
                            {
                                recordsIndex++;
                                continue;
                            }

                            var value = (string) typeof(L10nCsvModel)
                                .GetProperty(languageType.ToString())?
                                .GetValue(record);

                            if (string.IsNullOrEmpty(value))
                            {
                                value = record.English;
                            }

                            if (dictionary.ContainsKey(key))
                            {
                                throw new L10nAlreadyContainsKeyException(
                                    $"key: {key}, recordsIndex: {recordsIndex}, csvFileInfo: {fileName}");
                            }

                            dictionary.Add(key, value);
                            recordsIndex++;
                        }
                    }
                    catch (CsvHelper.MissingFieldException e)
                    {
                        Debug.LogWarning($"`{fileName}` file has empty field.\n{e}");
                    }
                }
            }
            #endif

            return dictionary;
        }

        #region Localize

        public static string Localize(string key)
        {
            TryLocalize(key, out var text);
            return text;
        }

        public static string Localize(string key, params object[] args)
        {
            return TryLocalize(key, out var text)
                ? string.Format(text, args)
                : text;
        }

        private static bool TryLocalize(string key, out string text)
        {
            try
            {
                ValidateStateAndKey(key);
                text = _dictionary[key];
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.GetType().FullName}: {e.Message} key: {key}");
                text = $"!{key}!";
                return false;
            }
        }

        public static string LocalizeCharacterName(int characterId)
        {
            var key = $"CHARACTER_NAME_{characterId}";
            return Localize(key);
        }

        public static string LocalizeItemName(int itemId)
        {
            var key = $"ITEM_NAME_{itemId}";
            return Localize(key);
        }

        public static int LocalizedCount(string key)
        {
            ValidateStateAndKey(key);

            var count = 0;
            while (true)
            {
                if (!_dictionary.ContainsKey($"{key}{count}"))
                {
                    return count;
                }

                count++;
            }
        }

        public static Dictionary<string, string> LocalizePattern(string pattern)
        {
            ValidateStateAndKey(pattern);

            return _dictionary.Where(pair => Regex
                    .IsMatch(pair.Key, pattern))
                .ToDictionary(
                    pair => pair.Key,
                    pair => pair.Value);
        }

        #endregion

        private static void ValidateStateAndKey(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (CurrentState != State.Initialized)
            {
                throw new L10nNotInitializedException();
            }
        }

        public static bool TryGetFontMaterial(FontMaterialType fontMaterialType, out Material material)
        {
            if (fontMaterialType == FontMaterialType.Default)
            {
                material = CurrentLanguageTypeSettings.fontAssetData.FontAsset.material;
                return true;
            }

            foreach (var data in CurrentLanguageTypeSettings.fontAssetData.FontMaterialDataList)
            {
                if (fontMaterialType != data.type)
                {
                    continue;
                }

                material = data.material;
                return true;
            }

            material = default;
            return false;
        }
    }
}
