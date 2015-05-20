using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using CodeTorch.Core;
using System.Web.UI.WebControls;
using CodeTorch.Core.Services;

namespace CodeTorch.Web.Special
{
    public class WorkflowStatusPicker: Page
    {
        protected DropDownList StatusList;
        protected Label CurrentStatusLabel;
        protected Label Message;
        protected System.Web.UI.WebControls.Button Save;
        protected TextBox Comments;
        protected Literal FieldName;

        public string WorkflowCode
        {
            get { return ViewState["WorkflowCode"].ToString(); }
            set { ViewState["WorkflowCode"] = value; }
        }


        public string EntityID
        {
            get { return ViewState["EntityID"].ToString(); }
            set { ViewState["EntityID"] = value; }
        }

        public bool ReloadAfterStatusChange
        {
            get { return (ViewState["ReloadAfterStatusChange"]) == null ? true : Convert.ToBoolean(ViewState["ReloadAfterStatusChange"]); }
            set { ViewState["ReloadAfterStatusChange"] = value; }
        }

        WorkflowService workflowService = WorkflowService.GetInstance();
        

        CodeTorch.Core.Workflow workflow = null;
        WorkflowStep step = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.Save.Click += new EventHandler(Save_Click);
        }

        

        protected void Page_Load(object sender, EventArgs e)
        {

            WorkflowCode = Request.QueryString["WorkflowCode"];
            EntityID = Request.QueryString["EntityID"];
            FieldName.Text = String.Format("'{0}'", Request.QueryString["FieldName"]);
            ReloadAfterStatusChange = Convert.ToBoolean(Request.QueryString["Reload"]);

            // first determine current status of workflow item
            workflow = Workflow.GetWorkflowByCode(WorkflowCode);
            step = workflowService.GetProvider(workflow).GetCurrentWorkflowStep(workflow, EntityID);
            
            if (workflow != null)
            {
                if (!IsPostBack)
                {

                    if (step != null)
                    {
                        CurrentStatusLabel.Text = step.Name;
                    }
                    else
                    {
                        CurrentStatusLabel.Text = "None";
                    }

                    FillStatusList(workflow);
                }
            }
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            //perform validation
            try
            {
                if (String.IsNullOrEmpty(StatusList.SelectedValue))
                {
                    throw new ApplicationException("Status is required");
                }

                WorkflowNextStep nextstep = 
                    step.PossibleNextSteps
                    .Where(s =>
                        (
                            (s.Code.ToLower() == StatusList.SelectedValue.ToLower())
                        )
                    )
                    .SingleOrDefault();

                if (nextstep != null)
                {
                    if (nextstep.RequireComment)
                    {
                        if (String.IsNullOrEmpty(Comments.Text))
                        {
                            throw new ApplicationException("Comments are required to change to this status");
                        }
                    }

                    //all is well execute workflow

                    bool Success = WorkflowService.GetInstance().GetProvider(workflow).ChangeWorkflowStep(workflow, nextstep, EntityID, Comments.Text);

                    if (Success)
                    {
                        this.ClientScript.RegisterStartupScript(this.GetType(), "OnLoad", BuildCloseWindowJavascript(nextstep.Name));

                    }
                }

            }
            catch (Exception ex)
            {
                string MessageFormat = "The following error(s) occurred:<br/>{0}";
                Message.Visible = true;
                Message.Text = String.Format(MessageFormat, ex.Message);
                Message.CssClass = "ErrorMessages";

            }
        }

        private void FillStatusList(CodeTorch.Core.Workflow workflow)
        {

            if (step != null)
            {
                StatusList.Items.Clear();
                StatusList.Items.Insert(0, new ListItem("-- Select --", String.Empty));

                List<WorkflowNextStep> possibleSteps = Workflow.GetPossibleNextSteps(workflow, step, EntityID, Common.UserName);
                foreach (WorkflowNextStep possibleStep in possibleSteps)
                {
                    StatusList.Items.Add(new ListItem(possibleStep.Name, possibleStep.Code));
                }

            }
            else
            {
                //default to new workflow step (ie the first workflow step only)
                if(workflow.Steps.Count > 0)
                {
                    StatusList.Items.Clear();
                    StatusList.Items.Insert(0, new ListItem(workflow.Steps[0].Name, workflow.Steps[0].Code));
                }
                else
                {
                    StatusList.Items.Clear();
                    StatusList.Items.Insert(0, new ListItem("No Worflow Steps Configured", String.Empty));
                }

            }
            
        }

        private string BuildCloseWindowJavascript(string StatusName)
        {
            StringBuilder javascript = new StringBuilder();
            javascript.AppendLine("<script language=\"javascript\">");
            javascript.AppendLine("CloseWindowWithStatusUpdate('" + StatusName + "','" + ReloadAfterStatusChange.ToString()  + "');");
            javascript.AppendLine("</script>");
            return javascript.ToString();
        }
    }
}
