using System;
using System.Collections.Generic;
using System.Text;

namespace DotNettyRPC
{
    public interface IRpcMessageOpcode
    {
        public Type GetType(short index);
        public short? GetIndex(Type type);
    }
}
