using QM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    [MessageHandler]
    public class UserNotifyHandler : NotifyHandler<UserNotify>
    {
        public override async Task Run(UserNotify notify, ISession session)
        {
            Console.WriteLine(notify.message);
            UserPush userPush = new UserPush() { Name = notify.message };
            await session.Send(userPush);
        }
    }
}
