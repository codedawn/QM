using DotNetty.Buffers;
using MessagePack;
using QM.Network;
using System.Diagnostics;

namespace QM.Tests
{
    public class QMProtocolTest
    {
        public void Run()
        {

            Test1();
        }

        private void Test1()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            for (int i = 0; i < 10000000; i++)
            {
                User user = new User() { Id = 1321421414 + i, Name = "liu", Email = "321419419@gmai.com" };
                var bytes = MessagePackSerializer.Serialize(user);
                var u = MessagePackSerializer.Deserialize(typeof(User), bytes);
            }
            
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds + "ms");

            QMProtocol qMProtocol = new QMProtocol();
            var allocator = PooledByteBufferAllocator.Default;
            IByteBuffer buffer = allocator.Buffer(1024);
            List<object> list = new List<object>();

            stopwatch.Restart();
            for (int i = 0; i < 10000000; i++)
            {
                list.Clear();
                User user = new User() { Id = 1321421414 + i, Name = "liu", Email = "321419419@gmai.com" };
                qMProtocol.Encode(user, buffer);
                qMProtocol.Decode(buffer, list);
            }

            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds + "ms");

        }
    }
}
