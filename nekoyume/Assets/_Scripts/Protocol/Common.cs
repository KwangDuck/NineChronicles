using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Gateway.Protocol
{
    public static class Info
    {
        public const string ProtocolVersion = "v1";
    }

    public enum ENUM_ErrorCode
    {
        ERR_NONE,
        ERR_SUCCESS,
        ERR_FAILED,

        // System
        ERR_AUTH_TOKEN_EXPIRED = 1000000,
    }

    public enum ENUM_UserKind
    {
        NONE,
        NRU,
        RAU,
        CAU,
    }

    public enum ENUM_UserType
    {
        NONE,
        NORMAL,
        CHURN,
        BLOCKED,
        PERIOD_BLOCKED,
        DORMANCY,
    }

    public enum ENUM_AccounType
    {
        NONE,
        GUEST,
        NORMAL,
        QA,
        GM,
        OPERATOR,
        BOT,
    }

    public enum ENUM_OS
    {
        NONE,
        ANDROID,
        IOS,
        WEB,
        DESKTOP,
    }

    public enum ENUM_Market
    {
        NONE,
        PLAYSTORE,
        ONESTORE,
        APPSTORE,        
    }

    public enum ENUM_Platform
    {
        NONE,
        FIREBASE,
        PLAYFAB,
    }

    public enum ENUM_ServiceProvider
    {
        NONE,
        FACEBOOK,
        GOOGLE,
        NAVER,
        TWITTER,
        LINE,
        PAYCO,
        PLAY_SERVICE,
        GAME_CENTER,
        GITHUB,
        YAHOO,
        MICROSOFT,
    }

    public enum ENUM_Login
    {
        NONE,
        GUEST,
        SERVICE_PROVIDER,
        EMAIL,
        PHONE,
    }

    public enum ENUM_InGameStatus
    {
        NONE,
        PLAYING,
        PAUSE,
        END,
    }

    public enum ENUM_CharacterSize
    {
        XS,
        S,
        M,
        L,
        XL,
    }

    public enum ENUM_Item
    {
        Consumable = 0,
        Costume = 1,
        Equipment = 2,
        Material = 3,
    }

    public enum ENUM_ItemSub
    {
        // Consumable
        Food = 0,

        // Costume
        FullCostume = 1,
        HairCostume = 2,
        EarCostume = 3,
        EyeCostume = 4,
        TailCostume = 5,

        // Equipment
        Weapon = 6,
        Armor = 7,
        Belt = 8,
        Necklace = 9,
        Ring = 10,

        // Material
        EquipmentMaterial = 11,
        FoodMaterial = 12,
        MonsterPart = 13,
        NormalMaterial = 14,
        Hourglass = 15,
        ApStone = 16,
        [Obsolete("ItemSubType.Chest has never been used outside the MaterialItemSheet. And we won't use it in the future until we have a specific reason.")]
        Chest = 17,

        // Costume
        Title = 18,
    }

    public enum ENUM_Elemental
    {
        Normal,
        Fire,
        Water,
        Land,
        Wind,
    }

    public enum ENUM_Stat
    {
        NONE,
        HP,
        CRI,
        ATK,
        DEF,
        HIT,
        SPD,
    }

    public enum ENUM_Modity
    {
        Add,
    }

    public enum ENUM_Event
    {
        None,
        Enhancement,
        Level,
        Die,
        Equipment,
        Complete,
    }

    public enum ENUM_GoldQuest
    {
        None,
        Buy,
        Sell,
    }

    public enum ENUM_TradeQuest
    {
        None,
        Buy,
        Sell,
    }

    public enum ENUM_Target
    {
        None,
        Self,
        Enemy,
        Enemies,
        Ally,
    }

    public enum ENUM_Skill
    {
        None,
        Attack,
        Buff,
        Debuff,
        Heal,
    }

    public enum ENUM_SkillCategory
    {
        None,
        AreaAttack,
        AttackBuff,
        BlowAttack,
        CriticalBuff,
        DefenseBuff,
        DoubleAttack,
        Heal,
        HitBuff,
        HPBuff,
        NormalAttack,
        SpeedBuff,
    }

    public enum ENUM_Reward
    {
        None,
        Item,
    }

    ///////////////////////////////////////////////////////////////////////////

    [MessagePackObject]
    public class ST_Auth
    {
        [Key(0)] public string UserId { get; set; }
        [Key(1)] public DateTime RegDate { get; set; }
        [Key(2)] public DateTime LastLoginDate { get; set; }
    }

    [MessagePackObject]
    public class ST_Device
    {
        [Key(0)] public ENUM_OS OS { get; set; }
        [Key(1)] public string OsVersion { get; set; }
        [Key(2)] public string Language { get; set; }
        [Key(3)] public string Country { get; set; }
        [Key(4)] public ENUM_Market Market { get; set; }
        [Key(5)] public ENUM_Platform Platform { get; set; }
        [Key(6)] public string PlatformSdkVersion { get; set; }
        [Key(7)] public ENUM_ServiceProvider ServiceProvider { get; set; }
        [Key(8)] public string ClientIp { get; set; }
        [Key(9)] public string AppVersion { get; set; }
        [Key(10)] public string UUID { get; set; }
        [Key(11)] public string DeviceName { get; set; }
        [Key(12)] public string Locale { get; set; }
        [Key(13)] public string GAID { get; set; }
        [Key(14)] public string IDFA { get; set; }
        [Key(15)] public string IDFV { get; set; }
        [Key(16)] public string AdjustDeviceId { get; set; }
        [Key(17)] public string FirebaseAnalyticsAppInstanceId { get; set; }
        [Key(18)] public string DeviceToken { get; set; }   // for fcm
        [Key(19)] public string ProcessorType { get; set; }
    }

    ///////////////////////////////////////////////////////////////////////////

    // 유저정보
    [MessagePackObject]
    public class ST_UserInfo
    {
        [Key(0)] public int Level { get; set; }
        [Key(1)] public int Exp { get; set; }
    }


    // 재화정보
    [MessagePackObject]
    public class ST_AssetInfo
    {
        [Key(0)] public int Cash { get; set; }
        [Key(1)] public int Gold { get; set; }
        [Key(2)] public int Gem { get; set; }
        [Key(3)] public int Mana { get; set; }
    }

    // 아바타정보
    [MessagePackObject]
    public class ST_Avatar
    {
        [Key(0)] public int Hair { get; set; }
        [Key(1)] public int Lens { get; set; }
        [Key(2)] public int Ear { get; set; }
        [Key(3)] public int Tail { get; set; }
        [Key(4)] public string Nickname { get; set; }
    }

    // TODO: 코스튬정보
    // TODO: 장비정보
    // TODO: 소비재정보
    
    // 보상정보
    [MessagePackObject]
    public class ST_Reward
    {
        [Key(0)] public int RewardId { get; set; }
        [Key(1)] public int RewardCount { get; set; }
    }

    // 알림정보
    [MessagePackObject]
    public class ST_QuestNotification : I_Notification
    {
        // type
        [Key(1)] public string Title { get; set; }
        [Key(2)] public string Explain { get; set; }
    }

    [MessagePackObject]
    public class ST_RewardNotification : I_Notification
    {
        // type
        [Key(1)] public string Title { get; set; }
        [Key(2)] public string Explain { get; set; }
        [Key(3)] public List<ST_Reward> RewardList { get; set; }
    }

    [MessagePackObject]
    public class ST_SocialNotification : I_Notification
    {
        // type
        [Key(1)] public string Title { get; set; }
        [Key(2)] public string Explain { get; set; }
    }

    [Union(1, typeof(ST_QuestNotification))]
    [Union(2, typeof(ST_RewardNotification))]
    [Union(3, typeof(ST_SocialNotification))]
    public interface I_Notification
    {

    }

    // 메일정보
    [MessagePackObject]
    public class ST_Mail
    {

    }

    // 인게임 보상정보
    [MessagePackObject]
    public class ST_InGameReward
    {
        [Key(0)] public DateTime Timestamp { get; set; }
        [Key(1)] public ST_Reward Reward { get; set; }
    }

    // 인게임 플레이 지표
    [MessagePackObject]
    public class ST_InGameMetric  
    {
        // 플레이 시간
        // 스킬 사용 정보
        // 잡은 몬스터 정보
    }


    ///////////////////////////////////////////////////////////////////////////
    public interface IREQ
    {
        REQ_Header Header { get; set; }
    }

    public interface IRES
    {
        RES_Header Header { get; set; }
    }

    ///////////////////////////////////////////////////////////////////////////
    // Messages
    // txId(request timestamp)
    // - Transaction Identifier
    // - Round Trip Time Calculate: currentTimestamp - response.header.txId
    [MessagePackObject]
    public class REQ_Header
    {
        [Key(0)] public DateTime Timestamp { get; set; } = DateTime.Now;
        [Key(1)] public long UserNo { get; set; }
    }

    [MessagePackObject]
    public class RES_Header
    {
        [Key(0)] public DateTime Timestamp { get; set; }
        [Key(1)] public ENUM_ErrorCode ErrorCode { get; set; } = ENUM_ErrorCode.ERR_SUCCESS;
        [Key(2)] public string ErrorMessage { get; set; }
    }
}
