using CodeTorch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CodeTorch.Core.Services
{
    public class AppServiceLogProvider : IServiceLogProvider
    {
        List<IServiceLogProvider> providers = new List<IServiceLogProvider>();

        public void Initialize(string config)
        {
            //do nothing
            App app = Configuration.GetInstance().App;

            foreach (ServiceLogProviderItem item in app.ServiceLogProviders)
            {
                if (!String.IsNullOrEmpty(item.Assembly) && !String.IsNullOrEmpty(item.Class))
                {
                    try
                    {
                        Assembly providerAssembly = Assembly.Load(item.Assembly);
                        if (providerAssembly != null)
                        {
                            Type type = providerAssembly.GetType(item.Class, true, true);

                            if (type != null)
                            {

                                ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                                IServiceLogProvider provider = constructor.Invoke(null) as IServiceLogProvider;

                                if (provider != null)
                                {
                                    provider.Initialize(config);
                                    providers.Add(provider);
                                }
                            }
                        }
                    }
                    catch
                    {
                        //silent error
                    }
                }

            }
        }
        public void Log(ServiceLogEntry entry)
        {
            foreach (IServiceLogProvider provider in providers)
            {
                provider.Log(entry);
            }
        }
    }
}
