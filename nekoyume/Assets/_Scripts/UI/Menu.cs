using System.Collections;
using Assets.SimpleLocalization;
using Nekoyume.BlockChain;
using Nekoyume.Game;
using Nekoyume.Game.Character;
using Nekoyume.Game.Controller;
using Nekoyume.UI.Module;
using Nekoyume.Manager;
using UnityEngine;

namespace Nekoyume.UI
{
    public class Menu : Widget
    {
        public MainMenu btnQuest;
        public MainMenu btnCombination;
        public MainMenu btnShop;
        public MainMenu btnRanking;
        public SpeechBubble[] SpeechBubbles;
        public Npc npc;
        public SpeechBubble speechBubble;

        public Stage Stage;

        protected override void Awake()
        {
            base.Awake();

            Stage = GameObject.Find("Stage").GetComponent<Stage>();
            SpeechBubbles = GetComponentsInChildren<SpeechBubble>();
        }

        private void ShowButtons(Player player)
        {
            btnQuest.Set(player);
            btnCombination.Set(player);
            btnShop.Set(player);
            btnRanking.Set(player);
        }

        private void HideButtons()
        {
            btnQuest.gameObject.SetActive(false);
            btnCombination.gameObject.SetActive(false);
            btnShop.gameObject.SetActive(false);
            btnRanking.gameObject.SetActive(false);
        }

        public void ShowRoom()
        {
            Find<QuestPreparation>().Close();
            var stage = Game.Game.instance.stage;
            stage.LoadBackground("room");
            stage.GetPlayer(stage.roomPosition);

            var player = stage.GetPlayer();
            player.UpdateEquipments(player.Model.Value.armor, player.Model.Value.weapon);
            player.UpdateCustomize();
            player.gameObject.SetActive(true);

            Show();
            StartCoroutine(ShowSpeeches());
            ShowButtons(player);

            AudioController.instance.PlayMusic(AudioController.MusicCode.Main);
        }

        public void ShowWorld()
        {
            Show();
            HideButtons();
        }

        public void QuestClick()
        {
            Close();
            var avatarState = States.Instance.CurrentAvatarState.Value;
            Find<WorldMap>().Show(avatarState.worldInformation);
            AudioController.PlayClick();
            AnalyticsManager.Instance.OnEvent(AnalyticsManager.EventName.ClickMainBattle);
        }

        public void ShopClick()
        {
            if (States.Instance.CurrentAvatarState.Value.level >= GameConfig.ShopRequiredLevel)
            {
                Close();
                Find<Shop>().Show();
                AudioController.PlayClick();
                AnalyticsManager.Instance.OnEvent(AnalyticsManager.EventName.ClickMainShop);
            }
            else
            {
                ShowRequiredLevelSpeech(btnShop.pointerClickKey, GameConfig.ShopRequiredLevel);
            }
        }

        public void CombinationClick()
        {
            if (States.Instance.CurrentAvatarState.Value.level >= GameConfig.CombinationRequiredLevel)
            {
                Close();
                Find<Combination>().Show();
                AudioController.PlayClick();
                AnalyticsManager.Instance.OnEvent(AnalyticsManager.EventName.ClickMainCombination);
            }
            else
            {
                ShowRequiredLevelSpeech(btnCombination.pointerClickKey, GameConfig.CombinationRequiredLevel);
            }
        }

        public void RankingClick()
        {
            if (States.Instance.CurrentAvatarState.Value.level >= GameConfig.RankingRequiredLevel)
            {
                Close();
                Find<RankingBoard>().Show();
                AudioController.PlayClick();
            }
            else
            {
                ShowRequiredLevelSpeech(btnRanking.pointerClickKey, GameConfig.RankingRequiredLevel);
            }
        }

        public override void Show()
        {
            base.Show();
            Find<Status>().Show();
            Find<BottomMenu>().Show(
                UINavigator.NavigationType.Quit,
                _ => Game.Game.Quit(),
                true,
                BottomMenu.ToggleableType.Mail,
                BottomMenu.ToggleableType.Quest,
                BottomMenu.ToggleableType.Chat,
                BottomMenu.ToggleableType.IllustratedBook,
                BottomMenu.ToggleableType.Character,
                BottomMenu.ToggleableType.Inventory,
                BottomMenu.ToggleableType.Settings);
        }

        public override void Close(bool ignoreCloseAnimation = false)
        {
            StopCoroutine(ShowSpeeches());
            foreach (var bubble in SpeechBubbles)
            {
                bubble.Hide();
            }

            Find<Inventory>().Close(ignoreCloseAnimation);
            Find<StatusDetail>().Close(ignoreCloseAnimation);
            Find<Quest>().Close(ignoreCloseAnimation);

            Find<BottomMenu>().Close(ignoreCloseAnimation);
            Find<Status>().Close(ignoreCloseAnimation);
            base.Close(ignoreCloseAnimation);
        }

        private IEnumerator ShowSpeeches()
        {
            foreach (var bubble in SpeechBubbles)
            {
                bubble.Init();
            }

            yield return new WaitForSeconds(2.0f);

            while (true)
            {
                var n = SpeechBubbles.Length;
                while (n > 1)
                {
                    n--;
                    var k = Mathf.FloorToInt(Random.value * (n + 1));
                    var value = SpeechBubbles[k];
                    SpeechBubbles[k] = SpeechBubbles[n];
                    SpeechBubbles[n] = value;
                }

                foreach (var bubble in SpeechBubbles)
                {
                    yield return StartCoroutine(bubble.CoShowText());
                    yield return new WaitForSeconds(Random.Range(2.0f, 4.0f));
                }
            }
        }

        private void ShowRequiredLevelSpeech(string pointerClickKey, int level)
        {
            speechBubble.SetKey(pointerClickKey);
            var format =
                LocalizationManager.Localize(
                    $"{pointerClickKey}{Random.Range(0, speechBubble.SpeechCount)}");
            var speech = string.Format(format, level);
            StartCoroutine(speechBubble.CoShowText(speech, true));
            if (npc)
            {
                npc.Emotion();
            }
        }
    }
}
