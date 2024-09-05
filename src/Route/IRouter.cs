using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace QM
{
    public interface IRouter
    {
        public IPEndPoint Route(List<ServerInfo> serverInfos);
    }
}
