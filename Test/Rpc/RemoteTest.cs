using QM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class RemoteTest : IRemoteTest
    {
        //Connector执行，Server转发
        public Task PushTest(IPush push, NetSession netSession)
        {
            return Task.CompletedTask;
        }

        public Task<IResponse> Test(IMessage message, NetSession netSession)
        {
            UserResponse response = new UserResponse() {Name = "Test" };
            return Task.FromResult((IResponse)response);
        }
    }
}
