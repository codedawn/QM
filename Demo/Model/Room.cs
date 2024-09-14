using System.Collections.Concurrent;
using System.Numerics;
using static org.apache.zookeeper.ZooKeeper;

namespace QM.Demo
{
    public class Room
    {
        private ILog _log = new NLogger(typeof(Room));
        private List<Player> _players = new List<Player>();
        private float _rewardInterval = 1.2f;//s
        private float _rewardAccTime;//s
        private ConcurrentDictionary<int, Reward> _rewards = new ConcurrentDictionary<int, Reward>();
        private int _idCount;

        public Room()
        {
            Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        await Task.Delay(20);
                        await Update(0.02f);
                    }
                }
                catch (Exception ex)
                {
                    _log.Error(ex);
                }
            });
        }

        public async Task Update(float dt)
        {
            CreateReward(dt);
            foreach (Player player in _players)
            {
                player.Update(dt);
            }
            await SendStates();
        }

        public async Task SendStates()
        {
            List<PlayerState> states = new List<PlayerState>();
            List<string> sids = new List<string>();
            foreach (Player player in _players)
            {
                states.Add(new PlayerState() { posX = player.Position.X, posY = player.Position.Y, point = player.Point, uid = player.UserId });
                sids.Add(player.Session.Sid);
            }
            RoomStatePush roomStatePush = new RoomStatePush() { playerStates = states };
            await Application.current.GetComponent<RpcComp>().BroadcastBySid(roomStatePush, sids);
        }

        private async void CreateReward(float dt)
        {
            _rewardAccTime += dt;
            if (_rewardAccTime > _rewardInterval)
            {
                _rewardAccTime = 0;
                Random random = new Random();
                Vector2 pos = new Vector2(random.Next(100), random.Next(100));
                Reward reward = new Reward(_idCount++, pos, random.Next(10) + 1);
                _rewards.TryAdd(reward.id, reward);
                await SendAddReward(reward);
            }
        }

        private async Task SendAddReward(Reward reward)
        {
            List<string> sids = new List<string>();
            foreach (Player player in _players)
            {
                sids.Add(player.Session.Sid);
            }
            RewardState rewardState = new RewardState() { id = reward.id, x = reward.position.X, y = reward.position.Y };
            AddRewardPush addRewardPush = new AddRewardPush() { reward = rewardState };
            await Application.current.GetComponent<RpcComp>().BroadcastBySid(addRewardPush, sids);
        }

        public async void TouchReward(Reward reward, Player player)
        {
            if (_rewards.TryRemove(reward.id, out Reward r))
            {
                player.AddPoint(r.point);
                await SendTouchReward(reward.id);
            }
            else
            {
                //Console.WriteLine($"{player.Name}来慢了");
            }
        }

        private async Task SendTouchReward(int id)
        {
            List<string> sids = new List<string>();
            foreach (Player player in _players)
            {
                sids.Add(player.Session.Sid);
            }
            TouchRewardPush touchRewardPush = new TouchRewardPush() { id = id };
            await Application.current.GetComponent<RpcComp>().BroadcastBySid(touchRewardPush, sids);
        }

        public async Task AddPlayer(Player player)
        {
            RemoteSession remoteSession = player.Session as RemoteSession;
            await SendAllReward(remoteSession.Sid, remoteSession.ServerId);
            _players.Add(player);
        }

        public async Task SendAllReward(string sid, string serverid)
        {
            List<RewardState> rewards = new List<RewardState>();
            foreach (Reward reward in _rewards.Values)
            {
                rewards.Add(new RewardState() { id = reward.id, x = reward.position.X, y = reward.position.Y });
            }
            AllRewardPush allRewardPush = new AllRewardPush() { rewardStates = rewards };
            await Application.current.GetComponent<RpcComp>().PushToConnector(allRewardPush, serverid, sid);
        }

        public Reward GetNearerReward(Vector2 pos)
        {
            float threshold = 10;
            float min = float.MaxValue;
            Reward result = null;
            foreach (Reward reward in _rewards.Values)
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
