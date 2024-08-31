namespace QM
{
    /**
     * connector组件是socket的一个上层封装，处理socket事件（连接，断开，接受、发送消息，调度其他组件维护连接信息）
     */
    public class ConnectorComp : Component
    {
        private Application _application;
        private ISocket _socket;
        private ConnectionManager _connectionManager;
        private ILog _log;
        private IMsgSchedule _schedule;
        private ServerComp _server;
        private SessionComp _sessionComp;

        public ConnectorComp(Application application)
        {
            this._application = application;
            this._connectionManager = new ConnectionManager();
            this._socket = new SocketServer();
            this._log = new ConsoleLog();
        }

        public override void Start()
        {
            _server = _application.GetComponent<ServerComp>();
            _sessionComp = _application.GetComponent<SessionComp>();
            base.Start();
        }

        public override void AfterStart()
        {
            StartListen();
            base.AfterStart();
        }

        private void StartListen()
        {
            _socket.onConnect += async (connection) =>
            {
                try
                {
                    await OnConnect(connection);
                }
                catch (Exception ex)
                {
                    _log.Error(ExceptionUtils.Print(ex));
                }
            };
            _socket.onDisConnect += OnDisConnect;
            _socket.onMessage += async (message, connection) =>
            {
                try
                {
                    await OnMessage(message, connection);
                }
                catch (Exception ex)
                {
                    _log.Error(ex.ToString());
                }
            };
            _socket.Start();
        }

        private async Task OnConnect(IConnection connection)
        {
            var totalConn = _connectionManager.GetConnectionCount();
            if (totalConn > _application.maxConnectCount)
            {
                _log.Warn($"连接数达到了{totalConn},最大连接数配置为{_application.maxConnectCount},所以断开当前连接");
                await connection.Close();
                return;
            }
            //已经存在
            if (_connectionManager.TryGetValue(connection.Address, out IConnection c))
            {
                await c.Close();
            }
            _connectionManager.Add(connection.Address, connection);
            Session session = new Session(connection.Cid, connection, Time.GetUnixTimestampMilliseconds());
            _sessionComp.AddOrUpdate(session.Sid, session);
        }

        private void OnDisConnect(IConnection connection)
        {
            _connectionManager.Remove(connection.Address);
        }

        private async Task OnMessage(IMessage message, IConnection connection)
        {
            Session session = (Session)_sessionComp.Get(connection.Cid);
            await HandleMessageAsync(message, session);
        }

        private async Task HandleMessageAsync(IMessage message, Session session)
        {
            await _server.GlobalHandleAsync(message, session);
        }

        public override void Stop()
        {
            base.Stop();
        }
    }
}
