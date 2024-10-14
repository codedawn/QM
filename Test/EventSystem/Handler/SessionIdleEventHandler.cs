
using QM;

namespace Test
{
    [EventHandler]
    public class SessionIdleEventHandler : QM.EventHandler<SessionIdleEvent>
    {
        public override Task Run(SessionIdleEvent e)
        {
            e.session.Connection.Close();
            return Task.CompletedTask;
        }
    }
}
