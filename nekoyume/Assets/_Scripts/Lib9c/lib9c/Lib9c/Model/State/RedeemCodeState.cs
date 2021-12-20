using System;
using Nekoyume.TableData;

namespace Nekoyume.Model.State
{
    [Serializable]
    public class RedeemCodeState : State
    {        
        public class Reward
        {
            public readonly int RewardId;

            public Reward(int rewardId)
            {
                RewardId = rewardId;
            }
        }

        public RedeemCodeState(RedeemCodeListSheet sheet)
        {            
        }
    }
}
