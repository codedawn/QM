using QM.Component;
using QM.Handle;
using QM.Network;

namespace QM.Service
{
    public class MessageDispatcher
    {
        private Application _application;
        private RpcForwardComp _rpcForward;

        public MessageDispatcher(Application application)
        {
            _application = application;
            _rpcForward = _application.GetComponent<RpcForwardComp>();
        }

        public IResponse Dispatch(IRequest request, Session session, RouteInfo routeInfo)
        {
            //当前服务器处理
            if(_application.serverType == routeInfo.ServerType)
            {
                return DoHandle(request, session);
            }
            //转发
            else
            {
                return DoForward(request, session, routeInfo);
            }
        }

        public IResponse DoHandle(IRequest request, Session session)
        {
            return MessageHandleDispather.Instance.Handle(request, session);
        }

        public IResponse DoForward(IRequest request, Session session, RouteInfo routeInfo)
        {
            return _rpcForward.Forward(request, session, routeInfo);
        }
    }
}
