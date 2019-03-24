using System.Collections;
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
            var stage = GetComponent<Stage>();
            var blind = UI.Widget.Find<UI.Blind>();
            yield return StartCoroutine(blind.FadeIn(1.0f, "ROOM"));

            UI.Widget.Find<UI.Menu>().ShowRoom();

            stage.id = 0;
            stage.LoadBackground("room");

            var objectPool = GetComponent<Util.ObjectPool>();
            var players = stage.GetComponentsInChildren<Character.Player>();
            foreach (var p in players)
            {
                objectPool.Remove<Character.Player>(p.gameObject);
            }
            objectPool.ReleaseAll();

            var boss = stage.GetComponentInChildren<Character.Boss.BossBase>();
            if (boss != null)
            {
                Destroy(boss.gameObject);
            }

            var playerFactory = GetComponent<Factory.PlayerFactory>();
            GameObject player = playerFactory.Create();
            player.transform.position = new Vector2(-2.47f, -1.37f);

            UI.Widget.Find<UI.SkillController>().Close();
            UI.Widget.Find<UI.Status>().UpdatePlayer(player);

            var cam = Camera.main.gameObject.GetComponent<ActionCamera>();
            var camPos = cam.transform.position;
            camPos.x = 0.0f;
            camPos.y = 0.0f;
            cam.transform.position = camPos;
            cam.target = null;

            yield return new WaitForSeconds(2.0f);
            yield return StartCoroutine(blind.FadeOut(1.0f));
            Destroy(this);
        }
    }
}
