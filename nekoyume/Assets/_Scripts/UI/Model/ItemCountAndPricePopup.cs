using UniRx;

namespace Nekoyume.UI.Model
{
    public class ItemCountAndPricePopup : ItemCountPopup<ItemCountAndPricePopup>
    {
        public readonly ReactiveProperty<long> Price;
        public readonly ReactiveProperty<bool> PriceInteractable = new ReactiveProperty<bool>(true);

        public ItemCountAndPricePopup()
        {
            Price = new ReactiveProperty<long>(0);
        }

        public override void Dispose()
        {
            Price.Dispose();
            PriceInteractable.Dispose();

            base.Dispose();
        }
    }
}
