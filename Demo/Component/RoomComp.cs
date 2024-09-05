using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM.Demo
{
    public class RoomComp : Component
    {
        private List<Room> rooms = new List<Room>();

        public override void AfterStart()
        {
            Room room = new Room();
            rooms.Add(room);
            for (int i = 0; i < 10; i++)
            {
                room.AddPlayer(new Player($"player{i}", room));
            }
            base.AfterStart();
        }
    }
}
