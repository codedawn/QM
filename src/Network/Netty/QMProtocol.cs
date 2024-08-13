using DotNetty.Buffers;
using MessagePack;
using QM.Log;
using System.Buffers;
using System.Reflection;

namespace QM.Network
{
    /// <summary>
    /// 协议结构：
    /// length(4 bytes) + index (1 byte) + data bytes
    /// </summary>
    public class QMProtocol : IProtocol
    {
        private ILog log;
        private byte HeaderLength => 4;

        private Dictionary<byte, Type> messageOpcode = new Dictionary<byte, Type>();

        public QMProtocol()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                foreach(var type in assembly.GetTypes())
                {
                    var attribute = type.GetCustomAttribute(typeof(MessageIndexAttribute), false);
                    if(attribute != null && attribute is MessageIndexAttribute m)
                    {
                        //不能出现两个相同的index
                        messageOpcode.Add(m.index, type);
                    }
                }
            }
        }

        public void Decode(IByteBuffer input, List<object> output)
        {
            if (HeaderLength > input.ReadableBytes)
            {
                return;
            }
            input.MarkReaderIndex();
            int dataLength = input.ReadInt();
            if (dataLength > input.ReadableBytes)
            {
                input.ResetReaderIndex();
                return;
            }

            byte index = input.ReadByte();
            //todo ping
            //if (dataLength - 1 == 0)
            //{
            //    return;
            //}
            byte[] bytes = new byte[dataLength - 1];
            input.ReadBytes(bytes);
            var message = Deserialize(index, bytes);
            if (message != null)
            {
                output.Add(message);
            }
        }

        protected virtual object Deserialize(Byte index, Byte[] bytes)
        {
            messageOpcode.TryGetValue(index, out Type type);
            if(type == null)
            {
                log.Warn($"无法找到对应index:{index}的消息类型");
                return null;
            }
            object o = MessagePackSerializer.Deserialize(type, bytes);
            if(o is IMessage message)
            {
                return message;
            }
            else
            {
                log.Warn($"消息类型{o}异常");
                return null;
            }
        }

        public void Encode(object msg, IByteBuffer output)
        {
            byte? index = GetKeyByValue(messageOpcode, msg.GetType());
            if (index == null)
            {
                log.Warn($"无法找到对应IMessage:{msg.GetType()}的index");
                return;
            }
            byte[] bytes = Serialize(msg);
            output.WriteInt(bytes.Length + 1);
            output.WriteByte((byte)index);
            output.WriteBytes(bytes);
        }

        protected virtual byte[] Serialize(object msg)
        {
            return MessagePackSerializer.Serialize(msg);
        }

        private byte? GetKeyByValue(Dictionary<byte, Type> messageOpcode, Type value)
        {
            foreach (var kvp in messageOpcode)
            {
                if (kvp.Value == value)
                {
                    return kvp.Key;
                }
            }
            return null;
        }
    }
}
