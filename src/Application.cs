using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace QM
{
    public class Application
    {
        public const string Connector = "Connector";
        public const string Server = "Server";

        private List<IComponent> _components;
        private ApplicationState _state;
        private ILog _log;

        public readonly bool isConnector;
        public readonly string serverId;
        public readonly string serverType;
        public readonly int port;
        public readonly int rpcPort;
        public int maxConnectCount = 100000;

        private static Application _current;
        public static Application current
        {
            get { return _current; }
            private set
            {
                if (_current != null) throw new QMException(ErrorCode.ServerBootDupli, "一个进程无法启动多个服务器");
                _current = value;
            }
        }

        private Application(string serverId, string serverType, int port)
        {
            current = this;
            this.serverId = serverId;
            this.serverType = serverType;
            this.port = port;
            this.rpcPort = port + 1;

            _components = new List<IComponent>();
            isConnector = serverType == Connector;
            _log = new ConsoleLogger();
            _log.Info($"服务器:{serverId} port:{port} rpcport:{rpcPort}开始启动==================================");

            Init();

        }

        private void Init()
        {
            if (isConnector)
            {
                _components.Add(new ConnectorComp(this));
                _components.Add(new ServerComp(this));
                _components.Add(new RpcComp(this));
                _components.Add(new RouteComp(this));
                _components.Add(new ZookDiscoverComp(this));
                _components.Add(new SessionComp(this));
            }
            else
            {
                _components.Add(new ServerComp(this));
                _components.Add(new RpcComp(this));
                _components.Add(new RouteComp(this));
                _components.Add(new ZookDiscoverComp(this));
            }

            _state = ApplicationState.Init;
        }

        public static Application CreateApplication(string serverId, string serverType, int port)
        {
            return new Application(serverId, serverType, port);
        }

        public void Start()
        {
            var beginTime = Time.GetUnixTimestampMilliseconds();
            if (_state >= ApplicationState.Start)
            {
                _log.Error("已经初始化");
                return;
            }

            foreach (var component in _components)
            {
                component.Start();
            }
            _state = ApplicationState.Start;
            var endTime = Time.GetUnixTimestampMilliseconds();
            _log.Debug($"Start 执行时间:{endTime - beginTime}ms");
            AfterStart();
        }

        private void AfterStart()
        {
            var beginTime = Time.GetUnixTimestampMilliseconds();
            if (_state >= ApplicationState.AfterStart)
            {
                _log.Error("已经执行AfterStart");
                return;
            }

            foreach (var component in _components)
            {
                component.AfterStart();
            }
            _state = ApplicationState.AfterStart;
            var endTime = Time.GetUnixTimestampMilliseconds();
            _log.Debug($"AfterStart 执行时间:{endTime - beginTime}ms");
            StartLoop();
        }

        private void StartLoop()
        {
            Console.ReadLine();
        }

        public void Stop()
        {
            var beginTime = Time.GetUnixTimestampMilliseconds();
            if (_state >= ApplicationState.Stop)
            {
                _log.Error("已经执行Stop");
                return;
            }
            foreach (var component in _components)
            {
                component.Stop();
            }
            var endTime = Time.GetUnixTimestampMilliseconds();
            _log.Debug($"Stop 执行时间:{endTime - beginTime}ms");
        }

        public T GetComponent<T>()
        {
            foreach (var component in _components)
            {
                if (component.GetType() == typeof(T))
                {
                    return (T)component;
                }
            }
            return default(T);
        }

        public void AddComponent(IComponent component)
        {
            _components.Add(component);
        }
    }
}
