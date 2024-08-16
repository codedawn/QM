using QM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Src.Attribute
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MessageDispatchAttribute : BaseAttribute
    {
        public string server;

        public MessageDispatchAttribute(string server)
        {
            this.server = server;
        }
    }
}
