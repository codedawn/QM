using DotNettyRPC;
using MessagePack;
using MessagePack.Resolvers;
using QM;
using QM.Demo;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class MessagePackTest
    {
        public static void Run()
        {
            Test_Array();
            Test_UserData();
        }

        private static void Test_Array()
        {
            List<string> list = new List<string>() { "Test_Array" };
            byte[] bytes = MessagePackSerializer.Serialize(list);
            Console.WriteLine(list.GetType());
        }

        private static void Test_UserData()
        {
            //  MessagePackSerializer.DefaultOptions = MessagePackSerializerOptions.Standard.WithResolver(
            //CompositeResolver.Create(
            //    NetSessionDataTypelessResolver.Instance, // 自定义解析器
            //    StandardResolver.Instance
            //));
            MessageOpcodeHelper.SetMessageOpcode(new RpcMessageOpcode());
            NetSession session = new NetSession() { DataIndex = MessageOpcodeHelper.GetIndex(typeof(UserData)),  Data = new UserData() };
            byte[] bytes = MessagePackSerializer.Serialize(session);
            NetSession decodeSession = MessagePackSerializer.Deserialize<NetSession>(bytes);
            Type type = MessageOpcodeHelper.GetType(decodeSession.DataIndex);
            byte[] bytes1 = MessagePackUtil.Serialize(decodeSession.Data);
            decodeSession.Data = MessagePackUtil.Deserialize(type, bytes1);
            Debug.Assert(session.Data.GetType() == decodeSession.Data.GetType());
            Console.WriteLine(MessagePackSerializer.ConvertToJson(bytes));
        }
    }
}
