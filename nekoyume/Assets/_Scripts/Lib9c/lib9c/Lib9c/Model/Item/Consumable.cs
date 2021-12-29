using System;
using System.Collections.Generic;
using System.Linq;
using Nekoyume.Model.Stat;
using Nekoyume.TableData;

namespace Nekoyume.Model.Item
{
    [Serializable]
    public class Consumable : ItemUsable
    {
        public StatType MainStat => Stats.Any() ? Stats[0].StatType : StatType.NONE;

        public List<StatMap> Stats { get; }

        public Consumable(ConsumableItemSheet.Row data, string id) : base(data, id)
        {
            Stats = data.Stats;
        }
    }
}