using System.Collections;
using Nekoyume.BlockChain;
using Nekoyume.UI;
using Nekoyume.UI.Module;
using UnityEngine;

namespace Nekoyume.Game.Entrance
{
    public class RoomEntering : MonoBehaviour
    {
        private void Start()
        {
            StartCoroutine(Act());
        }

        private IEnumerator Act()
        {
            var stage = Game.instance.Stage;
            if (stage.showLoadingScreen)
            {
                Widget.Find<LoadingScreen>().Show();
            }

            Widget.Find<BottomMenu>().Close(true);

            stage.stageId = 0;
            stage.LoadBackground("room");
            stage.roomAnimator.Play("EnteringRoom");

            yield return new WaitForEndOfFrame();
            stage.selectedPlayer = null;
            if (!(stage.AvatarState is null))
            {
                ActionRenderHandler.Instance.UpdateCurrentAvatarState(stage.AvatarState);
            }

            var player = stage.GetPlayer(stage.roomPosition - new Vector2(3.0f, 0.0f));
            player.StartRun();

            var status = Widget.Find<Status>();
            status.UpdatePlayer(player);
            status.Close(true);

            ActionCamera.instance.SetPosition(0f, 0f);
            ActionCamera.instance.Idle();

            yield return new WaitForSeconds(1.0f);
            Widget.Find<LoadingScreen>().Close();

            if (player)
            {
                yield return new WaitWhile(() => player.transform.position.x < stage.roomPosition.x);
            }

            player.RunSpeed = 0.0f;
            player.Animator.Idle();

            Widget.Find<Status>().Show();
            Widget.Find<BottomMenu>().Show(
                UINavigator.NavigationType.Quit,
                _ => Game.Quit(),
                false,
                BottomMenu.ToggleableType.Mail,
                BottomMenu.ToggleableType.Quest,
                BottomMenu.ToggleableType.Chat,
                BottomMenu.ToggleableType.IllustratedBook,
                BottomMenu.ToggleableType.Character,
                BottomMenu.ToggleableType.Settings,
                BottomMenu.ToggleableType.Combination
            );

            Destroy(this);
        }
    }
}
