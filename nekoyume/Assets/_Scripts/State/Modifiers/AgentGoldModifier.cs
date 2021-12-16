using System;
using System.Linq;
using Nekoyume.Model.State;
using UnityEngine;

namespace Nekoyume.State.Modifiers
{
    [Serializable]
    public class AgentGoldModifier : IAccumulatableStateModifier<GoldBalanceState>
    {
        [SerializeField] private string hex;

        public bool dirty { get; set; }

        public bool IsEmpty => false;

        public void Add(IAccumulatableStateModifier<GoldBalanceState> modifier)
        {
            if (!(modifier is AgentGoldModifier m))
            {
                return;
            }
        }

        public void Remove(IAccumulatableStateModifier<GoldBalanceState> modifier)
        {
            if (!(modifier is AgentGoldModifier m))
            {
                return;
            }
        }

        public GoldBalanceState Modify(GoldBalanceState state)
        {
            return state;

            // return state?.Add(Gold);
        }
    }
}
