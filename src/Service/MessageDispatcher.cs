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

        public async Task<IResponse> DispatchAsync(IMessage message, ISession session, RouteInfo routeInfo)
        {
            //当前服务器处理
            if(_application.serverType == routeInfo.ServerType)
            {
                return await DoHandleAsync(message, session);
            }
            //转发
            else
            {
                return await DoForwardAsync(message, session, routeInfo);
            }
        }

        public async Task<IResponse> DoHandleAsync(IMessage message, ISession session)
        {
            return await MessageHandleDispather.Instance.Handle(message, session);
        }

        public async Task<IResponse> DoForwardAsync(IMessage message, ISession session, RouteInfo routeInfo)
        {
            return await _rpcForward.ForwardToServer(message, session, routeInfo);
        }
    }
}
