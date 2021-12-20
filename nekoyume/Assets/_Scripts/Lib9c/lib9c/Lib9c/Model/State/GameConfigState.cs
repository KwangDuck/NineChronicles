using System;

namespace Nekoyume.Model.State
{
    [Serializable]
    public class GameConfigState : State
    {
        public int HourglassPerBlock { get; private set; } = 3;
        public int ActionPointMax { get; private set; } = 120;
        public int DailyRewardInterval { get; private set; } = 1700;
        public int DailyArenaInterval { get; private set; } = 2800;
        public int WeeklyArenaInterval { get; private set; } = 56000;
    }
}
