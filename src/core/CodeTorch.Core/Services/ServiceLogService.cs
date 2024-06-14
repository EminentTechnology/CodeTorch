using CodeTorch.Core.Interfaces;
using System;
using System.Reflection;

namespace CodeTorch.Core.Services
{
    public class ServiceLogService
    {
        static readonly ServiceLogService instance = new ServiceLogService();

        public static ServiceLogService GetInstance()
        {
            return instance;
        }

        private ServiceLogService()
        {
        }

        IServiceLogProvider serviceLogProvider = null;

        public IServiceLogProvider ServiceLogProvider
        {
            get
            {
                if (serviceLogProvider == null)
                {
                    serviceLogProvider = GetProvider();
                }
                return serviceLogProvider;
            }
            set
            {
                serviceLogProvider = value;
            }
        }

        private IServiceLogProvider GetProvider()
        {
            IServiceLogProvider retVal = null;
            App app = Configuration.GetInstance().App;

            string assemblyName = app.ServiceLogProviderAssembly;
            string className = app.ServiceLogProviderClass;
            string config = app.ServiceLogProviderConfig;


            if (!String.IsNullOrEmpty(assemblyName) && !String.IsNullOrEmpty(className))
            {

                try
                {
                    Assembly providerAssembly = Assembly.Load(assemblyName);
                    if (providerAssembly != null)
                    {
                        Type type = providerAssembly.GetType(className, true, true);

                        if (type != null)
                        {

                            ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                            retVal = constructor.Invoke(null) as IServiceLogProvider;

                            retVal.Initialize(config);


                        }
                    }
                }
                catch
                {
                    //silent error
                }
            }

            return retVal;
        }
    }
}
