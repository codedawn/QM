using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace QM.Demo
{
    public class Reward
    {
        public int id;
        public Vector2 position;
        public int point;

        public Reward(int id, Vector2 position, int point)
        {
            this.id = id;
            this.position = position;
            this.point = point;
        }
    }
}
