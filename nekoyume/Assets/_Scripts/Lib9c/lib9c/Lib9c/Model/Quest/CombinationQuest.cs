using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Nekoyume.Model.Item;
using Nekoyume.TableData;

namespace Nekoyume.Model.Quest
{
    [Serializable]
    public class CombinationQuest : Quest
    {
        public readonly ItemType ItemType;
        public readonly ItemSubType ItemSubType;

        public override QuestType QuestType => QuestType.Craft;

        public CombinationQuest(CombinationQuestSheet.Row data, QuestReward reward) 
            : base(data, reward)
        {
            ItemType = data.ItemType;
            ItemSubType = data.ItemSubType;
        }

        public override void Check()
        {
            if (Complete)
                return;

            Complete = _current >= Goal;
        }

        // FIXME: 이 메서드 구현은 중복된 코드가 다른 데서도 많이 있는 듯.
        public override string GetProgressText() =>
            string.Format(
                CultureInfo.InvariantCulture,
                GoalFormat,
                Math.Min(Goal, _current),
                Goal
            );

        protected override string TypeId => "combinationQuest";

        public void Update(List<ItemBase> items)
        {
            if (Complete)
                return;

            _current += items.Count(i => i.ItemType == ItemType && i.ItemSubType == ItemSubType);
            Check();
        }
    }
}
