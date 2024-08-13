using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM.Network
{
    /// <summary>
    /// 定义消息index
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class MessageIndexAttribute : Attribute
    {
        public byte index;

        public MessageIndexAttribute(byte index)
        {
            this.index = index;
        }
    }
}
