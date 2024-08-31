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
        public Vector2 target;
        private bool isWayfinding;

        public Player() 
        {
            Random random = new Random();
            position = new Vector2(random.Next(10), random.Next(10));
            speed = 5;
        }

        public void Update(float dt)
        {
            if (isWayfinding)
            {
                Vector2 dir = Vector2.Normalize(target - position);
                position += speed * dt * dir;

                Vector2 afterDir = Vector2.Normalize(target - position);
                if (Vector2.Distance(position, target) < 0.1 || afterDir == -dir)
                {
                    isWayfinding = false;
                }
            }
        }

        public void SetTarget(Vector2 target)
        {
            this.target = target;
            isWayfinding = true;
        }
    }
}
