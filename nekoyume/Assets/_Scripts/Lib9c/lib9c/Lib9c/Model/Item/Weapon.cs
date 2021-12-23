using System;
using Nekoyume.TableData;

namespace Nekoyume.Model.Item
{
    [Serializable]
    public class Weapon : Equipment
    {
        public Weapon(EquipmentItemSheet.Row data, string id) : base(data, id)
        {
        }
    }
}
