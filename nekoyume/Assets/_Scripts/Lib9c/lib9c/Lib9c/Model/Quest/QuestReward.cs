using System;
using System.Collections.Generic;
using System.Linq;
using Nekoyume.Model.State;

namespace Nekoyume.Model.Quest
{
    [Serializable]
    public class QuestReward : IState
    {
        private Dictionary<int, int> _itemMap;

        public QuestReward(Dictionary<int, int> map)
        {
            _itemMap = map
                .ToDictionary(kv => kv.Key, kv => kv.Value
            );
        }
    }
}
