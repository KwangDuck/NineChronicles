using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Nekoyume.Action;
using Nekoyume.Game.Character;
using Nekoyume.Model.Item;
using Nekoyume.State;
using Nekoyume.Model.State;
using Nekoyume.UI;
using Material = Nekoyume.Model.Item.Material;

namespace Nekoyume.BlockChain
{
    using Gateway.Protocol;
    using UniRx;

    /// <summary>
    /// Creates an action of the game and puts it in the agent.
    /// </summary>
    public class ActionManager : IDisposable
    {
        private static readonly TimeSpan ActionTimeout = TimeSpan.FromSeconds(360f);        
        private readonly IAgent _agent;
        private Guid _lastBattleActionId;

        public static ActionManager Instance => Game.Game.instance.ActionManager;
        public static bool IsLastBattleActionId(Guid actionId) => actionId == Instance._lastBattleActionId;

        public ActionManager(IAgent agent)
        {
            _agent = agent ?? throw new ArgumentNullException(nameof(agent));
        }

        public void Dispose()
        {

        }

        private REQ_Header MakeHeader()
        {
            return new REQ_Header
            {

            };
        }

        ///////////////////////////////////////////////////////////////////////////
        // login
        public IObservable<(REQ_Login, RES_Login)> LoginAsync()
        {
            var req = new REQ_Login
            {
                Header = MakeHeader(),
            };

            // remote request

            var res = new RES_Login
            {

            };

            return Observable.Return((req, res));
        }

        // logout
        public IObservable<(REQ_Logout, RES_Logout)> LogoutAsync()
        {
            var req = new REQ_Logout
            {
                Header = MakeHeader(),
            };

            // remote quest

            var res = new RES_Logout{

            };

            return Observable.Return((req, res));
        }

        // create avatar
        public IObservable<(REQ_CreateAvatar, RES_CreateAvatar)> CreateAvatarAsync(int index,
            string nickName, int hair = 0, int lens = 0, int ear = 0, int tail = 0)
        {

            // init dummy avatar
            var avatarState = new AvatarState(
                new Libplanet.Address(),
                new Libplanet.Address(),
                0,
                Game.Game.instance.TableSheets.GetAvatarSheets(),
                States.Instance.GameConfigState,
                new Libplanet.Address()
            );
            States.Instance.UpdateCurrentAvatarState(avatarState);
            States.Instance.AddOrReplaceAvatarState(avatarState, index);
            States.Instance.SelectAvatar(index);

            var req = new REQ_CreateAvatar
            {
                Header = MakeHeader(),
                Index = index,
                Avatar = new ST_Avatar
                {
                    Hair = hair,
                    Lens = lens,
                    Ear = ear,
                    Tail = tail,
                    Nickname = nickName
                }
            };

            // remote request

            var res = new RES_CreateAvatar
            {

            };

            return Observable.Return((req, res));
        }

        // retrieve all master data
        public IObservable<(REQ_RetrieveAllMasterData, RES_RetrieveAllMasterData)> RetrieveAllMasterDataAsync()
        {
            var req = new REQ_RetrieveAllMasterData
            {
                Header = MakeHeader(),
            };

            // remote request

            var res = new RES_RetrieveAllMasterData
            {

            };

            return Observable.Return((req, res));
        }

        // retrieve master data
        public IObservable<(REQ_RetrieveMasterData, RES_RetrieveMasterData)> RetrieveMasterDataAsync()
        {
            var req = new REQ_RetrieveMasterData
            {
                Header = MakeHeader(),
            };

            // remote request

            var res = new RES_RetrieveMasterData
            {

            };

            return Observable.Return((req, res));
        }

        ///////////////////////////////////////////////////////////////////////////
        // enter lobby
        public IObservable<(REQ_EnterLobbyEntryPoint, RES_EnterLobbyEntryPoint)> EnterLobbyEntryPointAsync()
        {
            var req = new REQ_EnterLobbyEntryPoint
            {
                Header = MakeHeader(),
            };

            // remote request

            var res = new RES_EnterLobbyEntryPoint
            {

            };

            return Observable.Return((req, res));
        }

        // daily reward
        public IObservable<(REQ_DailyReward, RES_DailyReward)> DailyRewardAsync()
        {
            var req = new REQ_DailyReward
            {
                Header = MakeHeader(),
            };

            // remote request

            var res = new RES_DailyReward
            {

            };

            return Observable.Return((req, res));
        }

