using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QM
{
    public class CodeType
    {
        public static readonly CodeType Instance = new CodeType();
        // AttrituteType => Type
        private readonly Dictionary<Type, List<Type>> types = new Dictionary<Type, List<Type>>();
        private CodeType()
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    object[] attributes = type.GetCustomAttributes(typeof(BaseAttribute), false);

                    foreach (object attribute in attributes)
                    {
                        if (attribute != null)
                        {
                            Type attrType = attribute.GetType();
                            types.TryGetValue(attrType, out var list);
                            if (list != null)
                            {
                                list.Add(type);
                            }
                            else
                            {
                                List<Type> vList = new List<Type> { type };
                                types.Add(attrType, vList);
                            }
                        }
                    }
                }
            }
        }

        public List<Type> GetTypes(Type attrType)
        {
            types.TryGetValue(attrType, out var list);
            return list;
        }

    }
}
