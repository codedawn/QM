namespace QM
{
    public class MessageDispatcher
    {
        private Application _application;
        private RpcComp _rpcForward;

        public MessageDispatcher(Application application)
        {
            _application = application;
            _rpcForward = _application.GetComponent<RpcComp>();
        }

        public IResponse Dispatch(IMessage message, ISession session, RouteInfo routeInfo)
        {
            //当前服务器处理
            if(_application.serverType == routeInfo.ServerType)
            {
                return DoHandle(message, session);
            }
            //转发
            else
            {
                return DoForward(message, session, routeInfo);
            }
        }

        public IResponse DoHandle(IMessage message, ISession session)
        {
            return MessageHandleDispather.Instance.Handle(message, session);
        }

        public IResponse DoForward(IMessage message, ISession session, RouteInfo routeInfo)
        {
            return _rpcForward.Forward(message, session, routeInfo);
        }
    }
}
