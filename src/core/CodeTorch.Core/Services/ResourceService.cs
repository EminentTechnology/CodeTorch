using CodeTorch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CodeTorch.Core.Services
{

    public class ResourceService
    {
        static readonly ResourceService instance = new ResourceService();

        public static ResourceService GetInstance()
        {
            return instance;
        }

        private ResourceService()
        {
        }

        IResourceProvider resourceProvider = null;

        public IResourceProvider ResourceProvider
        {
            get
            {
                if (resourceProvider == null)
                {
                    resourceProvider = GetProvider();
                }
                return resourceProvider;
            }
            set
            {
                resourceProvider = value;
            }
        }

        private IResourceProvider GetProvider()
        {
            IResourceProvider retVal = null;
            App app = Configuration.GetInstance().App;

            string assemblyName = app.ResourceProviderAssembly;
            string className = app.ResourceProviderClass;
            string config = app.ResourceProviderConfig;


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
                            retVal = constructor.Invoke(null) as IResourceProvider;

                            retVal.Initialize(config);


                        }
                    }
                }
                catch
                {
                    //silent error
                }
            }

            if (retVal == null)
            {
                throw new Exception(String.Format("No valid resource provider was found"));
            }

            return retVal;
        }
    }
}
