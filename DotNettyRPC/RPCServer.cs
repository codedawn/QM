﻿using Coldairarrow.Util;
using DotNetty.Codecs;
using DotNetty.Transport.Bootstrapping;
using DotNetty.Transport.Channels;
using DotNetty.Transport.Channels.Sockets;
using DotNettyRPC.Helper;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Coldairarrow.DotNettyRPC
{
    /// <summary>
    /// RPC服务端
    /// </summary>
    public class RPCServer
    {
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
        ServerBootstrap _serverBootstrap { get; }
        IChannel _serverChannel { get; set; }
        internal ResponseModel GetResponse(RequestModel request)
        {
            ResponseModel response = new ResponseModel();

            try
            {
                var requestModel = request;
                if (!_serviceHandle.ContainsKey(requestModel.ServiceName))
                    throw new Exception("未找到该服务");
                //todo 可以做缓存
                var serviceType = _serviceHandle[requestModel.ServiceName];
                var service = Activator.CreateInstance(serviceType);
                var method = serviceType.GetMethod(requestModel.MethodName);
                if (method == null)
                    throw new Exception("未找到该方法");
                List<short> generics = requestModel.Generics;
                List<short> paramterIndexs = requestModel.ParamterIndexs;
                object[] paramters = requestModel.Paramters.ToArray();
                var methodParamters = method.GetParameters();
                for (int i = 0; i < methodParamters.Length; i++)
                {
                    if (paramters[i].GetType() != methodParamters[i].ParameterType)
                    {
                        paramters[i] = paramters[i].ToJson().ToObject(MessageOpcodeHelper.GetType(paramterIndexs[i]));
                    }
                }
                Type[] genericTypes = new Type[generics.Count];
                for (int i = 0; i < generics.Count; i++)
                {
                    //todo 可以考虑string
                    genericTypes[i] = MessageOpcodeHelper.GetType(generics[i]);
                }
                if (method.IsGenericMethod)
                {
                    method = method.MakeGenericMethod(genericTypes);
                }
                var res = method.Invoke(service, paramters);

                response.Success = true;
                if (res != null)
                {
                    response.DataIndex = (byte)MessageOpcodeHelper.GetIndex(res.GetType());
                    response.Data = res.ToJson();
                }
                else
                {
                    response.DataIndex = -1;
                }
                response.Msg = "请求成功";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Msg = ExceptionHelper.GetExceptionAllMsg(ex);
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
