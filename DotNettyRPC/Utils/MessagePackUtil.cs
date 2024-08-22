using MessagePack;
using System;

namespace DotNettyRPC
{
    public class MessagePackUtil
    {
        public static byte[] Serialize(object v)
        {
            return MessagePackSerializer.Serialize(v);
        }

        public static T Deserialize<T>(byte[] bytes)
        {
            return MessagePackSerializer.Deserialize<T>(bytes);
        }

        public static object Deserialize(Type type, byte[] bytes)
        {
            return MessagePackSerializer.Deserialize(type, bytes);
        }
    }
}
