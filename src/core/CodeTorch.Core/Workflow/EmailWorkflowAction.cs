using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Net.Mail;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Reflection;
using CodeTorch.Core.Services;
using CodeTorch.Core.Interfaces;

namespace CodeTorch.Core
{
    [Serializable]
    public class EmailWorkflowAction: BaseWorkflowAction
    {
        MailPriority _Priority = MailPriority.Normal;
        List<WorkflowContentDataItem> _DataItems = new List<WorkflowContentDataItem>();
        List<EmailAddress> _ToAddresses = new List<EmailAddress>();
        List<EmailAddress> _CCAddresses = new List<EmailAddress>();
        List<EmailAddress> _BCCAddresses = new List<EmailAddress>();
        EmailAddress _FromAddress = new EmailAddress();

        public override string Type
        {
            get
            {
                return "EmailWorkflowAction";
            }
            set
            {
                base.Type = value;
            }
        }

        

        [Category("Common")]
        public bool IsBodyHtml { get; set; }

        [Category("Common")]
        public MailPriority Priority
        {
            get { return _Priority; }
            set { _Priority = value; }
        }

        [TypeConverter("CodeTorch.Core.Design.EmailConnectionTypeConverter,CodeTorch.Core.Design")]
        public string EmailConnection { get; set; }


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

        
        [Category("To Address")]
        public DynamicMode ToAddressType { get; set; }
        
