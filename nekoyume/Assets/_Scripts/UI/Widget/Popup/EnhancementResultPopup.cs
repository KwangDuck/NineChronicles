using System;
using System.Collections;
using System.Collections.Generic;
using Nekoyume.Game.Controller;
using Nekoyume.Model.Mail;
using Nekoyume.UI.Module;
using TMPro;
using UnityEngine;

namespace Nekoyume.UI
{
    using UniRx;

    public class EnhancementResultPopup : PopupWidget
    {
        [Serializable]
        public class ResultItem
        {
            public SimpleItemView itemView;
            public TextMeshProUGUI itemNameText;
            public TextMeshProUGUI beforeGradeText;
            public TextMeshProUGUI afterGradeText;
            public TextMeshProUGUI cpText;
        }

        [SerializeField]
        private GameObject _titleFailSuccessObject;

        [SerializeField]
        private GameObject _titleSuccessObject;

        [SerializeField]
        private GameObject _titleGreatSuccessObject;

        [SerializeField]
        private ResultItem _resultItem;

        [SerializeField]
        private ItemOptionView _itemMainStatView;

        [SerializeField]
        private List<ItemOptionWithCountView> _itemStatOptionViews;

        [SerializeField]
        private List<ItemOptionView> _itemSkillOptionViews;

        [SerializeField]
        private float _delayTimeOfShowOptions;

        [SerializeField]
        private float _intervalTimeOfShowOptions;

        private static readonly int AnimatorHashGreatSuccess = Animator.StringToHash("GreatSuccess");
        private static readonly int AnimatorHashSuccess = Animator.StringToHash("Success");
        private static readonly int AnimatorHashFail = Animator.StringToHash("Fail");
        private static readonly int AnimatorHashLoop = Animator.StringToHash("Loop");
        private static readonly int AnimatorHashLoopFail = Animator.StringToHash("Loop_Fail");
        private static readonly int AnimatorHashClose = Animator.StringToHash("Close");

        private IDisposable _disposableOfSkip;
        private Coroutine _coroutineOfPlayOptionAnimation;

        protected override void OnDisable()
        {
            _disposableOfSkip?.Dispose();
            _disposableOfSkip = null;

            base.OnDisable();
        }

        public void Show(ItemEnhanceMail mail)
        {
            
        }

        #region Invoke from Animation

        public void OnAnimatorStateBeginning(string stateName)
        {
            switch (stateName)
            {
                case "Show":
                case "GreatSuccess":
                case "Success":
                case "Fail":
                    _disposableOfSkip ??= Observable.EveryUpdate()
                        .Where(_ => Input.GetMouseButtonDown(0) ||
                                    Input.GetKeyDown(KeyCode.Return) ||
                                    Input.GetKeyDown(KeyCode.KeypadEnter) ||
                                    Input.GetKeyDown(KeyCode.Escape))
                        .Take(1)
                        .DoOnCompleted(() => _disposableOfSkip = null)
                        .Subscribe(_ =>
                        {
                            AudioController.PlayClick();
                            SkipAnimation();
                        });
                    break;
            }
        }

        public void OnAnimatorStateEnd(string stateName)
        {
            switch (stateName)
            {
                case "Close":
                    base.Close(true);
                    break;
            }
        }

        public void OnRequestPlaySFX(string sfxCode) =>
            AudioController.instance.PlaySfx(sfxCode);

        public void PlayOptionAnimation()
        {
            if (_coroutineOfPlayOptionAnimation != null)
            {
                StopCoroutine(_coroutineOfPlayOptionAnimation);
            }

            _coroutineOfPlayOptionAnimation = StartCoroutine(CoPlayOptionAnimation());
        }

        #endregion

        private void SkipAnimation()
        {
            if (_disposableOfSkip != null)
            {
                _disposableOfSkip.Dispose();
                _disposableOfSkip = null;
            }

            if (_coroutineOfPlayOptionAnimation != null)
            {
                StopCoroutine(_coroutineOfPlayOptionAnimation);
                _coroutineOfPlayOptionAnimation = null;
            }

            var animatorStateInfo = Animator.GetCurrentAnimatorStateInfo(0);
            if (animatorStateInfo.shortNameHash == AnimatorHashGreatSuccess ||
                animatorStateInfo.shortNameHash == AnimatorHashSuccess)
            {
                Animator.Play(AnimatorHashLoop, 0, 0);
            }
            else if (animatorStateInfo.shortNameHash == AnimatorHashFail)
            {
                Animator.Play(AnimatorHashLoopFail, 0, 0);
            }

            for (var i = 0; i < _itemStatOptionViews.Count; i++)
            {
                var optionView = _itemStatOptionViews[i];
                if (optionView.IsEmpty)
                {
                    continue;
                }

                optionView.Show(true);
            }

            for (var i = 0; i < _itemSkillOptionViews.Count; i++)
            {
                var optionView = _itemSkillOptionViews[i];
                if (optionView.IsEmpty)
                {
                    continue;
                }

                optionView.Show(true);
            }

            PressToContinue();
        }

        private IEnumerator CoPlayOptionAnimation()
        {
            yield return new WaitForSeconds(_delayTimeOfShowOptions);

            for (var i = 0; i < _itemStatOptionViews.Count; i++)
            {
                var optionView = _itemStatOptionViews[i];
                if (optionView.IsEmpty)
                {
                    continue;
                }

                yield return new WaitForSeconds(_intervalTimeOfShowOptions);
                optionView.Show();
            }

            for (var i = 0; i < _itemSkillOptionViews.Count; i++)
            {
                var optionView = _itemSkillOptionViews[i];
                if (optionView.IsEmpty)
                {
                    continue;
                }

                yield return new WaitForSeconds(_intervalTimeOfShowOptions);
                optionView.Show();
            }

            yield return null;

            _coroutineOfPlayOptionAnimation = null;

            if (_disposableOfSkip != null)
            {
                _disposableOfSkip.Dispose();
                _disposableOfSkip = null;
            }

            PressToContinue();
        }

        private void PressToContinue() => Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0) ||
                        Input.GetKeyDown(KeyCode.Return) ||
                        Input.GetKeyDown(KeyCode.KeypadEnter) ||
                        Input.GetKeyDown(KeyCode.Escape))
            .First()
            .Subscribe(_ =>
            {
                AudioController.PlayClick();
                Animator.SetTrigger(AnimatorHashClose);
            });
    }
}
