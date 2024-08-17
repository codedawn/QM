using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public interface ISocket
    {
        public event Action<IMessage, IConnection> onMessage;
        public event Action<IConnection> onConnect;
        public event Action<IConnection> onDisConnect;
        public void Start();
        public void Close();
    }
}
