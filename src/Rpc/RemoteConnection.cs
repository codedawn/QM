using QM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public class RemoteConnection : IConnection
    {
        public IMessage response;
        public string Address => throw new NotImplementedException();

        public string Cid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public void Send(IMessage message)
        {
            response = message;
        }
    }
}
