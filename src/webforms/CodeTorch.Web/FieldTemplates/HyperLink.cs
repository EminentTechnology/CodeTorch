using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeTorch.Core;
using System.Drawing;
using System.Data;
using System.Web.UI.WebControls;

namespace CodeTorch.Web.FieldTemplates
{
    public class HyperLink : BaseFieldTemplate
    {
        protected System.Web.UI.WebControls.HyperLink ctrl;

        protected override void CreateChildControls()
        {
            ctrl = new System.Web.UI.WebControls.HyperLink();
            ctrl.ID = "ctrl";
            Controls.Add(ctrl);
        }

        HyperLinkControl _Me = null;
        public HyperLinkControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (HyperLinkControl)this.BaseControl;
                }
                return _Me;
            }
        }

        public override string Value
        {
            get
            {
                return String.IsNullOrEmpty(ViewState["Value"].ToString()) ? String.Empty : ViewState["Value"].ToString();
            }
            set
            {
                ViewState["Value"] = value;
            }
        }

        public override void InitControl(object sender, EventArgs e)
        {
            base.InitControl(sender, e);

            try
            {
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

                

            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID,  Me.Type, this.ID);

                this.ctrl.Text = ErrorMessages;
                this.ctrl.BackColor = Color.Red;

            }
        }

        public override void LoadControl(object sender, EventArgs e)
        {

            try
            {
                DataRow row = null;

                if ((this.RecordObject != null) && (this.RecordObject is DataRow))
                {
                    row = (DataRow)this.RecordObject;
                }

                if (String.IsNullOrEmpty(Me.Text))
                {
                    if (!String.IsNullOrEmpty(Me.DataTextField))
                    {
                        if (row != null)
                        {
                            string format = "{0}";

                            if (!String.IsNullOrEmpty(Me.DataTextFormatString))
                                format = Me.DataTextFormatString;

                            ctrl.Text = String.Format(format, row[Me.DataTextField]);
                        }
                    }
                }
                else
                {
                    ctrl.Text = GetGlobalResourceString("Text",Me.Text);
                }

                if (String.IsNullOrEmpty(Me.Url))
                {
                    if (row != null)
                    {
                        string[] urlFields =  Me.DataNavigateUrlFields.Split(',');

                        for(int i=0; i<urlFields.Length; i++)
                        {
                            urlFields[i] = row[urlFields[i]].ToString();
                        }

                        ctrl.NavigateUrl = String.Format(Me.DataNavigateUrlFormatString, urlFields); 
                    }
                }
                else
                {
                    ctrl.NavigateUrl = Me.Url;
                }

                if (!String.IsNullOrEmpty(Me.Target))
                    ctrl.Target = Me.Target;

                if (!String.IsNullOrEmpty(Me.Relationship))
                {
                    ctrl.Attributes.Add("rel", Me.Relationship);
                }


            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID,  Me.Type, this.ID);

                this.ctrl.Text = ErrorMessages;
                this.ctrl.BackColor = Color.Red;

            }
        }

        public override bool SupportsValidation()
        {
            return false;
        }

       
    }
}
