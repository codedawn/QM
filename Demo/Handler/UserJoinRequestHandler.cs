using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM.Demo
{
    [MessageHandler]
    public class UserJoinRequestHandler : MessageHandler<UserJoinRequest, UserJoinResponse>
    {
        protected override Task Run(UserJoinRequest request, UserJoinResponse response, ISession session)
        {
            Application.current.GetComponent<RoomComp>().JoinRoom(request.UserId, session);
            return Task.CompletedTask;
        }
    }
}
