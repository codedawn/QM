namespace QM
{
    public class EventSystem
    {
        public static readonly EventSystem Instance = new EventSystem();
        private readonly Dictionary<Type, List<IEventHandler>> _eventHandlers = new Dictionary<Type, List<IEventHandler>>();

        private EventSystem()
        {
            foreach (Type type in CodeType.Instance.GetTypes(typeof(EventHandlerAttribute)))
            {
                IEventHandler handler = (IEventHandler)Activator.CreateInstance(type);
                Type eventType = handler.GetEventType();
                if (_eventHandlers.TryGetValue(eventType, out List<IEventHandler> v))
                {
                    v.Add(handler);
                }
                else
                {
                    List<IEventHandler> handlers = new List<IEventHandler>
                    {
                        handler
                    };
                    _eventHandlers.Add(eventType, handlers);
                }
            }
        }

        public async Task Publish(IEvent e)
        {
            if (_eventHandlers.TryGetValue(e.GetType(), out List<IEventHandler> handlers))
            {
                foreach (IEventHandler handler in handlers)
                {
                    await handler.Handle(e);
                }
            }
        }

    }
}
