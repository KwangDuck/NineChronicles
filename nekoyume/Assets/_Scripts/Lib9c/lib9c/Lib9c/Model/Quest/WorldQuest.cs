using System;
using Nekoyume.TableData;

namespace Nekoyume.Model.Quest
{
    [Serializable]
    public class WorldQuest : Quest
    {
        public WorldQuest(WorldQuestSheet.Row data, QuestReward reward) 
            : base(data, reward)
        {
        }

        public override QuestType QuestType => QuestType.Adventure;

        public override void Check()
        {
        }

        public override string GetProgressText()
        {
            return string.Empty;
        }

        protected override string TypeId => "worldQuest";

        public void Update(CollectionMap stageMap)
        {
            if (Complete)
                return;

            Complete = stageMap.TryGetValue(Goal, out _);
        }
    }
}
