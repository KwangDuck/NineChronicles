using Nekoyume.Game.Factory;
using Nekoyume.Helper;
using Nekoyume.State;
using UnityEngine;
using UnityEngine.Video;

namespace Nekoyume.UI
{
    public class PreloadingScreen : LoadingScreen
    {
        [SerializeField]
        private VideoPlayer videoPlayer;

        [SerializeField]
        private VideoClip showClip;

        [SerializeField]
        private VideoClip loopClip;

        protected override void Awake()
        {
            base.Awake();
            indicator.Close();
        #if UNITY_EDITOR
            videoPlayer.clip = showClip;
            videoPlayer.Prepare();
        #endif
        }

        public override void Show(bool ignoreShowAnimation = false)
        {
            base.Show(ignoreShowAnimation);
            if (!string.IsNullOrEmpty(Message))
            {
                indicator.Show(Message);
            }
        #if UNITY_EDITOR
            videoPlayer.Play();
            videoPlayer.loopPointReached += OnShowVideoEnded;
        #endif
        }

        public override async void Close(bool ignoreCloseAnimation = false)
        {            
        #if UNITY_EDITOR
            videoPlayer.Stop();        

            if (!GameConfig.IsEditor)
            {
                Find<Synopsis>().Show();
            }
            else
        #endif
            {
                PlayerFactory.Create();

                // current avatar state
                var avatarState = States.Instance.CurrentAvatarState;
                if (avatarState == null)
                {                    
                    EnterLogin();
                }
                else
                {
                    if (avatarState.inventory == null ||
                        avatarState.questList == null ||
                        avatarState.worldInformation == null)
                    {                        
                        EnterLogin();
                    }
                    else
                    {                        
                        Game.Event.OnRoomEnter.Invoke(false);
                    }
                }
            }
            
            base.Close(ignoreCloseAnimation);
            indicator.Close();
        }

        private static void EnterLogin()
        {
            Find<Login>().Show();
            Game.Event.OnNestEnter.Invoke();
        }

        private void OnShowVideoEnded(VideoPlayer player)
        {
            player.loopPointReached -= OnShowVideoEnded;
            videoPlayer.clip = loopClip;
            player.isLooping = true;
            videoPlayer.Play();
        }
    }
}
