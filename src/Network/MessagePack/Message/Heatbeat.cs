using MessagePack;
using QM;
using QM.Network;
using Src.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM.Network
{
    [MessageDispatch("connector")]
    [MessageIndex(0)]
    [MessagePackObject]
    public class Heatbeat : IMessage
    {
    }
}
