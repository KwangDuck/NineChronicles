using Nekoyume.Model.State;
using Nekoyume.State;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using Nekoyume.Battle;
using System.Threading.Tasks;
using Libplanet;

using Debug = UnityEngine.Debug;
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
            //var apiClient = Game.Game.instance.ApiClient;

            //if (apiClient.IsInitialized)
            //{
            //    return Task.Run(async () =>
            //    {
            //        if (!_rankingMapLoaded)
            //        {
            //            for (var i = 0; i < RankingState.RankingMapCapacity; ++i)
            //            {
            //                var address = RankingState.Derive(i);
            //                var mapState =
            //                    await Game.Game.instance.Agent.GetStateAsync(address) is
            //                        Bencodex.Types.Dictionary serialized
            //                    ? new RankingMapState(serialized)
            //                    : new RankingMapState(address);
            //                States.Instance.SetRankingMapStates(mapState);
            //            }

            //            var rankingMapStates = States.Instance.RankingMapStates;
            //            _rankingInfoSet = new HashSet<Nekoyume.Model.State.RankingInfo>();
            //            foreach (var pair in rankingMapStates)
            //            {
            //                var rankingInfo = pair.Value.GetRankingInfos(null);
            //                _rankingInfoSet.UnionWith(rankingInfo);
            //            }

            //            _rankingMapLoaded = true;
            //        }

            //        Debug.LogWarning($"total user count : {_rankingInfoSet.Count()}");

            //        var sw = new Stopwatch();
            //        sw.Start();

            //        await Task.WhenAll(
            //            LoadAbilityRankingInfos(displayCount),
            //            LoadStageRankingInfos(apiClient, displayCount),
            //            LoadMimisbrunnrRankingInfos(apiClient, displayCount),
            //            LoadCraftRankingInfos(apiClient, displayCount),
            //            LoadEquipmentRankingInfos(apiClient, displayCount)
            //        );
            //        IsInitialized = true;
            //        sw.Stop();
            //        UnityEngine.Debug.LogWarning($"total elapsed : {sw.Elapsed}");
            //    });
            //}

            return Task.CompletedTask;
        }
    }
}
