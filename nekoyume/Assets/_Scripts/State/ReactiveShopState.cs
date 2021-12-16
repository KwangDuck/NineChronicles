using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Nekoyume.Model.Item;

namespace Nekoyume.State
{
    /// <summary>
    /// Changes in the values included in ShopState are notified to the outside through each ReactiveProperty<T> field.
    /// </summary>
    public static class ReactiveShopState
    {
        private enum SortType
        {
            None = 0,
            Grade = 1,
            Cp = 2,
        }

        private static readonly List<ItemSubType> ItemSubTypes = new List<ItemSubType>()
        {
            ItemSubType.Weapon,
            ItemSubType.Armor,
            ItemSubType.Belt,
            ItemSubType.Necklace,
            ItemSubType.Ring,
            ItemSubType.Food,
            ItemSubType.FullCostume,
            ItemSubType.HairCostume,
            ItemSubType.EarCostume,
            ItemSubType.EyeCostume,
            ItemSubType.TailCostume,
            ItemSubType.Title,
            ItemSubType.Hourglass,
            ItemSubType.ApStone,
        };

        private static readonly List<ItemSubType> ShardedSubTypes = new List<ItemSubType>()
        {
            ItemSubType.Weapon,
            ItemSubType.Armor,
            ItemSubType.Belt,
            ItemSubType.Necklace,
            ItemSubType.Ring,
            ItemSubType.Food,
            ItemSubType.Hourglass,
            ItemSubType.ApStone,
        };

        private const int buyItemsPerPage = 24;
        private const int sellItemsPerPage = 20;

        public static async Task InitAndUpdateBuyDigests()
        {            
            UpdateBuyDigests();
        }

        public static async void InitSellDigests()
        {
            
        }

        public static async void InitAndUpdateSellDigests()
        {
            UpdateSellDigests();
        }

        private static void UpdateBuyDigests()
        {
        }

        private static void UpdateSellDigests()
        {
        }

        public static void RemoveBuyDigest(Guid orderId)
        {            
            UpdateBuyDigests();
        }

        public static void RemoveSellDigest(Guid orderId)
        {
            UpdateSellDigests();
        }
    }
}
