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


    public class ListBox : BaseFieldTemplate
    {
        protected Telerik.Web.UI.RadListBox ctrl;

        protected override void CreateChildControls()
        {
            ctrl = new Telerik.Web.UI.RadListBox();
            ctrl.ID = "ctrl";
            Controls.Add(ctrl);
        }
        private char delimiter = ',';


        DataCommandService dataCommand = DataCommandService.GetInstance();
        PageDB pageDB = new PageDB();

        ListBoxControl _Me = null;
        public ListBoxControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (ListBoxControl)this.Widget;
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

                StringBuilder builder = new StringBuilder();
                string separator = "";
                IList<RadListBoxItem> collection = null;
                if (Me.DisplayCheckBoxes)
                {
                    collection = ctrl.CheckedItems;

                }
                else
                {
                    collection = ctrl.SelectedItems;
                }

                foreach (RadListBoxItem item in collection)
                {
                    builder.AppendFormat("{0}{1}", separator, item.Value);
                    separator = delimiter.ToString();
                }
                return builder.ToString();
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && value != "00000000-0000-0000-0000-000000000000")
                {
                    //ctrl.SelectedItems
                }

                IList<RadListBoxItem> collection = null;
                if (Me.DisplayCheckBoxes)
                {
                    collection = ctrl.CheckedItems;

                }
                else
                {
                    collection = ctrl.SelectedItems;
                }

                //wipe out current selections
                foreach (RadListBoxItem item in collection)
                {
                    if (Me.DisplayCheckBoxes)
                    {
                        item.Checked = false;
                    }
                    else
                    {
                        item.Selected = false;
                    }
                }

                //set values from value
                foreach (string s in value.Split(delimiter))
                {
                    RadListBoxItem item = ctrl.FindItemByValue(s);
                    if (item != null)
                    {
                        if (Me.DisplayCheckBoxes)
                        {
                            item.Checked = true;
                        }
                        else
                        {
                            item.Selected = true;
                        }
                    }
                }
            }
        }

        public override string DisplayText
        {
            get
            {
                StringBuilder builder = new StringBuilder();
                string delimiter = "";
                IList<RadListBoxItem> collection = null;
                if (Me.DisplayCheckBoxes)
                {
                    collection = ctrl.CheckedItems;

                }
                else
                {
                    collection = ctrl.SelectedItems;
                }

                foreach (RadListBoxItem item in collection)
                {
                    builder.AppendFormat("{0}{1}", delimiter, item.Text);
                    delimiter = ",";
                }
                return builder.ToString();
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


                ctrl.CheckBoxes = Me.DisplayCheckBoxes;
                ctrl.AutoPostBack = AutoPostBack;
                ctrl.SelectionMode = (Telerik.Web.UI.ListBoxSelectionMode)Enum.Parse(typeof(Telerik.Web.UI.ListBoxSelectionMode), Me.SelectionMode.ToString());
                ctrl.CausesValidation = Me.CausesValidation;


                if (!String.IsNullOrEmpty(Me.Width))
                {
                    ctrl.Width = new Unit(Me.Width);
                }

                if (!String.IsNullOrEmpty(Me.Height))
                {
                    ctrl.Height = new Unit(Me.Height);
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

                ctrl.ItemDataBound += new RadListBoxItemEventHandler(ctrl_ItemDataBound);

            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID,  Me.Type, this.ID);

                this.ctrl.Items.Insert(0, new RadListBoxItem(ErrorMessages, String.Empty));
                this.ctrl.BackColor = Color.Red;

            }


        }

        void ctrl_ItemDataBound(object sender, RadListBoxItemEventArgs e)
        {
            DataRowView row = (DataRowView)e.Item.DataItem;

            
            if (!String.IsNullOrEmpty(Me.DataCheckedField))
            {
                e.Item.Checked = Convert.ToBoolean(row[Me.DataCheckedField]);
            }

            if (!String.IsNullOrEmpty(Me.DataCheckableField))
            {
                e.Item.Checkable = Convert.ToBoolean(row[Me.DataCheckableField]);
            }

            if (!String.IsNullOrEmpty(Me.DataSelectedField))
            {
                e.Item.Selected = Convert.ToBoolean(row[Me.DataSelectedField]);
            }

            if (!String.IsNullOrEmpty(Me.DataImageUrlField))
            {
                e.Item.ImageUrl = row[Me.DataImageUrlField].ToString();
            }

            if (!String.IsNullOrEmpty(Me.DataCssClassField))
            {
                e.Item.CssClass = row[Me.DataCssClassField].ToString();
            }

            if (!String.IsNullOrEmpty(Me.DataEnabledField))
            {
                e.Item.Enabled = Convert.ToBoolean(row[Me.DataEnabledField]);
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

                this.ctrl.Items.Insert(0, new RadListBoxItem(ErrorMessages, String.Empty));
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

                this.ctrl.Items.Insert(0, new RadListBoxItem(ErrorMessages, String.Empty));
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

            ctrl.DataSortField = "";
            ctrl.DataKeyField = "";
            ctrl.DataTextField = Me.DataTextField;
            ctrl.DataValueField = Me.DataValueField;

            this.ctrl.DataSource = GetData();
            this.ctrl.DataBind();

           

            //to support criteria list edit - add mode
            //ctrl.DataSource = null;

        }

        

        private DataTable GetData()
        {


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
            //List<ScreenDataCommandParameter> ctrlValueParameters = parameters.Where(p =>
            //           (
            //               (p.InputType == ScreenInputType.Special) &&
            //               (p.InputKey.ToLower().Trim() == "controltext")
            //           )
            //       ).ToList<ScreenDataCommandParameter>();

            //foreach (ScreenDataCommandParameter p in ctrlValueParameters)
            //{
            //    if (Me.IncludeAdditionalListItem)
            //    {
            //        if (this.QueryText != Me.AdditionalListItemText)
            //        {

            //            p.Value = this.QueryText;
            //        }
            //    }
            //    else
            //    {
            //        p.Value = QueryText;
            //    }
            //}
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
