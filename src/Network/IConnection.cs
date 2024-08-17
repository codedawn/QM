using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public interface IConnection
    {
        public string Address {  get;}
        public string Cid { get; set; }
        public void Send(IMessage message);
        public void Close();
    }
}
