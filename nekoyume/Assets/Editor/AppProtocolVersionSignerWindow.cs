using System;
using System.Globalization;
using System.Linq;
using Nekoyume;
using UnityEngine;
using UnityEditor;

namespace Editor
{
    public sealed class AppProtocolVersionSignerWindow : EditorWindow
    {
        private bool _showParameters = true;

        public string macOSBinaryUrl = string.Empty;

        public string windowsBinaryUrl = string.Empty;

        public int version;

        private string _versionString = "1";

        private DateTimeOffset _timestamp = DateTimeOffset.UtcNow;

        private bool _showPrivateKey = true;

        private string[] _privateKeyOptions;

        private int _selectedPrivateKeyIndex;

        private bool _toggledOnTypePrivateKey;

        private string _privateKey;

        private string _privateKeyPassphrase;

        [MenuItem("Tools/Libplanet/Sign A New Version")]
        public static void Init()
        {
            var window = EditorWindow.GetWindow<AppProtocolVersionSignerWindow>();
            window.Show();
        }

        public void Awake()
        {
            FillAttributes();
        }

        public void OnFocus()
        {
            FillAttributes();

            RefreshPrivateKeys();
        }

        void OnGUI()
        {
            _showPrivateKey = EditorGUILayout.Foldout(_showPrivateKey, "Private Key");
            if (_showPrivateKey)
            {
                _selectedPrivateKeyIndex = EditorGUILayout.Popup(
                    "Private Key",
                    _selectedPrivateKeyIndex,
                    _privateKeyOptions);
                if (_selectedPrivateKeyIndex == _privateKeyOptions.Length - 1)
                {
                    _toggledOnTypePrivateKey = EditorGUILayout.Toggle(
                        "Type New Private Key",
                        _toggledOnTypePrivateKey);
                    if (_toggledOnTypePrivateKey)
                    {
                        _privateKey =
                            EditorGUILayout.PasswordField("New Private Key", _privateKey) ??
                            string.Empty;
                        ShowError(_privateKey.Any() ? null : "New private key is empty.");
                    }
                }

                _privateKeyPassphrase =
                    EditorGUILayout.PasswordField("Passphrase", _privateKeyPassphrase) ??
                    string.Empty;
                ShowError(_privateKeyPassphrase.Any() ? null : "Passphrase is empty.");

                if (_selectedPrivateKeyIndex == _privateKeyOptions.Length - 1)
                {
                    EditorGUI.BeginDisabledGroup(!_privateKeyPassphrase.Any());
                    
                    EditorGUI.EndDisabledGroup();
                }
            }

            HorizontalLine();

            _showParameters = EditorGUILayout.Foldout(_showParameters, "Parameters");
            if (_showParameters)
            {
                _versionString = EditorGUILayout.TextField("Version", _versionString);
                try
                {
                    version = int.Parse(_versionString, CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    ShowError(e.Message);
                }

                macOSBinaryUrl = EditorGUILayout.TextField("macOS Binary URL", macOSBinaryUrl);
                windowsBinaryUrl =
                    EditorGUILayout.TextField("Windows Binary URL", windowsBinaryUrl);
            }

            HorizontalLine();

        }

        void FillAttributes()
        {           
            maxSize = new Vector2(600, 450);
            _timestamp = DateTimeOffset.UtcNow;
            titleContent = new GUIContent("Libplanet Version Signer");
        }

        void RefreshPrivateKeys()
        {
            
        }

        static void ShowError(string message)
        {
            if (message is null)
            {
                return;
            }

            var style = new GUIStyle();
            style.normal.textColor = Color.red;
            EditorGUILayout.LabelField(message, style);
        }

        static void HorizontalLine()
        {
            Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(6));
            r.height = 1;
            r.y += 5;
            r.x -= 2;
            r.width += 6;
            EditorGUI.DrawRect(r, Color.gray);
        }
    }
}
