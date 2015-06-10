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
using CodeTorch.Core.Services;
using CodeTorch.Web.Data;
using CodeTorch.Core;
using System.Drawing;
using Telerik.Web.UI;
using CodeTorch.Web.Templates;

namespace CodeTorch.Web.FieldTemplates
{
    public class AutoCompleteBox : BaseFieldTemplate
    {
        protected Telerik.Web.UI.RadAutoCompleteBox ctrl;

        protected override void CreateChildControls()
        {
            ctrl = new RadAutoCompleteBox();
            ctrl.ID = "ctrl";
            Controls.Add(ctrl);
        }

        DataCommandService dataCommand = DataCommandService.GetInstance();
        PageDB pageDB = new PageDB();

        AutoCompleteBoxControl _Me = null;
        public AutoCompleteBoxControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (AutoCompleteBoxControl)this.Widget;
                }
                return _Me;
            }
        }

        public override string Value
        {
            get
            {
                return ctrl.Text;
            }
            set
            {
                //
                //ctrl.Text = value;
                
            }
        }

        string RelatedControl
        {
            get
            {
                return (Me.RelatedControl == null) ? string.Empty : Me.RelatedControl;
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

        public override void InitControl(object sender, EventArgs e)
        {
            base.InitControl(sender, e);

            try
            {
               

                ctrl.InputType = (RadAutoCompleteInputType)Enum.Parse(typeof(RadAutoCompleteInputType), Me.InputType.ToString());
                ctrl.Filter = (RadAutoCompleteFilter)Enum.Parse(typeof(RadAutoCompleteFilter), Me.Filter.ToString());
                ctrl.MinFilterLength = Me.MinFilterLength;
                ctrl.MaxResultCount = Me.MaxResultCount;
                ctrl.AllowCustomEntry = Me.AllowCustomEntry;
                ctrl.TokensSettings.AllowTokenEditing = Me.AllowTokenEditing;
                ctrl.DropDownPosition = (RadAutoCompleteDropDownPosition)Enum.Parse(typeof(RadAutoCompleteDropDownPosition), Me.DropDownPosition.ToString());
            
                ctrl.EnableClientFiltering = Me.EnableClientFiltering;
                ctrl.Delimiter = Me.Delimiter;
                ctrl.ClientDropDownItemTemplate = String.Empty;

                if (!String.IsNullOrEmpty(Me.ClientDropDownItemTemplate))
                {
                    ctrl.ClientDropDownItemTemplate = GetGlobalResourceString("ClientDropDownItemTemplate", Me.ClientDropDownItemTemplate);
                }

                if (!String.IsNullOrEmpty(Me.EmptyMessage))
                {
                    ctrl.EmptyMessage = GetGlobalResourceString("EmptyMessage", Me.EmptyMessage);
                }

                ctrl.DataTextField = Me.DataTextField;
                ctrl.DataValueField = Me.DataValueField;


                ctrl.TextSettings.SelectionMode = (RadAutoCompleteSelectionMode)Enum.Parse(typeof(RadAutoCompleteSelectionMode), Me.SelectionMode.ToString());

                ctrl.DataSourceSelect += ctrl_DataSourceSelect;

                if (!String.IsNullOrEmpty(Me.Width))
                {
                    ctrl.Width = new Unit(Me.Width);
                }

                if (!String.IsNullOrEmpty(Me.CssClass))
                {
                    ctrl.CssClass = Me.CssClass;
                }

                if (!String.IsNullOrEmpty(Me.SkinID))
                {
                    ctrl.SkinID = Me.SkinID;
                }

                if (!String.IsNullOrEmpty(Me.Width))
                {
                    ctrl.DropDownWidth = new Unit(Me.Width);
                }

                if (!String.IsNullOrEmpty(Me.Width))
                {
                    ctrl.DropDownHeight = new Unit(Me.Width);
                }
                

            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID, Me.Type, this.ID);

                this.ctrl.Entries.Insert(0, new AutoCompleteBoxEntry(ErrorMessages, String.Empty));
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
                    if (FirstLoad || isRelatedControl())
                    {
                        FillList();
                    }
                }

            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID, Me.Type, this.ID);

                this.ctrl.Entries.Insert(0, new AutoCompleteBoxEntry(ErrorMessages, String.Empty));
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
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID, Me.Type, this.ID);

                this.ctrl.Entries.Insert(0, new AutoCompleteBoxEntry(ErrorMessages, String.Empty));
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
            if (!String.IsNullOrEmpty(Me.SelectDataCommand))
            {
                FirstLoad = false;
                ctrl.AutoPostBack = Me.AutoPostBack;
                ctrl.DataTextField = Me.DataTextField;
                ctrl.DataValueField = Me.DataValueField;

                this.ctrl.DataSource = GetData();
                this.ctrl.DataBind();


                //to support criteria list edit - add mode
                ctrl.DataSource = null;
            }

        }

        private DataTable GetData()
        {

            if (String.IsNullOrEmpty(Me.SelectDataCommand))
            {
                throw new ApplicationException("SelectDataCommand is invalid");
            }

            List<ScreenDataCommandParameter> parameters = pageDB.GetPopulatedCommandParameters(Me.SelectDataCommand, ((CodeTorch.Web.Templates.BasePage)this.Page));

            PopulateScreenParameterWithControlValue(parameters);
            PopulateScreenParameterWithControlText(parameters);
            PopulateScreenParameterWithRelatedControlValue(parameters);

            return dataCommand.GetDataForDataCommand(Me.SelectDataCommand, parameters);



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
                p.Value = ctrl.Text;// QueryText;
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

        void ctrl_DataSourceSelect(object sender, AutoCompleteBoxDataSourceSelectEventArgs e)
        {
            //throw new NotImplementedException();
            string a = "";
            a = e.FilterString;
        }

       
    }
}