        [Category("To Address")]
        public List<EmailAddress> ToAddresses
        {
            get { return _ToAddresses; }
            set { _ToAddresses = value; }
        }
        [Category("To Address")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandTypeConverter,CodeTorch.Core.Design")]
        public string ToAddressDataCommand { get; set; }

        [Category("To Address")]
        public EmailAddressSendMode ToAddressSendMode { get; set; } 

        [Category("To Address")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string ToAddressField { get; set; }

        [Category("To Address")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string ToAddressDisplayNameField { get; set; }
        
        [Category("To Address Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string ToAddressWorkflowCodeParameter { get; set; }
        [Category("To Address Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string ToAddressEntityIDParameter { get; set; }
        [Category("To Address Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string ToAddressFromWorkflowStepCodeParameter { get; set; }
        [Category("To Address Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string ToAddressToWorkflowStepCodeParameter { get; set; }
        [Category("To Address Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string ToAddressCommentParameter { get; set; }
        [Category("To Address Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string ToAddressUsernameParameter { get; set; }



        [Category("Recipient CC Address")]
        public DynamicMode CCAddressType { get; set; }

        [Category("Recipient CC Address")]
        public List<EmailAddress> CCAddresses
        {
            get { return _CCAddresses; }
            set { _CCAddresses = value; }
        }
        
        [Category("Recipient CC Address")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string CCAddressField { get; set; }
        [Category("Recipient CC Address")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string CCToAddressField { get; set; }
        [Category("Recipient CC Address")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string CCAddressDisplayNameField { get; set; }
        [Category("Recipient CC Address")]
        public string CCAddressDataCommand { get; set; }
        [Category("Recipient CC Address Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string CCAddressWorkflowCodeParameter { get; set; }
        [Category("Recipient CC Address Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string CCAddressEntityIDParameter { get; set; }
        [Category("Recipient CC Address Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string CCAddressFromWorkflowStepCodeParameter { get; set; }
        [Category("Recipient CC Address Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string CCAddressToWorkflowStepCodeParameter { get; set; }
        [Category("Recipient CC Address Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string CCAddressCommentParameter { get; set; }
        [Category("Recipient CC Address Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string CCAddressUsernameParameter { get; set; }
        
        

        [Category("Recipient BCC Address")]
        public DynamicMode BCCAddressType { get; set; }
        [Category("Recipient BCC Address")]
        public List<EmailAddress> BCCAddresses
        {
            get { return _BCCAddresses; }
            set { _BCCAddresses = value; }
        }
        [Category("Recipient BCC Address")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandTypeConverter,CodeTorch.Core.Design")]
        public string BCCAddressDataCommand { get; set; }
        [Category("Recipient BCC Address")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string BCCAddressField { get; set; }
        [Category("Recipient BCC Address")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string BCCToAddressField { get; set; }
        [Category("Recipient BCC Address")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string BCCAddressDisplayNameField { get; set; }
        [Category("Recipient BCC Address Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string BCCAddressWorkflowCodeParameter { get; set; }
        [Category("Recipient BCC Address Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string BCCAddressEntityIDParameter { get; set; }
        [Category("Recipient BCC Address Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string BCCAddressFromWorkflowStepCodeParameter { get; set; }
        [Category("Recipient BCC Address Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string BCCAddressToWorkflowStepCodeParameter { get; set; }
        [Category("Recipient BCC Address Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string BCCAddressCommentParameter { get; set; }
        [Category("Recipient BCC Address Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string BCCAddressUsernameParameter { get; set; }
        
        

        [Category("Mail Subject")]
        public DynamicMode SubjectType { get; set; }
        [Category("Mail Subject")]
        public string Subject { get; set; }
        [Category("Mail Subject")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandTypeConverter,CodeTorch.Core.Design")]
        public string SubjectDataCommand { get; set; }
        [Category("Mail Subject")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string SubjectField { get; set; }
        [Category("Mail Subject")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string SubjectToAddressField { get; set; }
        [Category("Mail Subject Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string SubjectWorkflowCodeParameter { get; set; }
        [Category("Mail Subject Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string SubjectEntityIDParameter { get; set; }
        [Category("Mail Subject Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string SubjectFromWorkflowStepCodeParameter { get; set; }
        [Category("Mail Subject Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string SubjectToWorkflowStepCodeParameter { get; set; }
        [Category("Mail Subject Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string SubjectCommentParameter { get; set; }
        [Category("Mail Subject Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string SubjectUsernameParameter { get; set; }
        

        [Category("From Address")]
        public DynamicMode FromAddressType { get; set; }
        [Category("From Address")]
        public EmailAddress FromAddress
        {
            get { return _FromAddress; }
            set { _FromAddress = value; }
        }
        [Category("From Address")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandTypeConverter,CodeTorch.Core.Design")]
        public string FromAddressDataCommand { get; set; }
        [Category("From Address")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string FromAddressField { get; set; }
        [Category("From Address")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string FromToAddressField { get; set; }
        [Category("From Address")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string FromAddressDisplayNameField { get; set; }
        [Category("From Address Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string FromAddressWorkflowCodeParameter { get; set; }
        [Category("From Address Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string FromAddressEntityIDParameter { get; set; }
        [Category("From Address Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string FromAddressFromWorkflowStepCodeParameter { get; set; }
        [Category("From Address Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string FromAddressToWorkflowStepCodeParameter { get; set; }
        [Category("From Address Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string FromAddressCommentParameter { get; set; }
        [Category("From Address Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string FromAddressUsernameParameter { get; set; }
        

        [Category("Mail Attachments")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandTypeConverter,CodeTorch.Core.Design")]
        public string AttachmentDataCommand { get; set; }
        [Category("Mail Attachments")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string AttachmentDocumentIDField { get; set; }
        [Category("Mail Attachments")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string AttachmentToAddressField { get; set; }
        [Category("Mail Attachments Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string AttachmentWorkflowCodeParameter { get; set; }
        [Category("Mail Attachments Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string AttachmentEntityIDParameter { get; set; }
        [Category("Mail Attachments Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string AttachmentFromWorkflowStepCodeParameter { get; set; }
        [Category("Mail Attachments Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string AttachmentToWorkflowStepCodeParameter { get; set; }
        [Category("Mail Attachments Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string AttachmentCommentParameter { get; set; }
        [Category("Mail Attachments Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string AttachmentUsernameParameter { get; set; }


        public override void Execute(System.Data.Common.DbTransaction tran,  string workflowCode, string fromWorkflowStepCode, string toWorkflowStepCode, string entityID, string comment, string userName)
        {
            EmailAddress address = null;
            Workflow workflow =  Workflow.GetWorkflowByCode(workflowCode);
            //select template
            Template contentTemplate = GetTemplate(tran, workflowCode, fromWorkflowStepCode, toWorkflowStepCode, entityID, comment, userName);
            DateMode dateMode = Configuration.GetInstance().App.DateMode;
            if (contentTemplate != null)
            {
                DataTable subjects = GetSubjects(tran, workflowCode, fromWorkflowStepCode, toWorkflowStepCode, entityID, comment, userName);
                DataTable from = GetAddresses(tran, "from", workflowCode, fromWorkflowStepCode, toWorkflowStepCode, entityID, comment, userName);
                DataTable cc = GetAddresses(tran, "cc", workflowCode, fromWorkflowStepCode, toWorkflowStepCode, entityID, comment, userName);
                DataTable bcc = GetAddresses(tran, "bcc", workflowCode, fromWorkflowStepCode, toWorkflowStepCode, entityID, comment, userName);
                DataTable to = GetAddresses(tran, "to", workflowCode, fromWorkflowStepCode, toWorkflowStepCode, entityID, comment, userName);
                DataTable replyto = GetAddresses(tran, "to", workflowCode, fromWorkflowStepCode, toWorkflowStepCode, entityID, comment, userName);
                DataTable attachments = GetAttachments(tran, workflowCode, fromWorkflowStepCode, toWorkflowStepCode, entityID, comment, userName);
                DataTable mailitems = GetMailItems(to);

                Hashtable contentItems = GetContentItems(tran, contentTemplate,workflowCode, fromWorkflowStepCode, toWorkflowStepCode, entityID, comment, userName);

                IEmailProvider email = null;

                //individual emails for each tos
                foreach (DataRow mail in mailitems.Rows)
                {
                    EmailMessage message = new EmailMessage();

                    

                    if (email == null)
                    {
                        //set connection
                        message.EmailConnection = this.EmailConnection;
                        email = EmailService.GetInstance().GetProvider(message);
                    }


                    message.Subject = GetSubject(mail, subjects);
                    message.Body = GetBody(mail, contentTemplate, contentItems);
                    message.EntityID = entityID;
                    message.EntityType = workflow.EntityName;
                    message.IsBodyHtml = this.IsBodyHtml;
                    message.Priority = this.Priority;
                    message.Template = contentTemplate.Name;


                    //from
                    DataRow[] fromList = from.Select(String.Format("(TO = '{0}') OR (TO IS NULL)", mail["TO"]));
                    if (fromList.Length > 0)
                    {
                        DataRow a = fromList[0];

                        address = new EmailAddress();
                        address.DisplayName = a["DisplayName"].ToString();
                        address.Address = a["Address"].ToString();

                        message.From = address;

                    }

                    //to
                    if (this.ToAddressSendMode == EmailAddressSendMode.Individual)
                    {
                        address = new EmailAddress();
                        address.DisplayName = mail["DisplayName"].ToString();
                        address.Address = mail["Address"].ToString();

                        message.To.Add(address);

                       
                    }
                    else
                    {
                        foreach (DataRow a in to.Rows)
                        {
                            address = new EmailAddress();
                            address.DisplayName = a["DisplayName"].ToString();
                            address.Address = a["Address"].ToString();

                            message.To.Add(address);
                        }
                    }

                   
                    //cc
                    foreach (DataRow a in cc.Select(String.Format("(TO = '{0}') OR (TO IS NULL)", mail["TO"])))
                    {
                        address = new EmailAddress();
                        address.DisplayName = a["DisplayName"].ToString();
                        address.Address = a["Address"].ToString();

                        message.CC.Add(address);
                    }

                    //bcc
                    foreach (DataRow a in bcc.Select(String.Format("(TO = '{0}') OR (TO IS NULL)", mail["TO"])))
                    {
                        address = new EmailAddress();
                        address.DisplayName = a["DisplayName"].ToString();
                        address.Address = a["Address"].ToString();

                        message.BCC.Add(address);
                    }

                    foreach (DataRow a in replyto.Select(String.Format("(TO = '{0}') OR (TO IS NULL)", mail["TO"])))
                    {
                        address = new EmailAddress();
                        address.DisplayName = a["DisplayName"].ToString();
                        address.Address = a["Address"].ToString();

                        message.ReplyTo.Add(address);
                    }

                    //attachments
                    foreach (DataRow a in attachments.Select(String.Format("(TO = '{0}') OR (TO IS NULL)", mail["TO"])))
                    {
                        EmailAttachment attachment = new EmailAttachment();
                        attachment.DocumentID = a["DocumentID"].ToString();

                        message.Attachments.Add(attachment);

                       
                    }

                    email.Send(message);
                }
            }
            else
            {
                throw new ApplicationException("No Email Template found");
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

        private DataTable GetMailItems(DataTable to)
        {
            DataTable retVal = new DataTable();
            if (this.ToAddressSendMode == EmailAddressSendMode.Individual)
            {
                retVal = to;
            }
            else
            {
                retVal.Columns.Add("To", typeof(string));
                retVal.Columns.Add("Address", typeof(string));
                retVal.Columns.Add("DisplayName", typeof(string));

                DataRow row = retVal.NewRow();
                row["To"] = DBNull.Value;
                row["Address"] = DBNull.Value;
                row["DisplayName"] = DBNull.Value;
                retVal.Rows.Add(row);

                
            }

            return retVal;
        }

        private string GetBody(DataRow mail, Template contentTemplate, Hashtable dataItems)
        {
            string retVal = String.Empty;
            //use code generator to generate body message
            Eminent.CodeGenerator.Template t = new Eminent.CodeGenerator.Template();

            t.ClassName = "EmailWorkflowActionTemplate";
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
                                    DataRow[] rows = null;

                                    if (item.Item.ContentToAddressField != null)
                                    {
                                        //"(TO = '{0}') OR (TO IS NULL)
                                        rows = data.Select(String.Format("({0} = '{1}') OR ({0} IS NULL)", item.Item.ContentToAddressField, mail["TO"]));
                                    }
                                    else
                                    {
                                        rows = data.Select();
                                    }

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
                                     DataRow[] rows = null;

                                     if (item.Item.ContentToAddressField != null)
                                     {
                                         //"(TO = '{0}') OR (TO IS NULL)
                                         rows = data.Select(String.Format("({0} = '{1}') OR ({0} IS NULL)", item.Item.ContentToAddressField, mail["TO"]));
                                     }
                                     else
                                     {
                                         rows = data.Select();
                                     }
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

        private string GetSubject(DataRow mail, DataTable subjects)
        {
            string retVal = String.Empty;
            DataView subjectsDV = subjects.DefaultView;
            //check to see if there is mail specific message
            subjectsDV.RowFilter = String.Format("To = '{0}'", mail["To"]);

            if (subjectsDV.Count > 0)
            {
                retVal = subjectsDV[0]["Subject"].ToString();
            }
            else
            {
                subjectsDV.RowFilter = "To IS NULL";
                if (subjectsDV.Count > 0)
                {
                    retVal = subjectsDV[0]["Subject"].ToString();
                }
            }

            subjectsDV.RowFilter = String.Empty;

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

        private DataTable GetAddresses(System.Data.Common.DbTransaction tran, string AddressType, string WorkflowCode, string FromWorkflowStepCode, string ToWorkflowStepCode, string EntityID, string Comment, string UserName)
        {
            DataTable retVal = new DataTable();
            DataRow row = null;
            retVal.Columns.Add("To", typeof(string));
            retVal.Columns.Add("Address", typeof(string));
            retVal.Columns.Add("DisplayName", typeof(string));

            DynamicMode mode = DynamicMode.Static;

            switch (AddressType.ToLower())
            { 
                case "to":
                    mode = this.ToAddressType;
                    break;
                case "cc":
                    mode = this.CCAddressType;
                    break;
                case "bcc":
                    mode = this.BCCAddressType;
                    break;
                case "from":
                    mode = this.FromAddressType;
                    break;
            }

            if (mode == DynamicMode.Static)
            {
                List<EmailAddress> emails = new List<EmailAddress>();

                

                switch (AddressType.ToLower())
                {
                    case "to":
                        emails = this.ToAddresses;
                        break;
                    case "cc":
                        emails = this.CCAddresses;
                        break;
                    case "bcc":
                        emails = this.BCCAddresses;
                        break;
                    case "from":
                        emails.Add(this.FromAddress);
                        break;
                }

                foreach (EmailAddress email in emails)
                {
                    row = retVal.NewRow();
                    row["To"] = DBNull.Value;
                    row["Address"] = email.Address;
                    row["DisplayName"] = email.DisplayName;
                    retVal.Rows.Add(row);
                }

                
            }
            else
            {
                string DataCommandName = null;
                string WorkflowCodeParameter = null; 
                string FromWorkflowStepCodeParameter = null; 
                string ToWorkflowStepCodeParameter = null; 
                string EntityIDParameter = null; 
                string CommentParameter = null; 
                string UsernameParameter = null;
                string ToField = null;
                string AddressField = null;
                string DisplayNameField = null;

                switch (AddressType.ToLower())
                {
                    case "to":
                        DataCommandName = this.ToAddressDataCommand;
                        WorkflowCodeParameter = this.ToAddressWorkflowCodeParameter;
                        FromWorkflowStepCodeParameter = this.ToAddressFromWorkflowStepCodeParameter;
                        ToWorkflowStepCodeParameter = this.ToAddressToWorkflowStepCodeParameter;
                        EntityIDParameter = this.ToAddressEntityIDParameter;
                        CommentParameter = this.ToAddressCommentParameter;
                        UsernameParameter = this.ToAddressUsernameParameter;
                        ToField = this.ToAddressField;
                        AddressField = this.ToAddressField;
                        DisplayNameField = this.ToAddressDisplayNameField;
                        break;
                    case "cc":
                        DataCommandName = this.CCAddressDataCommand;
                        WorkflowCodeParameter = this.CCAddressWorkflowCodeParameter;
                        FromWorkflowStepCodeParameter = this.CCAddressFromWorkflowStepCodeParameter;
                        ToWorkflowStepCodeParameter = this.CCAddressToWorkflowStepCodeParameter;
                        EntityIDParameter = this.CCAddressEntityIDParameter;
                        CommentParameter = this.CCAddressCommentParameter;
                        UsernameParameter = this.CCAddressUsernameParameter;
                        ToField = this.CCToAddressField;
                        AddressField = this.CCAddressField;
                        DisplayNameField = this.CCAddressDisplayNameField;
                        break;
                    case "bcc":
                        DataCommandName = this.BCCAddressDataCommand;
                        WorkflowCodeParameter = this.BCCAddressWorkflowCodeParameter;
                        FromWorkflowStepCodeParameter = this.BCCAddressFromWorkflowStepCodeParameter;
                        ToWorkflowStepCodeParameter = this.BCCAddressToWorkflowStepCodeParameter;
                        EntityIDParameter = this.BCCAddressEntityIDParameter;
                        CommentParameter = this.BCCAddressCommentParameter;
                        UsernameParameter = this.BCCAddressUsernameParameter;
                        ToField = this.BCCToAddressField;
                        AddressField = this.BCCAddressField;
                        DisplayNameField = this.BCCAddressDisplayNameField;
                        break;
                    case "from":
                        DataCommandName = this.FromAddressDataCommand;
                        WorkflowCodeParameter = this.FromAddressWorkflowCodeParameter;
                        FromWorkflowStepCodeParameter = this.FromAddressFromWorkflowStepCodeParameter;
                        ToWorkflowStepCodeParameter = this.FromAddressToWorkflowStepCodeParameter;
                        EntityIDParameter = this.FromAddressEntityIDParameter;
                        CommentParameter = this.FromAddressCommentParameter;
                        UsernameParameter = this.FromAddressUsernameParameter;
                        ToField = this.FromToAddressField;
                        AddressField = this.FromAddressField;
                        DisplayNameField = this.FromAddressDisplayNameField;
                        break;
                }

                DataTable data = GetData(tran, DataCommandName, 
                                    WorkflowCode, FromWorkflowStepCode, ToWorkflowStepCode, EntityID, Comment, UserName, 
                                    WorkflowCodeParameter,
                                    FromWorkflowStepCodeParameter,
                                    ToWorkflowStepCodeParameter,
                                    EntityIDParameter,
                                    CommentParameter,
                                    UsernameParameter);

                

                foreach (DataRow record in data.Rows)
                {
                    row = retVal.NewRow();
                    row["To"] = record[ToField];
                    row["Address"] = record[AddressField];
                    row["DisplayName"] = record[DisplayNameField];

                    retVal.Rows.Add(row);
                }
            }

            return retVal;
        }

        private DataTable GetSubjects(System.Data.Common.DbTransaction tran, string WorkflowCode, string FromWorkflowStepCode, string ToWorkflowStepCode, string EntityID, string Comment, string UserName)
        {
            DataTable retVal = new DataTable();
            DataRow row = null;
            retVal.Columns.Add("To", typeof(string));
            retVal.Columns.Add("Subject", typeof(string)); 
            


            if (this.SubjectType == DynamicMode.Static)
            {
                row = retVal.NewRow();
                row["To"] = DBNull.Value;
                row["Subject"] = Subject;

                retVal.Rows.Add(row);
            }
            else
            {

                DataTable data = GetData(tran, this.SubjectDataCommand,
                                    WorkflowCode, FromWorkflowStepCode, ToWorkflowStepCode, EntityID, Comment, UserName,
                                    SubjectWorkflowCodeParameter,
                                    SubjectFromWorkflowStepCodeParameter,
                                    SubjectToWorkflowStepCodeParameter,
                                    SubjectEntityIDParameter,
                                    SubjectCommentParameter,
                                    SubjectUsernameParameter);



                foreach (DataRow record in data.Rows)
                {
                    row = retVal.NewRow();
                    row["To"] = record[this.SubjectToAddressField];
                    row["Subject"] = record[this.SubjectField];

                    retVal.Rows.Add(row);
                }
            }

            return retVal;
        }

        private DataTable GetAttachments(System.Data.Common.DbTransaction tran, string WorkflowCode, string FromWorkflowStepCode, string ToWorkflowStepCode, string EntityID, string Comment, string UserName)
        {
            DataTable retVal = new DataTable();
            DataRow row = null;
            retVal.Columns.Add("To", typeof(string));
            retVal.Columns.Add("DocumentID", typeof(string));



            if (!String.IsNullOrEmpty(this.AttachmentDataCommand))
            {

                DataTable data = GetData(tran, this.AttachmentDataCommand,
                                    WorkflowCode, FromWorkflowStepCode, ToWorkflowStepCode, EntityID, Comment, UserName,
                                    AttachmentWorkflowCodeParameter,
                                    AttachmentFromWorkflowStepCodeParameter,
                                    AttachmentToWorkflowStepCodeParameter,
                                    AttachmentEntityIDParameter,
                                    AttachmentCommentParameter,
                                    AttachmentUsernameParameter);



                foreach (DataRow record in data.Rows)
                {
                    row = retVal.NewRow();
                    row["To"] = record[this.AttachmentToAddressField];
                    row["DocumentID"] = record[this.AttachmentDocumentIDField];

                    retVal.Rows.Add(row);
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
           // CommandType type = (command.Type == DataCommandCommandType.StoredProcedure) ? CommandType.StoredProcedure : CommandType.Text;
            DataTable data = dataCommandDB.GetData(tran, command, parameters,  command.Text);
            return data;
        }
        
    }

    

    

    public enum EmailAddressSendMode
    {
        Individual,
        Group
    }

    
}
