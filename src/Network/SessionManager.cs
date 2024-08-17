using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public class SessionManager
    {
        private ConcurrentDictionary<string, Session> _sessions = new ConcurrentDictionary<string, Session>();

        public void Add(string sid, Session session)
        {
            if (_sessions.TryGetValue(sid, out Session exist))
            {
                _sessions.TryUpdate(sid, session, exist);
            }
            else
            {
                _sessions.TryAdd(sid, session);
            }
        }

        public Session Get(string sid)
        {
            if(_sessions.TryGetValue(sid, out Session session))
            {
                return session;
            }
            return null;
        }

        public void Remove(string sid)
        {
            if ( _sessions.TryRemove(sid, out Session session))
            {
                //todo
            }
        }

        public int Count()
        {
            return _sessions.Count;
        }
    }
}
