using System;
using System.Numerics;

namespace QM.Demo
{
    public class Player
    {
        private Vector2 _position;
        public Vector2 Position => _position;
        private int _point;
        public int Point => _point;
        private float _speed;
        private Reward _target;

        private string _name;
        public string Name { get => _name; private set { } }
        private bool isWayfinding;
        private Room _room;
        private Random _random = new Random();
        private ISession _session;
        public ISession Session => _session;
        private long _userId;
        public long UserId => _userId;



        public Player(long id, string name, Room room, ISession session)
        {
            this._name = name;
            this._room = room;
            this._userId = id;
            this._session = session;

            _position = new Vector2(_random.Next(10), _random.Next(10));
            _speed = _random.Next(5, 10);
        }

        public void Update(float dt)
        {
            if (isWayfinding)
            {
                Vector2 dir = Vector2.Normalize(_target.position - _position);
                _position += _speed * dt * dir;

                Vector2 afterDir = Vector2.Normalize(_target.position - _position);
                if (Vector2.Distance(_position, _target.position) < 0.5 || IsOppositeDirection(afterDir, dir))
                {
                    isWayfinding = false;
                    _room.TouchReward(_target, this);
                }
            }
            else
            {
                if (_random.Next(10) % 2 == 0)
                {
                    SetTarget(_room.GetNearerReward(_position));
                }
            }
        }

        private bool IsOppositeDirection(Vector2 dir1, Vector2 dir2)
        {
            const float epsilon = 0.01f; // 容差值
            return Vector2.Dot(Vector2.Normalize(dir1), Vector2.Normalize(dir2)) < -1 + epsilon;
        }

        public void SetTarget(Reward target)
        {
            if (target == null) return;
            this._target = target;
            isWayfinding = true;
        }

        public void AddPoint(int point)
        {
            this._point += point;
            Console.WriteLine($"name:{_name}获得积分:{point},累计积分:{this._point}");
        }

    }
}
