using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace DotNettyRPC.Helper
{
    public static class MessageOpcodeHelper
    {
        private static IRpcMessageOpcode messageOpcode;

        public static List<short> GetParameterIndexs(object[] parameters)
        {
            if (messageOpcode == null) return null;
            List<short> indexs = new List<short>();
            foreach (var parameter in parameters)
            {
                short? index = messageOpcode.GetIndex(parameter.GetType());
                if (index != null )
                {
                    indexs.Add((short)index);
                }
            }
            return indexs;
        }

        public static List<short> GetParameterIndexs(Type[] parameters)
        {
            if (messageOpcode == null) return null;
            List<short> indexs = new List<short>();
            foreach (var parameter in parameters)
            {
                short? index = messageOpcode.GetIndex(parameter);
                if (index != null)
                {
                    indexs.Add((short)index);
                }
            }
            return indexs;
        }

        public static short? GetIndex(Type type)
        {
            if (messageOpcode == null) return null;
            return messageOpcode.GetIndex(type);
        }

        public static Type GetType(short index)
        {
            if (messageOpcode == null) return null;
            return messageOpcode.GetType(index);
        }

        public static void SetMessageOpcode(IRpcMessageOpcode messageOpcode)
        {
            MessageOpcodeHelper.messageOpcode = messageOpcode;
        }
    }
}
