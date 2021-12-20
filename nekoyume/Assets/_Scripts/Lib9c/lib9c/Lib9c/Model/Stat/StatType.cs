using System;
using System.Collections.Generic;

namespace Nekoyume.Model.Stat
{
    public enum StatType
    {
        /// <summary>
        /// Default, It's same with null.
        /// </summary>
        NONE,
        
        /// <summary>
        /// Health Point
        /// </summary>
        HP,

        /// <summary>
        /// Attack Power
        /// </summary>
        ATK,

        /// <summary>
        /// Defence
        /// </summary>
        DEF,

        /// <summary>
        /// Critical Chance
        /// </summary>
        CRI,
        
        /// <summary>
        /// Hit Chance
        /// </summary>
        HIT,
        
        /// <summary>
        /// Speed
        /// </summary>
        SPD
    }

    [Serializable]
    public class StatTypeComparer : IEqualityComparer<StatType>
    {
        public static readonly StatTypeComparer Instance = new StatTypeComparer();

        public bool Equals(StatType x, StatType y)
        {
            return x == y;
        }

        public int GetHashCode(StatType obj)
        {
            return (int) obj;
        }
    }
}
