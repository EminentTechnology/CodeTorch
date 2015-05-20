using CodeTorch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Core.Services
{
    public class UserIdentityService
    {
        static readonly UserIdentityService instance = new UserIdentityService();

        public static UserIdentityService GetInstance()
        {
            return instance;
        }

        private UserIdentityService()
        {
        }

        IUserIdentityProvider identityProvider = null;

        public IUserIdentityProvider IdentityProvider
        {
            get
            {
                if (identityProvider == null)
                {
                    identityProvider = GetProvider();
                }
                return identityProvider;
            }

        }

        private  IUserIdentityProvider GetProvider()
        {
            IUserIdentityProvider retVal = null;
            App app = Configuration.GetInstance().App;

            string assemblyName = app.UserIdentityProviderAssembly;
            string className = app.UserIdentityProviderClass;
            string config = app.UserIdentityProviderConfig;


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
                            retVal = constructor.Invoke(null) as IUserIdentityProvider;

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
