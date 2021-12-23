using System;
using Nekoyume.TableData;

namespace Nekoyume.Model.Item
{
    [Serializable]
    public class Ring : Equipment
    {
        public Ring(EquipmentItemSheet.Row data, string id) : base(data, id)
        {
        }
    }
}
