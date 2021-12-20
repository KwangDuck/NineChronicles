using System;

namespace Nekoyume.Model.State
{
    [Serializable]
    public class AdminState : State
    {
        public long ValidUntil { get; private set; }
    }
}