        // charge action point
        public IObservable<(REQ_ChargeActionPoint, RES_ChargeActionPoint)> ChargeActionPointAsync(Material material)
        {
            var avatarAddress = States.Instance.CurrentAvatarState.address;

            LocalLayerModifier.RemoveItem(avatarAddress, material.ItemId);
            LocalLayerModifier.ModifyAvatarActionPoint(avatarAddress, States.Instance.GameConfigState.ActionPointMax);

            var req = new REQ_ChargeActionPoint
            {
                Header = MakeHeader(),
            };

            // remote request

            var res = new RES_ChargeActionPoint
            {

            };

            return Observable.Return((req, res));
        }

        ///////////////////////////////////////////////////////////////////////////
        // enter workshop entry point
        public IObservable<(REQ_EnterWorkshopEntryPoint, RES_EnterWorkshopEntryPoint)> EnterWorkshopEntryPointAsync()
        {
            var req = new REQ_EnterWorkshopEntryPoint
            {
                Header = MakeHeader(),
            };

            // remote request

            var res = new RES_EnterWorkshopEntryPoint
            {

            };

            return Observable.Return((req, res));
        }

        // combination consumable
        public IObservable<(REQ_CombinationConsumable, RES_CombinationConsumable)> CombinationConsumableAsync(
            SubRecipeView.RecipeInfo recipeInfo,
            int slotIndex)
        {
            var agentAddress = States.Instance.AgentState.address;
            var avatarAddress = States.Instance.CurrentAvatarState.address;

            LocalLayerModifier.ModifyAgentGold(agentAddress, -recipeInfo.CostNCG);
            LocalLayerModifier.ModifyAvatarActionPoint(agentAddress, -recipeInfo.CostAP);

            foreach (var (material, count) in recipeInfo.Materials)
            {
                LocalLayerModifier.RemoveItem(avatarAddress, material, count);
            }

            Analyzer.Instance.Track("Unity/Create CombinationConsumable", new Dictionary<string, object>
            {
                ["RecipeId"] = recipeInfo.RecipeId,
            });

            var req = new REQ_CombinationConsumable
            {
                Header = MakeHeader(),
            };

            // remote request


            var res = new RES_CombinationConsumable
            {

            };

            return Observable.Return((req, res));
        }

        // combination equipment
        public IObservable<(REQ_CombinationEquipment, RES_CombinationEquipment)> CombinationEquipmentAsync(
            SubRecipeView.RecipeInfo recipeInfo,
            int slotIndex)
        {
            Analyzer.Instance.Track("Unity/Create CombinationEquipment", new Dictionary<string, object>
            {
                ["RecipeId"] = recipeInfo.RecipeId,
            });

            var agentAddress = States.Instance.AgentState.address;
            var avatarAddress = States.Instance.CurrentAvatarState.address;

            LocalLayerModifier.ModifyAgentGold(agentAddress, -recipeInfo.CostNCG);
            LocalLayerModifier.ModifyAvatarActionPoint(agentAddress, -recipeInfo.CostAP);

            foreach (var (material, count) in recipeInfo.Materials)
            {
                LocalLayerModifier.RemoveItem(avatarAddress, material, count);
            }

            var req = new REQ_CombinationEquipment
            {
                Header = MakeHeader(),
            };

            // remote request

            var res = new RES_CombinationEquipment
            {

            };

            return Observable.Return((req, res));
        }

        // item enhancement
        public IObservable<(REQ_ItemEnhancement, RES_ItemEnhancement)> ItemEnhancementAsync(
            Equipment baseEquipment,
            Equipment materialEquipment,
            int slotIndex,
            BigInteger costNCG)
        {
            var agentAddress = States.Instance.AgentState.address;
            var avatarAddress = States.Instance.CurrentAvatarState.address;

            LocalLayerModifier.ModifyAgentGold(agentAddress, -costNCG);
            LocalLayerModifier.ModifyAvatarActionPoint(avatarAddress, -GameConfig.EnhanceEquipmentCostAP);
            LocalLayerModifier.ModifyAvatarActionPoint(avatarAddress, -GameConfig.EnhanceEquipmentCostAP);
            LocalLayerModifier.RemoveItem(avatarAddress, baseEquipment.TradableId,
                baseEquipment.RequiredBlockIndex, 1);
            LocalLayerModifier.RemoveItem(avatarAddress, materialEquipment.TradableId,
                materialEquipment.RequiredBlockIndex, 1);
            // NOTE: 장착했는지 안 했는지에 상관없이 해제 플래그를 걸어 둔다.
            LocalLayerModifier.SetItemEquip(avatarAddress, baseEquipment.NonFungibleId, false);
            LocalLayerModifier.SetItemEquip(avatarAddress, materialEquipment.NonFungibleId, false);

            Analyzer.Instance.Track("Unity/Item Enhancement");

            var req = new REQ_ItemEnhancement
            {
                Header = MakeHeader(),
            };

            // remote request

            var res = new RES_ItemEnhancement
            {

            };

            return Observable.Return((req, res));
        }

