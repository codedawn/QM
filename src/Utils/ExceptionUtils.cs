using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public static class ExceptionUtils
    {
        public static string Print(Exception exception)
        {
            if (exception == null)
                return "";
            return exception.Message + "\n" + exception.StackTrace;
        }
    }
}
