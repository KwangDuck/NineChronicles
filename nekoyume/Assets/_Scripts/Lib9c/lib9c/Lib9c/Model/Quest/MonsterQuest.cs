using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Nekoyume.TableData;

namespace Nekoyume.Model.Quest
{
    [Serializable]
    public class MonsterQuest : Quest
    {
        public readonly int MonsterId;

        public MonsterQuest(MonsterQuestSheet.Row data, QuestReward reward) 
            : base(data, reward)
        {
            MonsterId = data.MonsterId;
        }

        public override QuestType QuestType => QuestType.Adventure;

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

        protected override string TypeId => "monsterQuest";

        public void Update(CollectionMap monsterMap)
        {
            if (Complete)
                return;

            monsterMap.TryGetValue(MonsterId, out _current);
            Check();
        }
    }
}
