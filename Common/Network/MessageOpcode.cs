using System;
using System.Collections.Generic;
using System.Reflection;

namespace QM
{
    /// <summary>
    /// 消息编码
    /// 一个消息对应一个short类型的编号
    /// </summary>
    public class MessageOpcode
    {
        public static MessageOpcode Instance = new MessageOpcode();
        private Dictionary<short, Type> _messageOpcode = new Dictionary<short, Type>();
        private Dictionary<Type, short> _messageOpcodeReverse = new Dictionary<Type, short>();

        private MessageOpcode()
        {
            foreach (Type type in CodeType.Instance.GetTypes(typeof(MessageIndexAttribute)))
            {
                var attribute = type.GetCustomAttribute(typeof(MessageIndexAttribute), false);
                if (attribute != null && attribute is MessageIndexAttribute m)
                {
                    AddMessageIndex(m.index, type);
                }
            }
        }

        public void AddMessageIndex(short index, Type type)
        {
            //不能出现两个相同的index
            if (!_messageOpcode.TryAdd(index, type) || !_messageOpcodeReverse.TryAdd(type, index))
            {
                throw new QMException(ErrorCode.MessageIndexDupli, $"不能定义两个相同的MessageIndex:{index} {type}");
            }
        }

        public Type GetType(short index)
        {
            if (!_messageOpcode.TryGetValue(index, out Type type))
            {
                throw new QMException(ErrorCode.MessageIndexNotFound, $"没有定义Type:{type}类型的MessageIndex");
            }
            return type;
        }

        public short GetIndex(Type type)
        {
            if (!_messageOpcodeReverse.TryGetValue(type, out short index))
            {
                throw new QMException(ErrorCode.MessageIndexNotFound, $"没有定义Type:{type}类型的MessageIndex");
            }
            return index;
        }
    }
}
