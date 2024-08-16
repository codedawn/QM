using QM.Log;
using QM.Network;
using src.Attribute;
using src.Handle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM.Handle
{
    /// <summary>
    /// 分发消息到具体的handle
    /// </summary>
    public class MessageHandleDispather
    {
        private ILog _log = new ConsoleLog();
        public static readonly MessageHandleDispather Instance = new MessageHandleDispather();
        public Dictionary<Type, IMHandler> handlers = new Dictionary<Type, IMHandler>();
        private MessageHandleDispather() 
        {
            foreach (Type type in CodeType.Instance.GetTypes(typeof(MessageHandlerAttribute)))
            {
                IMHandler handler = Activator.CreateInstance(type) as IMHandler;
                if (!handlers.TryAdd(handler.GetMessageType(), handler))
                {
                    throw new Exception($"不能定义两个相同的Request消息处理handler:{type}");
                }
            }
        }

        public IResponse Handle(IMessage message, Session session)
        {
            handlers.TryGetValue(message.GetType(), out IMHandler handler);
            if (handler != null)
            {
                IResponse response = handler.Handle(message, session);
                return response;
            }
            _log.Warn($"没有handle处理对应的message:{message}");
            return null;
        }
    }
}
