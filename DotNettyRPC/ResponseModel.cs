﻿namespace Coldairarrow.DotNettyRPC
{
    class ResponseModel
    {
        public bool Success { get; set; }
        public short DataIndex { get; set; }
        public string Data { get; set; }
        public string Msg { get; set; }
    }
}
