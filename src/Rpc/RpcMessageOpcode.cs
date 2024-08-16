using DotNettyRPC;
using QM.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM.Rpc
{
    public class RpcMessageOpcode : IRpcMessageOpcode
    {
        public short? GetIndex(Type type)
        {
            return MessageOpcode.Instance.GetIndex(type);
        }

        public Type GetType(short index)
        {
            return MessageOpcode.Instance.GetType(index);
        }
    }
}
