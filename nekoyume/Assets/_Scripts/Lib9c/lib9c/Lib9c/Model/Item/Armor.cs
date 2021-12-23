using System;
using Nekoyume.TableData;

namespace Nekoyume.Model.Item
{
    [Serializable]
    public class Armor : Equipment
    {
        public Armor(EquipmentItemSheet.Row data, string id) : base(data, id)
        {
        }
    }
}
