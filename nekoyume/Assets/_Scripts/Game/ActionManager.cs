using System;
using System.Collections.Generic;
using System.Numerics;
using Nekoyume.Game.Character;
using Nekoyume.Model.Item;
using Nekoyume.Model.State;
using Nekoyume.UI;
using Material = Nekoyume.Model.Item.Material;

namespace Nekoyume.Game
{
    using Gateway.Protocol;
    using Nekoyume.Battle;
    using Nekoyume.L10n;
    using Nekoyume.Model.Mail;
    using Nekoyume.State;
    using Nekoyume.TableData;
    using Nekoyume.UI.Scroller;
    using System.Linq;
    using UniRx;

    /// <summary>
    /// Creates an action of the game and puts it in the agent.
    /// </summary>
    public class ActionManager : IDisposable
    {
        private static readonly TimeSpan ActionTimeout = TimeSpan.FromSeconds(360f);        
        private Guid _lastBattleActionId;
        private IGatewayDispatcher _gatewayDispatcher;

        public static ActionManager Instance => Game.instance.ActionManager;
        public static bool IsLastBattleActionId(Guid actionId) => actionId == Instance._lastBattleActionId;

        public ActionManager()
        {
            
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
                AvatarInfo = new ST_AvatarInfo
                {
                    SelectedAvatarIndex = 0,
                    AvatarDict = new Dictionary<int, ST_Avatar>
                    {
                        {0, new ST_Avatar
                            {
                                CharacterId = 100010,
                                Level = 1,
                                Exp = 0,
                                Hair = 1,
                                Lens = 1,
                                Ear = 1,
                                Tail = 1,
                                Name = string.Empty,
                                ActionPoint = 100,
                            }
                        },
                        {1, new ST_Avatar
                            {
                                CharacterId = 100110,
                                Level = 1,
                                Exp = 0,
                                Hair = 1,
                                Lens = 1,
                                Ear = 1,
                                Tail = 1,
                                Name = string.Empty,
                                ActionPoint = 100,
                            }
                        },
                        {2, new ST_Avatar
                            {
                                CharacterId = 100210,
                                Level = 1,
                                Exp = 0,
                                Hair = 1,
                                Lens = 1,
                                Ear = 1,
                                Tail = 1,
                                Name = string.Empty,
                                ActionPoint = 100,
                            }
                        }
                    }
                }
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
                    Name = nickName
                }
            };

            // remote request

            var res = new RES_CreateAvatar
            {

            };

