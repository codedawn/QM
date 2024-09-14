using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public class Session : ISession
    {
        public string Sid { get; set; }
        public IConnection Connection { get; set; }
        public object Data { get; set; }

        public Session(string sid, IConnection connection)
        {
            this.Sid = sid;
            this.Connection = connection;
        }

        public async Task Send(IResponse response)
        {
            await Connection.Send(response);
        }

        public async Task Send(IPush push)
        {
            await Connection.Send(push);
        }

        public async Task Close()
        {
            await Connection.Close();
        }

        public Task Sync()
        {
            throw new QMException(ErrorCode.SessionSyncError, "当前就是原始Session,不能sync");
        }
    }
}
