using System;
using System.Collections;
using Lib9c.Renderer;
using Nekoyume.Helper;

namespace Nekoyume.BlockChain
{
    public interface IAgent
    {
        ActionRenderer ActionRenderer { get; }
        int AppProtocolVersion { get; }
        IEnumerator Initialize(CommandLineOptions options, Action<bool> callback);        
    }
}
