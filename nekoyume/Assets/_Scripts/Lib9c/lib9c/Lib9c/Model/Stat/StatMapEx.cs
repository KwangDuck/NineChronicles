using System;
using System.Collections.Generic;
using System.Linq;
using Nekoyume.Model.State;

namespace Nekoyume.Model.Stat
{
    // todo: 없어질 대상.
    [Serializable]
    public class StatMapEx : StatMap
    {
        private decimal _additionalValue;

        public bool HasAdditionalValue => AdditionalValue > 0m;

        public decimal AdditionalValue
        {
            get => _additionalValue;
            set
            {
                _additionalValue = value;
                AdditionalValueAsInt = (int)_additionalValue;
            }
        }

        public int AdditionalValueAsInt { get; private set; }

        public decimal TotalValue => Value + AdditionalValueAsInt;
        public int TotalValueAsInt => ValueAsInt + AdditionalValueAsInt;

        public StatMapEx(StatType statType, decimal value = 0m, decimal additionalValue = 0m) : base(statType, value)
        {
            AdditionalValue = additionalValue;
        }

        public StatMapEx(StatMap statMap) : this(statMap.StatType, statMap.Value)
        {
        }

        protected bool Equals(StatMapEx other)
        {
            return base.Equals(other) && _additionalValue == other._additionalValue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((StatMapEx)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return base.GetHashCode() * 397 ^ _additionalValue.GetHashCode();
            }
        }
    }
}
