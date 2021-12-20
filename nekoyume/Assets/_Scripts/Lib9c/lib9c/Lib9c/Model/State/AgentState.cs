using System;
using System.Collections.Generic;

namespace Nekoyume.Model.State
{
    /// <summary>
    /// Agent의 상태 모델이다.
    /// </summary>
    [Serializable]
    public class AgentState : State, ICloneable
    {
        public HashSet<int> unlockedOptions;
        public int MonsterCollectionRound { get; private set; }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public void IncreaseCollectionRound()
        {
            MonsterCollectionRound++;
        }
    }
}
