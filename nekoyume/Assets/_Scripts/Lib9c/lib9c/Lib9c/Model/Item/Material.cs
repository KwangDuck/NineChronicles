using System;
using Nekoyume.TableData;

namespace Nekoyume.Model.Item
{
    [Serializable]
    public class Material : ItemBase
    {
        public override string ItemId { get; }
        public string FungibleId => ItemId;

        public Material(MaterialItemSheet.Row data) : base(data)
        {
            ItemId = data.ItemId;
        }

        protected bool Equals(Material other)
        {
            return base.Equals(other) &&
                   ItemId.Equals(other.ItemId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Material) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ ItemId.GetHashCode();
                return hashCode;
            }
        }
    }
}
