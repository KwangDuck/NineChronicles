using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Nekoyume.Action;
using Nekoyume.Data;
using Nekoyume.Game.Character;
using Nekoyume.Game.Controller;
using Nekoyume.Game.Entrance;
using Nekoyume.Game.Factory;
using Nekoyume.Game.Item;
using Nekoyume.Game.Trigger;
using Nekoyume.Game.Util;
using Nekoyume.Model;
using Nekoyume.UI;
using UnityEngine;

namespace Nekoyume.Game
{
    public class Stage : MonoBehaviour, IStage
    {
        private GameObject _background;
        public int id;
        private BattleLog _battleLog;
        private const float AttackDelay = 0.3f;
        // dummy for stage background moving.
        [SerializeField]
        private GameObject dummy;
        public float loadingSpeed = 2.0f;
        public Character.Player selectedPlayer;
        private readonly Vector2 _stageStartPosition = new Vector2(-6.0f, -1.2f);
        public readonly Vector2 QuestPreparationPosition = new Vector2(1.65f, -1.3f);
        public readonly Vector2 RoomPosition = new Vector2(-2.66f, -1.85f);
        private PlayerFactory _factory;
        private MonsterSpawner _spawner;
        private Camera _camera;
        private ObjectPool _objectPool;

        private void Awake()
        {
            _camera = Camera.main;
            if (ReferenceEquals(_camera, null))
            {
                throw new NullReferenceException("`Camera.main` can't be null.");
            }

            _factory = GetComponent<PlayerFactory>();
            if (ReferenceEquals(_factory, null))
            {
                throw new NotFoundComponentException<PlayerFactory>();
            }

            _objectPool = GetComponent<ObjectPool>();
            if (ReferenceEquals(_objectPool, null))
            {
                throw new NotFoundComponentException<ObjectPool>();
            }

            _spawner = GetComponentInChildren<MonsterSpawner>();
            if (ReferenceEquals(_spawner, null))
            {
                throw new NotFoundComponentException<MonsterSpawner>();
            }

            if (ReferenceEquals(dummy, null))
            {
                throw new NullReferenceException("`Dummy` can't be null.");
            }

            Event.OnNestEnter.AddListener(OnNestEnter);
            Event.OnLoginDetail.AddListener(OnLoginDetail);
            Event.OnRoomEnter.AddListener(OnRoomEnter);
            Event.OnPlayerDead.AddListener(OnPlayerDead);
            Event.OnStageStart.AddListener(OnStageStart);
        }

        private void OnStageStart()
        {
            var qp = Widget.Find<QuestPreparation>();
            if (qp.IsActive())
            {
                qp.Close();
            }
            _battleLog = ActionManager.Instance.battleLog;
            Play(_battleLog);
        }

        private void Start()
        {
            OnNestEnter();
        }

        private void OnNestEnter()
        {
            gameObject.AddComponent<NestEntering>();
        }

        private void OnLoginDetail(int index)
        {
            var players = GetComponentsInChildren<Character.Player>(true);
            for (int i = 0; i < players.Length; ++i)
            {
                GameObject playerObject = players[i].gameObject;
                var anim = playerObject.GetComponentInChildren<Animator>(true);
                if (index == i)
                {
                    playerObject.transform.DOScale(1.1f, 2.0f).SetDelay(0.2f);
                    playerObject.transform.DOMove(new Vector3(-1.25f, -0.7f), 2.4f).SetDelay(0.2f);
                    if (anim && !anim.gameObject.activeSelf)
                    {
                        anim.gameObject.SetActive(true);
                        anim.Play("Appear");
                    }

                    selectedPlayer = players[i];
                }
                else
                {
                    playerObject.transform.DOScale(0.9f, 1.0f);
                    playerObject.transform.DOMoveY(-3.6f, 2.0f);
                    if (anim && anim.gameObject.activeSelf)
                    {
                        anim.gameObject.SetActive(true);
                        anim.Play("Disappear");
                    }
                }
            }
        }

        private void OnRoomEnter()
        {
            gameObject.AddComponent<RoomEntering>();
        }

        private void OnPlayerDead()
        {
        }

        public void LoadBackground(string prefabName, float fadeTime = 0.0f)
        {
            if (!(ReferenceEquals(_background, null)))
            {
                if (_background.name.Equals(prefabName)) return;
                if (fadeTime > 0.0f)
                {
                    var sprites = _background.GetComponentsInChildren<SpriteRenderer>();
                    foreach (var sprite in sprites)
                    {
                        sprite.sortingOrder += 1;
                        sprite.DOFade(0.0f, fadeTime);
                    }
                }

                Destroy(_background, fadeTime);
                _background = null;
            }

            var resName = $"Prefab/Background/{prefabName}";
            var prefab = Resources.Load<GameObject>(resName);
            if (ReferenceEquals(prefab, null)) return;
            _background = Instantiate(prefab, transform);
            _background.name = prefabName;
        }

        public void Play(BattleLog log)
        {
            if (log?.Count > 0)
            {
                StartCoroutine(PlayAsync(log));
            }
        }

        private IEnumerator PlayAsync(BattleLog log)
        {
            _objectPool.ReleaseAll();
            GetPlayer(_stageStartPosition);
            yield return StartCoroutine(CoStageEnter(log.stage));
            foreach (EventBase e in log)
            {
                {
                    yield return StartCoroutine(e.CoExecute(this));
                }
            }
            yield return StartCoroutine(CoStageEnd(log.result));
        }

