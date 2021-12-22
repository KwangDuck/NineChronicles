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
    using Nekoyume.UI.Scroller;
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

            var res = new RES_CombinationConsumable
            {
                Header = new RES_Header { }
            };

            return Observable.Return((req, res))
                .Do(result =>
                {
                    var (req, res) = result;
                    if (res.Header.ErrorCode != ENUM_ErrorCode.ERR_SUCCESS)
                    {
                        // TODO: update combination sloot state

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

            // remote request

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
                        CombinationSlotState slot = null;
                        States.Instance.UpdateCombinationSlotState(slotIndex, slot);

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
