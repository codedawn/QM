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
        private Socket socket;

        public Connector(Application application)
        {
            this.application = application;
        }

        public void Start()
        {

        }

        public void AfterStart()
        {
        }

        public void Stop()
        {
        }
    }
}
