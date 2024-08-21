namespace Test
{
    public class ProgramTest
    {
        //public static void Main(string[] args)
        //{
        //    // QMProtocolTest.Run();
        //    ApplicationTest.Run();
        //    //RpcTest.Run();
        //    //ZookeeperServiceTest.Run();
        //    //EventSystemTest.Run();
        //    //Console.ReadLine();
        //}

        public static async Task Main(string[] args)
        {
            // Instantiate the dynamic RPC client with the service URL
            dynamic rpcClient = new RpcClient("http://example.com/api");

            // Dynamic call to the remote method 'Add'
            dynamic result = await rpcClient.Add(5, 10);

            Console.WriteLine($"Result from 'Add' method: {result}");

            // Dynamic call to another remote method 'Multiply'
            result = await rpcClient.Multiply(3, 4);

            Console.WriteLine($"Result from 'Multiply' method: {result}");
        }
    }
}
