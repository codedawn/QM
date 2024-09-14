using DotNettyRPC;
using System;

namespace QM
{
    public class RpcMessageOpcode : IRpcMessageOpcode
    {
        public short GetIndex(Type type)
        {
            return MessageOpcode.Instance.GetIndex(type);
        }

        public Type GetType(short index)
        {
            return MessageOpcode.Instance.GetType(index);
        }
    }
}
