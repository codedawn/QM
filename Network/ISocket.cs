using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM.Network
{
    public interface ISocket
    {
        public event Action<IMessage> onMessage;
        public event Action<IMessage> onConnect;
        public event Action<IMessage> onDisConnect;
        public void Start();
        public void Close();
    }
}
