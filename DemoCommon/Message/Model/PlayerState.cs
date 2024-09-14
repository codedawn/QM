using MessagePack;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace QM
{
    [MessagePackObject]
    public class PlayerState
    {
        [Key(0)]
        public float posX;
        [Key(1)]
        public float posY;
        [Key(2)]
        public int point;
        [Key(3)]
        public long uid;
    }
}
