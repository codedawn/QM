using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace QM
{
    [MessageIndex(250)]
    [MessagePackObject]
    public class AllRewardPush : IPush
    {
        [Key(0)]
        public List<RewardState> rewardStates;
    }
}
