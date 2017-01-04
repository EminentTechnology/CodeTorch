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
    public partial class NewPermissionDialog : Form
    {

            
        public NewPermissionDialog()
        {
            InitializeComponent();
        }

        private void PermissionName_KeyPress(object sender, KeyPressEventArgs e)
        {
            PreventNonAlphaExceptUnderline(e);
        }

        private void Category_KeyPress(object sender, KeyPressEventArgs e)
        {
            PreventNonAlphaExceptUnderline(e);
        }

        private void PreventNonAlphaExceptUnderline(KeyPressEventArgs e)
        {
            if (!(char.IsLetter(e.KeyChar)))
            {
                if ((e.KeyChar != '_') && (e.KeyChar != '\b'))
                {
                    e.Handled = true;
                }

            }
        }

        private bool ValidateSaveRequest()
        {
            bool IsErrorPresent = false;
            StringBuilder errors = new StringBuilder();

            ValidateRequiredField(PermissionName, "Permission", ref IsErrorPresent, errors);
            ValidateRequiredField(Category, "Category", ref IsErrorPresent, errors);
            ValidateRequiredField(Description, "Description", ref IsErrorPresent, errors);

            if (PermissionName.Text.Trim() != String.Empty)
            {
                ValidateExistingPermission(PermissionName, PermissionName.Text, ref IsErrorPresent, errors);
            }

            
            
            if (IsErrorPresent)
            {
                MessageBox.Show("The following error(s) occurred:\n\n" + errors.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            return (!IsErrorPresent);

        }

        private void ValidateExistingPermission(Control ctrl, string Caption, ref bool IsErrorPresent, StringBuilder errors)
        {


            var retVal = from permission in Core.Configuration.GetInstance().Permissions
                         where permission.Name.ToLower() == ctrl.Text.ToLower()
                         select permission;

            if (retVal.Count<Permission>() > 0)
            {
                IsErrorPresent = true;
                string errorMessage = String.Format("Permission {0} already exists.", Caption);
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
                    
                    Permission permission = new Permission();

                    permission.Name = PermissionName.Text;
                    permission.Category = Category.Text;
                    permission.Description = Description.Text;

                    Core.Configuration.GetInstance().Permissions.Add(permission);
                    Permission.Save(permission);

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
