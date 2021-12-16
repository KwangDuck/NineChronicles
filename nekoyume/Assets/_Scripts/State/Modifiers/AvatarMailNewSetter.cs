using System;
using Nekoyume.Model.State;

namespace Nekoyume.State.Modifiers
{
    [Serializable]
    public class AvatarMailNewSetter : AvatarStateModifier
    {
        public override bool IsEmpty => false;

        public AvatarMailNewSetter(params Guid[] guidParams)
        {
        }

        public override void Add(IAccumulatableStateModifier<AvatarState> modifier)
        {
            if (!(modifier is AvatarMailNewSetter m))
            {
                return;
            }
        }

        public override void Remove(IAccumulatableStateModifier<AvatarState> modifier)
        {
            if (!(modifier is AvatarMailNewSetter m))
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
