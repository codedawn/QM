using Dynamitey;
using ImpromptuInterface;
using System;
using System.Collections.Concurrent;

namespace DotNettyRPC
{
    /// <summary>
    /// 客户端工厂
    /// </summary>
    public class RPCClientFactory
    {
        private static ConcurrentDictionary<string, object> _services { get; } = new ConcurrentDictionary<string, object>();

        /// <summary>
        /// 获取客户端
        /// 注：默认服务名为接口名
        /// </summary>
        /// <typeparam name="T">接口定义类型</typeparam>
        /// <param name="serverIp">远程服务IP</param>
        /// <param name="port">远程服务端口</param>
        /// <param name="timeout">超时时间（ms）</param>
        /// <returns></returns>
        public static T GetClient<T, IResult>(string serverIp, int port, int timeout = 2000) where T : class
        {
            return GetClient<T, IResult>(serverIp, port, typeof(T).Name, timeout);
        }

        /// <summary>
        /// 获取客户端
        /// 注：自定义服务名
        /// </summary>
        /// <typeparam name="T">接口定义类型</typeparam>
        /// <param name="serverIp">远程服务IP</param>
        /// <param name="port">远程服务端口</param>
        /// <param name="serviceName">服务名</param>
        /// <returns></returns>
        public static T GetClient<T, IResult>(string serverIp, int port, string serviceName, int timeout) where T : class
        {
            T service = null;
            string key = $"{serviceName}-{serverIp}-{port}";
            try
            {
                service = (T)_services[key];
            }
            catch
            {
                var clientProxy = new RPCClientProxy<IResult>(serverIp, port, typeof(T), timeout);
                service = clientProxy.ActLike<T>();
                _services[key] = service;
            }

            return service;
        }
    }
}
