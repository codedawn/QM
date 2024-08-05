using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public interface IComponent
    {
        public void Start();
        public void AfterStart();
        public void Stop();
    }
}
