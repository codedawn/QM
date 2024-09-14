using System.Diagnostics;
using System.Threading.Tasks;

namespace QM
{
    public class MessageDispatcher
    {
        private Application _application;
        private RpcComp _rpcComp;
        private ILog _log = new NLogger(typeof(MessageDispatcher));

        public MessageDispatcher(Application application)
        {
            _application = application;
            _rpcComp = _application.GetComponent<RpcComp>();
        }

        public async Task<IResponse> DispatchAsync(IMessage message, ISession session, RouteInfo routeInfo)
        {
            //当前服务器处理
            if(_application.serverType == routeInfo.ServerType)
            {
#if DEBUG
                Stopwatch stopwatch = Stopwatch.StartNew();
#endif
                IResponse response = await DoHandleAsync(message, session);
#if DEBUG
                stopwatch.Stop();
                _log.Debug($"执行DoHandleAsync耗时：${stopwatch.ElapsedMilliseconds}ms");
#endif
                return response;
            }
            //转发
            else
            {
#if DEBUG
                Stopwatch stopwatch = Stopwatch.StartNew();
#endif
                IResponse response = await DoForwardAsync(message, session, routeInfo);
#if DEBUG
                stopwatch.Stop();
                _log.Debug($"执行DoForwardAsync耗时：${stopwatch.ElapsedMilliseconds}ms");
#endif
                return response;
            }
        }

        public async Task<IResponse> DoHandleAsync(IMessage message, ISession session)
        {
            return await MessageHandleDispather.Instance.Handle(message, session);
        }

        public async Task<IResponse> DoForwardAsync(IMessage message, ISession session, RouteInfo routeInfo)
        {
            return await _rpcComp.ForwardToServer(message, session, routeInfo);
        }
    }
}
