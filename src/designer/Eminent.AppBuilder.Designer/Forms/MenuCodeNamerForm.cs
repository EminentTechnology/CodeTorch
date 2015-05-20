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
    public partial class MenuCodeNamerForm : Form
    {
        App app;
        List<Core.Menu> menus;
        public MenuCodeNamerForm()
        {
            InitializeComponent();

            app = CodeTorch.Core.Configuration.GetInstance().App;
            menus = CodeTorch.Core.Configuration.GetInstance().Menus;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            StartButton.Enabled = false;
            try
            {

                int i = 0;
                foreach (Core.Menu menu in menus)
                {
                    List<Core.MenuItem> items = menu.Items;
                    foreach(Core.MenuItem item in items)
                    {
                        ProcessMenuItem(item);
                        
                    }

                    i += Core.Menu.GetTotalMenuItemCount(menu);
                    Core.Menu.Save(menu);

                    progressBar.Value = i;
                    Application.DoEvents();
                }

                MessageBox.Show("Menu Code naming complete");

                this.Close();
            }
            catch (Exception ex)
            { 
                ErrorManager.HandleError(ex);
            }
            finally
            {
                StartButton.Enabled = true;
            }
        }

        private void ProcessMenuItem(Core.MenuItem item)
        {


            bool UpdateMenuItem = (ForceRename.Checked || String.IsNullOrEmpty(item.Code));

            if (UpdateMenuItem)
            {
                if(!String.IsNullOrEmpty(item.Name))
                {
                    item.Code = item.Name.Trim().Replace(' ','_').Replace('&','_');
                }
            }

            foreach(Core.MenuItem i in item.Items)
            {
                ProcessMenuItem(i);
            }
            


        }

        private void MenuCodeNamerForm_Load(object sender, EventArgs e)
        {
            

            int i = 0;
            foreach (Core.Menu menu in menus)
            {

                i += Core.Menu.GetTotalMenuItemCount(menu);
            }

            progressBar.Maximum = i;
            progressBar.Minimum=0;
            progressBar.Value = 0;
        }
    }
}
