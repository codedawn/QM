using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM.Demo
{
    public class DemoException : Exception
    {
        private DemoErrorCode _errorCode;

        public DemoException(DemoErrorCode code, string message) : base(message)
        {
            _errorCode = code;
        }

        public override string ToString()
        {
            return $"ErrorCode:{_errorCode}({((int)_errorCode)}) Message:{Message}";
        }
    }
}
