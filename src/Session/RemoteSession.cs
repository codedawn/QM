using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public class RemoteSession : ISession
    {
        public string Sid { get; set; }
        public string ServerId { get; set; }
        public IResponse Response { get; set; }
        public object Data { get; set; }

        public RemoteSession(string sid, string serverId, object data)
        {
            Sid = sid;
            ServerId = serverId;
            Data = data;
        }

        public Task Send(IResponse response)
        {
            if (Response != null)
            {
                throw new QMException(ErrorCode.ServerDupliResponse, "不能重复发送response");
            }
            this.Response = response;
            return Task.CompletedTask;
        }

        public async Task Send(IPush push)
        {
            await Application.current.GetComponent<RpcComp>().PushToConnector(push, ServerId, Sid);
        }

        public async Task Close()
        {
            await Application.current.GetComponent<RpcComp>().SessionCloseToConnector(this);
        }

        public async Task Sync()
        {
            await Application.current.GetComponent<RpcComp>().SyncSessionToConnector(this);
        }
    }
}