        private IEnumerator CoStageEnter(int stage)
        {
            Data.Table.Background data;
            var tables = this.GetRootComponent<Tables>();
            if (tables.Background.TryGetValue(stage, out data))
            {
                ReadyPlayer();
                var loadingScreen = Widget.Find<LoadingScreen>();
                loadingScreen.Show();

                LoadBackground(data.background, 3.0f);
                Widget.Find<Menu>().ShowWorld();

                switch (data.background)
                {
                    case "zone_0_0":
                        AudioController.instance.PlayMusic(AudioController.MusicCode.StageGreen);
                        break;
                    case "zone_0_1":
                        AudioController.instance.PlayMusic(AudioController.MusicCode.StageOrange);
                        break;
                    case "zone_0_2":
                        AudioController.instance.PlayMusic(AudioController.MusicCode.StageBlue);
                        break;
                }
                
                yield return new WaitForSeconds(1.5f);
                loadingScreen.Close();
            }
        }

        public IEnumerator CoStageEnd(BattleLog.Result result)
        {
            Widget.Find<BattleResult>().Show(result);
            if (result == BattleLog.Result.Win)
            {
                StartCoroutine(CoSlideBg());
            }
            else
            {
                _objectPool.ReleaseAll();
            }

            yield return null;
        }

        private IEnumerator CoSlideBg()
        {
            var roomPlayer = RunPlayer();
            dummy.transform.position = roomPlayer.transform.position;
            while (Widget.Find<BattleResult>().IsActive())
            {
                UpdateDummyPosition(roomPlayer, ActionCamera.instance);
                yield return new WaitForEndOfFrame();
            }
        }

        private void UpdateDummyPosition(Character.Player player, ActionCamera cam)
        {
            if (ReferenceEquals(cam, null)) throw new ArgumentNullException(nameof(cam));
            Vector2 position = dummy.transform.position;
            position.x += Time.deltaTime * player.Speed;
            dummy.transform.position = position;
            cam.ChaseX(dummy.transform);
        }

        public IEnumerator CoSpawnPlayer(Model.Player character)
        {
            var status = Widget.Find<Status>();
            var pos = _camera.ScreenToWorldPoint(status.transform.position);
            var playerPos = _stageStartPosition;
            var playerCharacter = RunPlayer(playerPos);
            playerPos.x = pos.x - 6.0f;
            playerCharacter.transform.position = playerPos;
            playerCharacter.Init(character);
            var player = playerCharacter.gameObject;
            status.UpdatePlayer(player);
            ActionCamera.instance.ChaseX(player.transform);
            while (true)
            {
                Debug.Log($"pos: {pos.x}");
                Debug.Log($"player: {player.transform.position.x}");
                if (pos.x <= player.transform.position.x)
                {
                    break;
                }

                yield return new WaitForEndOfFrame();
            }
            yield return null;
        }

        public IEnumerator CoAttack(int atk, Model.CharacterBase character, Model.CharacterBase target, bool critical)
        {
            Character.CharacterBase attacker;
            Character.CharacterBase defender;
            var player = GetPlayer();
            var enemies = GetComponentsInChildren<Enemy>();
            if (character is Model.Player)
            {
                attacker = player;
                defender = enemies.FirstOrDefault(e => e.id == target.id);
                if (!player.TargetInRange(defender))
                {
                    attacker.StartRun();
                }

                yield return new WaitUntil(() => player.TargetInRange(defender));
            }
            else
            {
                attacker = enemies.FirstOrDefault(e => e.id == character.id);
                defender = player;
            }

            if (attacker != null && defender != null)
            {
                yield return StartCoroutine(attacker.CoAttack(atk, defender, critical));

            }
            yield return new WaitForSeconds(AttackDelay);
        }

        public IEnumerator CoDropBox(List<ItemBase> items)
        {
            if (items.Count > 0)
            {
                var dropItemFactory = GetComponent<DropItemFactory>();
                var player = GetPlayer();
                var position = player.transform.position;
                position.x += 1.0f;
                yield return StartCoroutine(dropItemFactory.CoCreate(items, position));
            }

            yield return null;
        }

        public IEnumerator CoGetReward(List<ItemBase> rewards)
        {
            foreach (var item in rewards)
            {
                Widget.Find<BattleResult>().Add(item);
            }

            yield return null;
        }

        public IEnumerator CoSpawnWave(List<Monster> monsters, bool isBoss)
        {
            var playerCharacter = GetPlayer();
            playerCharacter.StartRun();
            _spawner.SetData(id, monsters);

            if (isBoss)
            {
                AudioController.instance.PlayMusic(AudioController.MusicCode.Boss1);
            }
            
            yield break;
        }

        public IEnumerator CoGetExp(long exp)
        {
            var player = GetPlayer();
            yield return StartCoroutine(player.CoGetExp(exp));
        }

        public Character.Player GetPlayer()
        {
            var player = GetComponentInChildren<Character.Player>();
            if (ReferenceEquals(player, null))
            {
                var go = _factory.Create(ActionManager.Instance.Avatar);
                player = go.GetComponent<Character.Player>();

                if (ReferenceEquals(player, null))
                {
                    throw new NotFoundComponentException<Character.Player>();
                }
            }

            return player;
        }

        public Character.Player GetPlayer(Vector2 position)
        {
            var player = GetPlayer();
            player.transform.position = position;
            return player;
        }

        private Character.Player RunPlayer()
        {
            var player = GetPlayer();
            player.StartRun();
            player.RunSpeed *= loadingSpeed;
            return player;
        }

        private Character.Player RunPlayer(Vector2 position)
        {
            var player = RunPlayer();
            player.transform.position = position;
            return player;
        }

        public Character.Player ReadyPlayer()
        {
            var player = GetPlayer(_stageStartPosition);
            player.RunSpeed = 0.0f;
            return player;
        }
    }
}
