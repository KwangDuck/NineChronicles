using System;
using System.Globalization;
using Nekoyume.TableData;

namespace Nekoyume.Model.Quest
{
    public class CombinationEquipmentQuest : Quest
    {
        public readonly int RecipeId;
        public readonly int StageId;

        public CombinationEquipmentQuest(QuestSheet.Row data, QuestReward reward, int stageId) : base(data, reward)
        {
            var row = (CombinationEquipmentQuestSheet.Row) data;
            RecipeId = row.RecipeId;
            StageId = stageId;
        }

        //임시처리. 새 타입을 만들어서 위젯에 띄워줘야합니다.
        public override QuestType QuestType => QuestType.Craft;

        public override void Check()
        {
            Complete = _current >= Goal;
        }

        protected override string TypeId => "combinationEquipmentQuest";

        public override string GetProgressText() =>
            string.Format(
                CultureInfo.InvariantCulture,
                GoalFormat,
                Math.Min(Goal, _current),
                Goal
            );

        public void Update(int recipeId)
        {
            if (Complete)
                return;

            if (recipeId == RecipeId)
            {
                _current++;
            }
            Check();
        }
    }
}
