using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM.Network
{
    public class RouteInfo
    {
        public bool isRequest;
        public bool isNotify;
        public string ServerType;

        //客户端只能发送这两种消息
        public bool IsValid()
        {
            return isRequest || isNotify;
        }
    }
}
