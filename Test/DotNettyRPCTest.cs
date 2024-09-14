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
            object[] args = new object[2] { new UserRequest() { Name = "test" }, new NetSession() { Sid = "123" } };
            RPCRequest requestModel = new RPCRequest
            {
                ServiceName = "ServiceName",
                MethodName = "MethodName",
                ParamterIndexs = MessageOpcodeHelper.GetParameterIndexs(args),
                Paramters = args.ToList()
            };
            byte[] bytes = MessagePackUtil.Serialize(requestModel);
            Console.WriteLine(bytes.Length);
            RPCRequest request = MessagePackUtil.Deserialize<RPCRequest>(bytes);
            Console.WriteLine(request);
            Type type = MessageOpcodeHelper.GetType(request.ParamterIndexs[0]);
            byte[] userBytes = MessagePackUtil.Serialize(request.Paramters[0]);
            UserRequest user = (UserRequest)MessagePackUtil.Deserialize(type, userBytes);
            Console.WriteLine(user);
        }

    }
}
