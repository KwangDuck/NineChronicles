using Libplanet;
using Libplanet.Assets;
using Nekoyume.State;
using UniRx;

namespace Nekoyume.UI.Model
{
    public class ItemCountAndPricePopup : ItemCountPopup<ItemCountAndPricePopup>
    {
        public readonly ReactiveProperty<FungibleAssetValue> Price;

        public readonly ReactiveProperty<bool> PriceInteractable = new ReactiveProperty<bool>(true);

        public ItemCountAndPricePopup()
        {
            var currency = new Currency("ticker", 0, new Address());
            Price = new ReactiveProperty<FungibleAssetValue>(new FungibleAssetValue(currency, 10, 0));
        }

        public override void Dispose()
        {
            Price.Dispose();
            PriceInteractable.Dispose();

            base.Dispose();
        }
    }
}
