using System;
using System.Collections.Generic;

namespace Coldairarrow.DotNettyRPC
{
    class RequestModel
    {
        public string ServiceName { get; set; }
        public string MethodName { get; set; }
        public List<short> Generics { get; set; }
        public List<short> ParamterIndexs { get; set; }
        public List<object> Paramters { get; set; }

        public override bool Equals(object obj)
        {
            return obj is RequestModel model &&
                   ServiceName == model.ServiceName &&
                   MethodName == model.MethodName &&
                   EqualityComparer<List<short>>.Default.Equals(Generics, model.Generics) &&
                   EqualityComparer<List<short>>.Default.Equals(ParamterIndexs, model.ParamterIndexs) &&
                   EqualityComparer<List<object>>.Default.Equals(Paramters, model.Paramters);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ServiceName, MethodName, Generics, ParamterIndexs, Paramters);
        }
    }
}
