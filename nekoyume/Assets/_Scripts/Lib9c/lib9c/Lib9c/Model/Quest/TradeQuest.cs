using System;
using System.Globalization;
using Nekoyume.Model.EnumType;
using Nekoyume.TableData;

namespace Nekoyume.Model.Quest
{
    [Serializable]
    public class TradeQuest : Quest
    {
        public override QuestType QuestType => QuestType.Exchange;
        public readonly TradeType Type;

        public TradeQuest(TradeQuestSheet.Row data, QuestReward reward) 
            : base(data, reward)
        {
            Type = data.Type;
        }

        public override void Check()
        {
            if (Complete)
                return;

            _current += 1;
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

        protected override string TypeId => "tradeQuest";
    }
}
