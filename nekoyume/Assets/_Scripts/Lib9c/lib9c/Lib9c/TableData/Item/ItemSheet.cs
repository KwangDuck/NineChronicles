using System;
using System.Collections.Generic;
using Nekoyume.Model.Elemental;
using Nekoyume.Model.Item;
using static Nekoyume.TableData.TableExtensions;

namespace Nekoyume.TableData
{
    [Serializable]
    public class ItemSheet : Sheet<int, ItemSheet.Row>
    {
        [Serializable]
        public abstract class RowBase : SheetRow<int>
        {
            
        }

        [Serializable]
        public class Row : RowBase
        {
            public override int Key => Id;
            public int Id { get; private set; }
            public virtual ItemType ItemType { get; }
            public ItemSubType ItemSubType { get; protected set; }
            public int Grade { get; private set; }
            public ElementalType ElementalType { get; private set; }

            public override void Set(IReadOnlyList<string> fields)
            {
                Id = ParseInt(fields[0]);
                ItemSubType = (ItemSubType) Enum.Parse(typeof(ItemSubType), fields[1]);
                Grade = ParseInt(fields[2]);
                ElementalType = (ElementalType) Enum.Parse(typeof(ElementalType), fields[3]);
            }
        }

        public ItemSheet() : base(nameof(ItemSheet))
        {
        }
    }
}
