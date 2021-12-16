using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Nekoyume.Game.Controller;
using Nekoyume.Helper;
using Nekoyume.L10n;
using Nekoyume.Model.Item;
using Nekoyume.Model.Mail;
using Nekoyume.Model.State;
using Nekoyume.State;
using Nekoyume.UI.Model;
using Nekoyume.UI.Module;
using Nekoyume.UI.Scroller;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nekoyume.UI
{
    using UniRx;

    public class MailPopup : XTweenPopupWidget, IMail
    {
        public enum MailTabState : int
        {
            All,
            Workshop,
            Market,
            System
        }

        [SerializeField]
        private MailTabState tabState = default;

        [SerializeField]
        private CategoryTabButton allButton = null;

        [SerializeField]
        private CategoryTabButton workshopButton = null;

        [SerializeField]
        private CategoryTabButton marketButton = null;

        [SerializeField]
        private CategoryTabButton systemButton = null;

        [SerializeField]
        private MailScroll scroll = null;

        [SerializeField]
        private GameObject emptyImage = null;

        [SerializeField]
        private TextMeshProUGUI emptyText = null;

        [SerializeField]
        private string emptyTextL10nKey = null;

        [SerializeField]
        private Blur blur = null;

        [SerializeField]
        private Button closeButton = null;

        private readonly Module.ToggleGroup _toggleGroup = new Module.ToggleGroup();

        private static Sprite _selectedButtonSprite;

        private const int TutorialEquipmentId = 10110000;

        public MailBox MailBox { get; private set; }

        #region override

        protected override void Awake()
        {
            base.Awake();
            _toggleGroup.RegisterToggleable(allButton);
            _toggleGroup.RegisterToggleable(workshopButton);
            _toggleGroup.RegisterToggleable(marketButton);
            _toggleGroup.RegisterToggleable(systemButton);
            closeButton.onClick.AddListener(() =>
            {
                Close();
                AudioController.PlayClick();
            });
        }

        public override void Initialize()
        {
            base.Initialize();
            _selectedButtonSprite = Resources.Load<Sprite>("UI/Textures/button_yellow_02");

            ReactiveAvatarState.MailBox?.Subscribe(SetList).AddTo(gameObject);

            emptyText.text = L10nManager.Localize(emptyTextL10nKey);
        }

        public override void Show(bool ignoreShowAnimation = false)
        {
            MailBox = States.Instance.CurrentAvatarState.mailBox;
            _toggleGroup.SetToggledOffAll();
            allButton.SetToggledOn();
            ChangeState(0);
            UpdateTabs();
            base.Show(ignoreShowAnimation);

            if (blur)
            {
                blur.Show();
            }
            HelpTooltip.HelpMe(100010, true);
        }

        public override void Close(bool ignoreCloseAnimation = false)
        {
            if (blur && blur.isActiveAndEnabled)
            {
                blur.Close();
            }

            base.Close(ignoreCloseAnimation);
        }

        #endregion

        public void ChangeState(int state)
        {
            tabState = (MailTabState) state;

            UpdateMailList(0);
        }

        private IEnumerable<Nekoyume.Model.Mail.Mail> GetAvailableMailList(long blockIndex, MailTabState state)
        {
            bool predicate(Nekoyume.Model.Mail.Mail mail)
            {
                if (state == MailTabState.All)
                {
                    return true;
                }

                return mail.MailType == (MailType) state;
            }

            return MailBox?.Where(mail =>
                mail.requiredBlockIndex <= blockIndex)
                .Where(predicate)
                .OrderByDescending(mail => mail.New);
        }

        private void UpdateMailList(long blockIndex)
        {
            var list = GetAvailableMailList(blockIndex, tabState)?.ToList();

            if (list is null)
            {
                return;
            }

            scroll.UpdateData(list, true);
            emptyImage.SetActive(!list.Any());
            UpdateTabs(blockIndex);
        }

        private void OnReceivedTutorialEquipment()
        {
            var tutorialController = Game.Game.instance.Stage.TutorialController;
            tutorialController.GetTutorialProgress();
            if (tutorialController.CurrentlyPlayingId < 37)
            {
                tutorialController.Stop(() => tutorialController.Play(37));
            }
        }

        public void UpdateTabs(long? blockIndex = null)
        {
            blockIndex = 0;

            // 전체 탭
            allButton.HasNotification.Value = MailBox
                .Any(mail => mail.New && mail.requiredBlockIndex <= blockIndex);

            var list = GetAvailableMailList(blockIndex.Value, MailTabState.Workshop);
            var recent = list?.FirstOrDefault();
            workshopButton.HasNotification.Value = recent is { New: true };

            list = GetAvailableMailList(blockIndex.Value, MailTabState.Market);
            recent = list?.FirstOrDefault();
            marketButton.HasNotification.Value = recent is { New: true };

            list = GetAvailableMailList(blockIndex.Value, MailTabState.System);
            recent = list?.FirstOrDefault();
            systemButton.HasNotification.Value = recent is { New: true };
        }

        private void SetList(MailBox mailBox)
        {
            if (mailBox is null)
            {
                return;
            }

            MailBox = mailBox;
            ChangeState((int) tabState);
        }

        private void UpdateNotification(long blockIndex)
        {
            var avatarState = States.Instance.CurrentAvatarState;
            if (avatarState is null)
            {
                return;
            }

            MailBox = avatarState.mailBox;
            UpdateTabs(blockIndex);
        }

        public void Read(CombinationMail mail)
        {
            
        }

        public async void Read(OrderBuyerMail orderBuyerMail)
        {
            var popup = Find<BuyItemInformationPopup>();         
        }

        public async void Read(OrderSellerMail orderSellerMail)
        {
        }

        public async void Read(OrderExpirationMail orderExpirationMail)
        {
            Find<OneButtonSystem>().Show(L10nManager.Localize("UI_SELL_CANCEL_INFO"),
                L10nManager.Localize("UI_YES"),
                () =>
                {
                    
                });
        }

        public async void Read(CancelOrderMail cancelOrderMail)
        {
        }

        public void Read(ItemEnhanceMail itemEnhanceMail)
        {
            Find<EnhancementResultPopup>().Show(itemEnhanceMail);
        }

        public void Read(DailyRewardMail dailyRewardMail)
        {
            // ignored.
        }

        public void Read(MonsterCollectionMail monsterCollectionMail)
        {
            var popup = Find<MonsterCollectionRewardsPopup>();
            popup.OnClickSubmit.First().Subscribe(widget =>
            {
                
                widget.Close();
            });            
        }

        public void TutorialActionClickFirstCombinationMailSubmitButton()
        {
            if (MailBox.Count == 0)
            {
                Debug.LogError("TutorialActionClickFirstCombinationMailSubmitButton() MailBox.Count == 0");
                return;
            }

            var mail = MailBox[0] as CombinationMail;
            if (mail is null)
            {
                Debug.LogError("TutorialActionClickFirstCombinationMailSubmitButton() mail is null");
                return;
            }

            Read(mail);
        }

        [Obsolete]
        public void Read(SellCancelMail mail) { }

        [Obsolete]
        public void Read(BuyerMail buyerMail) { }

        [Obsolete]
        public void Read(SellerMail sellerMail) { }
    }
}
