using System;
using System.Runtime.Serialization;
using Nekoyume.TableData;

namespace Nekoyume.Model.Item
{
    [Serializable]
    public class Belt : Equipment
    {
        public Belt(EquipmentItemSheet.Row data) : base(data)
        {
        }
    }
}
