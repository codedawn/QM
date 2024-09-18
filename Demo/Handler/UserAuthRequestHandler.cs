using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM.Demo
{
    [MessageHandler]
    public class UserAuthRequestHandler : MessageHandler<UserAuthRequest, UserAuthResponse>
    {
        protected async override Task Run(UserAuthRequest request, UserAuthResponse response, ISession session)
        {
            if (request.Token == "" || request.Token == null)
            {
                response.Code = 300; //认证失败
                Application.current.TaskSchedule(async () => { await session.Close(); }, 1000);
                return;
            }
            session.Sid = request.Id.ToString();
            UserData userData = session.Data as UserData;
            if (userData != null)
            {
                userData.IsAuth = true;
                await session.Sync();
            }
            return;
        }
    }
}
