using System;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using Nekoyume.Model.State;
using Nekoyume.TableData;

namespace Nekoyume.Model.Item
{
    [Serializable]
    public class TradableMaterial : Material
    {
        public Guid TradableId { get; }

        public long RequiredBlockIndex
        {
            get => _requiredBlockIndex;
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(
                        $"{nameof(RequiredBlockIndex)} must be greater than 0, but {value}");
                }
                _requiredBlockIndex = value;
            }
        }

        private long _requiredBlockIndex;

        public TradableMaterial(MaterialItemSheet.Row data) : base(data)
        {            
        }

        protected bool Equals(TradableMaterial other)
        {
            return base.Equals(other) && RequiredBlockIndex == other.RequiredBlockIndex && TradableId.Equals(other.TradableId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TradableMaterial) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = base.GetHashCode();
                hashCode = (hashCode * 397) ^ RequiredBlockIndex.GetHashCode();
                hashCode = (hashCode * 397) ^ TradableId.GetHashCode();
                return hashCode;
            }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
