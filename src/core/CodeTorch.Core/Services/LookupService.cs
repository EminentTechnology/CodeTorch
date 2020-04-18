using CodeTorch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CodeTorch.Core.Services
{

    public class LookupService
    {
        static readonly LookupService instance = new LookupService();

        public static LookupService GetInstance()
        {
            return instance;
        }

        private LookupService()
        {
        }

        ILookupProvider lookupProvider = null;

        public ILookupProvider LookupProvider
        {
            get
            {
                if (lookupProvider == null)
                {
                    lookupProvider = GetProvider();
                }
                return lookupProvider;
            }
            set
            {
                lookupProvider = value;
            }
        }

        private ILookupProvider GetProvider()
        {
            ILookupProvider retVal = null;
            App app = Configuration.GetInstance().App;

            string assemblyName = app.LookupProviderAssembly;
            string className = app.LookupProviderClass;
            string config = app.LookupProviderConfig;


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
                            retVal = constructor.Invoke(null) as ILookupProvider;

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
                throw new Exception(String.Format("No valid lookup provider was found"));
            }

            return retVal;
        }
    }
}
