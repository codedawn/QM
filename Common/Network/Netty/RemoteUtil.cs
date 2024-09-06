using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public class RemoteUtil
    {
        public static String parseRemoteAddress(IChannel channel)
        {
            EndPoint endPoint = channel.RemoteAddress;
            return endPoint.ToString();
        }
    }
}
