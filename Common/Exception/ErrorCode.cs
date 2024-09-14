using System;
using System.Collections.Generic;
using System.Text;

namespace QM
{
    public enum ErrorCode
    {
        AwaitNotFoundId,
        AwaitDupliId,
        AwaitTimeout,

        RPCConnectFail,
        RPCServerError,
        RPCNotFoundService,
        RPCNotFoundMethod,

        RouterNotFound,

        ServerTypeNoOne,

        MessageHandlerDupli,

        MessageIndexDupli,
        MessageIndexNotFound,
        MessageOpcodeNotFound,

        MessageInvalid,

        MessageNoDispather,

        ServerBootDupli,
        ServerBootError,
        ServerDupliResponse,
        RPCNotAllowed,
        SessionSyncError,
    }
}
