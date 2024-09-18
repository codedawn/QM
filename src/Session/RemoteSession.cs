using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    /// <summary>
    /// server类型服务器可访问的session实现，是对connector类型服务器持有的原始session的映射，
    /// 因为原始session随时可能改变，所以不能长期持有此引用。
    /// </summary>
    public class RemoteSession : ISession
    {
        public string Sid { get { return _sid; } set { _tmpSid = value; } }
        private string _sid;
        public string TmpSid { get { return _tmpSid; } }
        private string _tmpSid;//同步之前暂存
        public string ServerId { get; set; }
        public IResponse Response { get; set; }//请求的返回值，一次请求只能使用一次
        public object Data { get; set; }

        public RemoteSession(string sid, string serverId, object data)
        {
            _sid = sid;
            _tmpSid = sid;
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
            _sid = _tmpSid;
        }
    }
}
