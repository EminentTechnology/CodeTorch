using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeTorch.Core;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Web.UI;

namespace CodeTorch.Web.FieldTemplates
{
    public class Label: BaseFieldTemplate
    {
        System.Web.UI.HtmlControls.HtmlGenericControl ctrl;

        protected override void CreateChildControls()
        {
            ctrl = new System.Web.UI.HtmlControls.HtmlGenericControl();
            //ctrl.ID = "ctrl";
            Controls.Add(ctrl);
        }
        
        LabelControl _Me = null;
        public LabelControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (LabelControl)this.BaseControl;
                }
                return _Me;
            }
        }

        public override string Value
        {
            get
            {
                return (ViewState["Value"] == null) ? String.Empty : ViewState["Value"].ToString();
            }
            set
            {
                ViewState["Value"] = value;
                ctrl.InnerHtml = FormatValue(value);
            }
        }

    

        string FormatValue(string Value)
        {

            string retVal = Value;

            if (!String.IsNullOrEmpty(Me.FormatString))
            {
                retVal = String.Format(GetGlobalResourceString("FormatString", Me.FormatString), this.ValueObject);
            }
            else
            {
                retVal = Value;     
            }

            return retVal;


        }

        public override void InitControl(object sender, EventArgs e)
        {
            base.InitControl(sender, e);

            try
            {

                
                //ctrl = new System.Web.UI.HtmlControls.HtmlGenericControl("p");

                if (!String.IsNullOrEmpty(Me.Width))
                {
                    ctrl.Attributes.Add("style", String.Format("width: {0};",Me.Width));
                }

                if (!String.IsNullOrEmpty(Me.CssClass))
                {
                    ctrl.Attributes.Add("class",  Me.CssClass);
                }

                if (!String.IsNullOrEmpty(Me.SkinID))
                {
                    ctrl.SkinID = Me.SkinID;
                }

                //this.Controls.Add(ctrl);

            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID,  Me.Type, this.ID);

                this.Controls.Add(new LiteralControl(ErrorMessages));
                

            }
        }

        public override bool SupportsValidation()
        {
            return false;
        }
    }
}
