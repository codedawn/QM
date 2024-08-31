using System.Diagnostics;

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
        public int maxConnectCount = 10000;

        public static Application current;

        private Application(string serverId, string serverType, int port)
        {
            this.serverId = serverId;
            this.serverType = serverType;
            this.port = port;
            this.rpcPort = port + 1;

            _components = new List<IComponent>();
            isConnector = serverType == Connector;
            _log = new ConsoleLog();
            _log.Info($"服务器{serverId}开始启动==================================");

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
            current = new Application(serverId, serverType, port);
            return current;
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
            AfterStart();
            var endTime = Time.GetUnixTimestampMilliseconds();
            _log.Debug($"Start 执行时间:{endTime - beginTime}ms");
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
            Stopwatch stopwatch = Stopwatch.StartNew();
            Time.LastTime = stopwatch.Elapsed.TotalMilliseconds;
            while (_state == ApplicationState.AfterStart)
            {
                try
                {
                    double curTime = stopwatch.Elapsed.TotalMilliseconds;
                    Time.DeltaTime = (curTime - Time.LastTime);
                    Time.LastTime = curTime;
                    Time.AccTime += Time.DeltaTime;
                    //_log.Info($"deltaTime:{deltaTime}");
                    Thread.Sleep(1);
                }
                catch (Exception ex)
                {
                    _log.Error(ExceptionUtils.Print(ex));
                }
            }
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
                if(component.GetType() == typeof(T))
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
