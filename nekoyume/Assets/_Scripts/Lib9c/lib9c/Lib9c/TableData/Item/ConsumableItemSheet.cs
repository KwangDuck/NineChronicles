using System;
using System.Collections.Generic;
using Nekoyume.Model.Item;
using Nekoyume.Model.Stat;
using Nekoyume.Model.State;
using static Nekoyume.TableData.TableExtensions;

namespace Nekoyume.TableData
{
    [Serializable]
    public class ConsumableItemSheet : Sheet<int, ConsumableItemSheet.Row>
    {
        [Serializable]
        public class Row : ItemSheet.Row, IState
        {
            public override ItemType ItemType => ItemType.Consumable;
            public List<StatMap> Stats { get; private set; }
            
            public override void Set(IReadOnlyList<string> fields)
            {
                base.Set(fields);
                Stats = new List<StatMap>();
                for (var i = 0; i < 2; i++)
                {
                    if (string.IsNullOrEmpty(fields[4 + i * 2]) ||
                        string.IsNullOrEmpty(fields[5 + i * 2]))
                        return;

                    Stats.Add(new StatMap(
                        (StatType) Enum.Parse(typeof(StatType), fields[4 + i * 2]),
                        ParseDecimal(fields[5 + i * 2])));
                }
            }
        }
        
        public ConsumableItemSheet() : base(nameof(ConsumableItemSheet))
        {
        }

        protected override void AddRow(int key, Row value)
        {
            if (!TryGetValue(key, out var row))
            {
                Add(key, value);

                return;
            }

            if (value.Stats.Count == 0)
                return;

            row.Stats.Add(value.Stats[0]);
        }
    }
}
