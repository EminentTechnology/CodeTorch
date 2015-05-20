using CodeTorch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CodeTorch.Core.Services
{

    public class EmailService
    {
        static readonly EmailService instance = new EmailService();

        public static EmailService GetInstance()
        {
            return instance;
        }

        private EmailService()
        {
        }


        public Dictionary<string, IEmailProvider> Providers
        {
            get
            {
                return _Providers;
            }
            set
            {
                _Providers = value;
            }
        }


        Dictionary<string, IEmailProvider> _Providers = new Dictionary<string, IEmailProvider>();



        public IEmailProvider GetProvider(EmailMessage email)
        {
            IEmailProvider retVal = null;


            EmailConnection connection = email.GetConnection();

            if (connection != null)
            {
                if (this.Providers.ContainsKey(connection.Name))
                {
                    //get cached data command provider
                    retVal = this.Providers[connection.Name];
                }
                else
                {


                    

                    EmailConnectionType connectionType = connection.GetConnectionType();

                    if (connectionType != null)
                    {
                        string assemblyName = connectionType.Assembly;
                        string className = connectionType.Class;

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
                                        retVal = constructor.Invoke(null) as IEmailProvider;

                                        retVal.Initialize(connectionType.Settings);

                                        this.Providers.Add(connection.Name, retVal);
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
            }

            if (retVal == null)
            {
                throw new Exception(String.Format("No valid email provider was found"));
            }

            return retVal;
        }
    }
}
