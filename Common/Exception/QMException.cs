using System;
using System.Collections.Generic;
using System.Text;

namespace QM
{
    public class QMException : Exception
    {
        private ErrorCode _errorCode;

        public QMException(ErrorCode code, string message) : base(message) 
        {
            _errorCode = code;
        }

        public override string ToString()
        {
            return $"ErrorCode:{_errorCode}({((int)_errorCode)}) Message:{Message} \n{StackTrace}";
        }
    }
}
