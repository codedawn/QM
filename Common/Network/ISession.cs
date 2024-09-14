using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace QM
{
    public interface ISession
    {
        //session id
        public string Sid { get; set; }
        public object Data { get; set; }
        public Task Send(IResponse response);
        public Task Send(IPush push);
        public Task Close();
        public Task Sync();
    }
}
