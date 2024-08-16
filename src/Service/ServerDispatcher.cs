﻿using QM;
using QM.Component;
using QM.Network;
using Src.Attribute;
using System.Reflection;

namespace QM.Service
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
                    _server.Add(type, attribute.server);
                }
            }
        }

        public string Dispatch(IMessage message)
        {
            _server.TryGetValue(message.GetType(), out var server);
            return server;
        }
    }
}
