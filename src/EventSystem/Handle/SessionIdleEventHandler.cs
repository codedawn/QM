
namespace QM
{
    [EventHandler]
    public class SessionIdleEventHandler : EventHandler<SessionIdleEvent>
    {
        public override void Run(SessionIdleEvent e)
        {
            Console.WriteLine(e);
            //e.session.connection.Close();
        }
    }
}
