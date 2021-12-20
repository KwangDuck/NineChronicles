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
        [Serializable]
        public class Item : IState, IComparer<Item>
        {
            public ItemBase item { get; private set; }
            public int count = 0;
            public ILock Lock;
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

        private readonly List<Item> _items = new List<Item>();

        public IReadOnlyList<Item> Items => _items;

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

        public KeyValuePair<int, int> AddItem(ItemBase itemBase, int count = 1, ILock iLock = null)
        {
            switch (itemBase.ItemType)
            {
                case ItemType.Consumable:
                case ItemType.Equipment:
                case ItemType.Costume:
                    break;
                case ItemType.Material:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            _items.Sort();
            return new KeyValuePair<int, int>(itemBase.Id, count);
        }

        public bool HasItem(int rowId, int count = 1) => _items
            .Where(item =>
                item.item.Id == rowId
            ).Sum(item => item.count) >= count;

        public bool HasNotification(int level, long blockIndex)
        {
            var availableSlots = UnlockHelper.GetAvailableEquipmentSlots(level);

            foreach (var (type, slotCount) in availableSlots)
            {
                var equipments = Equipments
                    .Where(e =>
                        e.ItemSubType == type &&
                        e.RequiredBlockIndex <= blockIndex)
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
