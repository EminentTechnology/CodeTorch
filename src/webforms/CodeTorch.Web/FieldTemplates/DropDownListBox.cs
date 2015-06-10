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
    

    public class DropDownListBox : BaseFieldTemplate
    {
        protected Telerik.Web.UI.RadComboBox ctrl;

        protected override void CreateChildControls()
        {
            ctrl = new Telerik.Web.UI.RadComboBox();
            ctrl.ID = "ctrl";
            Controls.Add(ctrl);
        }
       

        DataCommandService dataCommand = DataCommandService.GetInstance();
        PageDB pageDB = new PageDB();

        DropDownListControl _Me = null;
        public DropDownListControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (DropDownListControl)this.Widget;
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

        public string QueryText
        {
            get
            {
                return ViewState["QueryText"] == null ? String.Empty : ViewState["QueryText"].ToString();
            }
            set
            {
                ViewState["QueryText"] = value;
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

        bool IncludeAdditionalListItem
        {
            get
            {
                return Me.IncludeAdditionalListItem;
            }
        }

        string SelectDataCommand
        {
            get
            {
                return Me.SelectDataCommand;
            }
        }

        string RelatedControl
        {
            get
            {
                return (Me.RelatedControl == null) ? string.Empty : Me.RelatedControl; 
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
                if (!String.IsNullOrEmpty(Me.EmptyMessage))
                {
                    ctrl.EmptyMessage = GetGlobalResourceString("EmptyMessage", Me.EmptyMessage);
                }

                ctrl.EnableLoadOnDemand = Me.EnableLoadOnDemand;
                ctrl.DropDownWidth = new Unit(Me.DropDownWidth);
                ctrl.EnableItemCaching = Me.EnableItemCaching;
                ctrl.MarkFirstMatch = Me.MarkFirstMatch;
                ctrl.Filter = (RadComboBoxFilter)Enum.Parse(typeof(RadComboBoxFilter), Me.FilterMode.ToString());
                ctrl.IsCaseSensitive = Me.IsCaseSensitive;
                ctrl.RenderingMode = (RadComboBoxRenderingMode)Enum.Parse(typeof(RadComboBoxRenderingMode), Me.RenderingMode.ToString());
                ctrl.ShowToggleImage = Me.ShowToggleImage;
                ctrl.CausesValidation = Me.CausesValidation;
                

                if (!String.IsNullOrEmpty(Me.Width))
                {
                    ctrl.Width = new Unit(Me.Width);
                }

                ctrl.CssClass = "form-control";
                if (!String.IsNullOrEmpty(Me.CssClass))
                {
                    ctrl.CssClass += " " + Me.CssClass;
                }

                if (!String.IsNullOrEmpty(Me.SkinID))
                {
                    ctrl.SkinID = Me.SkinID;
                }

                if (!String.IsNullOrEmpty(Me.Skin))
                {
                    ctrl.Skin = Me.Skin;
                }

                ctrl.ItemsRequested += new RadComboBoxItemsRequestedEventHandler(ctrl_ItemsRequested);
                ctrl.SelectedIndexChanged += ctrl_SelectedIndexChanged;

            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID, Me.Type, this.ID);

                this.ctrl.Text = ErrorMessages;
                this.ctrl.BackColor = Color.Red;

            }

            
        }

        void ctrl_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (Me.SelectedIndexChanged != null)
                {
                    ActionRunner runner = new ActionRunner();

                    Core.Action action = ObjectCopier.Clone<Core.Action>(Me.SelectedIndexChanged);

                    runner.Page = (BasePage)this.Page;
                    runner.Action = action;

                    runner.Execute();

                }

               

            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID,  Me.Type, this.ID);

                this.ctrl.Items.Insert(0, new RadComboBoxItem(ErrorMessages, String.Empty));
                this.ctrl.BackColor = Color.Red;

            }
        }

        void ctrl_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            //int itemOffset = e.NumberOfItems;
            this.QueryText = e.Text;

            DataTable data = GetData();

            ctrl.Items.Clear();

            foreach (DataRow row in data.Rows)
            {
                ctrl.Items.Add(new RadComboBoxItem(row[Me.DataTextField].ToString(), row[Me.DataValueField].ToString()));
          
            }

            SetupAdditionalListItem();
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
                    if(FirstLoad || isRelatedControl() )
                    {
                        FillList();
                    }
                }

            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID,  Me.Type, this.ID);

                this.ctrl.Items.Insert(0, new RadComboBoxItem(ErrorMessages, String.Empty));
                this.ctrl.BackColor = Color.Red;

            }
        }

        public override void Refresh()
        {

            try
            {
                FillList();

            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID,  Me.Type, this.ID);

                this.ctrl.Items.Insert(0, new RadComboBoxItem(ErrorMessages, String.Empty));
                this.ctrl.BackColor = Color.Red;

            }
        }

        private bool isRelatedControl()
        {
            bool retVal = false;

            string eventTarget = String.Format("{0}$", HttpContext.Current.Request["__EVENTTARGET"]);
            if (eventTarget.IndexOf(String.Format("${0}$", RelatedControl)) > 0)
            {
                retVal = true;
            }

            return retVal;
        }

        private void FillList()
        {
            FirstLoad = false;
            ctrl.AutoPostBack = AutoPostBack;
            ctrl.DataTextField = Me.DataTextField;
            ctrl.DataValueField = Me.DataValueField;

            this.ctrl.DataSource = GetData();
            this.ctrl.DataBind();

            SetupAdditionalListItem();

            //to support criteria list edit - add mode
            ctrl.DataSource = null;

        }

        private void SetupAdditionalListItem()
        {
            
                if (IncludeAdditionalListItem)
                {
                    string AdditionalListItemText = GetGlobalResourceString("AdditionalListItemText", Me.AdditionalListItemText);
                    string AdditionalListitemValue = Me.AdditionalListItemValue;

                    if (String.IsNullOrEmpty(AdditionalListitemValue))
                    {
                        AdditionalListitemValue = String.Empty;
                    }

                    ctrl.Items.Insert(0, new RadComboBoxItem(AdditionalListItemText, AdditionalListitemValue));
                    //ctrl.EmptyMessage = AdditionalListItemText;
                }
            
        }

        private DataTable GetData()
        {

            if (String.IsNullOrEmpty(SelectDataCommand))
            {
                throw new ApplicationException("SelectDataCommand is invalid");
            }

            List<ScreenDataCommandParameter> parameters = pageDB.GetPopulatedCommandParameters(SelectDataCommand, ((CodeTorch.Web.Templates.BasePage)this.Page));

            PopulateScreenParameterWithControlValue(parameters);
            PopulateScreenParameterWithControlText(parameters);
            PopulateScreenParameterWithRelatedControlValue(parameters);

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

        private void PopulateScreenParameterWithControlText(List<ScreenDataCommandParameter> parameters)
        {
            List<ScreenDataCommandParameter> ctrlValueParameters = parameters.Where(p =>
                       (
                           (p.InputType == ScreenInputType.Special) &&
                           (p.InputKey.ToLower().Trim() == "controltext")
                       )
                   ).ToList<ScreenDataCommandParameter>();

            foreach (ScreenDataCommandParameter p in ctrlValueParameters)
            {
                if (Me.IncludeAdditionalListItem)
                {
                    if (this.QueryText != Me.AdditionalListItemText)
                    {

                        p.Value = this.QueryText;
                    }
                }
                else
                {
                    p.Value = QueryText;
                }
            }
        }

        private void PopulateScreenParameterWithRelatedControlValue(List<ScreenDataCommandParameter> parameters)
        {
            if (!String.IsNullOrEmpty(this.RelatedControl))
            {
                List<ScreenDataCommandParameter> ctrlValueParameters = parameters.Where(p =>
                           (
                               (p.InputType == ScreenInputType.Special) &&
                               (p.InputKey.ToLower().Trim() == "relatedcontrolvalue")
                           )
                       ).ToList<ScreenDataCommandParameter>();

                foreach (ScreenDataCommandParameter p in ctrlValueParameters)
                {

                    CodeTorch.Web.FieldTemplates.BaseFieldTemplate f = ((BasePage)this.Page).FindFieldRecursive(this.RelatedControl);
                    if (f != null)
                    {
                        p.Value = f.Value;
                    }
                }

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
