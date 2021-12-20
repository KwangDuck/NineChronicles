using System;
using System.Collections.Generic;
using Nekoyume.Model.Item;

namespace Nekoyume.TableData
{
    [Serializable]
    public class MaterialItemSheet : Sheet<int, MaterialItemSheet.Row>
    {
        [Serializable]
        public class Row : ItemSheet.Row
        {
            public override ItemType ItemType => ItemType.Material;
        }

        public MaterialItemSheet() : base(nameof(MaterialItemSheet))
        {
        }
    }
}
