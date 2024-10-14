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
            try
            {
                //Application application = Application.CreateApplication("Connector01", Application.Connector, 20000);
                Application application = Application.CreateApplication("Connector01", Application.Connector, 20000);
                //application.LoadAssembly(["DemoCommon"]);
                //application.SetSessionFactory(new DemoSessionFactory());
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
            Task.Run(() =>
            {
                Task.Delay(1500).Wait();
                TestClient();
            });
            try
            {
                //Application application = Application.CreateApplication("Server01", Application.Server, port);
                Application application = Application.CreateApplication("Server01", Application.Server, port);
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
            //UserAuthRequest userAuthRequest = new UserAuthRequest() { Id = IdGenerator.NextId(), Token = "test" };
            //var response = await socketClient.SendRequestAsync(userAuthRequest);
            UserRequest request = new UserRequest() { Id = 234214, Name = "TestClient" };
            UserResponse userResponse = (UserResponse)await socketClient.SendRequestAsync(request);
            Debug.Assert(userResponse.Id == request.Id);
            Debug.Assert(userResponse.Name == request.Name);

            UserNotify userNotify = new UserNotify() { message = "一条聊天消息" };
            await socketClient.SendNotifyAsync(userNotify);
            Console.ReadLine();
        }
    }
}
