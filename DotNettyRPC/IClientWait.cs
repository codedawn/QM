using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNettyRPC
{
    public interface IClientWait
    {
        public void Set(byte[] bytes);
        public void Start(long id);
        public object Get(long id);
    }
}
