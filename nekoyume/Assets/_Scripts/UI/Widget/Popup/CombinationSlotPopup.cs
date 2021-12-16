using System;
using System.Collections.Generic;
using Nekoyume.Game.Controller;
using Nekoyume.Helper;
using Nekoyume.L10n;
using Nekoyume.Model.Item;
using Nekoyume.Model.State;
using Nekoyume.UI.Model;
using Nekoyume.UI.Module;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nekoyume.UI
{
    using UniRx;

    public class CombinationSlotPopup : PopupWidget
    {
        public enum CraftType
        {
            CombineEquipment,
            CombineConsumable,
            Enhancement,
        }

        [Serializable]
        public class Information
        {
            public CraftType Type;
            public GameObject Icon;
            public GameObject OptionContainer;
            public TextMeshProUGUI ItemLevel;
            public ItemOptionView MainStatView;
            public List<ItemOptionWithCountView> StatOptions;
            public List<ItemOptionView> SkillOptions;
        }

        [SerializeField]
        private SimpleItemView itemView;

        [SerializeField]
        private Slider progressBar;

        [SerializeField]
        private TextMeshProUGUI itemNameText;

        [SerializeField]
        private TextMeshProUGUI requiredBlockIndexText;

        [SerializeField]
        private TextMeshProUGUI timeText;

        [SerializeField]
        private ConditionalCostButton rapidCombinationButton;

        [SerializeField]
        private Button bgButton;

        [SerializeField]
        private List<Information> _informations;

        private CraftType _craftType;
        private CombinationSlotState _slotState;
        private int _slotIndex;
        private readonly List<IDisposable> _disposablesOfOnEnable = new List<IDisposable>();

        protected override void Awake()
        {
            base.Awake();

            rapidCombinationButton.OnSubmitSubject
                .Subscribe(_ =>
                {
                    AudioController.PlayClick();
                    Game.Game.instance.ActionManager.RapidCombinationAsync(_slotState, _slotIndex).Subscribe();
                    Find<CombinationSlotsPopup>().SetCaching(
                        _slotIndex,
                        true,
                        slotType: CombinationSlot.SlotType.WaitingReceive);
                    Close();
                })
                .AddTo(gameObject);

            bgButton.onClick.AddListener(() =>
            {
                AudioController.PlayClick();
                Close();
            });
        }

        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            _disposablesOfOnEnable.DisposeAllAndClear();
            base.OnDisable();
        }

        private void SubscribeOnBlockIndex(long currentBlockIndex)
        {
            UpdateInformation(_craftType, _slotState, currentBlockIndex);
        }

        public void Show(CombinationSlotState state, int slotIndex, long currentBlockIndex)
        {
            _slotState = state;
            _slotIndex = slotIndex;
            _craftType = GetCombinationType(state);
            UpdateInformation(_craftType, state, currentBlockIndex);
            base.Show();
        }

        private void UpdateInformation(CraftType type, CombinationSlotState state, long currentBlockIndex)
        {
            UpdateOption(type, state);
            UpdateButtonInformation(state, currentBlockIndex);
            UpdateRequiredBlockInformation(state, currentBlockIndex);
        }

        private void UpdateOption(CraftType type, CombinationSlotState slotState)
        {
            foreach (var information in _informations)
            {
                information.Icon.SetActive(information.Type.Equals(type));
                information.OptionContainer.SetActive(information.Type.Equals(type));
            }
        }

        private void UpdateRequiredBlockInformation(CombinationSlotState state, long currentBlockIndex)
        {
            progressBar.maxValue = Math.Max(state.RequiredBlockIndex, 1);
            var diff = Math.Max(state.UnlockBlockIndex - currentBlockIndex, 1);
            progressBar.value = diff;
            requiredBlockIndexText.text = $"{diff}.";
            timeText.text = string.Format(L10nManager.Localize("UI_REMAINING_TIME"), Util.GetBlockToTime((int)diff));
        }

        private void UpdateButtonInformation(CombinationSlotState state, long currentBlockIndex)
        {
            var diff = state.UnlockBlockIndex - currentBlockIndex;
            var cost = 0;
            rapidCombinationButton.SetCost(ConditionalCostButton.CostType.Hourglass, cost);
        }

        private void UpdateItemInformation(ItemUsable item)
        {
            itemView.SetData(new Item(item));
            itemNameText.text = TextHelper.GetItemNameInCombinationSlot(item);
        }

        private static CraftType GetCombinationType(CombinationSlotState state)
        {
            return CraftType.CombineConsumable;
        }
    }
}
