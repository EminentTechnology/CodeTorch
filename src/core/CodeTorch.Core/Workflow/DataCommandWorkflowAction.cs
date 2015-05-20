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
    public class DataCommandWorkflowAction: BaseWorkflowAction
    {
        public override string Type
        {
            get
            {
                return "DataCommandWorkflowAction";
            }
            set
            {
                base.Type = value;
            }
        }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandTypeConverter,CodeTorch.Core.Design")]
        public string ExecuteCommand { get; set; }

        [Category("Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string WorkflowCodeParameter { get; set; }

        [Category("Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string EntityIDParameter { get; set; }

        [Category("Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string FromWorkflowStepCodeParameter { get; set; }

        [Category("Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string ToWorkflowStepCodeParameter { get; set; }

        [Category("Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string CommentParameter { get; set; }

        [Category("Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string UsernameParameter { get; set; }

        public override void Execute(System.Data.Common.DbTransaction tran, string WorkflowCode, string FromWorkflowStepCode, string ToWorkflowStepCode, string EntityID, string Comment, string UserName)
        {
            //base.Execute(tran);


            DataCommandService dataCommandDB = DataCommandService.GetInstance();

            DataCommand command = DataCommand.GetDataCommand(this.ExecuteCommand);

            if (command == null)
            {
                throw new ApplicationException(String.Format("DataCommand {0} could not be found in configuration", this.ExecuteCommand));
            }


            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter parameter = null;

            if (!String.IsNullOrEmpty(this.WorkflowCodeParameter))
            {
                parameter = new ScreenDataCommandParameter();
                parameter.Name = this.WorkflowCodeParameter;
                parameter.Value = WorkflowCode;
                parameters.Add(parameter);
            }

            if (!String.IsNullOrEmpty(this.FromWorkflowStepCodeParameter))
            {
                parameter = new ScreenDataCommandParameter();
                parameter.Name = this.FromWorkflowStepCodeParameter;
                parameter.Value = FromWorkflowStepCode;
                parameters.Add(parameter);
            }

            if (!String.IsNullOrEmpty(this.ToWorkflowStepCodeParameter))
            {
                parameter = new ScreenDataCommandParameter();
                parameter.Name = this.ToWorkflowStepCodeParameter;
                parameter.Value = ToWorkflowStepCode;
                parameters.Add(parameter);
            }

            if (!String.IsNullOrEmpty(this.EntityIDParameter))
            {
                parameter = new ScreenDataCommandParameter();
                parameter.Name = this.EntityIDParameter;
                parameter.Value = EntityID;
                parameters.Add(parameter);
            }

            if (!String.IsNullOrEmpty(this.CommentParameter))
            {
                parameter = new ScreenDataCommandParameter();
                parameter.Name = this.CommentParameter;
                parameter.Value = Comment;
                parameters.Add(parameter);
            }

            if (!String.IsNullOrEmpty(this.UsernameParameter))
            {
                parameter = new ScreenDataCommandParameter();
                parameter.Name = this.UsernameParameter;
                parameter.Value = UserName;
                parameters.Add(parameter);
            }

            //execute command with transaction
            //CommandType type = (command.Type == DataCommandCommandType.StoredProcedure) ? CommandType.StoredProcedure : CommandType.Text;
            object retVal = dataCommandDB.ExecuteCommand(tran, command, parameters, command.Text);

            
        }
    }
}
