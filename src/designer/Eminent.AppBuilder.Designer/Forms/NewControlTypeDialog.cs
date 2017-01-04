using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CodeTorch.Core;

namespace CodeTorch.Designer.Forms
{
    public partial class NewControlTypeDialog : Form
    {

        public NewControlTypeDialog()
        {
            InitializeComponent();
        }

        private bool ValidateSaveRequest()
        {
            bool IsErrorPresent = false;
            StringBuilder errors = new StringBuilder();

            ValidateRequiredField(ControlName, "Control Name", ref IsErrorPresent, errors);
            ValidateRequiredField(ControlFilePath, "Control File Path", ref IsErrorPresent, errors);

            if (ControlName.Text.Trim() != String.Empty)
            {
                ValidateExistingControl(ControlName, ControlName.Text, ref IsErrorPresent, errors);
            }

            if (IsErrorPresent)
            {
                MessageBox.Show("The following error(s) occurred:\n\n" + errors.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            return (!IsErrorPresent);

        }

        private void ValidateExistingControl(Control ctrl, string Caption, ref bool IsErrorPresent, StringBuilder errors)
        {

            var retVal = from controlType in Core.Configuration.GetInstance().ControlTypes
                         where controlType.Name.ToLower() == ControlName.Text.ToLower()
                         select controlType;

            if(retVal.Count<ControlType>() > 0)
            {
                IsErrorPresent = true;
                string errorMessage = String.Format("Control {0} already exists.", Caption);
                errorProvider.SetError(ctrl, errorMessage);
                errors.AppendLine(errorMessage);
            }
            else
            {
                errorProvider.SetError(ctrl, String.Empty);
            }
        }

        private void ValidateRequiredField(Control ctrl, string Caption, ref bool IsErrorPresent, StringBuilder errors)
        {
            if (ctrl.Text.Trim() == String.Empty)
            {
                IsErrorPresent = true;
                string errorMessage = String.Format("{0} is required", Caption);
                errorProvider.SetError(ctrl, errorMessage);
                errors.AppendLine(errorMessage);
            }
            else
            {
                errorProvider.SetError(ctrl, String.Empty);
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateSaveRequest())
                {
                    ControlType controlType = new ControlType();

                    controlType.Name = ControlName.Text;
                    //controlType.Path = ControlFilePath.Text;

                    Core.Configuration.GetInstance().ControlTypes.Add(controlType);
                    ControlType.Save(controlType);
                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
        }

        

        
    }
}
