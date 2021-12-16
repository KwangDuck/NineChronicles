using System;
using Nekoyume.UI.Module;
using UniRx;

namespace Nekoyume.UI.Model
{
    public class ShopItem : CountableItem
    {
        public readonly ReactiveProperty<long> Price = new ReactiveProperty<long>();
        public readonly ReactiveProperty<Guid> OrderId = new ReactiveProperty<Guid>();
        public readonly ReactiveProperty<Guid> TradableId = new ReactiveProperty<Guid>();
        public readonly ReactiveProperty<long> ExpiredBlockIndex = new ReactiveProperty<long>();
        public readonly ReactiveProperty<int> Level = new ReactiveProperty<int>();

        public ShopItemView View;

        public ShopItem() : base(null, 0)
        {

        }

        public override void Dispose()
        {
            Price.Dispose();
            OrderId.Dispose();
            TradableId.Dispose();
            ExpiredBlockIndex.Dispose();
            Level.Dispose();
            base.Dispose();
        }
    }
}
