using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeTorch.Web.FieldTemplates;
using System.Web.UI.WebControls;
using CodeTorch.Core;
using System.Drawing;


namespace CodeTorch.Web.FieldTemplates
{
    public class NumericTextBox : BaseFieldTemplate
    {
        protected Telerik.Web.UI.RadNumericTextBox ctrl;

        protected override void CreateChildControls()
        {
            ctrl = new Telerik.Web.UI.RadNumericTextBox();
            ctrl.ID = "ctrl";
            Controls.Add(ctrl);
        }

        NumericTextBoxControl _Me = null;
        public NumericTextBoxControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (NumericTextBoxControl)this.BaseControl;
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

                if (!String.IsNullOrEmpty(Me.Skin))
                {
                    ctrl.Skin = Me.Skin;
                }


                ctrl.MinValue = Me.MinValue;
                ctrl.MaxValue = Me.MaxValue;
                ctrl.NumberFormat.DecimalDigits = Me.DecimalDigits;
                ctrl.NumberFormat.DecimalSeparator = Me.DecimalSeparator;
                ctrl.NumberFormat.GroupSeparator = Me.GroupSeparator;
                ctrl.NumberFormat.GroupSizes = Me.GroupSizes;

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
