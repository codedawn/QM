using QM;

namespace Test
{
    public class EventSystemTest
    {
        public static void Run()
        {
            Test1();
        }

        private static void Test1()
        {
            Session session = new Session("fweff", null, 321);
            EventSystem.Instance.Publish(new SessionIdleEvent() { session = session , intervalTime = 3});
        }
    }
}
