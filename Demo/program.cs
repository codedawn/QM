using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM.Demo
{
    public class program
    {
        public static void Main(string[] args)
        {
            Application application = Application.CreateApplication("Room01", Application.Server, 9999);
            application.AddComponent(new RoomComp());
            application.Start();
        }
    }
}
