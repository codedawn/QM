using QM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public interface IRemoteTest
    {
        public Task PushTest(IPush push, NetSession netSession);
        public Task<IResponse> Test(IMessage message, NetSession netSession);
    }
}
