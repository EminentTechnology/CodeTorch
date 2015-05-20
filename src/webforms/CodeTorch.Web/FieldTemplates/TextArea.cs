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
using CodeTorch.Web.Data;
using CodeTorch.Core;
using System.Drawing;

namespace CodeTorch.Web.FieldTemplates
{
    public class TextArea : BaseFieldTemplate
    {
        protected System.Web.UI.WebControls.TextBox ctrl;

        protected override void CreateChildControls()
        {
            ctrl = new System.Web.UI.WebControls.TextBox();
            ctrl.ID = "ctrl";
            Controls.Add(ctrl);
        }

        TextAreaControl _Me = null;
        public TextAreaControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (TextAreaControl)this.BaseControl;
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
                ctrl.Text = value;
            }
        }

        

        public override string DisplayText
        {
            get
            {
                return Value;
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

                ctrl.CssClass = "form-control";
                if (!String.IsNullOrEmpty(Me.CssClass))
                {
                    ctrl.CssClass += " " + Me.CssClass;
                }

                if (!String.IsNullOrEmpty(Me.SkinID))
                {
                    ctrl.SkinID = Me.SkinID;
                }

                ctrl.TextMode = TextBoxMode.MultiLine;






            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "<br/><span style='color:red'>ERROR - {0} - Control {1} ({2} - {3})</span>";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID, Me.Type, this.ID);

                //this.ctrl.Text = ErrorMessages;
                LiteralControl errorMessage = new LiteralControl(ErrorMessages);
                this.Controls.Add(errorMessage);

                this.ctrl.BackColor = Color.Red;

            }


        }



        public override void LoadControl(object sender, EventArgs e)
        {

            try
            {
                base.LoadControl(sender, e);

                //default look and feel of control
                ctrl.Columns = Me.Columns;
                ctrl.Rows = Me.Rows;

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

        public override string GetValidationControlIDSuffix()
        {
            return "$ctrl";
        }

    }
}
