using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Gateway.Protocol.Table;
using Gateway.Domain.GameContext.Descriptor;

public partial class DescriptorContext : MonoBehaviour
{
    #region Character
    public CharacterDescriptor.Loader characterDescLoader;
    public CharacterLevelDescriptor.Loader characterLevelDescLoader;
    #endregion Character
    #region Cost
    public EnhancementCostDescriptor.Loader enhancementCostDescLoader;
    #endregion Cost
    #region Item
    public ConsumableItemRecipeDescriptor.Loader consumableItemRecipeDescLoader;
    public ConsumableItemDescriptor.Loader consumableItemDescLoader;
    public CostumeItemDescriptor.Loader costumeItemDescLoader;
    public CostumeStatDescriptor.Loader costumeStatDescLoader;
    public EquipmentItemOptionDescriptor.Loader equipmentItemOptionDescLoader;
    public EquipmentItemRecipeDescriptor.Loader equipmentItemRecipeDescLoader;
    public EquipmentItemSetEffectDescriptor.Loader equipmentItemSetEffectDescLoader;
    public EquipmentItemDescriptor.Loader equipmentItemDescLoader;
    public EquipmentItemSubRecipeDescriptor.Loader equipmentItemSubRecipeDescLoader;
    public ItemConfigForGradeDescriptor.Loader itemConfigForGradeDescLoader;
    public ItemRequirementDescriptor.Loader itemRequirementDescLoader;
    public MaterialItemDescriptor.Loader materialItemDescLoader;
    #endregion Item
    #region Quest
    public CollectQuestDescriptor.Loader collectQuestDescLoader;
    public CombinationEquipmentQuestDescriptor.Loader combinationEquipmentQuestDescLoader;
    public CombinationQuestDescriptor.Loader combinationQuestDescLoader;
    public GeneralQuestDescriptor.Loader generalQuestDescLoader;
    public GoldQuestDescriptor.Loader goldQuestDescLoader;
    public ItemEnhancementQuestDescriptor.Loader itemEnhancementQuestDescLoader;
    public ItemGradeQuestDescriptor.Loader itemGradeQuestDescLoader;
    public ItemTypeCollectQuestDescriptor.Loader itemTypeCollectQuestDescLoader;
    public MonsterQuestDescriptor.Loader monsterQuestDescLoader;
    public QuestItemRewardDescriptor.Loader questItemRewardDescLoader;
    public QuestRewardDescriptor.Loader questRewardDescLoader;
    public TradeQuestDescriptor.Loader tradeQuestDescLoader;
    public WorldQuestDescriptor.Loader worldQuestDescLoader;
    #endregion Quest
    #region Skill
    public BuffDescriptor.Loader buffDescLoader;
    public EnemySkillDescriptor.Loader enemySkillDescLoader;
    public SkillBuffDescriptor.Loader skillBuffDescLoader;
    public SkillDescriptor.Loader skillDescLoader;
    #endregion Skill
    #region  World & Stage
    public MimisbrunnrDescriptor.Loader mimisbrunnrDescLoader;
    public StageDialogDescriptor.Loader stageDialogDescLoader;
    public StageDescriptor.Loader stageDescLoader;
    public StageWaveDescriptor.Loader stageWaveDescLoader;
    public WorldDescriptor.Loader worldDescLoader;
    public WorldUnlockDescriptor.Loader worldUnlockDescLoader;
    #endregion World & Stage
    
