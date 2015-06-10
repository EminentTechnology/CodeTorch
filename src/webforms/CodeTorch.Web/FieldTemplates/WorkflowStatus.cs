using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using CodeTorch.Web.Data;
using CodeTorch.Core;
using System.Web.UI.HtmlControls;
using System.Drawing;
using CodeTorch.Web.Templates;
using CodeTorch.Core.Services;

namespace CodeTorch.Web.FieldTemplates
{
    public class WorkflowStatus : BaseFieldTemplate
    {
        protected HtmlInputHidden ctrlvalue;
        protected System.Web.UI.HtmlControls.HtmlGenericControl ctrl;
        protected HtmlInputButton ChangeStatus;



        protected override void CreateChildControls()
        {
            ctrlvalue = new HtmlInputHidden();
            ctrlvalue.ID = "ctrlvalue";
            Controls.Add(ctrlvalue);

            ctrl = new System.Web.UI.HtmlControls.HtmlGenericControl();
            ctrl.ID = "ctrl";
            Controls.Add(ctrl);

            ChangeStatus = new HtmlInputButton();
            ChangeStatus.ID = "ChangeStatus";
            Controls.Add(ChangeStatus);
        }

        WorkflowService workflowService = WorkflowService.GetInstance();

        CodeTorch.Core.Workflow workflow = null;
        WorkflowStep step = null;

        DataCommandService dataCommand = DataCommandService.GetInstance();
        PageDB pageDB = new PageDB();

