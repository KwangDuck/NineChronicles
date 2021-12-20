using Nekoyume.Model.Item;

namespace Nekoyume.Model.State
{
    public class CombinationSlotState : State
    {
        public long UnlockBlockIndex { get; private set; }
        public int UnlockStage { get; private set; }
        public long StartBlockIndex { get; private set; }
        public long RequiredBlockIndex => UnlockBlockIndex - StartBlockIndex;

        public bool Validate(AvatarState avatarState, long blockIndex)
        {
            if (avatarState is null)
            {
                return false;
            }

            return avatarState.worldInformation != null &&
                   avatarState.worldInformation.IsStageCleared(UnlockStage) &&
                   blockIndex >= UnlockBlockIndex;
        }


        public void Update(long blockIndex)
        {
            UnlockBlockIndex = blockIndex;
        }

        public void Update(long blockIndex, Material material, int count)
        {
            Update(blockIndex);
        }
    }
}
