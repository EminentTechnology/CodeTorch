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
using System.Web;

namespace CodeTorch.Web.FieldTemplates
{


    public class MultiComboDropDownListBox : BaseFieldTemplate
    {
        protected Telerik.Web.UI.RadComboBox ctrl;

        protected override void CreateChildControls()
        {
            ctrl = new Telerik.Web.UI.RadComboBox();
            ctrl.ID = "ctrl";
            Controls.Add(ctrl);

            ctrl.HeaderTemplate = new MultiComboDropDownListBoxItemTemplate(this, ListItemType.Header, Me, this.ResourceKeyPrefix);
            ctrl.ItemTemplate = new MultiComboDropDownListBoxItemTemplate(this, ListItemType.Item, Me, this.ResourceKeyPrefix);

            ctrl.FooterTemplate = new MultiComboDropDownListBoxItemTemplate(this, ListItemType.Footer, Me, this.ResourceKeyPrefix);

            //ctrl.ItemDataBound += new RadComboBoxItemEventHandler(ctrl_ItemDataBound);

        }


        DataCommandService dataCommand = DataCommandService.GetInstance();
        PageDB pageDB = new PageDB();

        MultiComboDropDownListControl _Me = null;
        public MultiComboDropDownListControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (MultiComboDropDownListControl)this.BaseControl;
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

                ctrl.HighlightTemplatedItems = true;

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

                if (IncludeAdditionalListItem)
                {
                    ctrl.EmptyMessage = GetGlobalResourceString("AdditionalListItemText", Me.AdditionalListItemText);
                  
                }

                ctrl.ItemsRequested += new RadComboBoxItemsRequestedEventHandler(ctrl_ItemsRequested);

            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID,  Me.Type, this.ID);

                this.ctrl.Text = ErrorMessages;
                this.ctrl.BackColor = Color.Red;

            }


        }
        

        void ctrl_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            throw new NotImplementedException();
        }

        void ctrl_ItemsRequested(object o, RadComboBoxItemsRequestedEventArgs e)
        {
            //int itemOffset = e.NumberOfItems;
            this.QueryText = e.Text;

            DataTable data = GetData();

            this.ctrl.DataSource = data;
            this.ctrl.DataBind();

            //foreach (DataRow row in data.Rows)
            //{
            //    ctrl.Items.Add(new RadComboBoxItem(row[Me.DataTextField].ToString(), row[Me.DataValueField].ToString()));

            //}
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

            if (IncludeAdditionalListItem)
            {
                string AdditionalListItemText = GetGlobalResourceString("AdditionalListItemText", Me.AdditionalListItemText);
                string AdditionalListitemValue = Me.AdditionalListItemValue;

                if (String.IsNullOrEmpty(AdditionalListitemValue))
                    AdditionalListitemValue = String.Empty;

                ctrl.Items.Insert(0, new RadComboBoxItem(AdditionalListItemText, AdditionalListitemValue));
            }
            

            //to support criteria list edit - add mode
            //ctrl.DataSource = null;

        }

        private DataTable GetData()
        {

            EnsureChildControls();

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
    }
}
