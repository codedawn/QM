using DotNetty.Buffers;
using QM.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            User user = new User() { Id = 14142214214, Name = "liu" , Email = "fjewoif@gmail.com"};

            IByteBuffer byteBuff = PooledByteBufferAllocator.Default.Buffer(256);
            qMProtocol.Encode(user, byteBuff);

            List<object> list = new List<object>();
            qMProtocol.Decode(byteBuff, list);
            Debug.Assert(list.Count == 1);
            Debug.Assert(list[0] is User);
            User decodeUser = (User)list[0];
            Debug.Assert(decodeUser.Id == user.Id);
            Debug.Assert(decodeUser.Name == user.Name);
            Debug.Assert(decodeUser.Email == user.Email);

            Console.WriteLine($"Test success {typeof(QMProtocolTest)}");
        }
    }
}
