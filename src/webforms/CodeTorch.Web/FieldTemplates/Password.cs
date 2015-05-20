using System;
using System.Linq;
using CodeTorch.Core;
using System.Web.UI.WebControls;
using System.Drawing;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography;

namespace CodeTorch.Web.FieldTemplates
{
    public class Password : BaseFieldTemplate
    {
        protected System.Web.UI.WebControls.TextBox ctrl;

        protected override void CreateChildControls()
        {
            ctrl = new System.Web.UI.WebControls.TextBox();
            ctrl.ID = "ctrl";
            ctrl.TextMode = TextBoxMode.Password;
            Controls.Add(ctrl);
        }
        protected const string CLEARTEXT = "ClearText";

        PasswordControl _Me = null;
        public PasswordControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (PasswordControl)this.BaseControl;
                }
                return _Me;
            }
        }

       

        public override string DisplayText
        {
            get
            {
                return Value;
            }

        }

        public override string Value
        {
            get
            {
                string retVal = null;

                if (!String.IsNullOrEmpty(ctrl.Text))
                {
                    switch (Me.PasswordMode)
                    {
                        case PasswordMode.Hash:
                            if (String.IsNullOrEmpty(Me.PasswordAlgorithm))
                                throw new ApplicationException("Invalid Password Algorithm");

                            retVal = Cryptographer.CreateHash(Me.PasswordAlgorithm, ctrl.Text);
                            break;
                        case PasswordMode.Encrypted:
                            if (String.IsNullOrEmpty(Me.PasswordAlgorithm))
                                throw new ApplicationException("Invalid Password Algorithm");

                            retVal = Cryptographer.EncryptSymmetric(Me.PasswordAlgorithm, ctrl.Text);
                            break;
                        case PasswordMode.PlainText:
                            retVal = ctrl.Text;
                            break;
                    }
                    
                }

                return retVal;
            }
            
        }

        public override string ValidationValue
        {
            get
            {
                return ctrl.Text;
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
