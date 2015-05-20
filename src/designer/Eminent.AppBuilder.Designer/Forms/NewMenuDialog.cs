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
    public partial class NewMenuDialog : Form
    {

        public NewMenuDialog()
        {
            InitializeComponent();
        }

        

        private bool ValidateSaveRequest()
        {
            bool IsErrorPresent = false;
            StringBuilder errors = new StringBuilder();

            ValidateRequiredField(MenuName, "Menu Name", ref IsErrorPresent, errors);


            if (MenuName.Text.Trim() != String.Empty)
            {
                ValidateExistingMenu(MenuName, MenuName.Text, ref IsErrorPresent, errors);
            }

           
            if (IsErrorPresent)
            {
                MessageBox.Show("The following error(s) occurred:\n\n" + errors.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            return (!IsErrorPresent);

        }

      

        private void ValidateExistingMenu(Control ctrl, string Caption, ref bool IsErrorPresent, StringBuilder errors)
        {
            var retVal = from menu in Configuration.GetInstance().Menus
                         where menu.Name.ToLower() == MenuName.Text.ToLower()
                         select menu;

            if (retVal.Count<CodeTorch.Core.Menu>() > 0)
            {
                IsErrorPresent = true;
                string errorMessage = String.Format("Menu {0} already exists.", Caption);
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

        private void Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateSaveRequest())
                {

                    CodeTorch.Core.Menu menu = new CodeTorch.Core.Menu();
                    menu.Name = MenuName.Text;

                    Configuration.GetInstance().Menus.Add(menu);
                    CodeTorch.Core.Menu.Save(menu);
                   
                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
            
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MenuName_KeyPress(object sender, KeyPressEventArgs e)
        {
            PreventNonAlphaExceptUnderline(e);
        }

       
    }
}
