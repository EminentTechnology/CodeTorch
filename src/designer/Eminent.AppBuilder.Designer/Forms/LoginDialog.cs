using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CodeTorch.Designer.Code;
using System.IO;
using CodeTorch.Core;

namespace CodeTorch.Designer.Forms
{
    public partial class LoginDialog : Form
    {

       

        string ConfigurationPath
        {
            get
            {
                return this.ConfigFolder.Text.EndsWith("\\") ? this.ConfigFolder.Text : (this.ConfigFolder.Text+"\\");
            }
        }


        public Project Project { get; set; }
        public string ProjectFile { get; set; }
        
        

        public LoginDialog()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, EventArgs e)
        {

        }

        private bool PerformValidations()
        {
            bool IsErrorPresent = false;
            StringBuilder errors = new StringBuilder();

            ValidateRequiredField(this.ProjectName, "Project Name", ref IsErrorPresent, errors);
            ValidateRequiredField(this.ProjectFolder, "Project Folder", ref IsErrorPresent, errors);
            ValidateRequiredField(this.RootNamespace, "Root Namespace", ref IsErrorPresent, errors);
            ValidateRequiredField(this.ConfigFolder, "Config Folder", ref IsErrorPresent, errors);
            ValidateRequiredField(this.OutputLocation1Folder, "Output Location 1 is required", ref IsErrorPresent, errors);
            
            

            

            if (IsErrorPresent)
            {
                MessageBox.Show("The following error(s) occurred:\n\n" + errors.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            return (!IsErrorPresent);

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

        private void LoginDialog_Load(object sender, EventArgs e)
        {
            try
            {
                ProjectTypeList.DataSource = Enum.GetValues(typeof(ProjectType));

                ProjectMRU mru = ProjectMRU.Get();
                if (mru != null)
                {
                    RecentProjectList.Items.Clear();


                    foreach (ProjectMRUItem i in mru.Items)
                    {
                        if (File.Exists(i.Path))
                        {
                            RecentProjectList.Items.Add(i);
                        }
                    }

                    ContextMenuStrip contextMenu = new ContextMenuStrip();

                    ToolStripItem removeProject = contextMenu.Items.Add("Remove");
                    removeProject.Click += removeProject_Click;
                    RecentProjectList.ContextMenuStrip = contextMenu;

                    
                }
               
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
        }

        void removeProject_Click(object sender, EventArgs e)
        {
            try
            {
                if (RecentProjectList.SelectedIndex >= 0)
                {
                    ProjectMRUItem selectedItem = (ProjectMRUItem)RecentProjectList.Items[RecentProjectList.SelectedIndex];
                    ProjectMRU srcMRU = ProjectMRU.Get();
                    ProjectMRU dstMRU = new ProjectMRU();

                    foreach (ProjectMRUItem item in srcMRU.Items)
                    {
                        if (!item.Equals(selectedItem))
                        {
                            if (File.Exists(item.Path))
                            {
                                dstMRU.Items.Add(item);
                            }
                        }
                    }

                    

                    RecentProjectList.Items.Remove(selectedItem);

                    ProjectGroupBox.Enabled = false;

                    Open.Enabled = false;
                    ClearProjectDetails();

                    ProjectMRU.Save(dstMRU);
                    
                }
                
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
        }

        private void SelectFolder_Click(object sender, EventArgs e)
        {
            folderDialog.Description = "Please Select Configuration Folder";

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                this.ConfigFolder.Text = folderDialog.SelectedPath;
            }
        }



        private void Open_Click(object sender, EventArgs e)
        {
            OpenProject();
        }

        private static void AddToMRU(ProjectMRUItem item)
        {

            ProjectMRU mru = ProjectMRU.Get();
            if (mru == null)
            {
                mru = new ProjectMRU();
            }
            mru.Items.Add(item);

            ProjectMRU.Save(mru);
        }

        private Project PopulateProjectObjectFromForm()
        {
            

            ProjectType t;
            Enum.TryParse<ProjectType>(ProjectTypeList.SelectedValue.ToString(), out t);
            this.Project.ProjectType = t;
            this.Project.Name = this.ProjectName.Text;
            this.Project.RootNamespace = this.RootNamespace.Text;

            this.Project.ConfigurationFolder = this.ConfigFolder.Text;

            this.Project.OutputLocations.Clear();

            if (!String.IsNullOrEmpty(this.OutputLocation1Folder.Text))
                this.Project.OutputLocations.Add(this.OutputLocation1Folder.Text);

            if (!String.IsNullOrEmpty(this.OutputLocation2Folder.Text))
                this.Project.OutputLocations.Add(this.OutputLocation2Folder.Text);



            return this.Project;
        }

        private void PopulateProjectDetailsFromFile(string FileName)
        {
            this.Project = Project.Load(FileName);

            ProjectTypeList.SelectedItem = this.Project.ProjectType;

            this.ProjectName.Text = this.Project.Name;
            this.RootNamespace.Text = this.Project.RootNamespace;

            this.ConfigFolder.Text = this.Project.ConfigurationFolder;

            if (this.Project.OutputLocations.Count > 0)
                this.OutputLocation1Folder.Text = this.Project.OutputLocations[0];

            if (this.Project.OutputLocations.Count > 1)
                this.OutputLocation2Folder.Text = this.Project.OutputLocations[1];

            this.ProjectFolder.Text = FileName.Substring(0, FileName.LastIndexOf("\\"));

          

            ProjectGroupBox.Enabled = true;

            Open.Enabled = true;

          
        }

        private void CreateNewProject_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ProjectGroupBox.Enabled = true;


            ClearProjectDetails();

            ProjectFolder.Text = String.Format("{0}\\{1}", Environment.GetFolderPath( Environment.SpecialFolder.MyDocuments), "CodeTorch");
            Open.Enabled = true;
        }

        private void ClearProjectDetails()
        {
            this.ProjectTypeList.SelectedIndex = 0;
            this.ProjectName.Text = "";
            this.RootNamespace.Text = "";
            this.ProjectFolder.Text = "";
            this.ConfigFolder.Text = "";
            this.OutputLocation1Folder.Text = "";
            this.OutputLocation2Folder.Text = "";

           
        }

        private void RecentProjectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectProject(false);
        }

        private void SelectProject(bool Open)
        {
            if (RecentProjectList.SelectedIndex >= 0)
            {
                ProjectMRUItem item = (ProjectMRUItem)RecentProjectList.Items[RecentProjectList.SelectedIndex];

                PopulateProjectDetailsFromFile(item.Path);

                if (Open)
                {
                    OpenProject();
                }
            }
        }

        private void OpenProject()
        {
            try
            {


                if (PerformValidations())
                {

                    if (!Directory.Exists(ProjectFolder.Text))
                    {
                        //create project folder if it does not exist
                        Directory.CreateDirectory(ProjectFolder.Text);
                    }

                    //populate project folder
                    Project p = PopulateProjectObjectFromForm();

                    //save file
                    this.ProjectFile = String.Format("{0}\\{1}.codetorch", this.ProjectFolder.Text, ProjectName.Text);
                    this.Project = p;

                    ConfigurationLoader.SerializeObjectToFile(p, this.ProjectFile);

                    //add to recently opened list
                    ProjectMRUItem item = new ProjectMRUItem(ProjectName.Text, this.ProjectFile);
                    AddToMRU(item);

                    Core.Configuration.GetInstance().ConfigurationPath = ConfigurationPath;
         
                    this.DialogResult = DialogResult.OK;


                }
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
        }

        private void RecentProjectList_DoubleClick(object sender, EventArgs e)
        {
            SelectProject(true);
        }

        private void SelectProjectFolder_Click(object sender, EventArgs e)
        {
            folderDialog.Description = "Please Select Project Folder";

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                this.ProjectFolder.Text = folderDialog.SelectedPath;
            }
        }

        private void SelectOutputLocation1Folder_Click(object sender, EventArgs e)
        {
            folderDialog.Description = "Please Select Output Location Folder";

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                this.OutputLocation1Folder.Text = folderDialog.SelectedPath;
            }
        }

        private void SelectOutputLocation2Folder_Click(object sender, EventArgs e)
        {
            folderDialog.Description = "Please Select Output Location Folder";

            if (folderDialog.ShowDialog() == DialogResult.OK)
            {
                this.OutputLocation2Folder.Text = folderDialog.SelectedPath;
            }
        }

        private void OpenExistingProject_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                fileDialog.Filter = "CodeTorch Project|*.codetorch";

                if(Directory.Exists(String.Format("{0}\\CodeTorch", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))))
                {
                    fileDialog.InitialDirectory = String.Format("{0}\\CodeTorch", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
                }
                else
                {
                    fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                }

                
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = fileDialog.FileName;

                    PopulateProjectDetailsFromFile(fileName);
                    OpenProject();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
        }
    }
}
