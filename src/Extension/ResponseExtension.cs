using QM.Network;
using Src.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Src.Extension
{
    public static class ResponseExtension
    {
        public static bool IsSuccess(this IResponse response)
        {
            return response.Code == (int)NetworkCode.Success;
        }
    }
}
