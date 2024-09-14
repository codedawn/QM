using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace QM
{
    [MessageIndex(220)]
    [MessagePackObject]
    public class TouchRewardPush : IPush
    {
        [Key(0)]
        public int id;
    }
}
