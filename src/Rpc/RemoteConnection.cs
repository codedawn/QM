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
        public IResponse response;
        public string Address => throw new NotImplementedException();

        public string Cid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task Close()
        {
            throw new NotImplementedException();
        }

        public Task Send(IMessage message)
        {
            response = (IResponse)message;
            return Task.CompletedTask;
        }
    }
}
