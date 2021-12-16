using System;
using Nekoyume.UI.Module;
using UniRx;

namespace Nekoyume.UI.Model
{
    public class ShopBuyItem : CountableItem
    {
        public readonly ReactiveProperty<string> SellerAgentAddress = new ReactiveProperty<string>();
        public readonly ReactiveProperty<string> SellerAvatarAddress = new ReactiveProperty<string>();
        public readonly ReactiveProperty<long> Price = new ReactiveProperty<long>();
        public readonly ReactiveProperty<Guid> ProductId = new ReactiveProperty<Guid>();

        public ShopItemView View;

        public ShopBuyItem(Nekoyume.Model.Item.ShopItem item)
            : base(null, 0)
        {
        }

        public override void Dispose()
        {
            SellerAgentAddress.Dispose();
            SellerAvatarAddress.Dispose();
            Price.Dispose();
            ProductId.Dispose();
            base.Dispose();
        }
    }
}
