using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Application application = Application.CreateApplication("Room01", Application.Server, 25999);
            application.AddComponent(new RoomComp());
            application.SetSessionFactory(new DemoSessionFactory());
            application.Start();
            UserJoinRequest request = new UserJoinRequest();
            //for (int i = 1; i <= 5; i++)
            //{
            //    int tmp = i;
            //    Task.Run(() =>
            //    {
            //        Test1(tmp * 1000000);
            //    });
            //}
            Console.ReadLine();
        }
    }
}
