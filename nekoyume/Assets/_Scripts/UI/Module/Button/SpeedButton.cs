using System;

using UnityEngine;
using UnityEngine.UI;

using TMPro;

namespace Nekoyume.UI.Module
{
    using Game;
    using UniRx;

    public class SpeedButton : MonoBehaviour
    {
        [SerializeField]
        private Button button = null;
        [SerializeField]
        private GameObject onObject;
        [SerializeField]
        private GameObject offObject;        
        [SerializeField]
        private TextMeshProUGUI speedText;

        private int speedValue;
        public int SpeedValue
        {
            get => speedValue;
            set
            {                
                speedValue = value;
                
                speedText.text = $"X{value}";

                onObject.SetActive(value != Stage.DefaultSpeed);
                offObject.SetActive(value == Stage.DefaultSpeed);
            }
        }

        private void Awake()
        {
            SpeedValue = Stage.DefaultSpeed;
        }

        public void SetButtonAction(Stage stage)
        {            
            button.onClick.AddListener(() => 
            {
                int nextSpeed = SpeedValue * 2;
                
                if(nextSpeed > Stage.MaxSpeed)
                    nextSpeed = Stage.DefaultSpeed;

                stage.StageSpeed = nextSpeed;
            });

            stage.StageSpeedObservable.Where(_ => stage.IsPlayStage).Subscribe(speed => SpeedValue = speed).AddTo(this.gameObject);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
