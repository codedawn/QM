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
            //for (int i = 0; i < 10; i++)
            //{
            //    room.AddPlayer(new Player(i, $"player{i}", room, null));
            //}
            base.AfterStart();
        }

        public async void JoinRoom(long userId, ISession session)
        {
            Room room = rooms[0];
            await room.AddPlayer(new Player(userId, $"player{userId}", room, session));
        }
    }
}
