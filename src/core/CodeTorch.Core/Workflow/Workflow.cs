using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Data;
using System.ComponentModel;
using System.Drawing.Design;
using CodeTorch.Core.Services;

namespace CodeTorch.Core
{
    [Serializable]
    public class Workflow
    {
        List<WorkflowStep> _Steps = new List<WorkflowStep>();

        [Category("General")]
        [Description("The friendly name for this workflow")]
        public string Name { get; set; }

        [Category("General")]
        [Description("The code for this workflow")]
        [ReadOnly(true)]
        public string Code { get; set; }

        [Category("General")]
        public string Description { get; set; }

        [Category("Entity")]
        public string EntityName { get; set; }
        [Category("Entity")]
        public string EntityStatusColumn { get; set; }
        [Category("Entity")]
        public string EntityKeyColumn { get; set; }

        [XmlArray("Steps")]
        [XmlArrayItem("Step")]
        [Editor("CodeTorch.Core.Design.WorkflowStepCollectionEditor,CodeTorch.Core.Design", typeof(UITypeEditor))]  
        public List<WorkflowStep> Steps
        {
            get { return _Steps; }
            set { _Steps = value; }


        }

        [TypeConverter("CodeTorch.Core.Design.WorkflowTypeTypeConverter,CodeTorch.Core.Design")]
        public string WorkflowType { get; set; }

        List<Setting> _settings = new List<Setting>();

        [XmlArray("Settings")]
        [XmlArrayItem("Setting")]
        public List<Setting> Settings
        {
            get
            {
                return _settings;
            }
            set
            {
                _settings = value;
            }
        }

