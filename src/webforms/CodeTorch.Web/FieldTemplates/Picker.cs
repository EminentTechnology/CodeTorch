using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeTorch.Core;
using CodeTorch.Core.Services;
using System.Web.UI;

namespace CodeTorch.Web.FieldTemplates
{
    public class Picker : BaseFieldTemplate, IPostBackEventHandler
    {

        protected System.Web.UI.HtmlControls.HtmlInputHidden ctrlvalue;
        protected System.Web.UI.HtmlControls.HtmlInputText ctrl;
        protected System.Web.UI.HtmlControls.HtmlButton PickerButton;
        protected System.Web.UI.WebControls.Button ResetButton;
        protected System.Web.UI.HtmlControls.HtmlGenericControl inputGroup;
        protected System.Web.UI.HtmlControls.HtmlGenericControl inputGroupButton;


        protected override void CreateChildControls()
        {
            inputGroup = new System.Web.UI.HtmlControls.HtmlGenericControl("div");
            inputGroup.Attributes.Add("class", "input-group");
            Controls.Add(inputGroup);


                ctrl = new System.Web.UI.HtmlControls.HtmlInputText();
                ctrl.ID = "ctrl";
                ctrl.Attributes.Add("class", "form-control");
                ctrl.Attributes.Add("disabled", "disabled");
                inputGroup.Controls.Add(ctrl);

                inputGroupButton = new System.Web.UI.HtmlControls.HtmlGenericControl();
                inputGroupButton.Attributes.Add("class", "input-group-btn");
                inputGroup.Controls.Add(inputGroupButton);

                PickerButton = new System.Web.UI.HtmlControls.HtmlButton();
                PickerButton.ID = "PickerButton";
                PickerButton.Attributes.Add("class", "btn btn-default");
                PickerButton.InnerText = "...";
                inputGroupButton.Controls.Add(PickerButton);

                ResetButton = new System.Web.UI.WebControls.Button();
                ResetButton.ID = "ResetButton";
                ResetButton.Attributes.Add("class", "btn btn-default");
                ResetButton.Text = "Reset";
                inputGroupButton.Controls.Add(ResetButton);


                ctrlvalue = new System.Web.UI.HtmlControls.HtmlInputHidden();
                ctrlvalue.ID = "ctrlvalue";
                ctrlvalue.Name = "ctrlvalue";
                inputGroup.Controls.Add(ctrlvalue);
   
        }

        protected string PickerFunction = string.Empty;

