using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeTorch.Core;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Web.UI;

namespace CodeTorch.Web.FieldTemplates
{
    public class EmailAddress : BaseFieldTemplate, INamingContainer
    {
        protected System.Web.UI.WebControls.TextBox ctrl;
        protected System.Web.UI.WebControls.RegularExpressionValidator ctrlRegularExpressionValidator;



        protected override void CreateChildControls()
        {
            ctrl = new System.Web.UI.WebControls.TextBox();
            ctrl.ID = "ctrl";
            Controls.Add(ctrl);

            ctrlRegularExpressionValidator = new System.Web.UI.WebControls.RegularExpressionValidator();
            ctrlRegularExpressionValidator.ID = "ctrlRegularExpressionValidator";
            Controls.Add(ctrl);
        }

        EmailAddressControl _Me = null;
        public EmailAddressControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (EmailAddressControl)this.BaseControl;
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
                ctrlRegularExpressionValidator.ControlToValidate = "ctrl";
                ctrlRegularExpressionValidator.Display = ValidatorDisplay.None;
                ctrlRegularExpressionValidator.ErrorMessage = GetGlobalResourceString("EmailAddress.Validation.ErrorMessage", ctrlRegularExpressionValidator.ErrorMessage = "Please enter a valid email address");
                ctrlRegularExpressionValidator.ValidationExpression = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
                ctrlRegularExpressionValidator.EnableClientScript = false;



            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control - InitControl - {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID,  Me.Type, this.ID);

                this.ctrl.Text = ErrorMessages;
                this.ctrl.BackColor = Color.Red;

            }
        }

        public override void LoadControl(object sender, EventArgs e)
        {

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

                

            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID,  Me.Type, this.ID);

                this.ctrl.Text = ErrorMessages;
                this.ctrl.BackColor = Color.Red;

            }
        }

        //public override string GetValidationControlIDSuffix()
        //{
        //    return "$ctrl";
        //}
    }
}
