using Libplanet;
using Nekoyume.Model.State;
using Nekoyume.State;

namespace Nekoyume
{
    public static class ArenaHelper
    {
        public static bool TryGetThisWeekAddress(out Address weeklyArenaAddress)
        {
            return false;
        }

        public static bool TryGetThisWeekAddress(long blockIndex, out Address weeklyArenaAddress)
        {
            var gameConfigState = States.Instance.GameConfigState;
            var index = (int) blockIndex / gameConfigState.WeeklyArenaInterval;
            if (index < 0)
            {
                return false;
            }

            weeklyArenaAddress = WeeklyArenaState.DeriveAddress(index);
            return true;
        }
    }
}
