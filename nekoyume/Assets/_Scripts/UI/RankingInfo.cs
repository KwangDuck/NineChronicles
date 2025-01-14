using System;
using System.Collections.Generic;
using Nekoyume.Helper;
using Nekoyume.L10n;
using Nekoyume.Model.State;
using Nekoyume.State;
using Nekoyume.State.Subjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Nekoyume.UI
{
    using UniRx;

    public class RankingInfo : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI winCount = null;

        [SerializeField]
        private TextMeshProUGUI loseCount = null;

        [SerializeField]
        private TextMeshProUGUI remainTime = null;

        [SerializeField]
        private TextMeshProUGUI remainTitle = null;

        [SerializeField]
        private Slider remainTimeSlider = null;

        private long _resetIndex;

        private readonly List<IDisposable> _disposablesFromOnEnable = new List<IDisposable>();

        private void Awake()
        {
            remainTimeSlider.OnValueChangedAsObservable()
                .Subscribe(OnSliderChange)
                .AddTo(gameObject);
            remainTimeSlider.maxValue = States.Instance.GameConfigState.DailyArenaInterval;
            remainTimeSlider.value = 0;
        }

        private void OnEnable()
        {
            WeeklyArenaStateSubject.WeeklyArenaState
                .Subscribe(SetWeeklyArenaState)
                .AddTo(_disposablesFromOnEnable);

            var weeklyArenaState = States.Instance.WeeklyArenaState;
            SetWeeklyArenaState(weeklyArenaState);
        }

        private void OnDisable()
        {
            _disposablesFromOnEnable.DisposeAllAndClear();
        }

        private void SetBlockIndex(long blockIndex)
        {
            remainTimeSlider.value = blockIndex - _resetIndex;
        }

        private void SetWeeklyArenaState(WeeklyArenaState weeklyArenaState)
        {
            remainTimeSlider.value = 0 - _resetIndex;
            UpdateArenaInfo(weeklyArenaState);
        }

        private void UpdateArenaInfo(WeeklyArenaState weeklyArenaState)
        {   
        }

        private void OnSliderChange(float value)
        {
            var gameConfigState = States.Instance.GameConfigState;
            var remainBlock = gameConfigState.DailyArenaInterval - value;
            var time = Util.GetBlockToTime((int)remainBlock);
            remainTitle.text = L10nManager.Localize("UI_REMAINING_TIME_ONLY");
            remainTime.text = string.Format(
                L10nManager.Localize("UI_ABOUT"),
                time,
                (int) value, gameConfigState.DailyArenaInterval);
        }
    }
}
