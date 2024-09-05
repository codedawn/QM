using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public interface IAwait<T>
    {
        public void Set(long id, T result);
        public Task<T> Start(long id);
    }
}
