namespace QM
{
    public class Application
    {
        public const string Connector = "Connector";
        public const string Server = "Server";

        private List<IComponent> components;
        private ApplicationState state;
        private ILog log;

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

            components = new List<IComponent>();
            isConnector = serverType == Connector;
            log = new ConsoleLog();

            Init();
        }

        private void Init()
        {
            if (isConnector)
            {
                components.Add(new ConnectorComp(this));
                components.Add(new ServerComp(this));
                components.Add(new RpcComp(this));
                components.Add(new RouteComp(this));
                components.Add(new ZookDiscoverComp(this));
                components.Add(new SessionComp(this));
            }
            else
            {
                components.Add(new ServerComp(this));
                components.Add(new RpcComp(this));
                components.Add(new RouteComp(this));
                components.Add(new ZookDiscoverComp(this));
            }
           
            state = ApplicationState.Init;
        }

        public static Application CreateApplication(string serverId, string serverType, int port)
        {
            current = new Application(serverId, serverType, port);
            return current;
        }

        public void Start()
        {
            var beginTime = TimeUtils.GetUnixTimestampMilliseconds();
            if (state >= ApplicationState.Start)
            {
                log.Error("已经初始化");
                return;
            }

            foreach (var component in components)
            {
                component.Start();
            }
            state = ApplicationState.Start;
            AfterStart();
            var endTime = TimeUtils.GetUnixTimestampMilliseconds();
            log.Debug($"Start 执行时间:{endTime - beginTime}ms");
        }

        private void AfterStart()
        {
            var beginTime = TimeUtils.GetUnixTimestampMilliseconds();
            if (state >= ApplicationState.AfterStart)
            {
                log.Error("已经执行AfterStart");
                return;
            }

            foreach (var component in components)
            {
                component.AfterStart();
            }
            state = ApplicationState.AfterStart;
            var endTime = TimeUtils.GetUnixTimestampMilliseconds();
            log.Debug($"AfterStart 执行时间:{endTime - beginTime}ms");
        }

        public void Stop()
        {
            var beginTime = TimeUtils.GetUnixTimestampMilliseconds();
            if (state >= ApplicationState.Stop)
            {
                log.Error("已经执行Stop");
                return;
            }
            foreach (var component in components)
            {
                component.Stop();
            }
            var endTime = TimeUtils.GetUnixTimestampMilliseconds();
            log.Debug($"Stop 执行时间:{endTime - beginTime}ms");
        }

        public T GetComponent<T>()
        {
            foreach (var component in components)
            {
                if(component.GetType() == typeof(T))
                {
                    return (T)component;
                }
            }
            return default(T);
        }
    }
}
