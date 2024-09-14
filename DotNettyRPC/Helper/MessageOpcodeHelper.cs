using QM;
using System;
using System.Collections.Generic;

namespace DotNettyRPC
{
    public static class MessageOpcodeHelper
    {
        private static IRpcMessageOpcode messageOpcode;

        public static List<short> GetParameterIndexs(object[] parameters)
        {
            if (messageOpcode == null) throw new QMException(ErrorCode.MessageOpcodeNotFound, "没有及时设置messageOpcode");
            List<short> indexs = new List<short>();
            foreach (var parameter in parameters)
            {
                short index = messageOpcode.GetIndex(parameter.GetType());
                indexs.Add(index);
            }
            return indexs;
        }

        public static List<short> GetParameterIndexs(Type[] parameters)
        {
            if (messageOpcode == null) throw new QMException(ErrorCode.MessageOpcodeNotFound, "没有及时设置messageOpcode");
            List<short> indexs = new List<short>();
            foreach (var parameter in parameters)
            {
                short index = messageOpcode.GetIndex(parameter);
                indexs.Add(index);
            }
            return indexs;
        }

        public static short GetIndex(Type type)
        {
            if (messageOpcode == null) throw new QMException(ErrorCode.MessageOpcodeNotFound, "没有及时设置messageOpcode");
            return messageOpcode.GetIndex(type);
        }

        public static Type GetType(short index)
        {
            if (messageOpcode == null) throw new QMException(ErrorCode.MessageOpcodeNotFound, "没有及时设置messageOpcode");
            return messageOpcode.GetType(index);
        }

        public static void SetMessageOpcode(IRpcMessageOpcode messageOpcode)
        {
            MessageOpcodeHelper.messageOpcode = messageOpcode;
        }
    }
}
