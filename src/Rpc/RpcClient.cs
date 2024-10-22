﻿using DotNettyRPC;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using static org.apache.zookeeper.ZooDefs;

namespace QM
{
    public class RpcClient
    {
        private int _timeout = 100000;
        private ILog _log = new NLogger(typeof(RpcClient));
        public RpcClient()
        {
            MessageOpcodeHelper.SetMessageOpcode(new RpcMessageOpcode());
        }

        public async Task<IResponse> Forward(IMessage message, NetSession netSession, IPEndPoint iPEndPoint)
        {
            IRemote client = RPCClientFactory.GetClient<IRemote, IResponse>(iPEndPoint.Address.ToString(), iPEndPoint.Port, _timeout);
            return await client.Forward(message, netSession);
        }

        public async Task Push(IPush push, string sid, IPEndPoint iPEndPoint)
        {
            IRemote client = RPCClientFactory.GetClient<IRemote, IResponse>(iPEndPoint.Address.ToString(), iPEndPoint.Port, _timeout);
            await client.Push(push, sid);
        }

        public async Task Broadcast(IPush push, IPEndPoint iPEndPoint)
        {
            IRemote client = RPCClientFactory.GetClient<IRemote, IResponse>(iPEndPoint.Address.ToString(), iPEndPoint.Port, _timeout);
            await client.Broadcast(push);
        }

        public async Task BroadcastBySid(IPush push, List<string> sids, IPEndPoint iPEndPoint)
        {
            IRemote client = RPCClientFactory.GetClient<IRemote, IResponse>(iPEndPoint.Address.ToString(), iPEndPoint.Port, _timeout);
            await client.BroadcastBySid(push, sids);
        }

        public async Task SyncSession(NetSession netSession, IPEndPoint iPEndPoint)
        {
            IRemote client = RPCClientFactory.GetClient<IRemote, IResponse>(iPEndPoint.Address.ToString(), iPEndPoint.Port, _timeout);
            await client.SyncSession(netSession);
        }

        public async Task SessionClose(string sid, IPEndPoint iPEndPoint)
        {
            IRemote client = RPCClientFactory.GetClient<IRemote, IResponse>(iPEndPoint.Address.ToString(), iPEndPoint.Port, _timeout);
            await client.SessionClose(sid);
        }
    }
}
