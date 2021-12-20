using System;
using System.Collections.Generic;
using System.Linq;

namespace Nekoyume.Model.State
{
    [Serializable]
    public class WeeklyArenaState : State
    {        
        public long ResetIndex;
        public bool Ended;

        public List<ArenaInfo> OrderedArenaInfos { get; private set; }

        /// <returns>A list of tuples that contains <c>int</c> and <c>ArenaInfo</c>.</returns>
        public List<(int rank, ArenaInfo arenaInfo)> GetArenaInfos(
            int firstRank = 1,
            int? count = null)
        {
            if (OrderedArenaInfos.Count == 0)
            {
                return new List<(int rank, ArenaInfo arenaInfo)>();
            }

            if (!(0 < firstRank && firstRank <= OrderedArenaInfos.Count))
            {
                throw new ArgumentOutOfRangeException(
                    $"{nameof(firstRank)}({firstRank}) out of range({OrderedArenaInfos.Count})");
            }

            count = count.HasValue
                ? Math.Min(OrderedArenaInfos.Count - firstRank + 1, count.Value)
                : OrderedArenaInfos.Count - firstRank + 1;

            var offsetIndex = 0;
            return OrderedArenaInfos.GetRange(firstRank - 1, count.Value)
                .Select(arenaInfo => (firstRank + offsetIndex++, arenaInfo))
                .ToList();
        }
    }
}
