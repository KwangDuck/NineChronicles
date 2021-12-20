using System;

namespace Nekoyume.Model.State
{
    [Serializable]
    public class GoldBalanceState : State, ICloneable
    {
        
        public object Clone() => MemberwiseClone();
    }
}
