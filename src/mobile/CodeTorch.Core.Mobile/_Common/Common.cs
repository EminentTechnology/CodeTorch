using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Core
{
    public class Common
    {
        public static void LogException(Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }

        public static void LogException(Exception ex, bool p)
        {
            Debug.WriteLine(ex.Message);

            if (p)
            {
                throw ex;
            }
        }

        internal static object CreateInstance(string assemblyName, string className)
        {
            object retVal = null;

            AssemblyName name = new AssemblyName(assemblyName);
            Assembly assembly = Assembly.Load(name);
            if (assembly != null)
            {
                Type type = assembly.GetType(className);

                if (type != null)
                {
         
                    var constructor = type.GetTypeInfo().DeclaredConstructors.FirstOrDefault(c => c.GetParameters().All(p => p.IsOptional));
                    if (constructor == null)
                    {
                        throw new ArgumentException(String.Format("No parameterless Constructor found for {0} in {1}", className, assemblyName));
                    }

                    var parameters = constructor.GetParameters().Select(p => p.DefaultValue).ToArray();
                    retVal = constructor.Invoke(parameters);
                }
            }

            return retVal;
        }

        public static void LogInfo(string p)
        {
            Debug.WriteLine(p);
        }

        public static void LogDebug(string p)
        {
            Debug.WriteLine(p);
        }
    }
}
