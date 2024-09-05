using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace QM
{
    public class ServerInfo
    {
        public readonly IPEndPoint iPEndPoint;
        public readonly string serverType;
        public readonly string serverId;
        public readonly int state;

        public ServerInfo(IPEndPoint iPEndPoint, string serverType, string serverId, int state)
        {
            this.iPEndPoint = iPEndPoint;
            this.serverType = serverType;
            this.serverId = serverId;
            this.state = state;
        }
    }
}
