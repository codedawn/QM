using QM;

namespace Test
{
    public class EventSystemTest
    {
        public static void Run()
        {
            Test1();
        }

        private class TestConnection : IConnection
        {
            public string Address => throw new NotImplementedException();

            public string Cid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public Task Close()
            {
                Console.WriteLine("close Test1");
                return Task.CompletedTask;
            }

            public Task Send(IMessage message)
            {
                throw new NotImplementedException();
            }
        }

        private async static void Test1()
        {
            Session session = new Session("fweff", new TestConnection(), 321);
            await EventSystem.Instance.Publish(new SessionIdleEvent() { session = session, intervalTime = 3 });
        }
    }
}
