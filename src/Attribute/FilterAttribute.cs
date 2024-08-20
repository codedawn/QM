using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class FilterAttribute : BaseAttribute
    {
        public const string All = "All";
        public string includeServer;
        public string excludeServer;

        public FilterAttribute(string includeServer = FilterAttribute.All, string excludeServer = "")
        {
            this.includeServer = includeServer;
            this.excludeServer = excludeServer;
        }
    }
}
