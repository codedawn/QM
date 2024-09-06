using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public interface ISession
    {
        //session id
        public string Sid { get; set; }
        public IConnection Connection { get; set; }
    }
}