        PickerControl _Me = null;
        public PickerControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (PickerControl)this.BaseControl;
                }
                return _Me;
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
                ctrlvalue.Value = value;
                ctrl.Value = GetDatabaseDisplayValue(value);
               

            }
        }

        public bool AutoPostBack
        {
            get 
            {
                if (ViewState["AutoPostBack"] == null)
                {
                    return Me.AutoPostBack;
                }
                else
                {
                    return Convert.ToBoolean(ViewState["AutoPostBack"]);
                }
            }
            set 
            {
                ViewState["AutoPostBack"] = value;
            }
        }

        public bool Modal
        {
            get
            {
                if (ViewState["Modal"] == null)
                {
                    return Me.Modal;
                }
                else
                {
                    return Convert.ToBoolean(ViewState["Modal"]);
                }
            }
            set
            {
                ViewState["Modal"] = value;
            }
        }

        public string Tag
        {
            get
            {
                if (ViewState["Tag"] == null)
                {
                    return null;
                }
                else
                {
                    return ViewState["Tag"].ToString();
                }
            }
            set
            {
                ViewState["Tag"] = value;
            }
        }

        public event EventHandler OnClose;

        private string GetDatabaseDisplayValue(string value)
        {
            string retVal = null;
            DataCommandService dataCommandDB = DataCommandService.GetInstance();

            if (String.IsNullOrEmpty(Me.PickerObject.DataCommand))
            {
                throw new ApplicationException("Picker does not have a datacommand specified in configuration");
            }

            DataCommand command = DataCommand.GetDataCommand(Me.PickerObject.DataCommand);

            if (command == null)
            {
                throw new ApplicationException(String.Format("DataCommand {0} does not exist in configuration", Me.PickerObject.DataCommand));
            }

            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter parameter = new ScreenDataCommandParameter();

            parameter.Name = command.Parameters[0].Name;
            parameter.Value = value;

            parameters.Add(parameter);

            DataTable dt = dataCommandDB.GetDataForDataCommand(command.Name, parameters);

            if (dt.Rows.Count > 0)
            {
                retVal = dt.Rows[0][Me.PickerObject.DisplayField].ToString();

            }

            return retVal;
        }

        public override string DisplayText
        {
            get
            {
                return ctrl.Value;
            }
            
          
        }

        public string PickerType
        {
            get 
            {

                return ViewState["PickerType"].ToString();
            }
            set 
            {
                ViewState["PickerType"] = value;
            }
        }

        bool _InCustomPage = false;
        public bool InCustomPage
        {
            get 
            {
                return _InCustomPage;
            }
            set 
            {
                _InCustomPage = value;
            }
        }

        public override void InitControl(object sender, EventArgs e)
        {
            base.InitControl(sender, e);

            try
            {
                if (InCustomPage)
                {
                    PickerControl ctrl = new PickerControl();
                    ctrl.Picker = PickerType;
                    _Me = ctrl;
                }


                if (!String.IsNullOrEmpty(Me.Width))
                {
                    ctrl.Style.Add("width", Me.Width); 
                }

                if (!String.IsNullOrEmpty(Me.CssClass))
                {
                    ctrl.Attributes.Add("class", Me.CssClass);
                }

                if (!String.IsNullOrEmpty(Me.SkinID))
                {
                    ctrl.SkinID = Me.SkinID;
                }

                
                ResetButton.Text = GetGlobalResourceString("ResetButton.Label", "Reset");

                
            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID,  Me.Type, this.ID);

                this.ctrl.Value = ErrorMessages;
                this.ctrl.Style.Add("background-color", "red");

            }
        }

        public override void LoadControl(object sender, EventArgs e)
        {
            string newPickerType = string.Empty;
            try
            {

                

              
                this.ResetButton.Click += new EventHandler(ResetButton_Click);
                PickerFunction = string.Format("{0}Picker{1}", Me.Picker, this.ClientID);
                RegisterClientScript();
                string displayValue = GetDatabaseDisplayValue(Value);

                if (!String.IsNullOrEmpty(displayValue))
                {
                    ctrl.Value = displayValue;
                }
                else
                {
                    ctrlvalue.Value = string.Empty;
                }
            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "<span style='color:red'>ERROR - {0} - Picker {1} ({2} - {3})</span>";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID,  Me.Type, this.ID);

                this.ctrl.Value = ErrorMessages;

                this.Controls.Clear();
                this.Controls.Add(new LiteralControl(ErrorMessages));
               

            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            Value = string.Empty;
            ctrl.Value = string.Empty;
        }

        private void RegisterClientScript()
        {

            StringBuilder javascript = new StringBuilder();

            string PickerPage = string.Format("{0}?field={1}_ctrl&fieldValue={1}_ctrlvalue", Me.PickerObject.Url, this.ClientID.Trim());

            javascript.AppendLine("<script language=\"javascript\">");
            javascript.AppendLine("function " + PickerFunction + "(field) ");
            javascript.AppendLine("{\n");
            javascript.AppendLine("     $.fancybox.open({");
            javascript.AppendLine("         type: 'iframe',");
            javascript.AppendLine("         href: '" + PickerPage + "',");
            javascript.AppendLine("         height: " + Me.PickerObject.Height + ",");
            javascript.AppendLine("         width: " + Me.PickerObject.Width + ",");
           // javascript.AppendLine("         options: {");

            if (this.AutoPostBack)
            {
                javascript.AppendFormat("             afterClose:   function(){{  {0}; }},",

                    Page.ClientScript.GetPostBackEventReference(this, "OnClose")
                    );
            }
            javascript.AppendLine();
            javascript.AppendLine("             modal: " + this.Modal.ToString().ToLower());
            //javascript.AppendLine("             }");
            javascript.AppendLine("         });");
            javascript.AppendLine("}");
            javascript.AppendLine("</script>");

            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), PickerFunction, javascript.ToString());
            

            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            if ((scriptManager != null) && (scriptManager.IsInAsyncPostBack))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), PickerFunction, javascript.ToString(), false);
            }
            else
            {
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), PickerFunction, javascript.ToString(), false);
            }


            //for now returning false to prevent modal from submitting inside a form
            //http://stackoverflow.com/questions/932653/how-to-prevent-buttons-from-submitting-forms
            this.PickerButton.Attributes.Add("onclick", PickerFunction + "(" + this.ClientID.Trim() + "_ctrl);return false;");


        }









        public void RaisePostBackEvent(string eventArgument)
        {

            if (OnClose != null)
            {
                OnClose(this, EventArgs.Empty);
            }
        }
    }
}
