using System;
using System.Threading;
using System.Threading.Tasks;

namespace Coldairarrow.DotNettyRPC
{
    public class ClientObj
    {
        public TaskCompletionSource<object> tcs { get; set; }

        public ClientObj()
        {
            this.tcs = new TaskCompletionSource<object>();
        }
    }

}
