﻿using DotNettyRPC;
using MessagePack;
using QM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    [MessageIndex(3)]
    [MessagePackObject]
    public class NetSession : IMessagePackSerializationCallbackReceiver
    {
        [Key(0)]
        public string Sid { get; set; }
        [Key(1)]
        public string ServerId { get; set; }//这个ServerId应该指向拥有连接的Connector
        [Key(2)]
        public short DataIndex;
        [Key(3)]
        public object Data { get; set; }
        [Key(4)]
        public string TmpSid { get; set; }//待更新，新的sid

        public void OnBeforeSerialize()
        {
            if (Data == null)
            {
                DataIndex = -1;
                return;
            }
            DataIndex = MessageOpcode.Instance.GetIndex(Data.GetType());
        }

        public void OnAfterDeserialize()
        {
            if (DataIndex == -1) { return; }
            Type type = MessageOpcode.Instance.GetType(DataIndex);
            byte[] bytes = MessagePackUtil.Serialize(Data);
            Data = MessagePackUtil.Deserialize(type, bytes);
        }
    }
}
