using System.Collections.Generic;
using System.Linq;
using Nekoyume.Model.Item;
using Nekoyume.Model.State;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nekoyume.UI
{
    public class CodeRewardPopup : PopupWidget
    {
        [SerializeField]
        private CodeRewardEffector effector;

        [SerializeField]
        private Button button;

        [SerializeField]
        private TextMeshProUGUI count;

        private readonly Dictionary<string, List<(ItemBase, int)>> _codeRewards =
            new Dictionary<string, List<(ItemBase, int)>>();

        private RedeemCodeState _state = null;

        protected override void Awake()
        {
            base.Awake();
            button.onClick.AddListener(OnClickButton);
        }

        public void Show(string sealedCode, RedeemCodeState state)
        {
            UpdateButton(_codeRewards.Count);
            base.Show();
        }

        private void OnClickButton()
        {
            if (!_codeRewards.Any())
            {
                return;
            }

            var reward = _codeRewards.First();
            effector.gameObject.SetActive(true);
            effector.Play(reward.Value);
            _codeRewards.Remove(reward.Key);
            UpdateButton(_codeRewards.Count);
        }

        private void UpdateButton(int value)
        {
            count.text = value.ToString();
            button.gameObject.SetActive(value > 0);
        }
    }
}
