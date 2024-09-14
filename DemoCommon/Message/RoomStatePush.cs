using MessagePack;
using QM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    [MessageIndex(200)]
    [MessagePackObject]
    public class RoomStatePush : IPush
    {
        [Key(0)]
        public List<PlayerState> playerStates;
    }
}
