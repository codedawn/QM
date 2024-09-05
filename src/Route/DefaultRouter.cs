using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace QM
{
    public class DefaultRouter : IRouter
    {
        private Random _random = new Random();
        public IPEndPoint Route(List<ServerInfo> serverInfos)
        {
            int index = _random.Next(0, serverInfos.Count);
            return serverInfos[index].iPEndPoint;
        }
    }
}
