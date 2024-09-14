using MessagePack;
using System;
using System.Collections.Generic;
using System.Text;

namespace QM
{
    [MessagePackObject]
    public class RewardState
    {
        [Key(0)]
        public int id;
        [Key(1)]
        public float x;
        [Key(2)]
        public float y;
    }
}
