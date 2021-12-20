using System;
using System.Runtime.Serialization;
using Nekoyume.Model.Elemental;
using Nekoyume.TableData;

namespace Nekoyume.Model.Item
{
    [Serializable]
    public abstract class ItemBase : IItem
    {
        public int Id { get; }
        public int Grade { get; }
        public ItemType ItemType { get; }
        public ItemSubType ItemSubType { get; }
        public ElementalType ElementalType { get; }
        protected ItemBase(ItemSheet.Row data)
        {
            Id = data.Id;
            Grade = data.Grade;
            ItemType = data.ItemType;
            ItemSubType = data.ItemSubType;
            ElementalType = data.ElementalType;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException(nameof(info));
            }

        }

        protected bool Equals(ItemBase other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ItemBase) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override string ToString()
        {
            return
                $"{nameof(Id)}: {Id}" +
                $", {nameof(Grade)}: {Grade}" +
                $", {nameof(ItemType)}: {ItemType}" +
                $", {nameof(ItemSubType)}: {ItemSubType}" +
                $", {nameof(ElementalType)}: {ElementalType}";
        }
    }
}
