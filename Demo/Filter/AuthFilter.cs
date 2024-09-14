using QM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM.Demo
{
    [Filter(includeServer = ServerType.Connector)]
    public class AuthFilter : Filter
    {
        public override Task<bool> Before(IMessage message, ISession session)
        {
            if (session.Data is UserData userData)
            {
                if (!userData.IsAuth && message is not UserAuthRequest)
                {
                    throw new DemoException(DemoErrorCode.UserNoAuth,"用户没有认证");
                }
            }
            return Task.FromResult(true);
        }
    }
}
