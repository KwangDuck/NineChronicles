using System;
using System.Collections.Generic;
using System.Linq;
using Nekoyume.Battle;
using Nekoyume.Model.State;

namespace Nekoyume.Model.Item
{
    [Serializable]
    public class Inventory : IState
    {
        public class FungibleItem : Item
        {
            public FungibleItem(ItemBase itemBase, int count = 1) : base(itemBase, count)
            {
                Id = itemBase.ItemId;
            }
        }

        public class NonFungibleItem : Item
        {
            public NonFungibleItem(ItemBase itemBase, int count = 1) : base(itemBase, count)
            {
                Id = itemBase.ItemId;
            }
        }


        [Serializable]
        public class Item : IComparer<Item>
        {
            public string Id { get; protected set; }
            public ItemBase item { get; private set; }
            public int count { get; set; }
            public ILock Lock { get; private set; }
            public bool Locked => !(Lock is null);

            public Item(ItemBase itemBase, int count = 1)
            {
                this.item = itemBase;
                this.count = count;
            }

            public void LockUp(ILock iLock)
            {
                Lock = iLock;
            }

            public void Unlock()
            {
                Lock = null;
            }

            protected bool Equals(Item other)
            {
                return Equals(item, other.item) && count == other.count && Equals(Lock, other.Lock);
            }

            public int Compare(Item x, Item y)
            {
                return x.item.Grade != y.item.Grade
                    ? y.item.Grade.CompareTo(x.item.Grade)
                    : x.item.Id.CompareTo(y.item.Id);
            }

            public int CompareTo(Item other)
            {
                if (ReferenceEquals(this, other)) return 0;
                if (ReferenceEquals(null, other)) return 1;
                return Compare(this, other);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((Item)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var hashCode = (item != null ? item.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ count;
                    hashCode = (hashCode * 397) ^ (Lock != null ? Lock.GetHashCode() : 0);
                    return hashCode;
                }
            }
        }

        private List<Item> _items = new List<Item>();

        public IReadOnlyList<Item> Items => _items;

        public IEnumerable<FungibleItem> FungibleItems => _items
            .Where(item => item is FungibleItem)
            .OfType<FungibleItem>();

        public IEnumerable<NonFungibleItem> NonfungibleItems => _items
            .Where(item => item is NonFungibleItem)
            .OfType<NonFungibleItem>();

        public IEnumerable<Consumable> Consumables => _items
            .Select(item => item.item)
            .OfType<Consumable>();

        public IEnumerable<Costume> Costumes => _items
            .Select(item => item.item)
            .OfType<Costume>();

        public IEnumerable<Equipment> Equipments => _items
            .Select(item => item.item)
            .OfType<Equipment>();

        public IEnumerable<Material> Materials => _items
            .Select(item => item.item)
            .OfType<Material>();

        public Inventory()
        {
        }

        protected bool Equals(Inventory other)
        {
            if (_items.Count == 0 && other._items.Count == 0)
            {
                return true;
            }

            return _items.SequenceEqual(other._items);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Inventory)obj);
        }

        public override int GetHashCode()
        {
            return (_items != null ? _items.GetHashCode() : 0);
        }

        #region FungibleItem
        public FungibleItem AddFungibleItem(ItemBase itemBase, int count = 1, ILock ilock = null)
        {
            var item = FungibleItems
                .FirstOrDefault(e => e.item.Equals(itemBase) && !e.Locked);            
            if (item is null)
            {
                // add new item
                item = new FungibleItem(itemBase, count);
                _items.Add(item);
            }
            else
            {
                // append count
                item.count += count;
            }

            if (!(ilock is null))
            {
                item.LockUp(ilock);
            }

            return item;
        }

        public bool HasFungibleItem(int id, int count)
        {
            var totalCount = GetFungibleItemCount(id);
            return totalCount >= count;
        }

        public int GetFungibleItemCount(int id)
        {
            return FungibleItems
                .Where(e => e.Id == id.ToString())
                .DefaultIfEmpty()
                .Sum(e => e.count);
        }

        public FungibleItem GetFungibleItem(int id)
        {
            return FungibleItems
                .Where(e => e.Id == id.ToString())
                .FirstOrDefault();
        }

        public bool RemoveFungibleItem(int id, int count = 1)
        {
            var item = FungibleItems
                .Where(e => e.Id == id.ToString())
                .Where(e => !e.Locked)
                .FirstOrDefault();
            if (item == null)
            {
                return false;
            }

            if (item.count > count)
            {
                item.count -= count;
            }
            else
            {
                _items.Remove(item);
            }
            return true;
        }
        #endregion

        #region NonFungibleItem
        public NonFungibleItem AddNonFungibleItem(ItemBase itemBase, ILock ilock = null)
        {
            var item = new NonFungibleItem(itemBase);
            if (!(ilock is null))
            {
                item.LockUp(ilock);
            }
            _items.Add(item);
            return item;
        }

        public bool HasNonFungibleItem(string id)
        {
            var item = GetNonFungibleItem(id);
            return item != null;
        }

        public NonFungibleItem GetNonFungibleItem(string id)
        {
            return NonfungibleItems
                .Where(e => e.Id == id)
                .FirstOrDefault();
        }

        public bool RemoveNonFungibleItem(string id)
        {
            var item = NonfungibleItems
                .Where(e => e.Id == id)
                .FirstOrDefault();
            if (item == null)
            {
                return false;
            }

            _items.Remove(item);
            return true;
        }
        #endregion

        public KeyValuePair<int, int> AddItem(ItemBase itemBase, int count = 1, ILock ilock = null)
        {
            if (itemBase.ItemType == ItemType.Material)
            {
                AddFungibleItem(itemBase, count, ilock);
            }
            else
            {
                AddNonFungibleItem(itemBase, ilock);
            }
            return new KeyValuePair<int, int>(itemBase.Id, count);
        }

        public bool HasItem(int rowId, int count = 1)
        {
            var totalCount = GetItemCount(rowId);
            return totalCount >= count;
        }

        public int GetItemCount(int rowId)
        {
            return _items
                .Where(e => e.item.Id == rowId)
                .DefaultIfEmpty()
                .Sum(e => e.count);
        }

        public bool TryGetItem(int rowId, out Item outItem)
        {
            var item = GetItem(rowId);
            if (item == null)
            {
                outItem = default;
                return false;
            }

            outItem = item;
            return true;
        }

        public Item GetItem(int rowId)
        {
            return _items
                .Where(e => e.item.Id == rowId)
                .FirstOrDefault();
        }

        public bool RemoveItem(int rowId, int count = 1)
        {
            var item = GetItem(rowId);
            if (item == null)
            {
                return false;
            }

            item.count -= count;
            if (item.count <= 0)
            {
                _items.Remove(item);
            }
            return true;
        }

        public bool HasNotification(int level)
        {
            var availableSlots = UnlockHelper.GetAvailableEquipmentSlots(level);

            foreach (var (type, slotCount) in availableSlots)
            {
                var equipments = Equipments
                    .Where(e => e.ItemSubType == type)
                    .ToList();
                var current = equipments.Where(e => e.equipped).ToList();
                // When an equipment slot is empty.
                if (current.Count < Math.Min(equipments.Count, slotCount))
                {
                    return true;
                }

                // When any other equipments are stronger than current one.
                foreach (var equipment in equipments)
                {
                    if (equipment.equipped)
                    {
                        continue;
                    }

                    var cp = CPHelper.GetCP(equipment);
                    if (current.Any(i => CPHelper.GetCP(i) < cp))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

    }
}
