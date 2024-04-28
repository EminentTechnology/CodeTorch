using CodeTorch.Core;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CodeTorch.Designer.Forms
{
    public partial class GridUniqueNamerForm : Form
    {
        App app;
        List<Core.Screen> screens;

        public GridUniqueNamerForm()
        {
            InitializeComponent();

            app = CodeTorch.Core.Configuration.GetInstance().App;
            screens = CodeTorch.Core.Configuration.GetInstance().Screens;
        }

        private void GridUniqueNamerForm_Load(object sender, EventArgs e)
        {
            progressBar.Maximum = 100;
            progressBar.Minimum = 0;
            progressBar.Value = 0;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            StartButton.Enabled = false;
            try
            {
                foreach (Core.Screen screen in screens)
                {

                    ProcessScreen(screen);

                    progressBar.Value += 1;
                    if(progressBar.Value == progressBar.Maximum)
                    {
                        progressBar.Maximum += 100;
                    }
                    Application.DoEvents();
                }

                progressBar.Value = progressBar.Maximum;
                MessageBox.Show("Grid Unique Code naming complete");

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

        private void ProcessScreen(Core.Screen screen)
        {
            Grid grid;
            GridSection gridSection;

            foreach (Section section in screen.Sections)
            {
                if (section is GridSection)
                {
                    gridSection = (GridSection)section;
                    grid = gridSection.Grid;
                    ProcessGrid(grid);
                }

                if (section is EditableGridSection)
                {
                    gridSection = (EditableGridSection)section;
                    grid = gridSection.Grid;
                    ProcessGrid(grid);
                }
            }

            Core.Screen.Save(screen);
        }

        private void ProcessGrid(Grid grid)
        {
            int ColumnIndex = 0;
            foreach (GridColumn column in grid.Columns)
            { 
                bool UpdateGridColumn = (ForceRename.Checked || String.IsNullOrEmpty(column.UniqueName));

                if (UpdateGridColumn)
                {
                    if (!String.IsNullOrEmpty(column.HeaderText))
                    {
                        column.UniqueName = column.HeaderText.Trim().Replace(' ', '_').Replace('&', '_');
                    }
                    else
                    {
                        column.UniqueName = String.Format("Column{0}", ColumnIndex);
                    }
                }

                ColumnIndex++;
            }
        }
    }
}
