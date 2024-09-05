using System.Collections.Concurrent;
using System.Numerics;

namespace QM.Demo
{
    public class Room
    {
        private List<Player> _players = new List<Player>();
        private float _rewardInterval = 0.2f;//s
        private float _rewardAccTime;//s
        private ConcurrentDictionary<int, Reward> _rewards = new ConcurrentDictionary<int, Reward>();
        private int _idCount;

        public Room()
        {
            Task.Run(async () => 
            {
                while(true)
                {
                    await Task.Delay(20);
                    Update(0.02f);
                }
            });
        }

        public void Update(float dt)
        {
            CreateReward(dt);
            foreach (Player player in _players)
            {
                player.Update(dt);
            }
        }

        private void CreateReward(float dt)
        {
            _rewardAccTime += dt;
            if (_rewardAccTime > _rewardInterval)
            {
                _rewardAccTime = 0;
                Random random = new Random();
                Vector2 pos = new Vector2(random.Next(100), random.Next(100));
                Reward reward = new Reward(_idCount++, pos, random.Next(10) + 1);
                _rewards.TryAdd(reward.id, reward);
            }
        }

        public void TouchReward(Reward reward, Player player)
        {
            if (_rewards.TryRemove(reward.id, out Reward r))
            {
                player.AddPoint(r.point);
            }
            else
            {
                Console.WriteLine($"{player.name}来慢了");
            }
        }

        public void AddPlayer(Player player)
        {
            _players.Add(player);
        }

        public Reward GetNearerReward(Vector2 pos)
        {
            float threshold = 10;
            float min = float.MaxValue;
            Reward result = null;
            foreach(Reward reward in _rewards.Values)
            {
                float dis = Vector2.Distance(pos, reward.position);
                if (dis < threshold)
                {
                    return reward;
                }

                if (dis < min)
                {
                    result = reward;
                    min = dis;
                }
            }
            return result;
        }
    }
}
