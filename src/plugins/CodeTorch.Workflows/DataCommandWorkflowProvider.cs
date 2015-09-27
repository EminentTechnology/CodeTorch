using CodeTorch.Core;
using CodeTorch.Core.Interfaces;
using CodeTorch.Core.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace CodeTorch.Workflows
{
    public class DataCommandWorkflowProvider: IWorkflowProvider
    {
        DataCommandService sql = DataCommandService.GetInstance();

        private const string DataCommandWorkflowGetCurrentStep = "Workflow_GetCurrentStep";
        private const string DataCommandWorkflowSetEntityStatus = "Workflow_SetEntityStatus";
        private const string DataCommandWorkflowSetStep = "Workflow_SetStep";
        private const string DataCommandWorkflowDelete = "Workflow_Delete";
        private const string DataCommandWorkflowInsertStep = "Workflow_InsertStep";
        private const string DataCommandWorkflowInsert = "Workflow_Insert";
        
        

        private const string ParameterWorkflowStatusID = "@WorkflowStatusID";
        private const string ParameterWorkflowCode = "@WorkflowCode";
        private const string ParameterWorkflowName = "@WorkflowName";
        private const string ParameterWorkflowDescription = "@WorkflowDescription";

        private const string ParameterWorkflowStepCode = "@WorkflowStepCode";
        private const string ParameterWorkflowStepSequence = "@WorkflowStepSequence";
        private const string ParameterWorkflowStepName = "@WorkflowStepName";
        private const string ParameterEntityType = "@EntityType";
        private const string ParameterEntityID = "@EntityID";
        private const string ParameterEntityName = "@EntityName";
        private const string ParameterEntityKeyField = "@EntityKeyColumn";
        private const string ParameterEntityStatusField = "@EntityStatusColumn";
        private const string ParameterStatus = "@Status";
        private const string ParameterComment = "@Comment";
        private const string ParameterUpdateCurrent = "@UpdateCurrent";
        private const string ParameterUpdateEntityWithStatusCode = "@UpdateEntityWithStatusCode";
        private const string ParameterCreatedBy = "@CreatedBy";

        private const string ColumnCurrentStepCode = "CurrentStepCode";

        public DataConnection Connection { get; set; }
        public void Initialize(List<CodeTorch.Core.Setting> settings)
        {
            
        }

        public WorkflowStep GetCurrentWorkflowStep(CodeTorch.Core.Workflow workflow, string entityIDValue)
        {
            WorkflowStep retVal = null;

            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter p = null;

            p = new ScreenDataCommandParameter(ParameterWorkflowCode, workflow.Code);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterEntityType, workflow.EntityName);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterEntityID, entityIDValue);
            parameters.Add(p);

            //get data from data command
            DataTable data = GetDataForDataCommand(DataCommandWorkflowGetCurrentStep, parameters);

            
            if (data.Rows.Count > 0)
            {
                string CurrentCode = data.Rows[0][ColumnCurrentStepCode].ToString();

                retVal = workflow.GetStepByCode(CurrentCode);
            }

            return retVal;
        }

        public bool ChangeWorkflowStep(CodeTorch.Core.Workflow workflow, WorkflowNextStep nextStep, string entityIDValue, string comment)
        {
            bool success = false;


            //get current workflow step
            WorkflowStep currentStep = GetCurrentWorkflowStep(workflow, entityIDValue);

            //see if next workflowstep is in possible
            WorkflowNextStep validNextStep = currentStep.PossibleNextSteps.
                Where(s => s.Code.ToLower() == nextStep.Code.ToLower()).SingleOrDefault();

            if (validNextStep != null)
            {
                //check comments
                if ((validNextStep.RequireComment) && (String.IsNullOrEmpty(comment)))
                {
                    throw new ApplicationException("Comments are required to change to this step");
                }
                else
                {
                    WorkflowStep step = workflow.GetStepByCode(validNextStep.Code);


                    string userName;
                    userName = UserIdentityService.GetInstance().IdentityProvider.GetUserName();

                    using (TransactionScope rootScope = TransactionUtils.CreateTransactionScope())
                    {
                        foreach (BaseWorkflowAction action in step.Actions)
                        {
                            action.Execute(null, workflow.Code, currentStep.Code, validNextStep.Code, entityIDValue, comment, userName);
                        }

                        //update workflow step
                        SetStep(workflow, step, entityIDValue, comment);

                        if (step.UpdateEntityWithStatusCode)
                        {
                            //update existing table status
                            SetEntityStatus(workflow, step, entityIDValue);
                        }

                        success = true;

                        rootScope.Complete();
                    }

                    
                }
            }
            else
            {
                //invalid - someone may have changed status
                throw new ApplicationException("Status cannot be changed at this time");
            }


            return success;
        }

        public void SetStep(CodeTorch.Core.Workflow workflow, WorkflowStep step, string entityIDValue, string comment)
        {
            
            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter p = null;

            p = new ScreenDataCommandParameter(ParameterWorkflowStatusID, null);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterWorkflowCode, workflow.Code);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterWorkflowStepCode, step.Code);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterEntityID, entityIDValue);
            parameters.Add(p);

            string userName;
            userName = UserIdentityService.GetInstance().IdentityProvider.GetUserName();
            p = new ScreenDataCommandParameter(ParameterCreatedBy, userName);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterComment, comment);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterUpdateCurrent, step.UpdateEntityWithStatusCode);
            parameters.Add(p);

            //get data from data command
            ExecuteDataCommand(DataCommandWorkflowSetStep, parameters);

            
        }

        public void SetEntityStatus(CodeTorch.Core.Workflow workflow, WorkflowStep step, string entityIDValue)
        {
            

            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter p = null;

            p = new ScreenDataCommandParameter(ParameterEntityName, workflow.EntityName);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterEntityStatusField, workflow.EntityStatusColumn);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterEntityID, entityIDValue);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterStatus, step.Code);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterEntityKeyField, workflow.EntityKeyColumn);
            parameters.Add(p);

            //get data from data command
            ExecuteDataCommand(DataCommandWorkflowSetEntityStatus, parameters);

            
        }

        public void Save(CodeTorch.Core.Workflow workflow)
        {
                Delete( workflow);

                InsertWorkflow( workflow);

                int StepSequence = 0;
                foreach (WorkflowStep step in workflow.Steps)
                {
                    StepSequence++;
                    InsertWorkflowStep( workflow, step, StepSequence);
                }
            
        }

        public void Delete( CodeTorch.Core.Workflow workflow)
        {
            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter p = null;

            p = new ScreenDataCommandParameter(ParameterWorkflowCode, workflow.Code);
            parameters.Add(p);

           ExecuteDataCommand(DataCommandWorkflowDelete, parameters);
        }

        private void InsertWorkflow(CodeTorch.Core.Workflow workflow)
        {
            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter p = null;

            p = new ScreenDataCommandParameter(ParameterWorkflowCode, workflow.Code);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterWorkflowName, workflow.EntityName);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterWorkflowDescription, workflow.Description);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterEntityName, workflow.EntityName);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterEntityKeyField, workflow.EntityKeyColumn);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterEntityStatusField, workflow.EntityStatusColumn);
            parameters.Add(p);


            ExecuteDataCommand(DataCommandWorkflowInsert, parameters);
        }

        private void InsertWorkflowStep(CodeTorch.Core.Workflow workflow, WorkflowStep step, int stepSequence)
        {
            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter p = null;

            p = new ScreenDataCommandParameter(ParameterWorkflowCode, workflow.Code);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterWorkflowStepCode, step.Code);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterWorkflowStepSequence, stepSequence);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterWorkflowStepName, step.Name);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterUpdateEntityWithStatusCode, step.UpdateEntityWithStatusCode);
            parameters.Add(p);



           ExecuteDataCommand(DataCommandWorkflowInsertStep, parameters);
        }

        private void ExecuteDataCommand(string dataCommand, List<ScreenDataCommandParameter> parameters)
        {
            if (Connection == null)
            {
                sql.ExecuteDataCommand(dataCommand, parameters);
            }
            else
            {
                DataCommand command = DataCommand.GetDataCommand(dataCommand);

                if (command == null)
                    throw new Exception(String.Format("DataCommand {0} could not be found in configuration", dataCommand));

                sql.ExecuteCommand(null, Connection, command, parameters, command.Text);
            }
        }

        private DataTable GetDataForDataCommand(string dataCommand, List<ScreenDataCommandParameter> parameters)
        {
            DataTable retVal = null;

            if (Connection == null)
            {
                retVal = sql.GetDataForDataCommand(dataCommand, parameters);
            }
            else
            {
                DataCommand command = DataCommand.GetDataCommand(dataCommand);

                if (command == null)
                    throw new Exception(String.Format("DataCommand {0} could not be found in configuration", dataCommand));

                retVal = sql.GetData(null, Connection, command, parameters, command.Text);
            }

            return retVal;
        }
    }
}