    public IEnumerator DescriptorLoad(Dictionary<string, ST_Table> tableMap)
    {
        characterDescLoader = new CharacterDescriptor.Loader(characterDescManager, tableMap);
        characterDescLoader.Compile();

        characterLevelDescLoader = new CharacterLevelDescriptor.Loader(characterLevelDescManager, tableMap);
        characterLevelDescLoader.Compile();
        
        enhancementCostDescLoader = new EnhancementCostDescriptor.Loader(enhancementCostDescManager, tableMap);
        enhancementCostDescLoader.Compile();
        
        consumableItemRecipeDescLoader = new ConsumableItemRecipeDescriptor.Loader(consumableItemRecipeDescManager, tableMap);
        consumableItemRecipeDescLoader.Compile();

        consumableItemDescLoader = new ConsumableItemDescriptor.Loader(consumableItemDescManager, tableMap);
        consumableItemDescLoader.Compile();
        
        costumeItemDescLoader = new CostumeItemDescriptor.Loader(costumeItemDescManager, tableMap);
        costumeItemDescLoader.Compile();

        costumeStatDescLoader = new CostumeStatDescriptor.Loader(costumeStatDescManager, tableMap);
        costumeStatDescLoader.Compile();
        
        equipmentItemOptionDescLoader = new EquipmentItemOptionDescriptor.Loader(equipmentItemOptionDescManager, tableMap);
        equipmentItemOptionDescLoader.Compile();

        equipmentItemRecipeDescLoader = new EquipmentItemRecipeDescriptor.Loader(equipmentItemRecipeDescManager, tableMap);
        equipmentItemRecipeDescLoader.Compile();

        equipmentItemSetEffectDescLoader = new EquipmentItemSetEffectDescriptor.Loader(equipmentItemSetEffectDescManager, tableMap);
        equipmentItemSetEffectDescLoader.Compile();
        
        equipmentItemDescLoader = new EquipmentItemDescriptor.Loader(equipmentItemDescManager, tableMap);
        equipmentItemDescLoader.Compile();

        equipmentItemSubRecipeDescLoader = new EquipmentItemSubRecipeDescriptor.Loader(equipmentItemSubRecipeDescManager, tableMap);
        equipmentItemSubRecipeDescLoader.Compile();

        itemConfigForGradeDescLoader = new ItemConfigForGradeDescriptor.Loader(itemConfigForGradeDescManager, tableMap);
        itemConfigForGradeDescLoader.Compile();

        itemRequirementDescLoader = new ItemRequirementDescriptor.Loader(itemRequirementDescManager, tableMap);
        itemRequirementDescLoader.Compile();

        materialItemDescLoader = new MaterialItemDescriptor.Loader(materialItemDescManager, tableMap);
        materialItemDescLoader.Compile();
        
        collectQuestDescLoader = new CollectQuestDescriptor.Loader(collectQuestDescManager, tableMap);
        collectQuestDescLoader.Compile();

        combinationEquipmentQuestDescLoader = new CombinationEquipmentQuestDescriptor.Loader(combinationEquipmentQuestDescManager, tableMap);
        combinationEquipmentQuestDescLoader.Compile();

        combinationQuestDescLoader = new CombinationQuestDescriptor.Loader(combinationQuestDescManager, tableMap);
        combinationQuestDescLoader.Compile();

        generalQuestDescLoader = new GeneralQuestDescriptor.Loader(generalQuestDescManager, tableMap);
        generalQuestDescLoader.Compile();

        goldQuestDescLoader = new GoldQuestDescriptor.Loader(goldQuestDescManager, tableMap);
        goldQuestDescLoader.Compile();
        
        itemEnhancementQuestDescLoader = new ItemEnhancementQuestDescriptor.Loader(itemEnhancementQuestDescManager, tableMap);
        itemEnhancementQuestDescLoader.Compile();

        itemGradeQuestDescLoader = new ItemGradeQuestDescriptor.Loader(itemGradeQuestDescManager, tableMap);
        itemGradeQuestDescLoader.Compile();
        
        itemTypeCollectQuestDescLoader = new ItemTypeCollectQuestDescriptor.Loader(itemTypeCollectQuestDescManager, tableMap);
        itemTypeCollectQuestDescLoader.Compile();
        
        monsterQuestDescLoader = new MonsterQuestDescriptor.Loader(monsterQuestDescManager, tableMap);
        monsterQuestDescLoader.Compile();
        
        questItemRewardDescLoader = new QuestItemRewardDescriptor.Loader(questItemRewardDescManager, tableMap);
        questItemRewardDescLoader.Compile();
        
        questRewardDescLoader = new QuestRewardDescriptor.Loader(questRewardDescManager, tableMap);
        questRewardDescLoader.Compile();
        
        tradeQuestDescLoader = new TradeQuestDescriptor.Loader(tradeQuestDescManager, tableMap);
        tradeQuestDescLoader.Compile();

        worldQuestDescLoader = new WorldQuestDescriptor.Loader(worldQuestDescManager, tableMap);
        worldQuestDescLoader.Compile();

        buffDescLoader = new BuffDescriptor.Loader(buffDescManager, tableMap);
        buffDescLoader.Compile();

        enemySkillDescLoader = new EnemySkillDescriptor.Loader(enemySkillDescManager, tableMap);
        enemySkillDescLoader.Compile();

        skillBuffDescLoader = new SkillBuffDescriptor.Loader(skillBuffDescManager, tableMap);
        skillBuffDescLoader.Compile();

        skillDescLoader = new SkillDescriptor.Loader(skillDescManager, tableMap);
        skillDescLoader.Compile();

        mimisbrunnrDescLoader = new MimisbrunnrDescriptor.Loader(mimisbrunnrDescManager, tableMap);
        mimisbrunnrDescLoader.Compile();

        stageDialogDescLoader = new StageDialogDescriptor.Loader(stageDialogDescManager, tableMap);
        stageDialogDescLoader.Compile();

        stageDescLoader = new StageDescriptor.Loader(stageDescManager, tableMap);
        stageDescLoader.Compile();

        stageWaveDescLoader = new StageWaveDescriptor.Loader(stageWaveDescManager, tableMap);
        stageWaveDescLoader.Compile();

        worldDescLoader = new WorldDescriptor.Loader(worldDescManager, tableMap);
        worldDescLoader.Compile();
        
        worldUnlockDescLoader = new WorldUnlockDescriptor.Loader(worldUnlockDescManager, tableMap);        
        worldUnlockDescLoader.Compile();

        yield return null;
    }
}