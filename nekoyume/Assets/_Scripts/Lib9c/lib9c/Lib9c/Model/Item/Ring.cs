using System;
using System.Runtime.Serialization;
using Nekoyume.TableData;

namespace Nekoyume.Model.Item
{
    [Serializable]
    public class Ring : Equipment
    {
        public Ring(EquipmentItemSheet.Row data) : base(data)
        {
        }
    }
}
