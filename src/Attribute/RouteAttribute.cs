using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple =false)]
    public class RouteAttribute : BaseAttribute
    {
        public string serverType;

        public RouteAttribute(string serverType)
        {
            this.serverType = serverType;
        }
    }
}
