using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public class SessionComp : Component
    {
        private SessionManager _sessionManager;
        private Application _application;

        public SessionComp(Application application)
        {
            _application = application;
        }

        public override void Start()
        {
            _sessionManager = new SessionManager();
            base.Start();
        }

        public void AddOrUpdate(string sid, ISession session)
        {
            _sessionManager.AddOrUpdate(sid, session);
        }

        public void Remove(string sid)
        {
            _sessionManager.Remove(sid);
        }

        public ISession Get(string sid)
        {
            return _sessionManager.Get(sid);
        }

        public List<ISession> GetAll()
        {
            return _sessionManager.GetAll();
        }
    }
}