        // rapid combination
        public IObservable<(REQ_RapidCombination, RES_RapidCombination)> RapidCombinationAsync(
            CombinationSlotState state,
            int slotIndex)
        {
            var avatarAddress = States.Instance.CurrentAvatarState.address;
            var materialRow = Game.Game.instance.TableSheets.MaterialItemSheet.Values
                .First(r => r.ItemSubType == ItemSubType.Hourglass);
            var diff = state.UnlockBlockIndex - Game.Game.instance.Agent.BlockIndex;
            var cost = RapidCombination0.CalculateHourglassCount(States.Instance.GameConfigState, diff);

            LocalLayerModifier.RemoveItem(avatarAddress, materialRow.ItemId, cost);

            var req = new REQ_RapidCombination
            {
                Header = MakeHeader(),
            };

            // remote request

            var res = new RES_RapidCombination
            {

            };

            return Observable.Return((req, res));
        }

        ///////////////////////////////////////////////////////////////////////////
        // enter arena entry point
        public IObservable<(REQ_EnterArenaEntryPoint, RES_EnterArenaEntryPoint)> EnterArenaEntryPointAsync()
        {
            var req = new REQ_EnterArenaEntryPoint
            {
                Header = MakeHeader(),
            };

            // remote request

            var res = new RES_EnterArenaEntryPoint
            {

            };

            return Observable.Return((req, res));
        }

        // ranking battle
        public IObservable<(REQ_RankingBattle, RES_RankingBattle)> RankingBattleAsync(
            string enemyAddress,
            List<Guid> costumeIds,
            List<Guid> equipmentIds,
            List<Guid> consumableIds
        )
        {
            if (!ArenaHelper.TryGetThisWeekAddress(out var weeklyArenaAddress))
            {
                throw new NullReferenceException(nameof(weeklyArenaAddress));
            }

            Analyzer.Instance.Track("Unity/Ranking Battle");

            var req = new REQ_RankingBattle
            {
                Header = MakeHeader(),
            };

            var res = new RES_RankingBattle
            {

            };

            return Observable.Return((req, res));
        }


        ///////////////////////////////////////////////////////////////////////////
        // enter shop entry point
        public IObservable<(REQ_EnterShopEntryPoint, RES_EnterShopEntryPoint)> EnterShopEntryPointAsync()
        {
            var req = new REQ_EnterShopEntryPoint
            {
                Header = MakeHeader(),
            };

            var res = new RES_EnterShopEntryPoint
            {

            };

            return Observable.Return((req, res));
        }

        // sell
        public IObservable<(REQ_Sell, RES_Sell)> SellAsync(
            ITradableItem tradableItem,
            int count,
            BigInteger price,
            ItemSubType itemSubType)
        {
            var avatarAddress = States.Instance.CurrentAvatarState.address;

            if (!(tradableItem is TradableMaterial))
            {
                LocalLayerModifier.RemoveItem(avatarAddress, tradableItem.TradableId, tradableItem.RequiredBlockIndex, count);
            }

            // NOTE: 장착했는지 안 했는지에 상관없이 해제 플래그를 걸어 둔다.
            LocalLayerModifier.SetItemEquip(avatarAddress, tradableItem.TradableId, false);
            Analyzer.Instance.Track("Unity/Sell");

            var req = new REQ_Sell
            {

            };

            var res = new RES_Sell
            {

            };

            return Observable.Return((req, res));
        }

        // sell cancellation
        public IObservable<(REQ_SellCancellation, RES_SellCancellation)> SellCancellationAsync(
            string sellerAvatarAddress,
            Guid orderId,
            Guid tradableId,
            ItemSubType itemSubType)
        {

            var req = new REQ_SellCancellation
            {
                Header = MakeHeader(),
            };

            var res = new RES_SellCancellation
            {

            };


            return Observable.Return((req, res));
        }

