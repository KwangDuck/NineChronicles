using System;
using System.Collections;
using Lib9c.Renderer;
using Nekoyume.Helper;
using Nekoyume.Model.State;
using Nekoyume.State;
using UnityEngine;

namespace Nekoyume.BlockChain
{
    public class DummyAgent : MonoBehaviour, IDisposable, IAgent
    {
        public ActionRenderer ActionRenderer { get; private set; } = new ActionRenderer();

        public int AppProtocolVersion { get; private set; }

        public void Dispose()
        {
            ActionRenderHandler.Instance.Stop();
            ActionUnrenderHandler.Instance.Stop();
        }

        public IEnumerator Initialize(CommandLineOptions options, Action<bool> callback)
        {
            yield return new WaitForEndOfFrame();

            // assign appProtocolVersion
            var appProtocolVersion = options.AppProtocolVersion is null
                ? default
                : Libplanet.Net.AppProtocolVersion.FromToken(options.AppProtocolVersion);
            AppProtocolVersion = appProtocolVersion.Version;

            // set state config state
            States.Instance.SetGameConfigState(new GameConfigState());

            // register block/action renderer
            ActionRenderHandler.Instance.Start(ActionRenderer);
            ActionUnrenderHandler.Instance.Start(ActionRenderer);            

            callback?.Invoke(true);
        }
    }
}
