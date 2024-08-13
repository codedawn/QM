using QM.Log;
using QM.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace QM.Component
{
    /**
     * connector组件是socket的一个上层封装，处理socket事件（连接，断开，接受、发送消息，调度其他组件维护连接信息）
     */
    public class Connector : IComponent
    {
        private Application application;
        private ISocket socket;
        private ConnectionManager _connectionManager;
        private ILog log;
        private IMsgSchedule schedule;
        private Server _server;

        public Connector(Application application)
        {
            this.application = application;
            this._connectionManager = new ConnectionManager();
        }

        public void Start()
        {
            _server = application.GetComponent<Server>();
        }

        public void AfterStart()
        {
            StartListen();
        }

        private void StartListen()
        {
            socket.Start();
            socket.onConnect += OnConnect;
            socket.onDisConnect += OnDisConnect;
            socket.onMessage += OnMessage;
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
            Session session = new Session();
            session.sid = connection.Cid;
            session.connection = connection;
            application.sessionManager.Add(session.sid, session);
        }

        private void OnDisConnect(IConnection connection)
        {
            _connectionManager.Remove(connection.Address);
        }

        private void OnMessage(IMessage message, IConnection connection)
        {
            Session session = application.sessionManager.Get(connection.Cid);
            HandleMessage(message, session);
        }

        private void HandleMessage(IMessage message, Session session)
        {
            _server.GlobalHandle(message, session);
        }

        private void Response(string message)
        {
           // schedule.Send(
           // bytes);
        }

        public void Stop()
        {
        }
    }
}
