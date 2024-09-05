using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QM.Demo
{
    public class Player
    {
        public Vector2 position;
        public int point;
        public float speed;
        public Reward target;
        private bool isWayfinding;
        public string name;
        private Room room;
        private Random random = new Random();


        public Player(string name, Room room) 
        {
            this.name = name;
            this.room = room;

            position = new Vector2(random.Next(10), random.Next(10));
            speed = 5;
        }

        public void Update(float dt)
        {
            if (isWayfinding)
            {
                Vector2 dir = Vector2.Normalize(target.position - position);
                position += speed * dt * dir;

                Vector2 afterDir = Vector2.Normalize(target.position - position);
                if (Vector2.Distance(position, target.position) < 0.1 || afterDir == -dir)
                {
                    isWayfinding = false;
                    room.TouchReward(target, this);
                }
            }
            else
            {
                if (random.Next(10) % 2 ==0)
                {
                    SetTarget(room.GetNearerReward(position));
                }
            }
        }

        public void SetTarget(Reward target)
        {
            if (target == null) return;
            this.target = target;
            isWayfinding = true;
        }

        public void AddPoint(int point)
        {
            this.point += point;
            Console.WriteLine($"name:{name}获得积分:{point},累计积分:{this.point}");
        }
    }
}
