using DotNetty.Buffers;
using MessagePack;
using QM.Log;

namespace QM.Network
{
    /// <summary>
    /// 协议结构：
    /// length(4 bytes) + index (2 byte) + data bytes
    /// </summary>
    public class QMProtocol : IProtocol
    {
        private ILog _log;
        private byte _headerLength = 4;
        private byte _indexLength = sizeof(short);

        public QMProtocol()
        {
            _log = new ConsoleLog();
        }

        public void Decode(IByteBuffer input, List<object> output)
        {
            if (_headerLength > input.ReadableBytes)
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
            short index = input.ReadShort();
            //todo ping
            //if (dataLength - 1 == 0)
            //{
            //    return;
            //}
            byte[] bytes = new byte[dataLength - _indexLength];
            input.ReadBytes(bytes);
            var message = Deserialize(index, bytes);
            if (message != null)
            {
                output.Add(message);
            }
        }

        protected virtual object Deserialize(short index, Byte[] bytes)
        {
            Type type = MessageOpcode.Instance.GetType(index);
            if(type == null)
            {
                _log.Error($"无法找到对应index:{index}的消息类型");
                return null;
            }
            object o = MessagePackSerializer.Deserialize(type, bytes);
            if(o is IMessage message)
            {
                return message;
            }
            else
            {
                _log.Warn($"消息类型{o}异常");
                return null;
            }
        }

        public void Encode(object msg, IByteBuffer output)
        {
            short? index = MessageOpcode.Instance.GetIndex(msg.GetType());
            if (index == null)
            {
                _log.Error($"无法找到对应消息类型为Type:{msg.GetType()}的index");
                return;
            }
            byte[] bytes = Serialize(msg);
            output.WriteInt(bytes.Length + _indexLength);
            output.WriteShort((short)index);
            output.WriteBytes(bytes);
        }

        protected virtual byte[] Serialize(object msg)
        {
            return MessagePackSerializer.Serialize(msg);
        }
    }
}
