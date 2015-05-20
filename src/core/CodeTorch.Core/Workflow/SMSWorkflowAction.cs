using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Collections;
using System.Data.Common;
using System.Reflection;
using CodeTorch.Core.Services;

namespace CodeTorch.Core
{
    [Serializable]
    public class SMSWorkflowAction: BaseWorkflowAction
    {
        List<WorkflowContentDataItem> _DataItems = new List<WorkflowContentDataItem>();
        List<PhoneNumber> _PhoneNumbers = new List<PhoneNumber>();

        public override string Type
        {
            get
            {
                return "SMSWorkflowAction";
            }
            set
            {
                base.Type = value;
            }
        }


        [Category("Content")]
        public DynamicMode ContentType { get; set; }
        [Category("Content")]
        [TypeConverter("CodeTorch.Core.Design.TemplateTypeConverter,CodeTorch.Core.Design")]
        public string ContentTemplate { get; set; }
        [Category("Content")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandTypeConverter,CodeTorch.Core.Design")]
        public string ContentTemplateDataCommand { get; set; }
        [Category("Content")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string ContentTemplateField { get; set; }

        [Category("Content")]
        public List<WorkflowContentDataItem> DataItems
        {
            get { return _DataItems; }
            set { _DataItems = value; }
        }

        [Category("Content Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string ContentWorkflowCodeParameter { get; set; }
        [Category("Content Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string ContentEntityIDParameter { get; set; }
        [Category("Content Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string ContentFromWorkflowStepCodeParameter { get; set; }
        [Category("Content Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string ContentToWorkflowStepCodeParameter { get; set; }
        [Category("Content Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string ContentCommentParameter { get; set; }
        [Category("Content Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string ContentUsernameParameter { get; set; }

        [Category("Phone")]
        public DynamicMode PhoneType { get; set; }
        
        [Category("Phone")]
        public List<PhoneNumber> PhoneNumbers
        {
          get { return _PhoneNumbers; }
          set { _PhoneNumbers = value; }
        }
        [Category("Phone")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandTypeConverter,CodeTorch.Core.Design")]
        public string PhoneDataCommand { get; set; }
        [Category("Phone")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string PhoneField { get; set; }

        [Category("Phone Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string PhoneWorkflowCodeParameter { get; set; }
        [Category("Phone Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string PhoneEntityIDParameter { get; set; }
        [Category("Phone Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string PhoneFromWorkflowStepCodeParameter { get; set; }
        [Category("Phone Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string PhoneToWorkflowStepCodeParameter { get; set; }
        [Category("Phone Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string PhoneCommentParameter { get; set; }
        [Category("Phone Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string PhoneUsernameParameter { get; set; }



        public override void Execute(System.Data.Common.DbTransaction tran, string WorkflowCode, string FromWorkflowStepCode, string ToWorkflowStepCode, string EntityID, string Comment, string UserName)
        {
            SmsService smsDB = new SmsService();
            Workflow workflow = Workflow.GetWorkflowByCode(WorkflowCode);
            //select template
            Template contentTemplate = GetTemplate(tran, WorkflowCode, FromWorkflowStepCode, ToWorkflowStepCode, EntityID, Comment, UserName);
            DateMode dateMode = Configuration.GetInstance().App.DateMode;
            if (contentTemplate != null)
            {
                List<PhoneNumber> numbers = GetPhoneNumbers(tran, WorkflowCode, FromWorkflowStepCode, ToWorkflowStepCode, EntityID, Comment, UserName);

                Hashtable contentItems = GetContentItems(tran, contentTemplate, WorkflowCode, FromWorkflowStepCode, ToWorkflowStepCode, EntityID, Comment, UserName);

                //individual emails for each tos
                foreach (PhoneNumber phoneNo in numbers)
                {

                    string messageText = GetMessageText(phoneNo, contentTemplate, contentItems);

                    //create as many messages as needed in 160 chunks
                    while (messageText.Length > 0)
                    {
                        string messagePart;

                        if (messageText.Length > 160)
                        {
                            messagePart = messageText.Substring(0, 160);
                            messageText = messageText.Substring(160);
                        }
                        else
                        {
                            messagePart = messageText;
                            messageText = String.Empty;
                        }


                        string MessageID = smsDB.InsertMessage( dateMode, phoneNo.Phone, 
                            messagePart, 
                            contentTemplate.Name,
                            workflow.EntityName,
                            EntityID, null,null,null,
                            UserName);

                    }

                }

            }
            else
            {
                throw new ApplicationException("No SMS Template found");
            }

        }

      
        private Hashtable GetContentItems(DbTransaction tran, Template contentTemplate, string WorkflowCode, string FromWorkflowStepCode, string ToWorkflowStepCode, string EntityID, string Comment, string UserName)
        {
            Hashtable retVal = new Hashtable();

            var dataItems = from t in contentTemplate.DataItems
                            join c in this.DataItems
                                on t.Name.ToLower() equals c.DataItem.ToLower()
                            select new
                            {
                                t.Name,
                                Item = c
                            };

            foreach (var dataItem in dataItems)
            {
                DataTable data = GetData(tran, dataItem.Item.DataCommand,
                                    WorkflowCode, FromWorkflowStepCode, ToWorkflowStepCode, EntityID, Comment, UserName,
                                    dataItem.Item.ContentWorkflowCodeParameter,
                                    dataItem.Item.ContentFromWorkflowStepCodeParameter,
                                    dataItem.Item.ContentToWorkflowStepCodeParameter,
                                    dataItem.Item.ContentEntityIDParameter,
                                    dataItem.Item.ContentCommentParameter,
                                    dataItem.Item.ContentUsernameParameter);

                retVal.Add(dataItem.Name, data);
            }



            return retVal;
        }

        private string GetMessageText(PhoneNumber PhoneNo, Template contentTemplate, Hashtable dataItems)
        {
            string retVal = String.Empty;

            Eminent.CodeGenerator.Template t = new Eminent.CodeGenerator.Template();

            t.ClassName = "SMSWorkflowActionTemplate";
            t.Language = "C#";

            t.Assemblies.Add("System.Data");

            t.Namespaces.Add("System.Collections.Generic");
            t.Namespaces.Add("System.Data");
            t.Namespaces.Add("System.Linq");

            t.ParseTemplate(contentTemplate.Content);

            var items = from ct in contentTemplate.DataItems
                            join c in this.DataItems
                                on ct.Name.ToLower() equals c.DataItem.ToLower()
                            select new
                            {
                                ct.Name,
                                ct.Type,
                                Item = c
                            };

            foreach (var item in items)
            {
                Eminent.CodeGenerator.Property p = new Eminent.CodeGenerator.Property();
                p.Name = item.Name;
                p.Type = (item.Type == TemplateDataItemType.DataRow) ? "System.Data.DataRow" : "System.Data.DataRowCollection";

                t.Properties.Add(p);
            }

            Eminent.CodeGenerator.TemplateEngine engine = new Eminent.CodeGenerator.TemplateEngine();
            

            object templateObject = engine.Compile(t);

            if (engine.Errors.Count == 0)
            {
                //populate properties 
                foreach (var item in items)
                {
                    if ((dataItems.ContainsKey(item.Name)) && (dataItems[item.Name] != null))
                    {

                        if (dataItems[item.Name] is DataTable)
                        {
                            DataTable data = (DataTable)dataItems[item.Name];

                            if (item.Type == TemplateDataItemType.DataRow)
                            {

                                if (data.Rows.Count > 0)
                                {
                                    //"(TO = '{0}') OR (TO IS NULL)
                                    DataRow[] rows = data.Select(String.Format("({0} = '{1}') OR ({0} IS NULL)", item.Item.ContentToAddressField, PhoneNo.Phone));

                                    if (rows.Length > 0)
                                    {
                                        PropertyInfo itemProperty = templateObject.GetType().GetProperty(item.Name);
                                        itemProperty.SetValue(templateObject, rows[0], null);
                                    }
                                }
                            }
                            else
                            {
                                if (data.Rows.Count > 0)
                                {
                                    //"(TO = '{0}') OR (TO IS NULL)
                                    DataRow[] rows = data.Select(String.Format("({0} = '{1}') OR ({0} IS NULL)", item.Item.ContentToAddressField, PhoneNo.Phone));
                                    List<DataRow> rowList = new List<DataRow>();
                                    rowList = rows.ToList<DataRow>();

                                    PropertyInfo itemProperty = templateObject.GetType().GetProperty(item.Name);
                                    itemProperty.SetValue(templateObject, rowList, null);

                                }
                            }
                        }
                    }
                }


                retVal = Eminent.CodeGenerator.TemplateEngine.GenerateCode(templateObject);
                
            }
            else
            {
                retVal = engine.GenerateCode(t);
                StringBuilder sb = new StringBuilder();
                foreach (System.CodeDom.Compiler.CompilerError error in engine.Errors)
                {
                    sb.AppendFormat("\r\n({0},{1}): {2} {3}: {4}", error.Line, error.Column, ((error.IsWarning) ? "warning" : "error"), error.ErrorNumber, error.ErrorText);
                }

                throw new ApplicationException(sb.ToString());
            }

            return retVal;
        }


        private Template GetTemplate(System.Data.Common.DbTransaction tran, string WorkflowCode, string FromWorkflowStepCode, string ToWorkflowStepCode, string EntityID, string Comment, string UserName)
        {
            Template retVal = null;
            string TemplateName = null;

            if (this.ContentType == DynamicMode.Static)
            {
                TemplateName = this.ContentTemplate;
            }
            else
            {
                //dynamic -- execute data command and get template
                DataTable data = GetData(tran, this.ContentTemplateDataCommand,
                                    WorkflowCode, FromWorkflowStepCode, ToWorkflowStepCode, EntityID, Comment, UserName,
                                    ContentWorkflowCodeParameter,
                                    ContentFromWorkflowStepCodeParameter,
                                    ContentToWorkflowStepCodeParameter,
                                    ContentEntityIDParameter,
                                    ContentCommentParameter,
                                    ContentUsernameParameter);

                if (data.Rows.Count > 0)
                {
                    if (data.Columns.Contains(this.ContentTemplateField))
                    {
                        TemplateName = data.Rows[0][this.ContentTemplateField].ToString();
                    }
                }

            }

            if (!String.IsNullOrEmpty(TemplateName))
            {
                retVal = Template.GetByName(TemplateName);
            }

            return retVal;
        }

        private List<PhoneNumber> GetPhoneNumbers(System.Data.Common.DbTransaction tran, string WorkflowCode, string FromWorkflowStepCode, string ToWorkflowStepCode, string EntityID, string Comment, string UserName)
        {
            List<PhoneNumber> retVal = new List<PhoneNumber>();



            if (this.PhoneType == DynamicMode.Static)
            {
                retVal = this.PhoneNumbers;
            }
            else
            {

                DataTable data = GetData(tran, this.PhoneDataCommand,
                                    WorkflowCode, FromWorkflowStepCode, ToWorkflowStepCode, EntityID, Comment, UserName,
                                    PhoneWorkflowCodeParameter,
                                    PhoneFromWorkflowStepCodeParameter,
                                    PhoneToWorkflowStepCodeParameter,
                                    PhoneEntityIDParameter,
                                    PhoneCommentParameter,
                                    PhoneUsernameParameter);



                foreach (DataRow record in data.Rows)
                {
                    PhoneNumber phoneNo = new PhoneNumber();
                    phoneNo.Phone = record[this.PhoneField].ToString();
                    retVal.Add(phoneNo);
                }
            }

            return retVal;
        }

        private DataTable GetData(System.Data.Common.DbTransaction tran,
            string CommandName,
            string WorkflowCode,
            string FromWorkflowStepCode,
            string ToWorkflowStepCode,
            string EntityID,
            string Comment,
            string UserName,

            string WorkflowCodeParameter,
            string FromWorkflowStepCodeParameter,
            string ToWorkflowStepCodeParameter,
            string EntityIDParameter,
            string CommentParameter,
            string UsernameParameter
            )
        {
            DataCommandService dataCommandDB = DataCommandService.GetInstance();

            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter parameter = null;

            DataCommand command = DataCommand.GetDataCommand(CommandName);

            if (command == null)
            {
                throw new ApplicationException(String.Format("DataCommand {0} could not be found in configuration", CommandName));
            }

            if (!String.IsNullOrEmpty(WorkflowCodeParameter))
            {
                parameter = new ScreenDataCommandParameter();
                parameter.Name = WorkflowCodeParameter;
                parameter.Value = WorkflowCode;
                parameters.Add(parameter);
            }

            if (!String.IsNullOrEmpty(FromWorkflowStepCodeParameter))
            {
                parameter = new ScreenDataCommandParameter();
                parameter.Name = FromWorkflowStepCodeParameter;
                parameter.Value = FromWorkflowStepCode;
                parameters.Add(parameter);
            }

            if (!String.IsNullOrEmpty(ToWorkflowStepCodeParameter))
            {
                parameter = new ScreenDataCommandParameter();
                parameter.Name = ToWorkflowStepCodeParameter;
                parameter.Value = ToWorkflowStepCode;
                parameters.Add(parameter);
            }

            if (!String.IsNullOrEmpty(EntityIDParameter))
            {
                parameter = new ScreenDataCommandParameter();
                parameter.Name = EntityIDParameter;
                parameter.Value = EntityID;
                parameters.Add(parameter);
            }

            if (!String.IsNullOrEmpty(CommentParameter))
            {
                parameter = new ScreenDataCommandParameter();
                parameter.Name = CommentParameter;
                parameter.Value = Comment;
                parameters.Add(parameter);
            }

            if (!String.IsNullOrEmpty(UsernameParameter))
            {
                parameter = new ScreenDataCommandParameter();
                parameter.Name = UsernameParameter;
                parameter.Value = UserName;
                parameters.Add(parameter);
            }

            //execute command with transaction
            //CommandType type = (command.Type == DataCommandCommandType.StoredProcedure) ? CommandType.StoredProcedure : CommandType.Text;
            DataTable data = dataCommandDB.GetData(tran, command, parameters,  command.Text);
            return data;
        }
        
    }
}
