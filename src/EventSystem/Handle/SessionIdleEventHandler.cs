
namespace QM
{
    [EventHandler]
    public class SessionIdleEventHandler : EventHandler<SessionIdleEvent>
    {
        public override Task Run(SessionIdleEvent e)
        {
            Console.WriteLine(e);
            e.session.Connection.Close();
            return Task.CompletedTask;
        }
    }
}
