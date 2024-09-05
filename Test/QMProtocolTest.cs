using DotNetty.Buffers;
using QM;
using System.Diagnostics;

namespace Test
{
    public class QMProtocolTest
    {
        public static void Run()
        {
            Test1();
        }

        private static void Test1()
        {
            QMProtocol qMProtocol = new QMProtocol();
            UserRequest user = new UserRequest() { Id = 14142214214, Name = "liu" , Email = "fjewoif@gmail.com"};

            IByteBuffer byteBuff = PooledByteBufferAllocator.Default.Buffer(256);
            qMProtocol.Encode(user, byteBuff);

            List<object> list = new List<object>();
            qMProtocol.Decode(byteBuff, list);
            Debug.Assert(list.Count == 1);
            Debug.Assert(list[0] is UserRequest);
            UserRequest decodeUser = (UserRequest)list[0];
            Debug.Assert(decodeUser.Id == user.Id);
            Debug.Assert(decodeUser.Name == user.Name);
            Debug.Assert(decodeUser.Email == user.Email);

            Console.WriteLine($"Test success {typeof(QMProtocolTest)}");
        }
    }
}
