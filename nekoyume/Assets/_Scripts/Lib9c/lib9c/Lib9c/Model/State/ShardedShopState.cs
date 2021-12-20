using System;
using System.Collections.Generic;
using Nekoyume.Model.Item;

namespace Nekoyume.Model.State
{
    public class ShardedShopState : State
    {
        public readonly Dictionary<Guid, ShopItem> Products = new Dictionary<Guid, ShopItem>();
    }
}
