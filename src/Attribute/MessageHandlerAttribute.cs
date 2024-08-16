using QM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace src.Attribute
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MessageHandlerAttribute : BaseAttribute
    {
    }
}
