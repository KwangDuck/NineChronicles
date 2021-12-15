using Gateway.Protocol.Table;
using MagicOnion;
using MessagePack;
using System.Collections.Generic;

namespace Gateway.Protocol
{
    ///////////////////////////////////////////////////////////////////////////
    // Login/Logout
    [MessagePackObject]
    public class REQ_Login : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
        [Key(1)] public ENUM_Login Login { get; set; }
        [Key(2)] public string AccessToken { get; set; }
        [Key(3)] public string AccessTokenSecret { get; set; }
        [Key(4)] public ST_Auth Auth { get; set; }
        [Key(5)] public ST_Device Device { get; set; }
    }

    [MessagePackObject]
    public class RES_Login : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
        [Key(1)] public long UserNo { get; set; }
        [Key(2)] public string Nickname { get; set; }
    }

    [MessagePackObject]
    public class REQ_Logout : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
    }

    [MessagePackObject]
    public class RES_Logout : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
    }

    [MessagePackObject]
    public class REQ_RetrieveAllMasterData : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
        [Key(1)] public IDictionary<string, long> VersionMap { get; set; }
    }

    [MessagePackObject]
    public class RES_RetrieveAllMasterData : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
        [Key(1)] public IDictionary<string, ST_Table> TableMap { get; set; }
    }

    [MessagePackObject]
    public class REQ_RetrieveMasterData : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
        [Key(1)] public string TableName { get; set; }
        [Key(2)] public long Version { get; set; }
    }

    [MessagePackObject]
    public class RES_RetrieveMasterData : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
        [Key(1)] public ST_Table Table { get; set; }
    }

    [MessagePackObject]
    public class REQ_CreateAvatar : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
        [Key(1)] public int Index { get; set; }
        [Key(2)] public ST_Avatar Avatar { get; set; }
    }

    [MessagePackObject]
    public class RES_CreateAvatar : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
    }

    ///////////////////////////////////////////////////////////////////////////
    // Lobby
    [MessagePackObject]
    public class REQ_EnterLobbyEntryPoint : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
    }

    [MessagePackObject]
    public class RES_EnterLobbyEntryPoint : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
        [Key(1)] public ST_UserInfo UserInfo { get; set; }
        [Key(2)] public ST_AssetInfo AssetInfo { get; set; }
    }

    // daily reward
    [MessagePackObject]
    public class REQ_DailyReward : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
    }

    [MessagePackObject]
    public class RES_DailyReward : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
        // TODO...
    }

    // charge action point
    [MessagePackObject]
    public class REQ_ChargeActionPoint : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
    }

    [MessagePackObject]
    public class RES_ChargeActionPoint : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
        // TODO...
    }

    ///////////////////////////////////////////////////////////////////////////
    // workshop
    [MessagePackObject]
    public class REQ_EnterWorkshopEntryPoint : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
    }

    [MessagePackObject]
    public class RES_EnterWorkshopEntryPoint : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
    }

    // combination consumable
    [MessagePackObject]
    public class REQ_CombinationConsumable : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
    }

    [MessagePackObject]
    public class RES_CombinationConsumable : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
    }

    // combination equipment
    [MessagePackObject]
    public class REQ_CombinationEquipment : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
    }

    [MessagePackObject]
    public class RES_CombinationEquipment : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
    }

    // item enhancement
    [MessagePackObject]
    public class REQ_ItemEnhancement : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
    }

    [MessagePackObject]
    public class RES_ItemEnhancement : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
    }

    // rapid combination
    [MessagePackObject]
    public class REQ_RapidCombination : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
    }

    [MessagePackObject]
    public class RES_RapidCombination : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
    }

    ///////////////////////////////////////////////////////////////////////////
    // arena

    [MessagePackObject]
    public class REQ_EnterArenaEntryPoint : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
    }

    [MessagePackObject]
    public class RES_EnterArenaEntryPoint : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
    }

    [MessagePackObject]
    public class REQ_RankingBattle : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
        // enemy id
        // costume ids
        // equipment ids
        // cosumable ids
    }

    [MessagePackObject]
    public class RES_RankingBattle : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
    }

    ///////////////////////////////////////////////////////////////////////////
    // shop
    [MessagePackObject]
    public class REQ_EnterShopEntryPoint : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
    }

    [MessagePackObject]
    public class RES_EnterShopEntryPoint : IRES
    {
        [Key(1)] public RES_Header Header { get; set; }
    }

    // sell
    [MessagePackObject]
    public class REQ_Sell : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
    }

    [MessagePackObject]
    public class RES_Sell : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
    }

    // sell cancellation
    [MessagePackObject]
    public class REQ_SellCancellation : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
    }

    [MessagePackObject]
    public class RES_SellCancellation : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
    }

    // update sell
    [MessagePackObject]
    public class REQ_UpdateSell : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
    }

    [MessagePackObject]
    public class RES_UpdateSell : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
    }

    // buy
    [MessagePackObject]
    public class REQ_Buy : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
    }

    [MessagePackObject]
    public class RES_Buy : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
    }


    ///////////////////////////////////////////////////////////////////////////
    // mimisbrunnr
    [MessagePackObject]
    public class REQ_EnterMimisbrunnrEntryPoint : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
    }

    [MessagePackObject]
    public class RES_EnterMimisbrunnrEntryPoint : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
    }

    [MessagePackObject]
    public class REQ_MimisbrunnrBattle : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
        [Key(1)] public IReadOnlyList<int> CostumeIds { get; set; }
        [Key(2)] public IReadOnlyList<int> EquipmentIds { get; set; }
        [Key(3)] public IReadOnlyList<int> ConsumableIds { get; set; }
        [Key(4)] public int WorldId { get; set; }
        [Key(5)] public int StageId { get; set; }
        [Key(6)] public int PlayCount { get; set; }
    }

    [MessagePackObject]
    public class RES_MimisbrunnrBattle : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
        // TODO...
    }

    ///////////////////////////////////////////////////////////////////////////
    // world
    [MessagePackObject]
    public class REQ_EnterWorldEntryPoint : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
    }

    [MessagePackObject]
    public class RES_EnterWorldEntryPoint : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
    }

    [MessagePackObject]
    public class REQ_HackAndSlash : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
        [Key(1)] public IReadOnlyList<int> CostumeIds { get; set; }
        [Key(2)] public IReadOnlyList<int> EquipmentIds { get; set; }
        [Key(3)] public IReadOnlyList<int> ConsumableIds { get; set; }
        [Key(4)] public int WorldId { get; set; }
        [Key(5)] public int StageId { get; set; }
        [Key(6)] public int PlayCount { get; set; }
    }

    [MessagePackObject]
    public class RES_HackAndSlash : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
        // TODO...
    }

    ///////////////////////////////////////////////////////////////////////////
    // Settings
    [MessagePackObject]
    public class REQ_EnterSettingsEntryPoint : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
    }

    [MessagePackObject]
    public class RES_EnterSettingsEntryPoint : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
        [Key(1)] public ENUM_Login LoginType { get; set; }
        [Key(2)] public ENUM_ServiceProvider ServiceProvider { get; set; }
        [Key(3)] public string YoutubeLinkUrl { get; set; }
        [Key(4)] public string HelpLinkUrl { get; set; }
        [Key(5)] public string SurveyLinkUrl { get; set; }
        [Key(6)] public string HashedUserId { get; set; }
    }

    [MessagePackObject]
    public class REQ_LinkGooglePlayGames : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
        // user id
        // link account
    }

    [MessagePackObject]
    public class RES_LinkGooglePlayGames : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
    }

    [MessagePackObject]
    public class REQ_LinkAppleGameCenter : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
    }

    [MessagePackObject]
    public class RES_LinkAppleGameCenter : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
    }

    [MessagePackObject]
    public class REQ_LinkFacebook : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
    }

    [MessagePackObject]
    public class RES_LinkFacebook : IRES
    {
        [Key(1)] public RES_Header Header { get; set; }
    }

    [MessagePackObject]
    public class REQ_WithdrawAccount : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
    }

    [MessagePackObject]
    public class RES_WithdrawAccount : IRES
    {
        [Key(1)] public RES_Header Header { get; set; }
    }


    [MessagePackObject]
    public class REQ_ConfigLanguage : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
        [Key(1)] public string Language { get; set; }
    }

    [MessagePackObject]
    public class RES_ConfigLanguage : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
        [Key(1)] public string Language { get; set; }
    }

    [MessagePackObject]
    public class REQ_ConfigCountry : IREQ
    {
        [Key(0)] public REQ_Header Header { get; set; }
        [Key(1)] public string Country { get; set; }
    }

    [MessagePackObject]
    public class RES_ConfigCountry : IRES
    {
        [Key(0)] public RES_Header Header { get; set; }
        [Key(1)] public string Country { get; set; }
    }

    ///////////////////////////////////////////////////////////////////////////
    public interface IGatewayDispatcher : IService<IGatewayDispatcher>
    {
        // login/logout
        UnaryResult<RES_Login> LoginAsync(REQ_Login req);
        UnaryResult<RES_Logout> LogoutAsync(REQ_Logout req);
        // - create avatar
        UnaryResult<RES_RetrieveAllMasterData> RetrieveAllMasterDataAsync(REQ_RetrieveAllMasterData req);
        UnaryResult<RES_RetrieveMasterData> RetrieveMasterDataAsync(REQ_RetrieveMasterData req);

        // lobby
        UnaryResult<RES_EnterLobbyEntryPoint> EnterLobbyEntryPointAsync(REQ_EnterLobbyEntryPoint req);
        UnaryResult<RES_DailyReward> DailyRewardAsync(REQ_DailyReward req);
        UnaryResult<RES_ChargeActionPoint> ChargeActionPointAsync(REQ_ChargeActionPoint req);

        // workshop
        UnaryResult<RES_EnterWorkshopEntryPoint> EnterWorkshopEntryPointAsync(REQ_EnterWorkshopEntryPoint req);
        UnaryResult<RES_CombinationConsumable> CombinationConsumableAsync(REQ_CombinationConsumable req);
        UnaryResult<RES_CombinationEquipment> CombinationEquipmentAsync(REQ_CombinationEquipment req);
        UnaryResult<RES_ItemEnhancement> ItemEnhancementAsync(REQ_ItemEnhancement req);
        UnaryResult<RES_RapidCombination> RapidCombinationAsync(REQ_RapidCombination req);

        // arena
        UnaryResult<RES_EnterArenaEntryPoint> EnterArenaEntryPointAsync(REQ_EnterArenaEntryPoint req);
        UnaryResult<RES_RankingBattle> RankingBattleAsync(REQ_RankingBattle req);

        // shop
        UnaryResult<RES_EnterShopEntryPoint> EnterShopEntryPointAsync(REQ_EnterShopEntryPoint req);
        UnaryResult<RES_Sell> SellAsync(REQ_Sell req);
        UnaryResult<RES_SellCancellation> SellCancellationAsync(REQ_SellCancellation req);
        UnaryResult<RES_UpdateSell> UpdateSellAsync(REQ_UpdateSell req);
        UnaryResult<RES_Buy> BuyAsync(REQ_Buy req);

        // mimisbrunnr
        UnaryResult<RES_EnterMimisbrunnrEntryPoint> EnterMimisbrunnrEntryPointAsync(REQ_EnterMimisbrunnrEntryPoint req);
        UnaryResult<RES_MimisbrunnrBattle> MimisbrunnrBattleAsync(REQ_MimisbrunnrBattle req);

        // world
        UnaryResult<RES_EnterWorldEntryPoint> EnterWorldEntryPointAsync(REQ_EnterWorldEntryPoint req);
        UnaryResult<RES_HackAndSlash> HackAndSlashAsync(REQ_HackAndSlash req);

        // settings
        UnaryResult<RES_EnterSettingsEntryPoint> EnterSettingsEntryPointAsync(REQ_EnterSettingsEntryPoint req);
    }
}
