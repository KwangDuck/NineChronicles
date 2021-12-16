using Nekoyume.Model.State;

namespace Nekoyume.State.Modifiers
{
    public class CombinationSlotBlockIndexAndResultModifier : CombinationSlotStateModifier
    {
        private readonly long _unlockBlockIndex;
        private readonly long _blockIndex;

        public override bool IsEmpty => false;

        public CombinationSlotBlockIndexAndResultModifier(
            long blockIndex,
            long unlockBlockIndex
        )
        {
            _unlockBlockIndex = unlockBlockIndex;
            _blockIndex = blockIndex;
        }

        public override CombinationSlotState Modify(CombinationSlotState state)
        {
            return state;
        }
    }
}
