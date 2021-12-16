using System.Collections.Generic;
using System.Threading.Tasks;
using Nekoyume.Model.Item;

namespace Nekoyume.UI.Model
{
    public class Rank
    {
        public bool IsInitialized { get; private set; } = false;

        public List<AbilityRankingModel> AbilityRankingInfos = null;

        public List<StageRankingModel> StageRankingInfos = null;

        public List<StageRankingModel> MimisbrunnrRankingInfos = null;

        public List<CraftRankingModel> CraftRankingInfos = null;

        public Dictionary<ItemSubType, List<EquipmentRankingModel>> EquipmentRankingInfosMap = null;

        public Dictionary<int, AbilityRankingModel> AgentAbilityRankingInfos = new Dictionary<int, AbilityRankingModel>();

        public Dictionary<int, StageRankingModel> AgentStageRankingInfos = new Dictionary<int, StageRankingModel>();

        public Dictionary<int, StageRankingModel> AgentMimisbrunnrRankingInfos = new Dictionary<int, StageRankingModel>();

        public Dictionary<int, CraftRankingModel> AgentCraftRankingInfos = new Dictionary<int, CraftRankingModel>();

        public Dictionary<int, Dictionary<ItemSubType, EquipmentRankingModel>> AgentEquipmentRankingInfos =
            new Dictionary<int, Dictionary<ItemSubType, EquipmentRankingModel>>();

        private HashSet<Nekoyume.Model.State.RankingInfo> _rankingInfoSet = null;

        private bool _rankingMapLoaded;

        public Task Update(int displayCount)
        {
            return Task.CompletedTask;
        }
    }
}
