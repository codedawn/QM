﻿namespace QM
{
    public interface IRemote
    {
        public Task<IResponse> Forward(IMessage message, NetSession netSession);

        public Task<IResponse> Test(IMessage message, NetSession netSession);

        public void Push(IMessage message, NetSession netSession);
    }
}
