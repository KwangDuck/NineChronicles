using System.Collections.Generic;
using System.Linq;
using Assets.SimpleLocalization;
using Bencodex.Types;
using Nekoyume.Model;
using Nekoyume.TableData;

namespace Nekoyume.Game.Quest
{
    public class MonsterQuest: Quest
    {
        private readonly int _monsterId;
        private int _count;

        public MonsterQuest(MonsterQuestSheet.Row data) : base(data)
        {
            _monsterId = data.MonsterId;
        }

        public MonsterQuest(Bencodex.Types.Dictionary serialized) : base(serialized)
        {
            _count = (int) ((Integer) serialized[(Bencodex.Types.Text) "count"]).Value;
            _monsterId = (int) ((Integer) serialized[(Bencodex.Types.Text) "monsterId"]).Value;
        }

        public override QuestType QuestType => QuestType.Adventure;

        public override void Check()
        {
            Complete = _count >= Goal;
        }

        public override string ToInfo()
        {
            var format = LocalizationManager.Localize("QUEST_MONSTER_FORMAT");
            return string.Format(format, LocalizationManager.LocalizeCharacterName(_monsterId), _count, Goal);
        }

        protected override string TypeId => "monsterQuest";

        public void Update(CollectionMap monsterMap)
        {
            monsterMap.TryGetValue(_monsterId, out _count);
            Check();
        }

        public override IValue Serialize() =>
            new Bencodex.Types.Dictionary(new Dictionary<IKey, IValue>
            {
                [(Text) "count"] = (Integer) _count,
                [(Text) "monsterId"] = (Integer) _monsterId,
            }.Union((Bencodex.Types.Dictionary) base.Serialize()));
    }
}
