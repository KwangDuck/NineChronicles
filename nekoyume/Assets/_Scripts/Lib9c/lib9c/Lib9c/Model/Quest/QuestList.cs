using System;
using System.Collections.Generic;
using Nekoyume.Model.Item;
using Nekoyume.Model.State;
using Nekoyume.TableData;

namespace Nekoyume.Model.Quest
{
    [Serializable]
    public class QuestList : IState
    {
        private readonly List<Quest> _quests = new List<Quest>();
        public List<int> completedQuestIds { get; private set; } = new List<int>();

        public QuestList(QuestSheet questSheet,
            QuestRewardSheet questRewardSheet,
            QuestItemRewardSheet questItemRewardSheet,
            EquipmentItemRecipeSheet equipmentItemRecipeSheet,
            EquipmentItemSubRecipeSheet equipmentItemSubRecipeSheet
        )
        {
            
        }

        public void UpdateStageQuest(CollectionMap stageMap)
        {

        }

        public void UpdateMonsterQuest(CollectionMap monsterMap)
        {

        }

        public void UpdateCollectQuest(CollectionMap itemMap)
        {

        }

        public void UpdateItemTypeCollectQuest(IEnumerable<ItemBase> items)
        {

        }

        public CollectionMap UpdateGeneralQuest(IEnumerable<QuestEventType> types, CollectionMap eventMap)
        {
            return eventMap;
        }

        public CollectionMap UpdateCompletedQuest(CollectionMap eventMap)
        {
            return eventMap;
        }
    }
}
