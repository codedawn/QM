namespace QM
{
    /**
     * connector组件是socket的一个上层封装，处理socket事件（连接，断开，接受、发送消息，调度其他组件维护连接信息）
     */
    public class ConnectorComp : Component
    {
        private Application application;
        private ISocket socket;
        private ConnectionManager _connectionManager;
        private ILog log;
        private IMsgSchedule schedule;
        private ServerComp _server;
        private SessionComp _sessionComp;

        public ConnectorComp(Application application)
        {
            this.application = application;
            this._connectionManager = new ConnectionManager();
            this.socket = new SocketServer();
            this.log = new ConsoleLog();
        }

        public override void Start()
        {
            _server = application.GetComponent<ServerComp>();
            _sessionComp = application.GetComponent<SessionComp>();
            base.Start();
        }

        public override void AfterStart()
        {
            StartListen();
            base.AfterStart();
        }

        private void StartListen()
        {
            socket.onConnect += OnConnect;
            socket.onDisConnect += OnDisConnect;
            socket.onMessage += OnMessage;
            socket.Start();
        }

        private void OnConnect(IConnection connection)
        {
            var totalConn = _connectionManager.GetConnectionCount();
            if (totalConn > application.maxConnectCount)
            {
                log.Warn($"连接数达到了{totalConn},最大连接数配置为{application.maxConnectCount},所以断开当前连接");
                connection.Close();
                return;
            }
            _connectionManager.Add(connection.Address, connection);
            Session session = new Session(connection.Cid, connection, TimeUtils.GetUnixTimestampMilliseconds());
            _sessionComp.Add(session.Sid, session);
        }

        private void OnDisConnect(IConnection connection)
        {
            _connectionManager.Remove(connection.Address);
        }

        private void OnMessage(IMessage message, IConnection connection)
        {
            Session session = (Session)_sessionComp.Get(connection.Cid);
            HandleMessage(message, session);
        }

        private void HandleMessage(IMessage message, Session session)
        {
            _server.GlobalHandle(message, session);
        }

        public override void Stop()
        {
            base.Stop();
        }
    }
}
