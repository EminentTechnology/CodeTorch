using System;
using System.Drawing;
using System.Linq;
using System.Web.UI.WebControls;
using CodeTorch.Core;
using Telerik.Web.UI;
using CodeTorch.Web.Templates;
using System.Web;
using CodeTorch.Core.Services;

namespace CodeTorch.Web.FieldTemplates
{
    public class DropDownListBox_Lookup : BaseFieldTemplate
    {
        protected Telerik.Web.UI.RadComboBox ctrl;

        protected override void CreateChildControls()
        {
            ctrl = new Telerik.Web.UI.RadComboBox();
            ctrl.ID = "ctrl";
            Controls.Add(ctrl);
        }

        LookupDropDownListControl _Me = null;
        public LookupDropDownListControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (LookupDropDownListControl)this.BaseControl;
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
                return ViewState["QueryText"] == null ? null : ViewState["QueryText"].ToString();
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
                if (value != string.Empty && value != null)
                ctrl.SelectedValue = value;
       
            }
        }

        public override string DisplayText
        {
            get
            {
                return ctrl.Text;
            }

        }

        bool IncludeAdditionalListItem
        {
            get
            {
                return Me.IncludeAdditionalListItem;
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
                ctrl.DataTextField = "Description";
                ctrl.DataValueField = "Value";
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
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID,  Me.Type, this.ID);

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
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID, Me.Type, this.ID);

                this.ctrl.Items.Insert(0, new RadComboBoxItem(ErrorMessages, String.Empty));
                this.ctrl.BackColor = Color.Red;

            }
        }

        void ctrl_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            //int itemOffset = e.NumberOfItems;
            this.QueryText = e.Text;

            Lookup data = GetData();

            foreach (LookupItem item in data.Items)
            {
                ctrl.Items.Add(new RadComboBoxItem(item.Description, item.Value));

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
                    if (FirstLoad || isRelatedControl())
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

            if (HttpContext.Current.Request["__EVENTTARGET"].IndexOf(String.Format("${0}$", RelatedControl)) > 0)
            {
                retVal = true;
            }

            return retVal;
        }


        private void FillList()
        {
            FirstLoad = false;
            ctrl.AutoPostBack = AutoPostBack;
            ctrl.DataSource = GetData().Items;
            ctrl.DataBind();

            if (IncludeAdditionalListItem)
            {
                string AdditionalListItemText = GetGlobalResourceString("AdditionalListItemText", Me.AdditionalListItemText);
                string AdditionalListitemValue = Me.AdditionalListItemValue;
                if (String.IsNullOrEmpty(AdditionalListitemValue))
                    AdditionalListitemValue = String.Empty;

                ctrl.Items.Insert(0, new RadComboBoxItem(AdditionalListItemText, AdditionalListitemValue));
            }

            //to support criteria list edit - add mode
            ctrl.DataSource = null;
        }

        private Lookup GetData()
        {
            Lookup retVal;

            if (app.EnableLocalization)
            {

                retVal = LookupService.GetInstance().LookupProvider.GetActiveLookupItems(Common.CultureCode, Me.LookupType, QueryText, Value);
            }
            else
            {
                retVal = LookupService.GetInstance().LookupProvider.GetActiveLookupItems(Me.LookupType, QueryText, Value);
            }

       
            
            return retVal;
        }

        public override string GetValidationControlIDSuffix()
        {
            return "$ctrl";
        }

        public override System.Web.UI.WebControls.BaseValidator GetRequiredValidator(BaseControl control, bool IsControlEditable, string requiredErrorMessage)
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
