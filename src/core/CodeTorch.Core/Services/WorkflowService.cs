using CodeTorch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CodeTorch.Core.Services
{

    public class WorkflowService
    {
        static readonly WorkflowService instance = new WorkflowService();

        public static WorkflowService GetInstance()
        {
            return instance;
        }

        private WorkflowService()
        {
        }


        public Dictionary<string, IWorkflowProvider> Providers
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


        Dictionary<string, IWorkflowProvider> _Providers = new Dictionary<string, IWorkflowProvider>();



        public IWorkflowProvider GetProvider(Workflow workflow)
        {
            IWorkflowProvider retVal = null;
            App app = Configuration.GetInstance().App;

            WorkflowType workflowType = workflow.GetWorkflowType();

            if (workflowType != null)
            {
                if (this.Providers.ContainsKey(workflowType.Name))
                {
                    //get cached data command provider
                    retVal = this.Providers[workflowType.Name];
                }
                else
                {


                    string assemblyName = workflowType.Assembly;
                    string className = workflowType.Class;


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
                                    retVal = constructor.Invoke(null) as IWorkflowProvider;

                                    retVal.Initialize(workflowType.Settings);

                                    this.Providers.Add(workflowType.Name, retVal);
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

            if (retVal == null)
            {
                throw new Exception(String.Format("No valid workflow provider was found"));
            }

            return retVal;
        }
    }
}
