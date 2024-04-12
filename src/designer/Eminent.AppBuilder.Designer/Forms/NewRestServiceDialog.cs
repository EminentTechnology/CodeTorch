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
    public partial class NewRestServiceDialog : Form
    {
        public string ItemType;
        public string ItemDisplayName;

        public NewRestServiceDialog()
        {
            InitializeComponent();
        }

        private bool ValidateSaveRequest()
        {
            bool IsErrorPresent = false;
            StringBuilder errors = new StringBuilder();

            ValidateRequiredField(ItemName, ItemDisplayName, ref IsErrorPresent, errors);
            ValidateRequiredField(Folder, "Folder", ref IsErrorPresent, errors);

            if (ItemName.Text.Trim() != String.Empty)
            {
                ValidateExistingControl(ItemName, ItemName.Text, ref IsErrorPresent, errors);
                
            }

            if (IsErrorPresent)
            {
                MessageBox.Show("The following error(s) occurred:\n\n" + errors.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            return (!IsErrorPresent);

        }

        private void ValidateExistingControl(Control ctrl, string Caption, ref bool IsErrorPresent, StringBuilder errors)
        {
            int count = 0;

            count = GetRestServiceCountByName();

            if (count > 0)
            {
                IsErrorPresent = true;
                string errorMessage = String.Format("{0} {1} already exists.", ItemDisplayName, Caption);
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
                    SaveRestService();

                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
        }

        

        private void NewItemDialog_Load(object sender, EventArgs e)
        {
            NameLabel.Text = ItemDisplayName + ":";
        }


        

        private int GetRestServiceCountByName()
        {
            var retVal = from item in Configuration.GetInstance().RestServices
                         where 
                            (item.Name.ToLower() == ItemName.Text.ToLower()) &&
                            (item.Folder.ToLower() == Folder.Text.ToLower()) 
                         select item;
            return retVal.Count<RestService>();
        }

        private void SaveRestService()
        {
            RestService item = new RestService();

            item.Name = ItemName.Text;
            item.Folder = Folder.Text;

            Configuration.GetInstance().RestServices.Add(item);
            RestService.Save(item);

        }
    }
}
