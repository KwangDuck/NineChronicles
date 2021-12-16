using TMPro;

namespace Nekoyume.UI
{
    public class VersionSystem : SystemWidget
    {
        public TextMeshProUGUI informationText;
        private int _version;
        private long _blockIndex;

        protected override void Awake()
        {
            base.Awake();
        }

        public void SetVersion(int version)
        {
            _version = version;
            UpdateText();
        }

        private void SubscribeBlockIndex(long blockIndex)
        {
            _blockIndex = blockIndex;
            UpdateText();
        }

        private void UpdateText()
        {
            const string format = "APV: {0} / #{1} / Hash: {2}";
            var hash = string.Empty;
            var text = string.Format(
                format,
                _version,
                _blockIndex,
                hash.Length >= 4 ? hash.Substring(0, 4) : "...");
            informationText.text = text;
        }
    }
}
