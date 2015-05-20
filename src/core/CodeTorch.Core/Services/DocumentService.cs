using CodeTorch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CodeTorch.Core.Services
{

    public class DocumentService
    {
        static readonly DocumentService instance = new DocumentService();

        public static DocumentService GetInstance()
        {
            return instance;
        }

        private DocumentService()
        {
        }


        public Dictionary<string, IDocumentProvider> Repositories
        {
            get
            {
                return _repositories;
            }
            set
            {
                _repositories = value;
            }
        }
        

        Dictionary<string, IDocumentProvider> _repositories = new Dictionary<string, IDocumentProvider>();

        

        public IDocumentProvider GetProvider(DocumentRepository repository)
        {
            IDocumentProvider retVal = null;
            App app = Configuration.GetInstance().App;

            

            if (this.Repositories.ContainsKey(repository.Name))
            {
                //get cached data command provider
                retVal = this.Repositories[repository.Name];
            }
            else
            {
                DocumentRepositoryType repositoryType = repository.GetRepositoryType();

                string assemblyName = repositoryType.Assembly;
                string className = repositoryType.Class;
                

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
                                retVal = constructor.Invoke(null) as IDocumentProvider;

                                retVal.Initialize(repository.Settings);

                                this.Repositories.Add(repository.Name, retVal);
                            }
                        }
                    }
                    catch
                    {
                        //silent error
                    }
                }
            }

            if (retVal == null)
            {
                throw new Exception(String.Format("No valid document provider was found"));
            }

            return retVal;
        }
    }
}
