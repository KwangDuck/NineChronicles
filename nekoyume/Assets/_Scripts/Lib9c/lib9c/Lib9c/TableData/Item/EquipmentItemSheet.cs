using System;
using System.Collections.Generic;
using Nekoyume.Model.Item;
using Nekoyume.Model.Stat;
using static Nekoyume.TableData.TableExtensions;

namespace Nekoyume.TableData
{
    [Serializable]
    public class EquipmentItemSheet : Sheet<int, EquipmentItemSheet.Row>
    {
        [Serializable]
        public class Row : ItemSheet.Row
        {
            public override ItemType ItemType => ItemType.Equipment;
            public int SetId { get; private set; }
            public DecimalStat Stat { get; private set; }
            public decimal AttackRange { get; private set; }
            public string SpineResourcePath { get; private set; }

            public override void Set(IReadOnlyList<string> fields)
            {
                base.Set(fields);
                SetId = string.IsNullOrEmpty(fields[4]) ? 0 : ParseInt(fields[4]);
                Stat = new DecimalStat(
                    (StatType) Enum.Parse(typeof(StatType), fields[5]),
                    ParseDecimal(fields[6]));
                AttackRange = ParseDecimal(fields[7]);
                SpineResourcePath = fields[8];
            }
        }
        
        public EquipmentItemSheet() : base(nameof(EquipmentItemSheet))
        {
        }
    }
}
