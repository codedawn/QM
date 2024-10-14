using DotNetty.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using QM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace DotNettyRPC
{
    /// <summary>
    /// RPC服务端
    /// </summary>
    public class RPCServer
    {
        private ILog _log = new NLogger(typeof(RPCServer));

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="port">监听端口</param>
        public RPCServer(int port)
        {
            _port = port;

            _serverBootstrap = new ServerBootstrap()
                .Group(new MultithreadEventLoopGroup(), new MultithreadEventLoopGroup())
                .Channel<TcpServerSocketChannel>()
                .Option(ChannelOption.SoBacklog, 100)
                .Option(ChannelOption.TcpNodelay, true)
                .ChildHandler(new ActionChannelInitializer<IChannel>(channel =>
                {
                    IChannelPipeline pipeline = channel.Pipeline;
                    pipeline.AddLast("framing-enc", new LengthFieldPrepender(8));
                    pipeline.AddLast("framing-dec", new LengthFieldBasedFrameDecoder(int.MaxValue, 0, 8, 0, 8));

                    pipeline.AddLast(new ServerHandler(this));
                }));
        }

        #endregion

        #region 私有成员

        private int _port { get; set; }
        private Dictionary<string, Type> _serviceHandle { get; set; } = new Dictionary<string, Type>();
        private ServerBootstrap _serverBootstrap { get; }
        private IChannel _serverChannel { get; set; }

        private Type voidTaskResultType = Type.GetType("System.Threading.Tasks.VoidTaskResult", throwOnError: false);
        internal async Task<RPCResponse> GetResponse(RPCRequest request)
        {
            RPCResponse response = new RPCResponse();
            response.Id = request.Id;
            try
            {
                var requestModel = request;
                if (!_serviceHandle.ContainsKey(requestModel.ServiceName))
                {
                    throw new QMException(ErrorCode.RPCNotFoundService, $"RPC调用未找到该服务{requestModel.ServiceName}");
                }
                //todo 可以做缓存
                Type serviceType = _serviceHandle[requestModel.ServiceName];
                object service = Activator.CreateInstance(serviceType);
                //只能获取第一个方法，所以不能重名
                var method = serviceType.GetMethod(requestModel.MethodName);
                if (method == null)
                    throw new QMException(ErrorCode.RPCNotFoundMethod, $"RPC调用未找到该方法{requestModel.MethodName}");
                List<short> paramterIndexs = requestModel.ParamterIndexs;
                object[] paramters = requestModel.Paramters.ToArray();
                for (int i = 0; i < paramters.Length; i++)
                {
                    Type type = MessageOpcodeHelper.GetType(paramterIndexs[i]);
                    byte[] bytes = MessagePackUtil.Serialize(paramters[i]);
                    paramters[i] = MessagePackUtil.Deserialize(type, bytes);
                }
                object res = method.Invoke(service, paramters);
                if (res is Task task)
                {
                    await task;
                    var resultProperty = task.GetType().GetProperty("Result");

                    if (resultProperty != null)
                    {
                        res = resultProperty.GetValue(task);
                        if (res != null && res.GetType() == voidTaskResultType)
                        {
                            res = null;
                        }
                    }
                    else
                    {
                        res = null;
                    }

                }

                response.Success = true;
                if (res != null)
                {
                    response.DataIndex = (short)MessageOpcodeHelper.GetIndex(res.GetType());
                    response.Data = MessagePackUtil.Serialize(res);
                }
                else
                {
                    response.DataIndex = -1;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Msg = ExceptionHelper.GetExceptionAllMsg(ex);
#if DEBUG
                throw;
#endif
                _log.Error(ex);
            }
            return response;
        }

        #endregion

        #region 外部接口

        /// <summary>
        /// 注册服务
        /// 注：默认注册的服务为接口名
        /// </summary>
        /// <typeparam name="IService">服务接口</typeparam>
        /// <typeparam name="Service">服务实现</typeparam>
        public void RegisterService<IService, Service>() where Service : class, IService where IService : class
        {
            RegisterService<IService, Service>(typeof(IService).Name);
        }

        /// <summary>
        /// 注册服务,指定服务名
        /// </summary>
        /// <typeparam name="IService">服务接口</typeparam>
        /// <typeparam name="Service">服务实现</typeparam>
        /// <param name="serviceName">服务名</param>
        public void RegisterService<IService, Service>(string serviceName) where Service : class, IService where IService : class
        {
            _serviceHandle.Add(serviceName, typeof(Service));
        }

        /// <summary>
        /// 开始运行服务
        /// </summary>
        public void Start()
        {
            _serverChannel = _serverBootstrap.BindAsync(_port).Result;
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void Stop()
        {
            _serverChannel.CloseAsync();
        }

        #endregion
    }
}
