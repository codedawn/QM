using DotNetty.Buffers;
using DotNetty.Common;
using MessagePack.Resolvers;
using MessagePack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace QM
{
    public class Application
    {
        public const string Connector = ServerType.Connector;
        public const string Server = ServerType.Server;

        private List<IComponent> _components;
        private ApplicationState _state;
        private ILog _log;
        private ILog _Nlog;
        private ISessionFactory _sessionFactory;
        private bool _simpleDebug;//单线程处理消息，调试会简单很多，多线程调试需要锁定某个线程
        private TaskTimer _timer;

        public readonly bool isConnector;
        public readonly string serverId;
        public readonly string serverType;
        public readonly int port;
        public readonly int rpcPort;
        public readonly string zookeeperIp;
        public readonly int zookeeperPort;
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

        private Application(string serverId, string serverType, int port, string zookeeperIp, int zookeeperPort, bool simpleDebug)
        {
            current = this;
            this.serverId = serverId;
            this.serverType = serverType;
            this.port = port;
            this.rpcPort = port + 1;
            this.zookeeperIp = zookeeperIp;
            this.zookeeperPort = zookeeperPort;


            _simpleDebug = simpleDebug;
            _timer = new TaskTimer();
            _components = new List<IComponent>();
            isConnector = serverType == Connector;
            _log = new ConsoleLogger();
            _Nlog = new NLogger(typeof(Application));
            _sessionFactory = new SessionFactory();
            _log.Info($"服务器:{serverId} port:{port} rpcport:{rpcPort}开始启动==================================");
            if (_simpleDebug)
            {
                _log.Warn("警告：调试模式下性能会下降");
            }
#if DEBUG
            ResourceLeakDetector.Level = ResourceLeakDetector.DetectionLevel.Paranoid;
            NLogger.logLevel = LogLevel.Debug;
#endif

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            //需要触发gc
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            _Nlog.Error(e.Exception);
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            _Nlog.Error((Exception)e.ExceptionObject);
        }

        public static Application CreateApplication(string serverId, string serverType, int port, string zookeeperIp = "127.0.0.1", int zookeeperPort = 2181, bool simpleDebug = false)
        {
            return new Application(serverId, serverType, port, zookeeperIp, zookeeperPort, simpleDebug);
        }

        /// <summary>
        /// 提前加载程序集，或者需要显示调用，不然程序集没加载找不到类型
        /// </summary>
        /// <param name="assemblies"></param>
        public void LoadAssembly(string[] assemblies)
        {
            if (_state != ApplicationState.None) throw new QMException(ErrorCode.ServerBootError, "必须在启动之前执行");
            foreach (string assembly in assemblies)
            {
                Assembly.Load(assembly);
            }
        }

        public void SetSessionFactory(ISessionFactory sessionFactory)
        {
            if (_state != ApplicationState.None) throw new QMException(ErrorCode.ServerBootError, "必须在启动之前执行");
            _sessionFactory = sessionFactory;
        }

        public ISessionFactory GetSessionFactory()
        {
            return _sessionFactory;
        }

        private void Init()
        {
            if (isConnector)
            {
                _components.Add(new ConnectorComp(this));
                _components.Add(new ServerComp(this));
                _components.Add(new RpcComp(this));
                _components.Add(new RouteComp(this));
                _components.Add(new ZookeeperComp(this));
                _components.Add(new SessionComp(this));
            }
            else
            {
                _components.Add(new ServerComp(this));
                _components.Add(new RpcComp(this));
                _components.Add(new RouteComp(this));
                _components.Add(new ZookeeperComp(this));
            }

            _state = ApplicationState.Init;
        }

        public void Start()
        {
            Init();

            var beginTime = Time.GetUtc8TimestampMilliseconds();
            if (_state >= ApplicationState.Start)
            {
                _Nlog.Error("已经初始化");
                return;
            }

            foreach (var component in _components)
            {
                component.Start();
            }
            _state = ApplicationState.Start;
            var endTime = Time.GetUtc8TimestampMilliseconds();
            _log.Debug($"Start 执行时间:{endTime - beginTime}ms");
            AfterStart();
        }

        private void AfterStart()
        {
            var beginTime = Time.GetUtc8TimestampMilliseconds();
            if (_state >= ApplicationState.AfterStart)
            {
                _Nlog.Error("已经执行AfterStart");
                return;
            }

            foreach (var component in _components)
            {
                component.AfterStart();
            }
            _state = ApplicationState.AfterStart;
            var endTime = Time.GetUtc8TimestampMilliseconds();
            _log.Debug($"AfterStart 执行时间:{endTime - beginTime}ms");
            StartLoop();
        }

        private void StartLoop()
        {
            Console.WriteLine("输入stop退出");
            while ("stop" != Console.ReadLine())
            {
                Thread.Sleep(1);
            }
            Stop();
        }

        public void Stop()
        {
            var beginTime = Time.GetUtc8TimestampMilliseconds();
            if (_state >= ApplicationState.Stop)
            {
                _Nlog.Error("已经执行Stop");
                return;
            }
            foreach (var component in _components)
            {
                component.Stop();
            }
            var endTime = Time.GetUtc8TimestampMilliseconds();
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

        public bool GetDebug()
        {
            return _simpleDebug;
        }

        public void TaskSchedule(Action action, int timeoutMs)
        {
            _timer.schedule(action, timeoutMs);
        }
    }
}
