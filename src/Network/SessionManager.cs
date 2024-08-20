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
        private ConcurrentDictionary<string, ISession> _sessions = new ConcurrentDictionary<string, ISession>();

        public void Add(string sid, ISession session)
        {
            if (_sessions.TryGetValue(sid, out ISession exist))
            {
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

        public void Remove(string sid)
        {
            if ( _sessions.TryRemove(sid, out ISession session))
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
