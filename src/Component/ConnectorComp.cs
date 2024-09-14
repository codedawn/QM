using System;
using System.Threading;
using System.Threading.Tasks;

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
        private ServerComp _server;
        private SessionComp _sessionComp;
        private long _messageCount;
        private long _startTime;
        private bool _isMeasuring;
        private long _tmpCount;
        private ISessionFactory _sessionFactory;

        public ConnectorComp(Application application)
        {
            this._application = application;
            this._connectionManager = new ConnectionManager();
            this._socket = new SocketServer();
            this._log = new NLogger(typeof(ConnectorComp));
        }

        public override void Start()
        {
            _server = _application.GetComponent<ServerComp>();
            _sessionComp = _application.GetComponent<SessionComp>();
            _sessionFactory = _application.GetSessionFactory();
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
                    _log.Error(ex);
                }
            };

            _socket.onDisConnect += async (connection) => 
            {
                try
                {
                    await OnDisConnect(connection);
                }
                catch (Exception ex)
                {
                    _log.Error(ex);
                }
            };

            _socket.onMessage += async (message, connection) =>
            {
                try
                {
                    await OnMessage(message, connection);
                }
                catch (Exception ex)
                {
                    _log.Error(ex);
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
                await ConnectionClose(c);
            }
            _connectionManager.AddOrUpdate(connection.Address, connection);
            ISession session = _sessionFactory.CreateSession(connection);
            _sessionComp.AddOrUpdate(session.Sid, session);
        }

        private async Task OnDisConnect(IConnection connection)
        {
            await ConnectionClose(connection);
        }

        private async Task OnMessage(IMessage message, IConnection connection)
        {
            Interlocked.Increment(ref _messageCount);
            _log.Debug($"接收消息总数：{_messageCount}");
            
            ISession session = connection.Session;
            await HandleMessageAsync(message, session);
            //measurement
            Measurement();
        }

        private async Task ConnectionClose(IConnection connection)
        {
            _connectionManager.Remove(connection.Address);
            ISession session = connection.Session;
            if (session != null)
            {
                _log.Warn("Connection连接断开，Session可能无法继续发送消息");
            }
            await connection.Close();
        }

        private void Measurement()
        {
            Interlocked.Increment(ref _tmpCount);
            if (!_isMeasuring)
            {
                _isMeasuring = true;
                Interlocked.Exchange(ref _tmpCount, 1);
                _startTime = Time.GetUtc8TimestampMilliseconds();
            }
            else
            {
                long endTime = Time.GetUtc8TimestampMilliseconds();
                if (endTime - _startTime > 1000)
                {
                    _isMeasuring = false;
                    Console.WriteLine($"吞吐{_tmpCount}/sec");
                }
            }
        }

        private async Task HandleMessageAsync(IMessage message, ISession session)
        {
            await _server.GlobalHandleAsync(message, session);
        }

        public override void Stop()
        {
            base.Stop();
        }
    }
}
