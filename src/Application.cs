using QM.Component;
using QM.Log;
using QM.Network;
using QM.Utils;

namespace QM
{
    public class Application
    {
        private List<IComponent> components;
        private ApplicationState state;
        private ILog log;
        private string serverId;
        public readonly string serverType;

        public int maxConnectCount = 10000;
        public SessionManager sessionManager;

        private Application()
        {
            components = new List<IComponent>();
            sessionManager = new SessionManager();
            log = new ConsoleLog();

            Init();
        }

        private void Init()
        {
            components.Add(new ConnectorComp(this));
            components.Add(new ServerComp(this));
            components.Add(new RpcForwardComp(this));
            components.Add(new LoadBalanceComp(this));
            components.Add(new ZookDiscoverComp(this));
            state = ApplicationState.Init;
        }

        public static Application CreatApplication()
        {
            Application application = new Application();
            return application;
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
