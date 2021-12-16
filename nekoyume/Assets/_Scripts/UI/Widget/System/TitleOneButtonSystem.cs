using Nekoyume.EnumType;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Collections.Generic;

namespace Nekoyume.UI
{
    public class TitleOneButtonSystem : Alert
    {
        public override WidgetType WidgetType => WidgetType.System;

        [SerializeField]
        private Button bgImageButton = null;

        protected override void Awake()
        {
            base.Awake();
            bgImageButton.OnClickAsObservable()
                .Subscribe(_ => Close())
                .AddTo(gameObject);
        }

        public override void Show(string title, string content, string labelOK = "UI_OK", bool localize = true)
        {
            base.Show(title, content, labelOK, localize);
        }

        public void ShowAndQuit(string title, string content, string labelOK = "UI_OK", bool localize = true)
        {
#if UNITY_EDITOR
            CloseCallback = UnityEditor.EditorApplication.ExitPlaymode;
#else
            CloseCallback = UnityEngine.Application.Quit;
#endif
            Show(title, content, labelOK, localize);
        }
    }
}
