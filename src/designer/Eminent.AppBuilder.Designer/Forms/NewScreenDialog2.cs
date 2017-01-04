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
    public partial class NewScreenDialog2 : Form
    {
 
        public NewScreenDialog2()
        {
            InitializeComponent();
        }

        private bool ValidateSaveRequest()
        {
            bool IsErrorPresent = false;
            StringBuilder errors = new StringBuilder();

            ValidateRequiredField(PageName, "Page Name", ref IsErrorPresent, errors);
            ValidatePageNameFormat(PageName, "Page Name", ref IsErrorPresent, errors);
            ValidateRequiredField(PageTypeList, "Page Type", ref IsErrorPresent, errors);
            ValidateRequiredField(Folder, "Folder", ref IsErrorPresent, errors);
            ValidateRequiredField(PageTitle, "Page Title", ref IsErrorPresent, errors);
            ValidateRequiredField(PageSubTitle, "Page Subtitle", ref IsErrorPresent, errors);

            if (PageName.Text.Trim() != String.Empty)
            {
                ValidateExistingPage(PageName, PageName.Text, ref IsErrorPresent, errors);
            }

            if (IsErrorPresent)
            {
                MessageBox.Show("The following error(s) occurred:\n\n" + errors.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            return (!IsErrorPresent);

        }

        private void ValidateExistingPage(Control ctrl, string Caption, ref bool IsErrorPresent, StringBuilder errors)
        {

            var retVal = from screen in Core.Configuration.GetInstance().Screens
                         where 
                            (
                                (screen.Name.ToLower() == PageName.Text.ToLower()) &&
                                (screen.Folder.ToLower() == Folder.Text.ToLower())
                            )

                         select screen;

            if (retVal.Count<CodeTorch.Core.Screen>() > 0)
            {
                IsErrorPresent = true;
                string errorMessage = String.Format("Screen {0} already exists.", Caption);
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

        private void ValidatePageNameFormat(Control ctrl, string Caption, ref bool IsErrorPresent, StringBuilder errors)
        {
            if (!ctrl.Text.Trim().ToLower().EndsWith(".aspx") )
            {
                IsErrorPresent = true;
                string errorMessage = String.Format("Screen name must end in .aspx", Caption);
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
                    CodeTorch.Core.Screen screen = CodeTorch.Core.Screen.GetScreenObjectByType(PageTypeList.Text);

                    screen.Name = PageName.Text;
                    screen.Folder = Folder.Text;
                    screen.Type = PageTypeList.Text;
                    screen.Title.Name = PageTitle.Text;
                    screen.Title.UseCommand = false;
                    screen.SubTitle.Name = PageSubTitle.Text;
                    screen.SubTitle.UseCommand = false;
                    screen.RequireAuthentication = true;
                    screen.ScreenPermission.CheckPermission = true;

                    
                    

                    Core.Configuration.GetInstance().Screens.Add(screen);

                    CodeTorch.Core.Screen.Save(screen);

                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
        }

        

        private void NewPageDialog_Load(object sender, EventArgs e)
        {


            this.PageTypeList.DisplayMember = "Name";
            this.PageTypeList.ValueMember = "Name";
            this.PageTypeList.DataSource = Core.Configuration.GetInstance().ScreenTypes;
        }

       

       
    }
}
