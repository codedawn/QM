using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace QM
{
    [MessageIndex(210)]
    [MessagePackObject]
    public class AddRewardPush : IPush
    {
        [Key(0)]
        public RewardState reward;
    }
}
