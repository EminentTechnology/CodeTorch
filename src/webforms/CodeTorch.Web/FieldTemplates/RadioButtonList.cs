using System;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using CodeTorch.Core.Services;
using CodeTorch.Web.Data;
using CodeTorch.Web.Templates;
using CodeTorch.Core;
using Telerik.Web.UI;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web;

namespace CodeTorch.Web.FieldTemplates
{
    public class RadioButtonList : BaseFieldTemplate
    {
        protected System.Web.UI.WebControls.RadioButtonList ctrl;

        protected override void CreateChildControls()
        {
            ctrl = new System.Web.UI.WebControls.RadioButtonList();
            ctrl.ID = "ctrl";
            Controls.Add(ctrl);
        }


        DataCommandService dataCommand = DataCommandService.GetInstance();
        PageDB pageDB = new PageDB();
        RadioButtonListControl _Me = null;

        public RadioButtonListControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (RadioButtonListControl)this.Widget;
                }
                return _Me;
            }
        }

        public bool FirstLoad
        {
            get
            {
                return ViewState["FirstLoad"] == null ? true : Convert.ToBoolean(ViewState["FirstLoad"]);
            }
            set
            {
                ViewState["FirstLoad"] = value;
            }
        }

        public override string Value
        {
            get
            {
                return ctrl.SelectedValue;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && value != "00000000-0000-0000-0000-000000000000")
                    ctrl.SelectedValue = value;

            }
        }

        public override string DisplayText
        {
            get
            {
                string retVal = ctrl.Text;

                if (String.IsNullOrEmpty(retVal))
                {
                    retVal = ctrl.SelectedValue;
                }

                return retVal;
            }

        }



        string SelectDataCommand
        {
            get
            {
                return Me.SelectDataCommand;
            }
        }

        bool AutoPostBack
        {
            get
            {
                return Me.AutoPostBack;
            }
        }


        public override void InitControl(object sender, EventArgs e)
        {
            base.InitControl(sender, e);

            try
            {


                ctrl.RepeatDirection = (RepeatDirection)Enum.Parse(typeof(RepeatDirection), Me.RepeatDirection.ToString());

                if (!String.IsNullOrEmpty(Me.Width))
                {
                    ctrl.Width = new Unit(Me.Width);
                }

                //ctrl.CssClass = "form-control";
                if (!String.IsNullOrEmpty(Me.CssClass))
                {
                    ctrl.CssClass += " " + Me.CssClass;
                }

                if (!String.IsNullOrEmpty(Me.SkinID))
                {
                    ctrl.SkinID = Me.SkinID;
                }





            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID, Me.Type, this.ID);

                this.ctrl.Text = ErrorMessages;
                this.ctrl.BackColor = Color.Red;

            }

        }

        public override void LoadControl(object sender, EventArgs e)
        {

            try
            {
                if (!this.Page.IsPostBack)
                {
                    FillList();

                }
                else
                {
                    if (FirstLoad)
                    {
                        FillList();
                    }
                }

            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID, Me.Type, this.ID);
                this.ctrl.Items.Insert(0, new System.Web.UI.WebControls.ListItem(ErrorMessages, String.Empty));
                this.ctrl.BackColor = Color.Red;

            }
        }

        private void FillList()
        {
            FirstLoad = false;
            ctrl.AutoPostBack = AutoPostBack;
            ctrl.DataTextField = Me.DataTextField;
            ctrl.DataValueField = Me.DataValueField;

            this.ctrl.DataSource = GetData();
            this.ctrl.DataBind();

           

            //to support criteria list edit - add mode
            ctrl.DataSource = null;

        }


        private DataTable GetData()
        {

            if (String.IsNullOrEmpty(SelectDataCommand))
            {
                throw new ApplicationException("SelectDataCommand is invalid");
            }

            List<ScreenDataCommandParameter> parameters = pageDB.GetPopulatedCommandParameters(SelectDataCommand, ((CodeTorch.Web.Templates.BasePage)this.Page));

            PopulateScreenParameterWithControlValue(parameters);
            

            return dataCommand.GetDataForDataCommand(SelectDataCommand, parameters);



        }

        private void PopulateScreenParameterWithControlValue(List<ScreenDataCommandParameter> parameters)
        {
            List<ScreenDataCommandParameter> ctrlValueParameters = parameters.Where(p =>
                       (
                           (p.InputType == ScreenInputType.Special) &&
                           (p.InputKey.ToLower().Trim() == "controlvalue")
                       )
                   ).ToList<ScreenDataCommandParameter>();

            foreach (ScreenDataCommandParameter p in ctrlValueParameters)
            {
                p.Value = this.Value;
            }
        }

        public override string GetValidationControlIDSuffix()
        {
            return "$ctrl";
        }

        public override System.Web.UI.WebControls.BaseValidator GetRequiredValidator(Widget control, bool IsControlEditable, string requiredErrorMessage)
        {
            System.Web.UI.WebControls.CustomValidator val = new CustomValidator();

            val.ID = "RequiredFieldValidator" + control.Name;
            val.Display = ValidatorDisplay.None;

            val.EnableClientScript = true;
            val.ServerValidate += val_ServerValidate;
            val.ValidateEmptyText = true;
            val.ClientValidationFunction = "validateComboValue";

            System.Web.UI.Page p = (System.Web.UI.Page)System.Web.HttpContext.Current.Handler;
            DropDownListBox.RegisterComboRequiredFieldValidatorClientScript(p);

            val.ErrorMessage = requiredErrorMessage;
            val.Text = "*";
            val.ControlToValidate = control.Name + this.GetValidationControlIDSuffix();
            val.Enabled = (IsControlEditable && control.IsRequired);
            return val;

        }

        public static void RegisterComboRequiredFieldValidatorClientScript(System.Web.UI.Page page)
        {

            String scriptString = @"<script language=javascript>
            function validateComboValue(source, args) 
            {
                var retVal = false; 
                var combo = $find(source.controltovalidate);
                var v = combo.get_value();
                retVal = (v.length > 0);
                args.IsValid = retVal;
              }
            </script>";


            page.ClientScript.RegisterClientScriptBlock(page.GetType(), "validateComboValue", scriptString);
        }

        void val_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool retVal = false;
            string selectedValue = ctrl.SelectedValue;
            if (!String.IsNullOrEmpty(selectedValue))
            {
                retVal = true;
            }
            else
            {
                retVal = false;
            }

            args.IsValid = retVal;
        }


    }
}
