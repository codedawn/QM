using DotNettyRPC;
using QM;

namespace Test
{
    public class DotNettyRPCTest
    {
        public static void Run()
        {
            Test1();
        }

        private static void Test1()
        {
            MessageOpcodeHelper.SetMessageOpcode(new RpcMessageOpcode());
            object[] args = new object[2] { new User() { Name = "test" }, new NetSession() { sid = "123" } };
            RequestModel requestModel = new RequestModel
            {
                ServiceName = "ServiceName",
                MethodName = "MethodName",
                ParamterIndexs = MessageOpcodeHelper.GetParameterIndexs(args),
                Paramters = args.ToList()
            };
            byte[] bytes = MessagePackUtil.Serialize(requestModel);
            Console.WriteLine(bytes.Length);
            RequestModel request = MessagePackUtil.Deserialize<RequestModel>(bytes);
            Console.WriteLine(request);
            Type type = MessageOpcodeHelper.GetType(request.ParamterIndexs[0]);
            byte[] userBytes = MessagePackUtil.Serialize(request.Paramters[0]);
            User user = (User)MessagePackUtil.Deserialize(type, userBytes);
            Console.WriteLine(user);
        }
    }
}
