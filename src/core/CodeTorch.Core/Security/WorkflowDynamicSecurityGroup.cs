using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using CodeTorch.Core.Services;

namespace CodeTorch.Core
{
    [Serializable]
    public class WorkflowDynamicSecurityGroup: BaseSecurityGroup
    {
        public override string Type
        {
            get
            {
                return "Dynamic";
            }
            set
            {
                base.Type = value;
            }
        }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandTypeConverter,CodeTorch.Core.Design")]
        public string DataCommand { get; set; }

        [Category("Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string WorkflowCodeParameter { get; set; }

        [Category("Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string EntityIDParameter { get; set; }

        [Category("Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string CurrentWorkflowStepCodeParameter { get; set; }

        [Category("Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string UsernameParameter { get; set; }

        [Category("Fields")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string UserNameField { get; set; }

        public static DataTable GetUsers(WorkflowDynamicSecurityGroup group, Workflow workflow, WorkflowStep step, string EntityID, string UserName)
        {
            DataTable retVal = null;


            DataCommandService dataCommandDB = DataCommandService.GetInstance();

            
            DataCommand command = Core.DataCommand.GetDataCommand(group.DataCommand);

            if (command == null)
            {
                throw new ApplicationException(String.Format("DataCommand {0} could not be found in configuration", group.DataCommand));
            }


            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter parameter = null;

            if (!String.IsNullOrEmpty(group.WorkflowCodeParameter))
            {
                parameter = new ScreenDataCommandParameter();
                parameter.Name = group.WorkflowCodeParameter;
                parameter.Value = workflow.Code;
                parameters.Add(parameter);
            }

            if (!String.IsNullOrEmpty(group.CurrentWorkflowStepCodeParameter))
            {
                parameter = new ScreenDataCommandParameter();
                parameter.Name = group.CurrentWorkflowStepCodeParameter;
                parameter.Value = step.Code;
                parameters.Add(parameter);
            }

            if (!String.IsNullOrEmpty(group.EntityIDParameter))
            {
                parameter = new ScreenDataCommandParameter();
                parameter.Name = group.EntityIDParameter;
                parameter.Value = EntityID;
                parameters.Add(parameter);
            }

            if (!String.IsNullOrEmpty(group.UsernameParameter))
            {
                parameter = new ScreenDataCommandParameter();
                parameter.Name = group.UsernameParameter;
                parameter.Value = UserName;
                parameters.Add(parameter);
            }

            //execute command with transaction
            //CommandType type = (command.Type == DataCommandCommandType.StoredProcedure) ? CommandType.StoredProcedure : CommandType.Text;
            retVal = dataCommandDB.GetDataForDataCommand(command.Name, parameters);


            return retVal;
        }
    }
}