        WorkflowStatusControl _Me = null;
        public WorkflowStatusControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (WorkflowStatusControl)this.Widget;
                }
                return _Me;
            }
        }

        public string EntityIDValue
        {
            get
            {
                return (ViewState["EntityIDValue"] == null) ? String.Empty : ViewState["EntityIDValue"].ToString();
            }
            set
            {
                ViewState["EntityIDValue"] = value;
            }
        }

        public override string Value
        {
            get
            {
                return ctrlvalue.Value;
            }
            set
            {

                try
                {
                    ctrlvalue.Value = value;
                    ctrl.InnerHtml = GetStatusText(value);

                    if (!String.IsNullOrEmpty(Me.CssClass))
                    {
                        ctrl.Attributes.Add("class", Me.CssClass);
                    }
          
                    //if (!String.IsNullOrEmpty(Me.CssClass))
                    //{
                    //    ctrl.Attributes.Add("class", "form-control " + Me.CssClass);
                    //}
                    //else
                    //{
                    //    ctrl.Attributes.Add("class", "form-control");
                    //}

                    if (!String.IsNullOrEmpty(Me.SkinID))
                    {
                        ctrl.SkinID = Me.SkinID;
                    }

                    

                }
                catch (Exception ex)
                {
                    string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3}) - Value";
                    string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID,  Me.Type, this.ID);

                    this.ctrl.InnerHtml = ErrorMessages;
                    this.ctrl.Style.Add( HtmlTextWriterStyle.BackgroundColor, "Color.Red");

                }

                
            }
        }


        public override void InitControl(object sender, EventArgs e)
        {
            try
            {
                base.InitControl(sender, e);

                if (workflow == null)
                {
                    workflow = GetWorkflow();
                }

                if (!String.IsNullOrEmpty(Me.Width))
                {
                    
                    this.ctrl.Style.Add(HtmlTextWriterStyle.Width, Me.Width);
                }

                ChangeStatus.Attributes.Add("class", "WorkflowChangeStatusButton");
                ChangeStatus.Value = GetGlobalResourceString("WorkflowChangeStatusButton.Label", "Change Status");

            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3}) - InitControl";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID,  Me.Type, this.ID);

                this.ctrl.InnerHtml = ErrorMessages;
                this.ctrl.Style.Add(HtmlTextWriterStyle.BackgroundColor, "Color.Red");

            }
        }
       

        public override void LoadControl(object sender, EventArgs e)
        {
            try
            {
                base.LoadControl(sender, e);

                SetupChangeStatusButton();

            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3}) - LoadControl";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID,  Me.Type, this.ID);

                this.ctrl.InnerHtml = ErrorMessages;
                this.ctrl.Style.Add(HtmlTextWriterStyle.BackgroundColor, "Color.Red");

            }

            
        }

        private void SetupChangeStatusButton()
        {
            bool CanChangeStatus = false;

            if (step == null)
            {
                GetCurrentStep();
            }

            if (step != null)
            {
                //check to see if current user is allowed to change the status from current status to another status
                CanChangeStatus = Workflow.CanUserChangeStep(workflow, step, EntityIDValue, Common.UserName);
            }


            if (CanChangeStatus)
            {
                ChangeStatus.Visible = true;
                RegisterClientScript();
            }
            else
            {
                ChangeStatus.Visible = false;
            }

        }

        private void RegisterClientScript()
        {

            string FunctionName = String.Format("{0}_ChangeWorkflowStatus", Me.Name);
            string Url = ResolveClientUrl("~/Templates/Pages/WorkflowStatusPicker.aspx");
            string FieldName = String.Format("{0}_ctrl",this.ClientID.Trim());
            string UrlFormat = String.Format("{0}?WorkflowCode={1}&EntityID={2}&FieldName={3}&Reload={4}", Url, workflow.Code, EntityIDValue, FieldName, Me.ReloadAfterStatusChange);

            string javascript = BuildPopupWindowClientScript(FunctionName, UrlFormat, 310, 600);

            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), FunctionName, javascript.ToString());
            this.ChangeStatus.Attributes.Add("onclick", FunctionName + "(" + this.ClientID.Trim() + "_ctrlvalue)");

        }

        private CodeTorch.Core.Workflow GetWorkflow()
        {

            CodeTorch.Core.Workflow workflow = null;
            string workflowCode = null;

            switch (Me.WorkflowSelectionMode)
            { 
                case WorkflowSelectionMode.Static:
                    if (String.IsNullOrEmpty(Me.WorkflowCode))
                    {
                        throw new ApplicationException("Invalid Workflow Code");
                    }

                    workflowCode = Me.WorkflowCode;

                    
                    break;
                case WorkflowSelectionMode.DataCommand:
                    //call screen datacommand
                    if (String.IsNullOrEmpty(Me.WorkflowSelectionCommandName) || String.IsNullOrEmpty(Me.WorkflowSelectionFieldName))
                    {
                        throw new ApplicationException("Invalid WorkflowSelectionCommandName OR WorkflowSelectionFieldName");
                    }
                    
                    List<ScreenDataCommandParameter> parameters = pageDB.GetPopulatedCommandParameters(Me.WorkflowSelectionCommandName, ((CodeTorch.Web.Templates.BasePage)this.Page));
                    DataTable data =  dataCommand.GetDataForDataCommand(Me.WorkflowSelectionCommandName, parameters);
                    if (data.Rows.Count > 0)
                    {
                        workflowCode = data.Rows[0][Me.WorkflowSelectionFieldName].ToString();
                    }
                    break;

            }

            if (!String.IsNullOrEmpty(workflowCode))
            {
                workflow = CodeTorch.Core.Workflow.GetWorkflowByCode(workflowCode);
            }

            return workflow;
        }

        

        private string GetStatusText(string value)
        {
            string retVal = null;

            if (step == null)
            { 
                GetCurrentStep();
            }

            if (step != null)
            {
                retVal = step.Name;
            }

            return retVal;
        }

        private void GetCurrentStep()
        {

            BasePage p = ((BasePage)this.Page);

            if (workflow == null)
                workflow = GetWorkflow();


            EntityIDValue = p.GetEntityIDValue(p.Screen, Me.EntityID, Me.EntityInputType);

            step = workflowService.GetProvider(workflow).GetCurrentWorkflowStep(workflow, EntityIDValue);
        }

        private static string BuildPopupWindowClientScript(string FunctionName, string Url)
        {
            return BuildPopupWindowClientScript(FunctionName, Url, 600, 600);
        }

        private static string BuildPopupWindowClientScript(string FunctionName, string Url, int Height, int Width)
        {
            StringBuilder javascript = new StringBuilder();
            javascript.AppendLine("<script language=\"javascript\">");
            javascript.AppendLine("function " + FunctionName + "(field) ");
            javascript.AppendLine("{\n");
            javascript.AppendLine("$.fancybox.open({");
            javascript.AppendLine("type: 'iframe',");
            javascript.AppendLine("href: '" + Url + "',");
            javascript.AppendFormat("height: {0},", Height);
            javascript.AppendLine();
            javascript.AppendFormat("width: {0}", Width);
            javascript.AppendLine();
            javascript.AppendLine("});");
            javascript.AppendLine("");
            javascript.AppendLine("}");
            javascript.AppendLine("</script>");
            return javascript.ToString();
        }

        public override bool SupportsValidation()
        {
            return false;
        }
    }
}
