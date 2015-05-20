using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

using CodeTorch.Core;

using Telerik.Web.UI;
using System.Web;
using CodeTorch.Core.Services;

namespace CodeTorch.Web.FieldTemplates
{
    public class ListBox_Lookup : BaseFieldTemplate
    {
        protected Telerik.Web.UI.RadListBox ctrl;

        protected override void CreateChildControls()
        {
            ctrl = new Telerik.Web.UI.RadListBox();
            ctrl.ID = "ctrl";
            Controls.Add(ctrl);
        }
        private char delimiter = ',';

        LookupListBoxControl _Me = null;
        public LookupListBoxControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (LookupListBoxControl)this.BaseControl;
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

               

            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID,  Me.Type, this.ID);

                this.ctrl.Items.Insert(0, new RadListBoxItem(ErrorMessages, String.Empty));
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
            ctrl.DataTextField = "Items.Description";
            ctrl.DataValueField = "Items.Value";
            ctrl.AutoPostBack = AutoPostBack;
            ctrl.DataSource = GetData();
            ctrl.DataBind();

            

            //to support criteria list edit - add mode
            //ctrl.DataSource = null;
        }

        private Lookup GetData()
        {

            Lookup retVal;

            if (app.EnableLocalization)
            {

                retVal = LookupService.GetInstance().LookupProvider.GetActiveLookupItems(Common.CultureCode, Me.LookupType, null, Value);
            }
            else
            {
                retVal = LookupService.GetInstance().LookupProvider.GetActiveLookupItems(Me.LookupType, null, Value);
            }



            return retVal;
        }

        public override string GetValidationControlIDSuffix()
        {
            return "$ctrl";
        }
    }
}
