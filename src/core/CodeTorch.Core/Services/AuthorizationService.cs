using CodeTorch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CodeTorch.Core.Services
{
  
    public class AuthorizationService
    {
        static readonly AuthorizationService instance = new AuthorizationService();

        public static AuthorizationService GetInstance()
        {
            return instance;
        }

        private AuthorizationService()
        {
        }

        IAuthorizationProvider authorizationProvider = null;

        public IAuthorizationProvider AuthorizationProvider
        {
            get
            {
                if (authorizationProvider == null)
                {
                    authorizationProvider = GetProvider();
                }
                return authorizationProvider;
            }

            set
            {
                authorizationProvider = value;
            }
        }

        private IAuthorizationProvider GetProvider()
        {
            IAuthorizationProvider retVal = null;
            App app = Configuration.GetInstance().App;

            string assemblyName = app.AuthorizationProviderAssembly;
            string className = app.AuthorizationProviderClass;
            string config = app.AuthorizationProviderConfig;


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
                            retVal = constructor.Invoke(null) as IAuthorizationProvider;

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
                throw new Exception(String.Format("No valid identity provider was found"));
            }

            return retVal;
        }
    }
}
