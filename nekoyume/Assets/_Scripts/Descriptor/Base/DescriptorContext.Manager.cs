using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Gateway.Protocol.Table;
using Gateway.Domain.GameContext.Descriptor;

public partial class DescriptorContext : MonoBehaviour
{    
    #region Character
    public CharacterDescriptor.Manager characterDescManager { get; private set; } = new CharacterDescriptor.Manager();
    public CharacterLevelDescriptor.Manager characterLevelDescManager { get; private set; } = new CharacterLevelDescriptor.Manager();
    #endregion Character
    #region Cost
    public EnhancementCostDescriptor.Manager enhancementCostDescManager { get; private set; } = new EnhancementCostDescriptor.Manager();
    #endregion Cost
    #region Item
    public ConsumableItemRecipeDescriptor.Manager consumableItemRecipeDescManager { get; private set; } = new ConsumableItemRecipeDescriptor.Manager();
    public ConsumableItemDescriptor.Manager consumableItemDescManager { get; private set; } = new ConsumableItemDescriptor.Manager();
    public CostumeItemDescriptor.Manager costumeItemDescManager { get; private set; } = new CostumeItemDescriptor.Manager();
    public CostumeStatDescriptor.Manager costumeStatDescManager { get; private set; } = new CostumeStatDescriptor.Manager();
    public EquipmentItemOptionDescriptor.Manager equipmentItemOptionDescManager { get; private set; } = new EquipmentItemOptionDescriptor.Manager();
    public EquipmentItemRecipeDescriptor.Manager equipmentItemRecipeDescManager { get; private set; } = new EquipmentItemRecipeDescriptor.Manager();
    public EquipmentItemSetEffectDescriptor.Manager equipmentItemSetEffectDescManager { get; private set; } = new EquipmentItemSetEffectDescriptor.Manager();
    public EquipmentItemDescriptor.Manager equipmentItemDescManager { get; private set; } = new EquipmentItemDescriptor.Manager();
    public EquipmentItemSubRecipeDescriptor.Manager equipmentItemSubRecipeDescManager { get; private set; } = new EquipmentItemSubRecipeDescriptor.Manager();
    public ItemConfigForGradeDescriptor.Manager itemConfigForGradeDescManager { get; private set; } = new ItemConfigForGradeDescriptor.Manager();
    public ItemRequirementDescriptor.Manager itemRequirementDescManager { get; private set; } = new ItemRequirementDescriptor.Manager();
    public MaterialItemDescriptor.Manager materialItemDescManager { get; private set; } = new MaterialItemDescriptor.Manager();
    #endregion Item
    #region Quest
    public CollectQuestDescriptor.Manager collectQuestDescManager { get; private set; } = new CollectQuestDescriptor.Manager();
    public CombinationEquipmentQuestDescriptor.Manager combinationEquipmentQuestDescManager { get; private set; } = new CombinationEquipmentQuestDescriptor.Manager();
    public CombinationQuestDescriptor.Manager combinationQuestDescManager { get; private set; } = new CombinationQuestDescriptor.Manager();
    public GeneralQuestDescriptor.Manager generalQuestDescManager { get; private set; } = new GeneralQuestDescriptor.Manager();
    public GoldQuestDescriptor.Manager goldQuestDescManager { get; private set; } = new GoldQuestDescriptor.Manager();
    public ItemEnhancementQuestDescriptor.Manager itemEnhancementQuestDescManager { get; private set; } = new ItemEnhancementQuestDescriptor.Manager();
    public ItemGradeQuestDescriptor.Manager itemGradeQuestDescManager { get; private set; } = new ItemGradeQuestDescriptor.Manager();
    public ItemTypeCollectQuestDescriptor.Manager itemTypeCollectQuestDescManager { get; private set; } = new ItemTypeCollectQuestDescriptor.Manager();
    public MonsterQuestDescriptor.Manager monsterQuestDescManager { get; private set; } = new MonsterQuestDescriptor.Manager();
    public QuestItemRewardDescriptor.Manager questItemRewardDescManager { get; private set; } = new QuestItemRewardDescriptor.Manager();
    public QuestRewardDescriptor.Manager questRewardDescManager { get; private set; } = new QuestRewardDescriptor.Manager();
    public TradeQuestDescriptor.Manager tradeQuestDescManager { get; private set; } = new TradeQuestDescriptor.Manager();
    public WorldQuestDescriptor.Manager worldQuestDescManager { get; private set; } = new WorldQuestDescriptor.Manager();    
    #endregion Quest
    #region Skill
    public BuffDescriptor.Manager buffDescManager { get; private set; } = new BuffDescriptor.Manager();
    public EnemySkillDescriptor.Manager enemySkillDescManager { get; private set; } = new EnemySkillDescriptor.Manager();
    public SkillBuffDescriptor.Manager skillBuffDescManager { get; private set; } = new SkillBuffDescriptor.Manager();
    public SkillDescriptor.Manager skillDescManager { get; private set; } = new SkillDescriptor.Manager();
    #endregion Skill
    #region  World & Stage
    public MimisbrunnrDescriptor.Manager mimisbrunnrDescManager { get; private set; } = new MimisbrunnrDescriptor.Manager();
    public StageDialogDescriptor.Manager stageDialogDescManager { get; private set; } = new StageDialogDescriptor.Manager();
    public StageDescriptor.Manager stageDescManager { get; private set; } = new StageDescriptor.Manager();
    public StageWaveDescriptor.Manager stageWaveDescManager { get; private set; } = new StageWaveDescriptor.Manager();    
    public WorldDescriptor.Manager worldDescManager { get; private set; } = new WorldDescriptor.Manager();
    public WorldUnlockDescriptor.Manager worldUnlockDescManager { get; private set; } = new WorldUnlockDescriptor.Manager();
    #endregion World & Stage    
}