            return Observable.Return((req, res));
        }

        // select avatar
        public IObservable<(REQ_SelectAvatar, RES_SelectAvatar)> SelectAvatarAsync(int index)
        {
            var req = new REQ_SelectAvatar
            {
                Header = MakeHeader(),
                Index = index,
            };

            // remote request

            var res = new RES_SelectAvatar
            {
                Header = new RES_Header { }
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
                Header = new RES_Header { },
            };

            return Observable.Return((req, res))
                .Do(result =>
                {
                    var (req, res) = result;
                    if (res.Header.ErrorCode == ENUM_ErrorCode.ERR_SUCCESS)
                    {
                        UI.NotificationSystem.Push(
                            MailType.System,
                            L10nManager.Localize("UI_RECEIVED_DAILY_REWARD"),
                            NotificationCell.NotificationType.Notification);
                    }
                });
        }

        // charge action point
        public IObservable<(REQ_ChargeActionPoint, RES_ChargeActionPoint)> ChargeActionPointAsync(Material material)
        {            
            var req = new REQ_ChargeActionPoint
            {
                Header = MakeHeader(),
            };

            // remote request

            var res = new RES_ChargeActionPoint
            {
                Header = new RES_Header { },
            };

            return Observable.Return((req, res))
                .Do(result =>
                {
                    var (req, res) = result;
                    if (res.Header.ErrorCode == ENUM_ErrorCode.ERR_SUCCESS)
                    {
                        // TODO: implementation
                    }
                });
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
            var req = new REQ_CombinationConsumable
            {
                Header = MakeHeader(),
            };

            // TODO: remote request

            // TODO: remote request
            var tableSheets = Game.instance.TableSheets;
            var avatarState = States.Instance.CurrentAvatarState;

            var recipeId = recipeInfo.RecipeId;
            var subRecipeId = recipeInfo.SubRecipeId;

            // - validate requred cleared stage
            // - validate slotindex
            // - validate work
            var costActionPoint = 0;
            var requiredItems = new Dictionary<int, int>();

            // - validate recipeId
            var consumableItemRecipeSheet = tableSheets.ConsumableItemRecipeSheet;
            if (!consumableItemRecipeSheet.TryGetValue(recipeId, out var recipeRow))
            {
                throw new Exception();
            }

            // - validate recipe result equipmentId
            var consumableItemSheet = tableSheets.ConsumableItemSheet;
            if (!consumableItemSheet.TryGetValue(recipeRow.ResultConsumableItemId, out var consumableRow))
            {
                throw new Exception();
            }

            // - validate recipe material
            var materialItemSheet = tableSheets.MaterialItemSheet;
            for (var i = recipeRow.Materials.Count; i > 0; i--)
            {
                var materialInfo = recipeRow.Materials[i - 1];
                if (!materialItemSheet.TryGetValue(materialInfo.Id, out var materialRow))
                {
                    throw new Exception();
                }

                if (requiredItems.ContainsKey(materialRow.Id))
                {
                    requiredItems[materialRow.Id] += materialInfo.Count;
                }
                else
                {
                    requiredItems[materialRow.Id] = materialInfo.Count;
                }
            }

            costActionPoint += recipeRow.RequiredActionPoint;

            // - remove required materials
            var inventory = avatarState.inventory;
            foreach (var pair in requiredItems.OrderBy(pair => pair.Key))
            {
                if (!materialItemSheet.TryGetValue(pair.Key, out var materialRow) ||
                    !inventory.RemoveItem(materialRow.Id, pair.Value))
                {
                    throw new Exception();
                }
            }

            // - substract required action point
            if (costActionPoint > 0)
            {
                if (avatarState.actionPoint < costActionPoint)
                {
                    throw new Exception();
                }

                avatarState.actionPoint -= costActionPoint;
            }

            // - create consumable
            var consumable = ItemFactory.CreateItemUsable(consumableRow) as Consumable;
            avatarState.UpdateFromCombination(consumable);
            avatarState.UpdateQuestRewards(materialItemSheet);

            // - update slot
            // - create mail


            var res = new RES_CombinationConsumable
            {
                Header = new RES_Header { }
            };

            return Observable.Return((req, res))
                .Do(result =>
                {
                    var (req, res) = result;
                    if (res.Header.ErrorCode == ENUM_ErrorCode.ERR_SUCCESS)
                    {                       

                        // TODO: nofity
                        var format = L10nManager.Localize("NOTIFICATION_COMBINATION_COMPLETE");
                        UI.NotificationSystem.Reserve(
                            MailType.Workshop,
                            string.Format(format, "name"),
                            0,
                            new Guid());
                    }
                    Widget.Find<CombinationSlotsPopup>().SetCaching(slotIndex, false);
                });
        }

        // combination equipment
        public IObservable<(REQ_CombinationEquipment, RES_CombinationEquipment)> CombinationEquipmentAsync(
            SubRecipeView.RecipeInfo recipeInfo,
            int slotIndex)
        {
            var req = new REQ_CombinationEquipment
            {
                Header = MakeHeader(),
            };

            // TODO: remote request
            var tableSheets = Game.instance.TableSheets;
            var avatarState = States.Instance.CurrentAvatarState;
            
            var recipeId = recipeInfo.RecipeId;
            var subRecipeId = recipeInfo.SubRecipeId;

            // - validate requred cleared stage
            // - validate slotindex
            // - validate work
            var costActionPoint = 0;
            var costNCG = 0L;
            var requiredItems = new Dictionary<int, int>();

            // - validate recipe
            var equipmentItemReceiptSheet = tableSheets.EquipmentItemRecipeSheet;
            if (!equipmentItemReceiptSheet.TryGetValue(recipeId, out var recipeRow))
            {
                throw new Exception();
            }

            // - validate recipe unlocked
            if (!avatarState.worldInformation.IsStageCleared(recipeRow.UnlockStage))
            {
                throw new Exception();
            }

            // - validate recipe equipmentId
            var equipmentItemSheet = tableSheets.EquipmentItemSheet;
            if (!equipmentItemSheet.TryGetValue(recipeRow.ResultEquipmentId, out var equipmentRow))
            {
                throw new Exception();
            }

            // - validate recipe material
            var materialItemSheet = tableSheets.MaterialItemSheet;
            if (!materialItemSheet.TryGetValue(recipeRow.MaterialId, out var materialRow))
            {
                throw new Exception();
            }

            if (requiredItems.ContainsKey(materialRow.Id))
            {
                requiredItems[materialRow.Id] += recipeRow.MaterialCount;
            }
            else
            {
                requiredItems[materialRow.Id] = recipeRow.MaterialCount;
            }

            // TODO: contains material?

            // - validate sub recipeId
            EquipmentItemSubRecipeSheetV2.Row subRecipeRow = null;
            if (subRecipeId.HasValue)
            {
                // - validate sub receiptId
                if (!recipeRow.SubRecipeIds.Contains(subRecipeId.Value))
                {
                    throw new Exception();
                }

                var equipmentItemSubRecipeSheetV2 = tableSheets.EquipmentItemSubRecipeSheetV2;
                if (!equipmentItemSubRecipeSheetV2.TryGetValue(subRecipeId.Value, out subRecipeRow))
                {
                    throw new Exception();
                }

                // - validate subrecipe material
                for (var i = subRecipeRow.Materials.Count; i > 0; i--)
                {
                    var materialInfo = subRecipeRow.Materials[i - 1];
                    if (!materialItemSheet.TryGetValue(materialInfo.Id, out materialRow))
                    {
                        throw new Exception();
                    }

                    if (requiredItems.ContainsKey(materialRow.Id))
                    {
                        requiredItems[materialRow.Id] += materialInfo.Count;
                    }
                    else
                    {
                        requiredItems[materialRow.Id] = materialInfo.Count;
                    }
                }

                costActionPoint += subRecipeRow.RequiredActionPoint;
                costNCG += subRecipeRow.RequiredGold;
            }

            costActionPoint += recipeRow.RequiredActionPoint;
            costNCG += recipeRow.RequiredGold;

            // - remove required materials
            var inventory = avatarState.inventory;
            foreach (var pair in requiredItems.OrderBy(pair => pair.Key))
            {
                if (!materialItemSheet.TryGetValue(pair.Key, out materialRow) ||
                    !inventory.RemoveItem(materialRow.Id, pair.Value))
                {
                    throw new Exception();
                }
            }

            // - subtract required action point
            if (costActionPoint > 0)
            {
                if (avatarState.actionPoint < costActionPoint)
                {
                    throw new Exception();
                }

                // TODO: update action point
                avatarState.actionPoint -= costActionPoint;
            }

            // - subtract required NGC
            if (costNCG > 0)
            {
                // TODO: transfer required NCG
            }

            // - create equipment
            var equipment = ItemFactory.CreateItemUsable(equipmentRow) as Equipment;
            if (!(subRecipeRow is null))
            {
                // TODO; something
            }

            // - add or update equipment
            avatarState.questList.UpdateCombinationEquipmentQuest(recipeId);
            avatarState.UpdateFromCombination(equipment);
            avatarState.UpdateQuestRewards(materialItemSheet);

            // - update slot

            // - create mail


            var res = new RES_CombinationEquipment
            {
                Header = new RES_Header { },
            };

            return Observable.Return((req, res))
                .Do(result =>
                {
                    var (req, res) = result;
                    if (res.Header.ErrorCode == ENUM_ErrorCode.ERR_SUCCESS)
                    {
                        // update combination slot
                        // find completed quest by result
                        // notify
                    }

                    Widget.Find<CombinationSlotsPopup>().SetCaching(slotIndex, false);
                });
        }

        // item enhancement
        public IObservable<(REQ_ItemEnhancement, RES_ItemEnhancement)> ItemEnhancementAsync(
            Equipment baseEquipment,
            Equipment materialEquipment,
            int slotIndex,
            BigInteger costNCG)
        {
            var req = new REQ_ItemEnhancement
            {
                Header = MakeHeader(),
            };

            // remote request

            var tableSheets = Game.instance.TableSheets;
            var avatarState = States.Instance.CurrentAvatarState;

            // - try get non fungible item
            // - validate required block index
            // - validate combination slot state

            // - validate enhancement cost
            var enhancementCostSheet = tableSheets.EnhancementCostSheetV2;
            var enhancementCostRow = enhancementCostSheet.OrderedList
                .FirstOrDefault(x => x.Grade == baseEquipment.Grade && x.Level == baseEquipment.level + 1 && x.ItemSubType == baseEquipment.ItemSubType);
            if (enhancementCostRow is null)
            {
                throw new Exception();
            }

            // - is max level?
            var maxLevel = enhancementCostSheet.OrderedList
                .Where(x => x.Grade == baseEquipment.Grade)
                .Max(x => x.Level);
            if (baseEquipment.level >= maxLevel)
            {
                throw new Exception();
            }

            // - get material item
            // - validate material item required bloxk index
            // - validate material item (id, grade, level, subType)

            // - substract action point
            var requiredActionPoint = GameConfig.EnhanceEquipmentCostAP;
            if (avatarState.actionPoint < requiredActionPoint)
            {
                throw new Exception();
            }
            avatarState.actionPoint -= requiredActionPoint;

            // - substract ngc
            var requiredNgc = enhancementCostRow.Cost;
            if (requiredNgc > 0)
            {
                // - transfer ngc
            }

            // - unequip items
            baseEquipment.Unequip();
            materialEquipment.Unequip();

            // - execute enhancement
            ENUM_ItemEnhancementResult _GetEnhancementResult(EnhancementCostSheetV2.Row row, Random random)
            {
                var rand = random.Next(1, GameConfig.MaximumProbability + 1);
                if (rand <= row.GreatSuccessRatio)
                    return ENUM_ItemEnhancementResult.GreatSuccess;
                return rand <= row.GreatSuccessRatio + row.SuccessRatio ? ENUM_ItemEnhancementResult.Success : ENUM_ItemEnhancementResult.Fail;
            }
            var random = new Random();
            var enhancementResult = _GetEnhancementResult(enhancementCostRow, random);
            if (enhancementResult != ENUM_ItemEnhancementResult.Fail)
            {
                baseEquipment.LevelUpV2(random, enhancementCostRow, enhancementResult == ENUM_ItemEnhancementResult.GreatSuccess);
            }
            // - update required block count

            // - remove material equipment
            avatarState.inventory.RemoveNonFungibleItem(materialEquipment.ItemId);

            // - send item enhance mail
            avatarState.inventory.RemoveNonFungibleItem(baseEquipment.ItemId);
            avatarState.UpdateFromItemEnhancement(baseEquipment);

            // - update quest reward

            // - update slot state


            var res = new RES_ItemEnhancement
            {
                Header = new RES_Header { },
                Result = ENUM_ItemEnhancementResult.GreatSuccess,
            };

            return Observable.Return((req, res))
                .Do(result =>
                {
                    var (req, res) = result;
                    if (res.Header.ErrorCode == ENUM_ErrorCode.ERR_SUCCESS)
                    {
                        // update combination slot state
                        CombinationSlotState slot = null;
                        States.Instance.UpdateCombinationSlotState(slotIndex, slot);

                        // notify
                        string formatKey = string.Empty;
                        switch (res.Result)
                        {
                            case ENUM_ItemEnhancementResult.GreatSuccess:
                                formatKey = "NOTIFICATION_ITEM_ENHANCEMENT_COMPLETE_GREATER";
                                break;
                            case ENUM_ItemEnhancementResult.Success:
                                formatKey = "NOTIFICATION_ITEM_ENHANCEMENT_COMPLETE";
                                break;
                            case ENUM_ItemEnhancementResult.Fail:
                                formatKey = "NOTIFICATION_ITEM_ENHANCEMENT_COMPLETE_FAIL";
                                break;
                            default:
                                formatKey = "NOTIFICATION_ITEM_ENHANCEMENT_COMPLETE";
                                break;
                        }

                        var format = L10nManager.Localize(formatKey);
                        UI.NotificationSystem.Reserve(
                            MailType.Workshop,
                            string.Format(format, "name"),
                            0,
                            new Guid());
                    }

                    Widget.Find<CombinationSlotsPopup>().SetCaching(slotIndex, false);
                });
        }

        // rapid combination
        public IObservable<(REQ_RapidCombination, RES_RapidCombination)> RapidCombinationAsync(
            CombinationSlotState state,
            int slotIndex)
        {
            var req = new REQ_RapidCombination
            {
                Header = MakeHeader(),
            };

            // remote request

            var res = new RES_RapidCombination
            {
                Header = new RES_Header { },
            };

            return Observable.Return((req, res))
                .Do(result =>
                {
                    var (req, res) = result;
                    if (res.Header.ErrorCode == ENUM_ErrorCode.ERR_SUCCESS)
                    {

                    }

                    Widget.Find<CombinationSlotsPopup>().SetCaching(slotIndex, false);
                });
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
            var req = new REQ_RankingBattle
            {
                Header = MakeHeader(),
            };

            // TODO: remote request

            var res = new RES_RankingBattle
            {
                Header = new RES_Header { }
            };

            return Observable.Return((req, res))
                .Do(result =>
                {
                    var (req, res) = result;
                    if (res.Header.ErrorCode != ENUM_ErrorCode.ERR_SUCCESS)
                    {
                        var showLoadingScreen = false;
                        if (Widget.Find<ArenaBattleLoadingScreen>().IsActive())
                        {
                            Widget.Find<ArenaBattleLoadingScreen>().Close();
                        }

                        if (Widget.Find<RankingBattleResultPopup>().IsActive())
                        {
                            showLoadingScreen = true;
                            Widget.Find<RankingBattleResultPopup>().Close();
                        }

                        Game.BackToMain(showLoadingScreen, new Exception());
                    }
                    else
                    {
                        // subscribe stageEnd
                        Game.instance.Stage.onEnterToStageEnd
                            .First()
                            .Subscribe(_ =>
                            {
                                Game.instance.Stage.IsAvatarStateUpdatedAfterBattle = true;
                            });

                        // ranking simulator
                        AvatarState enemyAvatarState = null;
                        ArenaInfo arenaInfo = null;
                        ArenaInfo enemyInfo = null;
                        const int stageId = 999999;

                        var simulator = new RankingSimulator(
                            new Random(),
                            States.Instance.CurrentAvatarState,
                            enemyAvatarState,
                            new List<Guid>(),
                            Game.instance.TableSheets.GetRankingSimulatorSheets(),
                            stageId,
                            arenaInfo,
                            enemyInfo,
                            Game.instance.TableSheets.CostumeStatSheet
                        );
                        simulator.Simulate();
                        var log = simulator.Log;

                        if (Widget.Find<ArenaBattleLoadingScreen>().IsActive())
                        {
                            Widget.Find<RankingBoard>().GoToStage(log);
                        }
                    }
                });
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
            var req = new REQ_Sell
            {
                Header = MakeHeader(),
            };

            var res = new RES_Sell
            {
                Header = new RES_Header { },
            };

            return Observable.Return((req, res))
                .Do(result =>
                {
                    var (req, res) = result;
                    if (res.Header.ErrorCode == ENUM_ErrorCode.ERR_SUCCESS)
                    {
                        var message = string.Empty;
                        if (count > 1)
                        {
                            message = string.Format(L10nManager.Localize("NOTIFICATION_MULTIPLE_SELL_COMPLETE"), itemSubType, count);
                        }
                        else
                        {
                            message = string.Format(L10nManager.Localize("NOTIFICATION_SELL_COMPLETE"), itemSubType);
                        }

                        OneLineSystem.Push(MailType.Auction, message, NotificationCell.NotificationType.Information);

                        var shopSell = Widget.Find<ShopSell>();
                        if (shopSell.isActiveAndEnabled)
                        {
                            shopSell.Refresh();
                        }
                    }
                });
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

            // TODO: remote request

            var res = new RES_SellCancellation
            {
                Header = new RES_Header { },
            };
            return Observable.Return((req, res))
                .Do(result =>
                {
                    var (req, res) = result;
                    if (res.Header.ErrorCode == ENUM_ErrorCode.ERR_SUCCESS)
                    {
                        var message = string.Format(L10nManager.Localize("NOTIFICATION_SELL_CANCEL_COMPLETE"), itemSubType);
                        OneLineSystem.Push(MailType.Auction, message, NotificationCell.NotificationType.Information);

                        var shopSell = Widget.Find<ShopSell>();
                        if (shopSell.isActiveAndEnabled)
                        {
                            shopSell.Refresh();
                        }
                    }
                });
        }

        // update sell
        public IObservable<(REQ_UpdateSell, RES_UpdateSell)> UpdateSellAsync(
            Guid orderId,
            ITradableItem tradableItem,
            int count,
            BigInteger price,
            ItemSubType itemSubType)
        {
            var req = new REQ_UpdateSell
            {
                Header = MakeHeader(),
            };

            var res = new RES_UpdateSell
            {
                Header = new RES_Header { },
            };

            return Observable.Return((req, res))
                .Do(result =>
                {
                    var (req, res) = result;
                    if (res.Header.ErrorCode == ENUM_ErrorCode.ERR_SUCCESS)
                    {
                        var message = string.Format(L10nManager.Localize("NOTIFICATION_REREGISTER_COMPLETE"), itemSubType);
                        OneLineSystem.Push(MailType.Auction, message, NotificationCell.NotificationType.Information);

                        var shopSell = Widget.Find<ShopSell>();
                        if (shopSell.isActiveAndEnabled)
                        {
                            shopSell.Refresh();
                        }
                    }
                });
        }

        // buy
        public IObservable<(REQ_Buy, RES_Buy)> BuyAsync()
        {
            var req = new REQ_Buy
            {
                Header = MakeHeader(),
            };

            var res = new RES_Buy
            {
                Header = new RES_Header { },
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
            var req = new REQ_MimisbrunnrBattle
            {
                Header = MakeHeader(),
            };

            // TODO: remote request

            var res = new RES_MimisbrunnrBattle
            {
                Header = new RES_Header { }
            };

            return Observable.Return((req, res))
                .Do(result =>
                {
                    var (req, res) = result;
                    if (res.Header.ErrorCode != ENUM_ErrorCode.ERR_SUCCESS)
                    {
                        var showLoadingScreen = false;
                        if (Widget.Find<StageLoadingEffect>().IsActive())
                        {
                            Widget.Find<StageLoadingEffect>().Close();
                        }

                        if (Widget.Find<BattleResultPopup>().IsActive())
                        {
                            showLoadingScreen = true;
                            Widget.Find<BattleResultPopup>().Close();
                        }

                        Game.BackToMain(showLoadingScreen, new Exception());
                    }
                    else
                    {
                        // subscribe stageEnd
                        Game.instance.Stage.onEnterToStageEnd
                            .First()
                            .Subscribe(_ =>
                            {
                                Game.instance.Stage.IsAvatarStateUpdatedAfterBattle = true;
                            });

                        var simulator = new StageSimulator(
                            new Random(),
                            States.Instance.CurrentAvatarState,
                            new List<Guid>(),
                            worldId,
                            stageId,
                            Game.instance.TableSheets.GetStageSimulatorSheets(),
                            Game.instance.TableSheets.CostumeStatSheet,
                            StageSimulator.ConstructorVersionV100080,
                            playCount
                        );
                        simulator.Simulate(playCount);
                        var log = simulator.Log;
                        Game.instance.Stage.PlayCount = playCount;

                        // update avatar state
                        var avatarState = States.Instance.CurrentAvatarState;
                        avatarState.Update(simulator, Game.instance.TableSheets.MaterialItemSheet);

                        if (Widget.Find<LoadingScreen>().IsActive())
                        {
                            if (Widget.Find<MimisbrunnrPreparation>().IsActive())
                            {
                                Widget.Find<MimisbrunnrPreparation>().GoToStage(log);
                            }
                            else if (Widget.Find<Menu>().IsActive())
                            {
                                Widget.Find<Menu>().GoToStage(log);
                            }
                        }
                        else if (Widget.Find<StageLoadingEffect>().IsActive() &&
                                    Widget.Find<BattleResultPopup>().IsActive())
                        {
                            Widget.Find<BattleResultPopup>().NextMimisbrunnrStage(log);
                        }                        
                    }
                });
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
            var req = new REQ_HackAndSlash
            {
                Header = MakeHeader(),
            };

            // remote request

            var avatarState = States.Instance.CurrentAvatarState;
            // - equip costumes
            avatarState.EquipCostumes(costumes);

            // - equip items
            avatarState.EquipItems(equipments);

            var res = new RES_HackAndSlash
            {
                Header = new RES_Header { }
            };

            return Observable.Return((req, res))
                .Do(result =>
                {
                    var (req, res) = result;
                    if (res.Header.ErrorCode != ENUM_ErrorCode.ERR_SUCCESS)
                    {
                        var showLoadingScreen = false;
                        if (Widget.Find<StageLoadingEffect>().IsActive())
                        {
                            Widget.Find<StageLoadingEffect>().Close();
                        }

                        if (Widget.Find<BattleResultPopup>().IsActive())
                        {
                            showLoadingScreen = true;
                            Widget.Find<BattleResultPopup>().Close();
                        }

                        Game.BackToMain(showLoadingScreen, new Exception());
                    }
                    else
                    {
                        // subscribe stageEnd
                        Game.instance.Stage.onEnterToStageEnd
                            .First()
                            .Subscribe(_ =>
                            {
                                Game.instance.Stage.IsAvatarStateUpdatedAfterBattle = true;
                            });

                        // make simulator
                        var simulator = new StageSimulator(
                            new Random(),
                            States.Instance.CurrentAvatarState,
                            new List<Guid>(),
                            worldId,
                            stageId,
                            Game.instance.TableSheets.GetStageSimulatorSheets(),
                            Game.instance.TableSheets.CostumeStatSheet,
                            StageSimulator.ConstructorVersionV100080,
                            playCount
                        );
                        simulator.Simulate(playCount);
                        var log = simulator.Log;
                        Game.instance.Stage.PlayCount = playCount;

                        if(log.IsClear)                        
                            simulator.Player.worldInformation.ClearStage(worldId, stageId, Game.instance.TableSheets.WorldSheet, Game.instance.TableSheets.WorldUnlockSheet);

                        // update avatar state
                        var avatarState = States.Instance.CurrentAvatarState;
                        avatarState.Update(simulator, Game.instance.TableSheets.MaterialItemSheet);

                        if (Widget.Find<LoadingScreen>().IsActive())
                        {
                            if (Widget.Find<QuestPreparation>().IsActive())
                            {
                                Widget.Find<QuestPreparation>().GoToStage(log);
                            }
                            else if (Widget.Find<Menu>().IsActive())
                            {
                                Widget.Find<Menu>().GoToStage(log);
                            }
                        }
                        else if (Widget.Find<StageLoadingEffect>().IsActive() &&
                                 Widget.Find<BattleResultPopup>().IsActive())
                        {
                            Widget.Find<BattleResultPopup>().NextStage(log);
                        }
                    }
                });
        }
    }
}
