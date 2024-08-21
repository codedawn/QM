using System.Reflection;

namespace QM
{
    public class ServerDispatcher
    {
        private readonly Application application;
        private readonly Dictionary<Type, string> _server = new Dictionary<Type, string>();

        public ServerDispatcher(Application application)
        {
            this.application = application;

            foreach (Type type in CodeType.Instance.GetTypes(typeof(MessageDispatchAttribute)))
            {
                MessageDispatchAttribute attribute = type.GetCustomAttribute<MessageDispatchAttribute>(false);
                if (attribute != null)
                {
                    _server.Add(type, attribute.serverType);
                }
            }
        }

        /// <summary>
        /// 获取处理消息的服务器类型
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string Dispatch(IMessage message)
        {
            if (_server.TryGetValue(message.GetType(), out var server))
            {
                return server;
            }
            throw new Exception($"该message：{message}没有使用MessageDisaptchAttribute标记转发的serverType");
        }
    }
}
