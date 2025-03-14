﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public class SessionManager
    {
        private ILog _log = new NLogger(typeof(SessionManager));
        private ConcurrentDictionary<string, ISession> _sessions = new ConcurrentDictionary<string, ISession>();

        /// <summary>
        /// 如果已经存在就更新
        /// </summary>
        /// <param name="sid"></param>
        /// <param name="session"></param>
        public void AddOrUpdate(string sid, ISession session)
        {
            if (_sessions.TryGetValue(sid, out ISession exist))
            {
                _log.Warn($"已经存在Session，进行覆盖，sid：{sid}");
                _sessions.TryUpdate(sid, session, exist);
            }
            else
            {
                _sessions.TryAdd(sid, session);
            }
        }

        public ISession Get(string sid)
        {
            if(_sessions.TryGetValue(sid, out ISession session))
            {
                return session;
            }
            return null;
        }

        public List<ISession> GetAll()
        {
            return _sessions.Values.ToList();
        }

        public bool TryRemove(string sid, out ISession session)
        {
            return _sessions.TryRemove(sid, out session);
        }

        public int Count()
        {
            return _sessions.Count;
        }
    }
}
