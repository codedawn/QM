using System.Diagnostics;
using System.Threading.Tasks;

namespace QM
{
    public class MessageDispatcher
    {
        private Application _application;
        private RpcComp _rpcForward;
        private ILog _log = new NLogger(typeof(MessageDispatcher));

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
                Stopwatch stopwatch = Stopwatch.StartNew();
                IResponse response = await DoHandleAsync(message, session);
                stopwatch.Stop();
                _log.Debug($"执行DoHandleAsync耗时：${stopwatch.ElapsedMilliseconds}ms");
                return response;
            }
            //转发
            else
            {
                IResponse response = await DoForwardAsync(message, session, routeInfo);
                return response;
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
