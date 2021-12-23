using System;
using Nekoyume.TableData;

namespace Nekoyume.Model.Item
{
    [Serializable]
    public class Necklace : Equipment
    {
        public Necklace(EquipmentItemSheet.Row data, string id) : base(data, id)
        {
        }
    }
}
