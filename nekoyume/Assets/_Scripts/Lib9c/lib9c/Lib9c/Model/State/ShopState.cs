using System;
using System.Collections.Generic;
using System.Linq;
using Nekoyume.Model.Item;

namespace Nekoyume.Model.State
{
    /// <summary>
    /// This is a model class of shop state.
    /// </summary>
    [Serializable]
    public class ShopState : State
    {
        private readonly Dictionary<Guid, ShopItem> _products = new Dictionary<Guid, ShopItem>();

        public IReadOnlyDictionary<Guid, ShopItem> Products => _products;      
        
        public void Register(ShopItem shopItem)
        {
            var productId = shopItem.ProductId;
            if (_products.ContainsKey(productId))
            {
                throw new ShopStateAlreadyContainsException($"Aborted as the item already registered # {productId}.");
            }
            _products[productId] = shopItem;
        }

        public void Unregister(ShopItem shopItem)
        {
            Unregister(shopItem.ProductId);
        }

        public void Unregister(Guid productId)
        {
            if (!TryUnregister(productId, out _))
            {
                throw new FailedToUnregisterInShopStateException(
                    $"{nameof(_products)}, {productId}");
            }
        }

        public bool TryUnregister(
            Guid productId,
            out ShopItem unregisteredItem)
        {
            if (!_products.ContainsKey(productId))
            {
                unregisteredItem = null;
                return false;
            }

            var targetShopItem = _products[productId];
            _products.Remove(productId);
            unregisteredItem = targetShopItem;
            return true;
        }
    }
}