        // update sell
        public IObservable<(REQ_UpdateSell, RES_UpdateSell)> UpdateSellAsync(
            Guid orderId,
            ITradableItem tradableItem,
            int count,
            BigInteger price,
            ItemSubType itemSubType)
        {
            var avatarAddress = States.Instance.CurrentAvatarState.address;

            if (!(tradableItem is TradableMaterial))
            {
                LocalLayerModifier.RemoveItem(avatarAddress, tradableItem.TradableId, tradableItem.RequiredBlockIndex, count);
            }

            // NOTE: 장착했는지 안 했는지에 상관없이 해제 플래그를 걸어 둔다.
            LocalLayerModifier.SetItemEquip(avatarAddress, tradableItem.TradableId, false);
            Analyzer.Instance.Track("Unity/UpdateSell");

            var req = new REQ_UpdateSell
            {
                Header = MakeHeader(),
            };

            var res = new RES_UpdateSell
            {

            };

            return Observable.Return((req, res));
        }

        // buy
        public IObservable<(REQ_Buy, RES_Buy)> BuyAsync(List<PurchaseInfo> purchaseInfos)
        {
            var buyerAgentAddress = States.Instance.AgentState.address;
            foreach (var purchaseInfo in purchaseInfos)
            {
                LocalLayerModifier.ModifyAgentGold(buyerAgentAddress, -purchaseInfo.Price);
            }

            var req = new REQ_Buy
            {
                Header = MakeHeader(),
            };

            var res = new RES_Buy
            {

            };

            return Observable.Return((req, res));
        }

        ///////////////////////////////////////////////////////////////////////////
        // enter mimisbrunnr entry point
        public IObservable<(REQ_EnterMimisbrunnrEntryPoint, RES_EnterMimisbrunnrEntryPoint)> EnterMimisbrunnrEntryPointAsync()
        {
            var req = new REQ_EnterMimisbrunnrEntryPoint
            {
                Header = MakeHeader(),
            };

            var res = new RES_EnterMimisbrunnrEntryPoint
            {

            };

            return Observable.Return((req, res));
        }

        // mimisbrunnr battle
        public IObservable<(REQ_MimisbrunnrBattle, RES_MimisbrunnrBattle)> MimisbrunnrBattleAsync(
            List<Costume> costumes,
            List<Equipment> equipments,
            List<Consumable> foods,
            int worldId,
            int stageId,
            int playCount)
        {
            var avatarAddress = States.Instance.CurrentAvatarState.address;
            costumes ??= new List<Costume>();
            equipments ??= new List<Equipment>();
            foods ??= new List<Consumable>();

            var req = new REQ_MimisbrunnrBattle
            {
                Header = MakeHeader(),
            };

            var res = new RES_MimisbrunnrBattle
            {

            };

            return Observable.Return((req, res));
        }

        ///////////////////////////////////////////////////////////////////////////
        // enter world entry point
        public IObservable<(REQ_EnterWorldEntryPoint, RES_EnterWorldEntryPoint)> EnterWorldEntryPointAsync()
        {
            var req = new REQ_EnterWorldEntryPoint
            {
                Header = MakeHeader(),
            };

            var res = new RES_EnterWorldEntryPoint
            {

            };

            return Observable.Return((req, res));
        }

        // hack and slash
        public IObservable<(REQ_HackAndSlash, RES_HackAndSlash)> HackAndSlashAsync(Player player, int worldId, int stageId, int playCount) => HackAndSlashAsync(
            player.Costumes,
            player.Equipments,
            null,
            worldId,
            stageId,
            playCount);

        public IObservable<(REQ_HackAndSlash, RES_HackAndSlash)> HackAndSlashAsync(
            List<Costume> costumes,
            List<Equipment> equipments,
            List<Consumable> foods,
            int worldId,
            int stageId,
            int playCount)
        {
            Analyzer.Instance.Track("Unity/HackAndSlash", new Dictionary<string, object>
            {
                ["WorldId"] = worldId,
                ["StageId"] = stageId,
                ["PlayCount"] = playCount,

            });

            var avatarAddress = States.Instance.CurrentAvatarState.address;
            costumes ??= new List<Costume>();
            equipments ??= new List<Equipment>();
            foods ??= new List<Consumable>();

            var req = new REQ_HackAndSlash
            {
                Header = MakeHeader(),
            };

            var res = new RES_HackAndSlash
            {

            };

            return Observable.Return((req, res));
        }
    }
}