        public static Workflow Load(XDocument doc)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Workflow));
            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            Workflow item = null;

            try
            {
                item = (Workflow)serializer.Deserialize(reader);
                Workflow.Setup(item);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Error occurred while processing Workflow", doc.Root.FirstNode.ToString()), ex);
            }

            return item;

        }

        private static void Setup(Workflow workflow)
        {
            foreach (WorkflowStep step in workflow.Steps)
            {
                step.Workflow = workflow;

                foreach (BaseWorkflowAction action in step.Actions)
                {
                    action.WorkflowStep = step;
                }

                foreach (WorkflowNextStep nextstep in step.PossibleNextSteps)
                {
                    nextstep.Parent = step;

                    foreach (BaseSecurityGroup group in nextstep.SecurityGroups)
                    {
                        group.Parent = nextstep;
                    }
                }

            }
        }

        public static void Save(Workflow item)
        {
            string ConfigPath = ConfigurationLoader.GetFileConfigurationPath();

            if (!Directory.Exists(String.Format("{0}Workflows", ConfigPath)))
            {
                Directory.CreateDirectory(String.Format("{0}Workflows", ConfigPath));
            }
            
            string filePath = String.Format("{0}Workflows\\{1}.xml", ConfigPath, item.Name);
            ConfigurationLoader.SerializeObjectToFile(item, filePath);

        }

        public static Workflow GetWorkflowByName(string WorkflowName)
        {
            Workflow workflow = Configuration.GetInstance().Workflows
                            .Where(w =>
                                (
                                    (w.Name.ToLower() == WorkflowName.ToLower())
                                )
                            )
                            .SingleOrDefault();

            return workflow;
        }

        public static Workflow GetWorkflowByCode(string WorkflowCode)
        {
            Workflow workflow = Configuration.GetInstance().Workflows
                            .Where(w =>
                                (
                                    (w.Code.ToLower() == WorkflowCode.ToLower())
                                )
                            )
                            .SingleOrDefault();

            return workflow;
        }

        public static Workflow GetPossibleWorkflowSteps(string WorkflowCode, string WorkflowStepCode)
        {
            Workflow workflow = GetWorkflowByCode(WorkflowCode);

            if(workflow != null)
            {
                WorkflowStep step = workflow.Steps
                                    .Where(s => 
                                        (
                                            (s.Code.ToLower() == WorkflowStepCode.ToLower())
                                        )
                                    )
                                    .SingleOrDefault();

                if (step != null)
                {
                    foreach (WorkflowNextStep s in step.PossibleNextSteps)
                    { 
                        
                    }
                }
            }
            

            return workflow;
        }

        
        internal static int GetItemCount(string Name)
        {
            int retVal = 0;

            if (String.IsNullOrEmpty(Name))
            {
                retVal = Configuration.GetInstance().Workflows.Count;
            }
            else
            {
                retVal = Configuration.GetInstance().Workflows
                                .Where(i =>
                                    (
                                        (i.Name.ToLower() == Name.ToLower())
                                    )
                                ).Count();
            }

            return retVal;
        }

        

        public WorkflowStep GetStepByCode(string StepCode)
        {
            WorkflowStep step = this.Steps
                            .Where(s =>
                                (
                                    (s.Code.ToLower() == StepCode.ToLower())
                                )
                            )
                            .SingleOrDefault();

            return step;
        }



        public static bool CanUserChangeStep(Workflow workflow, WorkflowStep step, string EntityID, string UserName)
        {
            bool retVal = false;

            foreach (WorkflowNextStep possibleStep in step.PossibleNextSteps)
            {
                foreach (BaseSecurityGroup group in possibleStep.SecurityGroups)
                {
                    retVal = IsUserInSecurityGroup(group, workflow, step, EntityID, UserName);

                    if (retVal)
                        break;
                }

                if (retVal)
                    break;
            }

            return retVal;
        }

        public static List<WorkflowNextStep> GetPossibleNextSteps(Workflow workflow, WorkflowStep step, string EntityID, string UserName)
        {
            List<WorkflowNextStep> possibleSteps = new List<WorkflowNextStep>();

            foreach (WorkflowNextStep possibleStep in step.PossibleNextSteps)
            {
                bool CanChangeToStep = false;

                foreach (BaseSecurityGroup group in possibleStep.SecurityGroups)
                {
                    CanChangeToStep = IsUserInSecurityGroup(group, workflow, step, EntityID, UserName);

                    if (CanChangeToStep)
                        break;
                }

                if (CanChangeToStep)
                {
                    WorkflowNextStep copy = ObjectCopier.Clone<WorkflowNextStep>(possibleStep);
                    possibleSteps.Add(copy);
                }
            }

            return possibleSteps;
        }

        private static bool IsUserInSecurityGroup(BaseSecurityGroup group, Workflow workflow, WorkflowStep step, string EntityID, string UserName)
        {
            bool retVal = false;


            switch (group.Type.ToLower())
            { 
                case "everyone":
                    retVal = true;
                    break;
                
                case "user":
                    UserSecurityGroup userGroup = (UserSecurityGroup)group;
                    if (!String.IsNullOrEmpty(userGroup.User))
                    {
                        if (userGroup.User.ToLower() == UserName)
                        {
                            retVal = true;
                        }
                    }
                    break;
                
                

                case "role":

                    RoleSecurityGroup roleGroup = (RoleSecurityGroup)group;

                    retVal = AuthorizationService.GetInstance().AuthorizationProvider.IsInRole(roleGroup.Role);

                    
                    break;
                case "dynamic":
                    WorkflowDynamicSecurityGroup dynamicGroup = (WorkflowDynamicSecurityGroup)group;
                    DataTable users = WorkflowDynamicSecurityGroup.GetUsers(dynamicGroup, workflow, step, EntityID, UserName);

                    if (users.Rows.Count > 0)
                    {
                        retVal = true;
                    }

                    break;
            }

            return retVal;
        }

        public WorkflowType GetWorkflowType()
        {
            WorkflowType retVal = null;

            if (!String.IsNullOrEmpty(this.WorkflowType))
            {
                retVal = CodeTorch.Core.WorkflowType.GetByName(this.WorkflowType);
            }

            return retVal;
        }
        
    }
}
