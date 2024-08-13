using QM.Network;

namespace QM.Component
{
    public class MessageDispatcher
    {
        private Application _application;

        public MessageDispatcher(Application application)
        {
            _application = application;
        }

        public IResponse Dispatch(IRequest request, Session session)
        {
            if(_application.serverType == GetServerType(request))
            {
                DoHandle(request, session);
            }
            return null;
        }

        public string GetServerType(IRequest request)
        {
            return "";
        }

        public void DoHandle(IRequest request, Session session)
        {

        }
    }
}
