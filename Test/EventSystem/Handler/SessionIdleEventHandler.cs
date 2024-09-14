
using System;
using System.Threading.Tasks;

namespace QM
{
    [EventHandler]
    public class SessionIdleEventHandler : EventHandler<SessionIdleEvent>
    {
        public override Task Run(SessionIdleEvent e)
        {
            e.session.Connection.Close();
            return Task.CompletedTask;
        }
    }
}
