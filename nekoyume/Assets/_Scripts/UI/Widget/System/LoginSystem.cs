using System;
using Nekoyume.L10n;
using Nekoyume.UI.Module;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nekoyume.UI
{
    using UniRx;

    public class LoginSystem : SystemWidget
    {
        public enum States
        {
            Show,
            CreateAccount,
            Login,
            FindPassphrase,
            ResetPassphrase,
            Failed,
            CreatePassword,
        }

        public InputField passPhraseField;
        public InputField retypeField;
        public InputField loginField;
        public InputField findPassphraseField;
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI contentText;
        public GameObject passPhraseGroup;
        public GameObject retypeGroup;
        public GameObject loginGroup;
        public TextMeshProUGUI findPassphraseTitle;
        public GameObject findPassphraseGroup;
        public GameObject accountGroup;
        public GameObject header;
        public GameObject bg;
        public GameObject loginWarning;
        public GameObject findPrivateKeyWarning;
        public GameObject createSuccessGroup;
        public TextMeshProUGUI strongText;
        public TextMeshProUGUI weakText;
        public TextMeshProUGUI correctText;
        public TextMeshProUGUI incorrectText;
        public TextMeshProUGUI findPassphraseText;
        public TextMeshProUGUI backToLoginText;
        public TextMeshProUGUI passPhraseText;
        public TextMeshProUGUI retypeText;
        public TextMeshProUGUI loginText;
        public TextMeshProUGUI enterPrivateKeyText;
        public TextMeshProUGUI accountAddressText;
        public TextMeshProUGUI accountAddressHolder;
        public TextMeshProUGUI accountWarningText;
        public TextMeshProUGUI successText;
        public ConditionalButton submitButton;
        public Button findPassphraseButton;
        public Button backToLoginButton;
        public Image accountImage;
        public readonly ReactiveProperty<States> State = new ReactiveProperty<States>();
        public bool Login { get; private set; }
        private string _privateKeyString;
        private States _prevState;
        public Blur blur;

        protected override void Awake()
        {
            State.Value = States.Show;
            State.Subscribe(SubscribeState).AddTo(gameObject);
            strongText.gameObject.SetActive(false);
            weakText.gameObject.SetActive(false);
            correctText.gameObject.SetActive(false);
            incorrectText.gameObject.SetActive(false);
            contentText.text = L10nManager.Localize("UI_LOGIN_CONTENT");
            accountAddressHolder.text = L10nManager.Localize("UI_ACCOUNT_PLACEHOLDERS");
            findPassphraseTitle.text = L10nManager.Localize("UI_LOGIN_FIND_PASSPHRASE_TITLE");
            findPassphraseText.text = L10nManager.Localize("UI_LOGIN_FIND_PASSPHRASE");
            backToLoginText.text = L10nManager.Localize("UI_LOGIN_BACK_TO_LOGIN");
            passPhraseText.text = L10nManager.Localize("UI_LOGIN_PASSWORD_INFO");
            retypeText.text = L10nManager.Localize("UI_LOGIN_RETYPE_INFO");
            loginText.text = L10nManager.Localize("UI_LOGIN_INFO");
            enterPrivateKeyText.text = L10nManager.Localize("UI_LOGIN_PRIVATE_KEY_INFO");
            successText.text = L10nManager.Localize("UI_ID_CREATE_SUCCESS");
            passPhraseField.placeholder.GetComponent<Text>().text =
                L10nManager.Localize("UI_LOGIN_INPUT_PASSPHRASE");
            retypeField.placeholder.GetComponent<Text>().text =
                L10nManager.Localize("UI_LOGIN_RETYPE_PASSPHRASE");
            loginField.placeholder.GetComponent<Text>().text =
                L10nManager.Localize("UI_LOGIN_LOGIN");
            findPassphraseField.placeholder.GetComponent<Text>().text =
                L10nManager.Localize("UI_LOGIN_ENTER_PRIVATE_KEY");
            submitButton.Text = L10nManager.Localize("UI_GAME_START");
            submitButton.OnSubmitSubject
                .Subscribe(_ => Submit())
                .AddTo(gameObject);

            base.Awake();
            SubmitWidget = Submit;
        }
        private void SubscribeState(States states)
        {
            titleText.gameObject.SetActive(true);
            contentText.gameObject.SetActive(false);
            passPhraseGroup.SetActive(false);
            retypeGroup.SetActive(false);
            loginGroup.SetActive(false);
            findPassphraseTitle.gameObject.SetActive(false);
            findPassphraseGroup.SetActive(false);
            accountGroup.SetActive(false);
            submitButton.Interactable = false;
            findPassphraseButton.gameObject.SetActive(false);
            backToLoginButton.gameObject.SetActive(false);
            accountAddressText.gameObject.SetActive(false);
            accountAddressHolder.gameObject.SetActive(false);
            accountWarningText.gameObject.SetActive(false);
            retypeText.gameObject.SetActive(false);
            loginWarning.SetActive(false);
            findPrivateKeyWarning.SetActive(false);
            createSuccessGroup.SetActive(false);

            switch (states)
            {
                case States.Show:
                    header.SetActive(true);
                    contentText.gameObject.SetActive(true);
                    incorrectText.gameObject.SetActive(false);
                    correctText.gameObject.SetActive(false);
                    strongText.gameObject.SetActive(false);
                    weakText.gameObject.SetActive(false);
                    accountGroup.SetActive(true);
                    accountAddressHolder.gameObject.SetActive(true);
                    passPhraseField.text = "";
                    retypeField.text = "";
                    loginField.text = "";
                    findPassphraseField.text = "";
                    submitButton.Text = L10nManager.Localize("UI_GAME_SIGN_UP");
                    bg.SetActive(false);
                    break;
                case States.CreatePassword:
                    titleText.gameObject.SetActive(false);
                    accountAddressText.gameObject.SetActive(true);
                    submitButton.Text = L10nManager.Localize("UI_GAME_START");
                    passPhraseGroup.SetActive(true);
                    retypeGroup.SetActive(true);
                    accountGroup.SetActive(true);
                    passPhraseField.Select();
                    break;
                case States.CreateAccount:
                    titleText.gameObject.SetActive(false);
                    submitButton.Text = L10nManager.Localize("UI_GAME_CREATE_PASSWORD");
                    createSuccessGroup.SetActive(true);
                    passPhraseField.Select();
                    break;
                case States.ResetPassphrase:
                    titleText.gameObject.SetActive(false);
                    submitButton.Text = L10nManager.Localize("UI_GAME_START");
                    passPhraseGroup.SetActive(true);
                    retypeGroup.SetActive(true);
                    accountGroup.SetActive(true);
                    passPhraseField.Select();
                    break;
                case States.Login:
                    header.SetActive(false);
                    titleText.gameObject.SetActive(false);
                    submitButton.Text = L10nManager.Localize("UI_GAME_START");
                    loginGroup.SetActive(true);
                    accountGroup.SetActive(true);
                    findPassphraseButton.gameObject.SetActive(true);
                    loginField.Select();
                    accountAddressText.gameObject.SetActive(true);
                    bg.SetActive(true);
                    break;
                case States.FindPassphrase:
                    titleText.gameObject.SetActive(false);
                    findPassphraseTitle.gameObject.SetActive(true);
                    findPassphraseGroup.SetActive(true);
                    backToLoginButton.gameObject.SetActive(true);
                    submitButton.Text = L10nManager.Localize("UI_OK");
                    findPassphraseField.Select();
                    break;
                case States.Failed:
                    var upper = _prevState.ToString().ToUpper();
                    var format = L10nManager.Localize($"UI_LOGIN_{upper}_FAIL");
                    titleText.text = string.Format(format, _prevState);
                    contentText.gameObject.SetActive(true);
                    var contentFormat = L10nManager.Localize($"UI_LOGIN_{upper}_CONTENT");
                    contentText.text = string.Format(contentFormat);
                    submitButton.Text = L10nManager.Localize("UI_OK");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(states), states, null);
            }
            UpdateSubmitButton();
        }

        public void CheckPassphrase()
        {
            var text = passPhraseField.text;
            var strong = CheckPassWord(text);
            strongText.gameObject.SetActive(strong);
            weakText.gameObject.SetActive(!strong);
            passPhraseText.gameObject.SetActive(!strong);
            retypeField.interactable = strong;
        }

        private static bool CheckPassWord(string text)
        {
            var result = Zxcvbn.Zxcvbn.MatchPassword(text);
            return result.Score >= 2;
        }

        public void CheckRetypePassphrase()
        {
            submitButton.UpdateObjects();
            var vaild = submitButton.IsSubmittable;
            correctText.gameObject.SetActive(vaild);
            incorrectText.gameObject.SetActive(!vaild);
            retypeText.gameObject.SetActive(!vaild);
        }

        private bool CheckPasswordVaildInCreate()
        {
            var passPhrase = passPhraseField.text;
            var retyped = retypeField.text;
            return !(string.IsNullOrEmpty(passPhrase) || string.IsNullOrEmpty(retyped)) &&
                passPhrase == retyped &&
                CheckPassWord(passPhrase);
        }

        private void CheckLogin()
        {
            
        }

        public void Submit()
        {
            if (!submitButton.IsSubmittable)
            {
                return;
            }

            submitButton.Interactable = false;
            switch (State.Value)
            {
                case States.Show:
                    SetState(States.CreateAccount);
                    break;
                case States.CreateAccount:
                    SetState(States.CreatePassword);
                    break;
                case States.CreatePassword:
                    Close();
                    break;
                case States.Login:
                    CheckLogin();
                    break;
                case States.FindPassphrase:
                {
                    if (CheckPrivateKeyHex())
                    {
                        SetState(States.ResetPassphrase);
                    }
                    else
                    {
                        findPrivateKeyWarning.SetActive(true);
                        findPassphraseField.text = null;
                    }
                    break;
                }
                case States.ResetPassphrase:
                    ResetPassphrase();
                    Close();
                    break;
                case States.Failed:
                    SetState(_prevState);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void FindPassphrase()
        {
            SetState(States.FindPassphrase);
        }

        public void BackToLogin()
        {
            SetState(States.Login);
        }

        public void Show(string path, string privateKeyString)
        {
            
        }

        private void UpdateSubmitButton()
        {
            submitButton.Interactable = true;

            switch (State.Value)
            {
                case States.ResetPassphrase:
                case States.CreatePassword:
                    submitButton.Interactable = CheckPasswordVaildInCreate();
                    break;
                case States.Login:
                    submitButton.Interactable = !string.IsNullOrEmpty(loginField.text);
                    break;
                case States.FindPassphrase:
                    submitButton.Interactable = !string.IsNullOrEmpty(findPassphraseField.text);
                    break;
                case States.CreateAccount:
                case States.Show:
                    submitButton.Interactable = true;
                    break;
                case States.Failed:
                    break;
            }
        }

        protected override void Update()
        {
            base.Update();

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                switch (State.Value)
                {
                    case States.ResetPassphrase:
                    case States.CreatePassword:
                    {
                        {
                            if (passPhraseField.isFocused)
                            {
                                retypeField.Select();
                            }
                            else
                            {
                                passPhraseField.Select();
                            }
                        }
                        break;
                    }
                    case States.Login:
                        loginField.Select();
                        break;
                    case States.FindPassphrase:
                        findPassphraseField.Select();
                        break;
                    case States.CreateAccount:
                    case States.Show:
                    case States.Failed:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            UpdateSubmitButton();
        }

        private bool CheckPrivateKeyHex()
        {
            return false;
        }

        private void ResetPassphrase()
        {
        }

        private void SetState(States states)
        {
            _prevState = State.Value;
            State.Value = states;
        }

        public override void Close(bool ignoreCloseAnimation = false)
        {
            blur?.Close();
            base.Close(ignoreCloseAnimation);
        }
    }
}
