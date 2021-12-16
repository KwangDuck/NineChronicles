using System;
using Nekoyume.Model.State;

namespace Nekoyume.State.Modifiers
{
    [Serializable]
    public class AvatarInventoryItemEquippedModifier : AvatarStateModifier
    {
        public override bool IsEmpty => false;

        public AvatarInventoryItemEquippedModifier(Guid nonFungibleId, bool equipped)
        {            
        }

        public override void Add(IAccumulatableStateModifier<AvatarState> modifier)
        {
            if (!(modifier is AvatarInventoryItemEquippedModifier m))
            {
                return;
            }
        }

        public override void Remove(IAccumulatableStateModifier<AvatarState> modifier)
        {
            if (!(modifier is AvatarInventoryItemEquippedModifier m))
            {
                return;
            }
        }

        public override AvatarState Modify(AvatarState state)
        {            
            return state;
        }
    }
}
