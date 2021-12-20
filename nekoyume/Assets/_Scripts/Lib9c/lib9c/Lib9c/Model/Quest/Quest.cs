using System;
using Nekoyume.Model.State;
using Nekoyume.TableData;

namespace Nekoyume.Model.Quest
{
    public enum QuestType
    {
        Adventure,
        Obtain,
        Craft,
        Exchange
    }

    [Serializable]
    public abstract class Quest : IState
    {
        [NonSerialized]
        public bool isReceivable = false;

        protected int _current;

        public abstract QuestType QuestType { get; }

        public bool Complete { get; protected set; }

        public int Goal { get; set; }

        public int Id { get; }

        public QuestReward Reward { get; }

        /// <summary>
        /// 이미 퀘스트 보상이 액션에서 지급되었는가?
        /// </summary>
        public bool IsPaidInAction { get; set; }

        public virtual float Progress => (float) _current / Goal;

        public const string GoalFormat = "({0}/{1})";

        protected Quest(QuestSheet.Row data, QuestReward reward)
        {
            Id = data.Id;
            Goal = data.Goal;
            Reward = reward;
        }

        public abstract void Check();
        protected abstract string TypeId { get; }

        public abstract string GetProgressText();
    }
}
