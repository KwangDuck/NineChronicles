using MessagePack;
using System.Collections.Generic;

namespace Gateway.Protocol.Table
{
    #region Character
    [MessagePackObject]
    public class ST_TableCharacter : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public string _name { get; set; }
        [Key(2)] public ENUM_CharacterSize size_type { get; set; }
        [Key(3)] public int elemental_type { get; set; }
        [Key(4)] public int hp { get; set; }
        [Key(5)] public int atk { get; set; }
        [Key(6)] public int def { get; set; }
        [Key(7)] public int cri { get; set; }
        [Key(8)] public int hit { get; set; }
        [Key(9)] public int spd { get; set; }
        [Key(10)] public float lv_hp { get; set; }
        [Key(11)] public float lv_atk { get; set; }
        [Key(12)] public float lv_def { get; set; }
        [Key(13)] public float lv_cri { get; set; }
        [Key(14)] public float lv_hit { get; set; }
        [Key(15)] public float lv_spd { get; set; }
        [Key(16)] public float attack_range { get; set; }
        [Key(17)] public float run_speed { get; set; }
    }

    [MessagePackObject]
    public class ST_TableCharacterLevel : I_TableData
    {
        [Key(0)] public int level { get; set; }
        [Key(1)] public int exp { get; set; }
        [Key(2)] public int exp_need { get; set; }
    }
    #endregion

    #region Cost
    [MessagePackObject]
    public class ST_TableEnhancementCost : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public ENUM_ItemSub item_sub_type { get; set; }
        [Key(2)] public int grade { get; set; }
        [Key(3)] public int level { get; set; }
        [Key(4)] public int cost { get; set; }
        [Key(5)] public int success_ratio { get; set; }
        [Key(6)] public int great_success_ratio { get; set; }
        [Key(7)] public int fail_ratio { get; set; }
        [Key(8)] public int success_required_block_index { get; set; }
        [Key(9)] public int great_success_required_block_index { get; set; }
        [Key(10)] public int fail_required_block_index { get; set; }
        [Key(11)] public int base_stat_growth_min { get; set; }
        [Key(12)] public int base_stat_growth_max { get; set; }
        [Key(13)] public int extra_stat_growth_min { get; set; }
        [Key(14)] public int extra_stat_growth_max { get; set; }
        [Key(15)] public int extra_skill_damage_growth_min { get; set; }
        [Key(16)] public int extra_skill_damage_growth_max { get; set; }
        [Key(17)] public int extra_skill_chance_growth_min { get; set; }
        [Key(18)] public int extra_skill_chance_growth_max { get; set; }
    }
    #endregion

    #region Item
    [MessagePackObject]
    public class ST_TableConsumableItemRecipe : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public int required_block_index { get; set; }
        [Key(2)] public int required_ap { get; set; }
        [Key(3)] public int required_gold { get; set; }
        [Key(4)] public int? material_item_id_1 { get; set; }
        [Key(5)] public int? material_item_count_1 { get; set; }
        [Key(6)] public int? material_item_id_2 { get; set; }
        [Key(7)] public int? material_item_count_2 { get; set; }
        [Key(8)] public int? material_item_id_3 { get; set; }
        [Key(9)] public int? material_item_count_3 { get; set; }
        [Key(10)] public int? material_item_id_4 { get; set; }
        [Key(11)] public int? material_item_count_4 { get; set; }
        [Key(12)] public int result_consumable_item_id { get; set; }
    }

    [MessagePackObject]
    public class ST_TableConsumableItem : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public string _name { get; set; }
        [Key(2)] public ENUM_ItemSub item_sub_type { get; set; }
        [Key(3)] public int grade { get; set; }
        [Key(4)] public ENUM_Elemental elemental_type { get; set; }
        [Key(5)] public string stat_type_1 { get; set; }
        [Key(6)] public int? stat_value_1 { get; set; }
        [Key(7)] public string stat_type_2 { get; set; }
        [Key(8)] public int? stat_value_2 { get; set; }
    }

    [MessagePackObject]
    public class ST_TableCostumeItem : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public string _name { get; set; }
        [Key(2)] public ENUM_ItemSub item_sub_type { get; set; }
        [Key(3)] public int grade { get; set; }
        [Key(4)] public ENUM_Elemental elemental_type { get; set; }
        [Key(5)] public string spine_resource_path { get; set; }
    }

    [MessagePackObject]
    public class ST_TableCostumeStat : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public int costume_id { get; set; }
        [Key(2)] public ENUM_Stat stat_type { get; set; }
        [Key(3)] public int stat { get; set; }
    }

    [MessagePackObject]
    public class ST_TableEquipmentItemOption : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public string stat_type { get; set; }
        [Key(2)] public int? stat_min { get; set; }
        [Key(3)] public int? stat_max { get; set; }
        [Key(4)] public int? skill_id { get; set; }
        [Key(5)] public int? skill_damage_min { get; set; }
        [Key(6)] public int? skill_damage_max { get; set; }
        [Key(7)] public int? skill_chance_min { get; set; }
        [Key(8)] public int? skill_chance_max { get; set; }
    }

    [MessagePackObject]
    public class ST_TableEquipmentItemRecipe : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public int result_equipment_id { get; set; }
        [Key(2)] public int material_id { get; set; }
        [Key(3)] public int material_count { get; set; }
        [Key(4)] public int required_action_point { get; set; }
        [Key(5)] public int required_gold { get; set; }
        [Key(6)] public int required_block_index { get; set; }
        [Key(7)] public int unlock_stage { get; set; }
        [Key(8)] public int sub_recipe_id { get; set; }
        [Key(9)] public int sub_recipe_id_2 { get; set; }
        [Key(10)] public int sub_recipe_id_3 { get; set; }
    }

    [MessagePackObject]
    public class ST_TableEquipmentItemSetEffect : I_TableData
    {
        [Key(0)] public int id { get; set; }
        public int count { get; set; }
        public ENUM_Stat stat_type { get; set; }
        public ENUM_Modity modify_type { get; set; }
        public int modify_value { get; set; }
    }

    [MessagePackObject]
    public class ST_TableEquipmentItem : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public string _name { get; set; }
        [Key(2)] public ENUM_ItemSub item_sub_type { get; set; }
        [Key(3)] public int grade { get; set; }
        [Key(4)] public ENUM_Elemental elemental_type { get; set; }
        [Key(5)] public int _set_id { get; set; }
        [Key(6)] public ENUM_Stat stat_type { get; set; }
        [Key(7)] public int stat_value { get; set; }
        [Key(8)] public int attak_range { get; set; }
        [Key(9)] public string spine_resource_path { get; set; }
    }

    [MessagePackObject]
    public class ST_TableEquipmentItemSubRecipe : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public int required_action_point { get; set; }
        [Key(2)] public int required_gold { get; set; }
        [Key(3)] public int required_block_index { get; set; }
        [Key(4)] public int? material_id { get; set; }
        [Key(5)] public int? material_count { get; set; }
        [Key(6)] public int? material_2_id { get; set; }
        [Key(7)] public int? material_2_count { get; set; }
        [Key(8)] public int? material_3_id { get; set; }
        [Key(9)] public int? material_3_count { get; set; }
        [Key(10)] public int? option_id { get; set; }
        [Key(11)] public int? option_ratio { get; set; }
        [Key(12)] public int? option_1_required_block_index { get; set; }
        [Key(13)] public int? option_2_id { get; set; }
        [Key(14)] public int? option_2_ratio { get; set; }
        [Key(15)] public int? option_2_required_block_index { get; set; }
        [Key(16)] public int? option_3_id { get; set; }
        [Key(17)] public int? option_3_ratio { get; set; }
        [Key(18)] public int? option_3_required_block_index { get; set; }
        [Key(19)] public int? option_4_id { get; set; }
        [Key(20)] public int? option_4_ratio { get; set; }
        [Key(21)] public int? option_4_required_block_index { get; set; }
    }

    [MessagePackObject]
    public class ST_TableItemConfigForGrade : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public int monster_parts_count_for_combination { get; set; }
        [Key(2)] public int monster_parts_count_for_combination_with_ncg { get; set; }
        [Key(3)] public int random_buff_skill_min_count_for_combination { get; set; }
        [Key(4)] public int random_buff_skill_max_count_for_combination { get; set; }
        [Key(5)] public int enhancement_limit { get; set; }
    }

    [MessagePackObject]
    public class ST_TableItemRequirement : I_TableData
    {
        [Key(0)] public int item_id { get; set; }
        [Key(1)] public int level { get; set; }
    }

    [MessagePackObject]
    public class ST_TableMaterialItem : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public string _name { get; set; }
        [Key(2)] public ENUM_ItemSub item_sub_type { get; set; }
        [Key(3)] public int grade { get; set; }
        [Key(4)] public ENUM_Elemental elemental_type { get; set; }
    }
    #endregion

    #region Quest
    [MessagePackObject]
    public class ST_TableCollectQuest : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public int goal { get; set; }
        [Key(2)] public int quest_reward_id { get; set; }
        [Key(3)] public int item_id { get; set; }
    }

    [MessagePackObject]
    public class ST_TableCombinationEquipmentQuest : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public int goal { get; set; }
        [Key(2)] public int quest_reward_id { get; set; }
        [Key(3)] public int receipt_id { get; set; }
    }

    [MessagePackObject]
    public class ST_TableCombinationQuest : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public int goal { get; set; }
        [Key(2)] public int quest_reward_id { get; set; }
        [Key(3)] public ENUM_Item item_type { get; set; }
        [Key(4)] public ENUM_ItemSub item_sub_type { get; set; }
    }

    [MessagePackObject]
    public class ST_TableGeneralQuest : I_TableData
    {
        [Key(0)] public string id { get; set; }
        [Key(1)] public int goal { get; set; }
        [Key(2)] public int quest_reward_id { get; set; }
        [Key(3)] public ENUM_Event @event { get; set; }
    }

    [MessagePackObject]
    public class ST_TableGoldQuest : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public int goal { get; set; }
        [Key(2)] public int quest_reward_id { get; set; }
        [Key(3)] public ENUM_GoldQuest type { get; set; }
    }

    [MessagePackObject]
    public class ST_TableItemEnhancementQuest : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public int goal { get; set; }
        [Key(2)] public int quest_reward_id { get; set; }
        [Key(3)] public int grade { get; set; }
        [Key(4)] public int count { get; set; }
    }

    [MessagePackObject]
    public class ST_TableItemGradeQuest : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public int goal { get; set; }
        [Key(2)] public int quest_reward_id { get; set; }
        [Key(3)] public int grade { get; set; }
    }

    [MessagePackObject]
    public class ST_TableItemTypeCollectQuest : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public int goal { get; set; }
        [Key(2)] public int quest_reward_id { get; set; }
        [Key(3)] public ENUM_Item item_type { get; set; }
    }

    [MessagePackObject]
    public class ST_TableMonsterQuest : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public int goal { get; set; }
        [Key(2)] public int quest_reward_id { get; set; }
        [Key(3)] public int monster_id { get; set; }
    }

    [MessagePackObject]
    public class ST_TableQuestItemReward : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public int item_id { get; set; }
        [Key(2)] public int count { get; set; }
    }

    [MessagePackObject]
    public class ST_TableQuestReward : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public int reward_id { get; set; }
    }

    [MessagePackObject]
    public class ST_TableTradeQuest : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public int goal { get; set; }
        [Key(2)] public int quest_reward_id { get; set; }
        [Key(3)] public ENUM_TradeQuest type { get; set; }
    }

    [MessagePackObject]
    public class ST_TableWorldQuest : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public int goal { get; set; }
        [Key(2)] public int quest_reward_id { get; set; }
    }
    #endregion

    #region Skill
    [MessagePackObject]
    public class ST_TableBuff : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public int group { get; set; }
        [Key(2)] public string _name { get; set; }
        [Key(3)] public int chance { get; set; }
        [Key(4)] public int duration { get; set; }
        [Key(5)] public ENUM_Target target_type { get; set; }
        [Key(6)] public ENUM_Stat stat_type { get; set; }
        [Key(7)] public ENUM_Modity modify_type { get; set; }
        [Key(8)] public int modify_value { get; set; }
        [Key(9)] public string icon_resource { get; set; }
    }

    [MessagePackObject]
    public class ST_TableEnemySkill : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public int character_id { get; set; }
        [Key(2)] public int skill_id { get; set; }
    }

    [MessagePackObject]
    public class ST_TableSkillBuff : I_TableData
    {
        [Key(0)] public int skill_id { get; set; }
        [Key(1)] public int buff_id { get; set; }
    }

    [MessagePackObject]
    public class ST_TableSkill : I_TableData
    {
        [Key(0)] public string id { get; set; }
        [Key(1)] public string _name { get; set; }
        [Key(2)] public ENUM_Elemental elemental_type { get; set; }
        [Key(3)] public ENUM_Skill skill_type { get; set; }
        [Key(4)] public ENUM_SkillCategory skill_category { get; set; }
        [Key(5)] public ENUM_Target skill_target_type { get; set; }
        [Key(6)] public int hit_count { get; set; }
        [Key(7)] public string cooldown { get; set; }
        
    }
    #endregion

    #region WorldAndStage
    [MessagePackObject]
    public class ST_TableMimisbrunnr : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public string elemental_type_1 { get; set; }
        [Key(2)] public string elemental_type_2 { get; set; }
        [Key(3)] public string elemental_type_3 { get; set; }
        [Key(4)] public string elemental_type_4 { get; set; }
        [Key(5)] public string elemental_type_5 { get; set; }
    }

    [MessagePackObject]
    public class ST_TableStageDialog : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public int stage_id { get; set; }
        [Key(2)] public int dialog_id { get; set; }
    }

    [MessagePackObject]
    public class ST_TableStage : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public int cost_ap { get; set; }
        [Key(2)] public int turn_limit { get; set; }
        [Key(3)] public int hp_option { get; set; }
        [Key(4)] public int atk_option { get; set; }
        [Key(5)] public int def_option { get; set; }
        [Key(6)] public int cri_option { get; set; }
        [Key(7)] public int hit_option { get; set; }
        [Key(8)] public int spd_option { get; set; }
        [Key(9)] public string background { get; set; }
        [Key(10)] public string bgm { get; set; }
        [Key(11)] public int? item1 { get; set; }
        [Key(12)] public float? item1_ratio { get; set; }
        [Key(13)] public int? item1_min { get; set; }
        [Key(14)] public int? item1_max { get; set; }
        [Key(15)] public int? item2 { get; set; }
        [Key(16)] public float? item2_ratio { get; set; }
        [Key(17)] public int? item2_min { get; set; }
        [Key(18)] public int? item2_max { get; set; }
        [Key(19)] public int? item3 { get; set; }
        [Key(20)] public float? item3_ratio { get; set; }
        [Key(21)] public int? item3_min { get; set; }
        [Key(22)] public int? item3_max { get; set; }
        [Key(23)] public int? item4 { get; set; }
        [Key(24)] public float? item4_ratio { get; set; }
        [Key(25)] public int? item4_min { get; set; }
        [Key(26)] public int? item4_max { get; set; }
        [Key(27)] public int? item5 { get; set; }
        [Key(28)] public float? item5_ratio { get; set; }
        [Key(29)] public int? item5_min { get; set; }
        [Key(30)] public int? item5_max { get; set; }
        [Key(31)] public int? item6 { get; set; }
        [Key(32)] public float? item6_ratio { get; set; }
        [Key(33)] public int? item6_min { get; set; }
        [Key(34)] public int? item6_max { get; set; }
        [Key(35)] public int? item7 { get; set; }
        [Key(36)] public float? item7_ratio { get; set; }
        [Key(37)] public int? item7_min { get; set; }
        [Key(38)] public int? item7_max { get; set; }
        [Key(39)] public int? item8 { get; set; }
        [Key(40)] public float? item8_ratio { get; set; }
        [Key(41)] public int? item8_min { get; set; }
        [Key(42)] public int? item8_max { get; set; }
        [Key(43)] public int? item9 { get; set; }
        [Key(44)] public float? item9_ratio { get; set; }
        [Key(45)] public int? item9_min { get; set; }
        [Key(46)] public int? item9_max { get; set; }
        [Key(47)] public int? item10 { get; set; }
        [Key(48)] public float? item10_ratio { get; set; }
        [Key(49)] public int? item10_min { get; set; }
        [Key(50)] public int? item10_max { get; set; }
        [Key(51)] public int? min_drop { get; set; }
        [Key(52)] public int? max_drop { get; set; }
    }

    [MessagePackObject]
    public class ST_TableStageWave : I_TableData
    {
        [IgnoreMember] public int id => stage_id * 100 + wave;
        [Key(0)] public int stage_id { get; set; }
        [Key(1)] public int wave { get; set; }
        [Key(2)] public int monster1_id { get; set; }
        [Key(3)] public int monster1_level { get; set; }
        [Key(4)] public int monster1_count { get; set; }
        [Key(5)] public int? monster2_id { get; set; }
        [Key(6)] public int? monster2_level { get; set; }
        [Key(7)] public int? monster2_count { get; set; }
        [Key(8)] public int? monster3_id { get; set; }
        [Key(9)] public int? monster3_level { get; set; }
        [Key(10)] public int? monster3_count { get; set; }
        [Key(11)] public int? monster4_id { get; set; }
        [Key(12)] public int? monster4_level { get; set; }
        [Key(13)] public int? monster4_count { get; set; }
        [Key(14)] public int? has_boss { get; set; }
    }

    [MessagePackObject]
    public class ST_TableWorld : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public string name { get; set; }
        [Key(2)] public int stage_begin { get; set; }
        [Key(3)] public int stage_end { get; set; }
    }

    [MessagePackObject]
    public class ST_TableWorldUnlock : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public int world_id { get; set; }
        [Key(2)] public int stage_id { get; set; }
        [Key(3)] public int world_id_to_unlock { get; set; }
    }
    #endregion

    #region System
    [MessagePackObject]
    public class ST_TableGameConfig : I_TableData
    {
        [Key(0)] public string key { get; set; }
        [Key(1)] public string value { get; set; }
    }

    [MessagePackObject]
    public class ST_TableMonsterCollectionReward : I_TableData
    {
        [Key(0)] public int collection_level { get; set; }
        [Key(1)] public int item_id { get; set; }
        [Key(2)] public int quantity { get; set; }
    }

    [MessagePackObject]
    public class ST_TableMonsterCollection : I_TableData
    {
        [Key(0)] public int level { get; set; }
        [Key(1)] public int required_gold { get; set; }
        [Key(2)] public int reward_id { get; set; }
    }

    [MessagePackObject]
    public class ST_TableWeeklyArenaReward : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public int item_id { get; set; }
        [Key(2)] public float ratio { get; set; }
        [Key(3)] public int min { get; set; }
        [Key(4)] public int max { get; set; }
        [Key(5)] public int required_level { get; set; }
    }

    [MessagePackObject]
    public class ST_TableRedeemCodeList : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public int reward_id { get; set; }
        [Key(2)] public string public_key { get; set; }
    }

    [MessagePackObject]
    public class ST_TableRedeemReward : I_TableData
    {
        [Key(0)] public int id { get; set; }
        [Key(1)] public ENUM_Reward type { get; set; }
        [Key(2)] public int qty { get; set; }
        [Key(3)] public int item_id { get; set; }
    }
    #endregion

    // Character
    [Union(1, typeof(ST_TableCharacter))]
    [Union(2, typeof(ST_TableCharacterLevel))]
    // Cost
    [Union(11, typeof(ST_TableEnhancementCost))]
    // Item
    [Union(21, typeof(ST_TableConsumableItemRecipe))]
    [Union(22, typeof(ST_TableConsumableItem))]
    [Union(23, typeof(ST_TableCostumeItem))]
    [Union(24, typeof(ST_TableCostumeStat))]
    [Union(25, typeof(ST_TableEquipmentItemOption))]
    [Union(26, typeof(ST_TableEquipmentItemRecipe))]
    [Union(27, typeof(ST_TableEquipmentItemSetEffect))]
    [Union(28, typeof(ST_TableEquipmentItem))]
    [Union(29, typeof(ST_TableEquipmentItemSubRecipe))]
    [Union(30, typeof(ST_TableItemConfigForGrade))]
    [Union(31, typeof(ST_TableItemRequirement))]
    [Union(32, typeof(ST_TableMaterialItem))]
    // Quest
    [Union(41, typeof(ST_TableCollectQuest))]
    [Union(42, typeof(ST_TableCombinationEquipmentQuest))]
    [Union(43, typeof(ST_TableCombinationQuest))]
    [Union(44, typeof(ST_TableGeneralQuest))]
    [Union(45, typeof(ST_TableGoldQuest))]
    [Union(46, typeof(ST_TableItemEnhancementQuest))]
    [Union(47, typeof(ST_TableItemGradeQuest))]
    [Union(48, typeof(ST_TableItemTypeCollectQuest))]
    [Union(49, typeof(ST_TableMonsterCollection))]
    [Union(50, typeof(ST_TableQuestItemReward))]
    [Union(51, typeof(ST_TableQuestReward))]
    [Union(52, typeof(ST_TableTradeQuest))]
    [Union(53, typeof(ST_TableWorldQuest))]
    // Skill
    [Union(61, typeof(ST_TableBuff))]
    [Union(62, typeof(ST_TableEnemySkill))]
    [Union(63, typeof(ST_TableSkillBuff))]
    [Union(64, typeof(ST_TableSkill))]
    // WorldAndStage
    [Union(71, typeof(ST_TableMimisbrunnr))]
    [Union(72, typeof(ST_TableStageDialog))]
    [Union(73, typeof(ST_TableStage))]
    [Union(74, typeof(ST_TableStageWave))]
    [Union(75, typeof(ST_TableWorld))]
    [Union(76, typeof(ST_TableWorldUnlock))]
    // System
    [Union(91, typeof(ST_TableGameConfig))]
    [Union(92, typeof(ST_TableMonsterCollectionReward))]
    [Union(93, typeof(ST_TableMonsterCollection))]
    [Union(94, typeof(ST_TableWeeklyArenaReward))]
    [Union(95, typeof(ST_TableRedeemCodeList))]
    [Union(96, typeof(ST_TableRedeemReward))]
    public interface I_TableData
    {

    }

    [MessagePackObject]
    public class ST_Table
    {
        [Key(0)] public IReadOnlyList<I_TableData> dataList { get; set; }
        [Key(1)] public long version { get; set; }
    }

}
