using System.Collections.Generic;
using System.Linq;

namespace Nekoyume.Model.State
{
    public class RankingMapState : State
    {
        public const int Capacity = 500;

        public List<RankingInfo> GetRankingInfos(long? blockOffset)
        {
            return new List<RankingInfo>();
        }
    }

    public class RankingInfo : IState
    {
        public readonly int ArmorId;
        public readonly int Level;
        public readonly string AvatarName;
        public readonly long Exp;
        public readonly long StageClearedBlockIndex;
        public readonly long UpdatedAt;
    }
}
