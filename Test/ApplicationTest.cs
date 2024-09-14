using QM;
using QM.Demo;
using System.Diagnostics;

namespace Test
{
    public class ApplicationTest
    {
        public static async void Run()
        {
            Task.Run(() =>
            {
                TestConnector();
            });

            //await Task.Delay(1000);
            //Task.Run(() => { TestClient(); });
            //Test2();
        }

        public static void TestConnector()
        {
            //Task.Run(() =>
            //{
            //    Task.Delay(1500).Wait();
            //    SocketClient socketClient = new SocketClient();
            //    socketClient.Init();
            //});

            try
            {
                //Application application = Application.CreateApplication("Connector01", Application.Connector, 20000);
                Application application = Application.CreateApplication("Connector01", Application.Connector, 20000, false);
                application.LoadAssembly(["DemoCommon"]);
                application.SetSessionFactory(new DemoSessionFactory());
                application.Start();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

            Console.ReadLine();
        }

        public static void TestServer(int port, bool isSimple)
        {
            try
            {
                //Application application = Application.CreateApplication("Server01", Application.Server, port);
                Application application = Application.CreateApplication("Server01", Application.Server, port, isSimple);
                application.Start();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }

            Console.ReadLine();
        }

        public static async void TestClient()
        {
            SocketClient socketClient = new SocketClient();
            socketClient.Init();
            socketClient.SetOnPushCallback(push => { Console.WriteLine("TestClient:" + push); });

            await socketClient.ConnectAsync("127.0.0.1", 20000);
            UserRequest request = new UserRequest() { Id = 234214, Name = "TestClient" };
            UserResponse userResponse = (UserResponse)await socketClient.SendRequestAsync(request);
            Debug.Assert(userResponse.Id == request.Id);
            Debug.Assert(userResponse.Name == request.Name);

            Console.ReadLine();
        }
    }
}
