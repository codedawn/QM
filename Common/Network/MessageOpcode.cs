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
                    //不能出现两个相同的index
                    if (!_messageOpcode.TryAdd(m.index, type))
                    {
                        throw new QMException(ErrorCode.MessageIndexDupli, $"不能定义两个相同的MessageIndex:{m.index} {type}");
                    }
                    _messageOpcodeReverse.Add(type, m.index);
                }
            }
        }

        public Type GetType(short index)
        {
            _messageOpcode.TryGetValue(index, out Type type);
            return type;
        }

        public short? GetIndex(Type type)
        {
            if (!_messageOpcodeReverse.TryGetValue(type, out short index))
            {
                return null;
            }
            return index;
        }
    }
}
