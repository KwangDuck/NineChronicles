using System;
using Nekoyume.Model.BattleStatus;
using Nekoyume.TableData;

namespace Nekoyume.Model.State
{
    public class ArenaInfo : IState
    {
        public class Record : IState
        {
            public int Win;
            public int Lose;
            public int Draw;

            public Record()
            {
            }
        }

        public readonly Record ArenaRecord;
        public int Score { get; private set; }
        public int DailyChallengeCount { get; private set; }
        public bool Active { get; private set; }

        public readonly string AvatarName;

        public ArenaInfo(AvatarState avatarState, CharacterSheet characterSheet, bool active)
        {
            
        }

        public ArenaInfo(AvatarState avatarState, CharacterSheet characterSheet, CostumeStatSheet costumeStatSheet, bool active)
            : this(avatarState, characterSheet, active)
        {
            
        }

        public int Update(ArenaInfo enemyInfo, BattleLog.Result result)
        {
            DailyChallengeCount--;
            switch (result)
            {
                case BattleLog.Result.Win:
                    ArenaRecord.Win++;
                    break;
                case BattleLog.Result.Lose:
                    ArenaRecord.Lose++;
                    break;
                case BattleLog.Result.TimeOver:
                    ArenaRecord.Draw++;
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException(nameof(result), result, null);
            }

            return 0;
        }

        public void Activate()
        {
            Active = true;
        }

        public void ResetCount()
        {
            DailyChallengeCount = GameConfig.ArenaChallengeCountMax;
        }

        public int GetRewardCount()
        {
            if (Score >= 1800)
            {
                return 6;
            }

            if (Score >= 1400)
            {
                return 5;
            }

            if (Score >= 1200)
            {
                return 4;
            }

            if (Score >= 1100)
            {
                return 3;
            }

            if (Score >= 1001)
            {
                return 2;
            }

            return 1;
        }
    }
}
