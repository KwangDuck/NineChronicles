using System;
using System.Collections.Generic;
using Nekoyume.Model.State;
using Nekoyume.TableData;

namespace Nekoyume.Model.Quest
{
    [Serializable]
    public class QuestList : IState
    {
        // FIXME: Consider removing the `_listVersion` field.
        public const string ListVersionKey = "v";
        private int _listVersion = 1;
        public int ListVersion => _listVersion;

        public const string CompletedQuestIdsKey = "c";
        public List<int> completedQuestIds = new List<int>();

        public QuestList(QuestSheet questSheet,
            QuestRewardSheet questRewardSheet,
            QuestItemRewardSheet questItemRewardSheet,
            EquipmentItemRecipeSheet equipmentItemRecipeSheet,
            EquipmentItemSubRecipeSheet equipmentItemSubRecipeSheet
        )
        {
            
        }
    }
}
