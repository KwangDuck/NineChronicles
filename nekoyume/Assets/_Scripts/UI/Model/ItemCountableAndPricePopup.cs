using UniRx;

namespace Nekoyume.UI.Model
{
    public class ItemCountableAndPricePopup : ItemCountPopup<ItemCountableAndPricePopup>
    {
        public readonly ReactiveProperty<long> Price = new ReactiveProperty<long>();
        public readonly ReactiveProperty<long> TotalPrice = new ReactiveProperty<long>();
        public readonly ReactiveProperty<long> PreTotalPrice = new ReactiveProperty<long>();
        public readonly ReactiveProperty<int> Count = new ReactiveProperty<int>(1);
        public readonly ReactiveProperty<bool> IsSell = new ReactiveProperty<bool>();

        public readonly Subject<int> OnChangeCount = new Subject<int>();
        public readonly Subject<decimal> OnChangePrice = new Subject<decimal>();
        public readonly Subject<ItemCountableAndPricePopup> OnClickReregister = new Subject<ItemCountableAndPricePopup>();

        public override void Dispose()
        {
            Price.Dispose();
            TotalPrice.Dispose();
            PreTotalPrice.Dispose();
            Count.Dispose();
            IsSell.Dispose();
            OnChangeCount.Dispose();
            OnChangePrice.Dispose();
            OnClickReregister.Dispose();
            base.Dispose();
        }
    }
}
