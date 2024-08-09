using QM.Log;
using QM.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
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
        private ConnectionService connectionService;
        private ILog log;
        private IMsgSchedule schedule;

        public Connector(Application application)
        {
            this.application = application;
            this.connectionService = new ConnectionService();
        }

        public void Start()
        {

        }

        public void AfterStart()
        {
            StartListen();
        }

        private void StartListen()
        {
            socket.Start();
            socket.onMessage += OnMessage;
        }

        private void Connection(Socket socket)
        {
            var totalConn = connectionService.getConnectionCount();
            if (totalConn > application.maxConnectCount)
            {
                log.Warn($"连接数达到了{totalConn},最大连接数配置为{application.maxConnectCount},所以断开当前连接");
                socket.Close();
                return;
            }

        }

        private void OnMessage(IMessage message)
        {
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
