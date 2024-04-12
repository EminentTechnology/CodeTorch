namespace CodeTorch.Designer.Forms
{
    partial class LoginDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginDialog));
            this.Open = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.ConfigFolder = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SelectConfigFolder = new System.Windows.Forms.Button();
            this.folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.CreateNewProject = new System.Windows.Forms.LinkLabel();
            this.OpenExistingProject = new System.Windows.Forms.LinkLabel();
            this.RecentProjectList = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ProjectGroupBox = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.ProjectTypeList = new System.Windows.Forms.ComboBox();
            this.ProjectName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.SelectProjectFolder = new System.Windows.Forms.Button();
            this.ProjectFolder = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.SelectOutputLocation2Folder = new System.Windows.Forms.Button();
            this.OutputLocation2Folder = new System.Windows.Forms.TextBox();
            this.SelectOutputLocation1Folder = new System.Windows.Forms.Button();
            this.OutputLocation1Folder = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.RootNamespace = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.fileDialog = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.SelectDatabaseProjectFolder = new System.Windows.Forms.Button();
            this.DatabaseProjectFolder = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.ProjectGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // Open
            // 
            this.Open.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Open.Enabled = false;
            this.Open.Location = new System.Drawing.Point(578, 442);
            this.Open.Name = "Open";
            this.Open.Size = new System.Drawing.Size(75, 23);
            this.Open.TabIndex = 6;
            this.Open.Text = "&Open";
            this.Open.UseVisualStyleBackColor = true;
            this.Open.Click += new System.EventHandler(this.Open_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(659, 442);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 7;
            this.Cancel.Text = "&Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // ConfigFolder
            // 
            this.ConfigFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ConfigFolder.Location = new System.Drawing.Point(124, 173);
            this.ConfigFolder.Name = "ConfigFolder";
            this.ConfigFolder.Size = new System.Drawing.Size(263, 20);
            this.ConfigFolder.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 176);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Config Folder:";
            // 
            // SelectConfigFolder
            // 
            this.SelectConfigFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectConfigFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectConfigFolder.Location = new System.Drawing.Point(393, 173);
            this.SelectConfigFolder.Name = "SelectConfigFolder";
            this.SelectConfigFolder.Size = new System.Drawing.Size(26, 21);
            this.SelectConfigFolder.TabIndex = 11;
            this.SelectConfigFolder.Text = "..";
            this.SelectConfigFolder.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.SelectConfigFolder.UseVisualStyleBackColor = true;
            this.SelectConfigFolder.Click += new System.EventHandler(this.SelectFolder_Click);
            // 
            // folderDialog
            // 
            this.folderDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // CreateNewProject
            // 
            this.CreateNewProject.AutoSize = true;
            this.CreateNewProject.Location = new System.Drawing.Point(12, 9);
            this.CreateNewProject.Name = "CreateNewProject";
            this.CreateNewProject.Size = new System.Drawing.Size(99, 13);
            this.CreateNewProject.TabIndex = 0;
            this.CreateNewProject.TabStop = true;
            this.CreateNewProject.Text = "Create New Project";
            this.CreateNewProject.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.CreateNewProject_LinkClicked);
            // 
            // OpenExistingProject
            // 
            this.OpenExistingProject.AutoSize = true;
            this.OpenExistingProject.Location = new System.Drawing.Point(158, 9);
            this.OpenExistingProject.Name = "OpenExistingProject";
            this.OpenExistingProject.Size = new System.Drawing.Size(108, 13);
            this.OpenExistingProject.TabIndex = 1;
            this.OpenExistingProject.TabStop = true;
            this.OpenExistingProject.Text = "Open Existing Project";
            this.OpenExistingProject.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.OpenExistingProject_LinkClicked);
            // 
            // RecentProjectList
            // 
            this.RecentProjectList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.RecentProjectList.FormattingEnabled = true;
            this.RecentProjectList.Location = new System.Drawing.Point(13, 46);
            this.RecentProjectList.Name = "RecentProjectList";
            this.RecentProjectList.Size = new System.Drawing.Size(254, 381);
            this.RecentProjectList.TabIndex = 3;
            this.RecentProjectList.SelectedIndexChanged += new System.EventHandler(this.RecentProjectList_SelectedIndexChanged);
            this.RecentProjectList.DoubleClick += new System.EventHandler(this.RecentProjectList_DoubleClick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(242, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Recently Opened Projects   (double-click to open)";
            // 
            // ProjectGroupBox
            // 
            this.ProjectGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectGroupBox.Controls.Add(this.SelectDatabaseProjectFolder);
            this.ProjectGroupBox.Controls.Add(this.DatabaseProjectFolder);
            this.ProjectGroupBox.Controls.Add(this.label1);
            this.ProjectGroupBox.Controls.Add(this.label11);
            this.ProjectGroupBox.Controls.Add(this.ProjectTypeList);
            this.ProjectGroupBox.Controls.Add(this.ProjectName);
            this.ProjectGroupBox.Controls.Add(this.label10);
            this.ProjectGroupBox.Controls.Add(this.SelectProjectFolder);
            this.ProjectGroupBox.Controls.Add(this.ProjectFolder);
            this.ProjectGroupBox.Controls.Add(this.label9);
            this.ProjectGroupBox.Controls.Add(this.SelectOutputLocation2Folder);
            this.ProjectGroupBox.Controls.Add(this.OutputLocation2Folder);
            this.ProjectGroupBox.Controls.Add(this.SelectOutputLocation1Folder);
            this.ProjectGroupBox.Controls.Add(this.OutputLocation1Folder);
            this.ProjectGroupBox.Controls.Add(this.label7);
            this.ProjectGroupBox.Controls.Add(this.SelectConfigFolder);
            this.ProjectGroupBox.Controls.Add(this.RootNamespace);
            this.ProjectGroupBox.Controls.Add(this.label8);
            this.ProjectGroupBox.Controls.Add(this.ConfigFolder);
            this.ProjectGroupBox.Controls.Add(this.label5);
            this.ProjectGroupBox.Enabled = false;
            this.ProjectGroupBox.Location = new System.Drawing.Point(300, 46);
            this.ProjectGroupBox.Name = "ProjectGroupBox";
            this.ProjectGroupBox.Size = new System.Drawing.Size(433, 389);
            this.ProjectGroupBox.TabIndex = 4;
            this.ProjectGroupBox.TabStop = false;
            this.ProjectGroupBox.Text = "Project Details";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(16, 55);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(74, 13);
            this.label11.TabIndex = 2;
            this.label11.Text = "Project Name:";
            // 
            // ProjectTypeList
            // 
            this.ProjectTypeList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectTypeList.FormattingEnabled = true;
            this.ProjectTypeList.Location = new System.Drawing.Point(124, 25);
            this.ProjectTypeList.Name = "ProjectTypeList";
            this.ProjectTypeList.Size = new System.Drawing.Size(295, 21);
            this.ProjectTypeList.TabIndex = 1;
            // 
            // ProjectName
            // 
            this.ProjectName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectName.Location = new System.Drawing.Point(124, 52);
            this.ProjectName.Name = "ProjectName";
            this.ProjectName.Size = new System.Drawing.Size(295, 20);
            this.ProjectName.TabIndex = 3;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 28);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Project Type:";
            // 
            // SelectProjectFolder
            // 
            this.SelectProjectFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectProjectFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectProjectFolder.Location = new System.Drawing.Point(392, 78);
            this.SelectProjectFolder.Name = "SelectProjectFolder";
            this.SelectProjectFolder.Size = new System.Drawing.Size(27, 21);
            this.SelectProjectFolder.TabIndex = 6;
            this.SelectProjectFolder.Text = "..";
            this.SelectProjectFolder.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.SelectProjectFolder.UseVisualStyleBackColor = true;
            this.SelectProjectFolder.Click += new System.EventHandler(this.SelectProjectFolder_Click);
            // 
            // ProjectFolder
            // 
            this.ProjectFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProjectFolder.Location = new System.Drawing.Point(124, 78);
            this.ProjectFolder.Name = "ProjectFolder";
            this.ProjectFolder.Size = new System.Drawing.Size(263, 20);
            this.ProjectFolder.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 81);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Project Folder:";
            // 
            // SelectOutputLocation2Folder
            // 
            this.SelectOutputLocation2Folder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectOutputLocation2Folder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectOutputLocation2Folder.Location = new System.Drawing.Point(393, 225);
            this.SelectOutputLocation2Folder.Name = "SelectOutputLocation2Folder";
            this.SelectOutputLocation2Folder.Size = new System.Drawing.Size(26, 21);
            this.SelectOutputLocation2Folder.TabIndex = 16;
            this.SelectOutputLocation2Folder.Text = "..";
            this.SelectOutputLocation2Folder.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.SelectOutputLocation2Folder.UseVisualStyleBackColor = true;
            this.SelectOutputLocation2Folder.Click += new System.EventHandler(this.SelectOutputLocation2Folder_Click);
            // 
            // OutputLocation2Folder
            // 
            this.OutputLocation2Folder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputLocation2Folder.Location = new System.Drawing.Point(124, 225);
            this.OutputLocation2Folder.Name = "OutputLocation2Folder";
            this.OutputLocation2Folder.Size = new System.Drawing.Size(263, 20);
            this.OutputLocation2Folder.TabIndex = 15;
            // 
            // SelectOutputLocation1Folder
            // 
            this.SelectOutputLocation1Folder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectOutputLocation1Folder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectOutputLocation1Folder.Location = new System.Drawing.Point(393, 200);
            this.SelectOutputLocation1Folder.Name = "SelectOutputLocation1Folder";
            this.SelectOutputLocation1Folder.Size = new System.Drawing.Size(26, 21);
            this.SelectOutputLocation1Folder.TabIndex = 14;
            this.SelectOutputLocation1Folder.Text = "..";
            this.SelectOutputLocation1Folder.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.SelectOutputLocation1Folder.UseVisualStyleBackColor = true;
            this.SelectOutputLocation1Folder.Click += new System.EventHandler(this.SelectOutputLocation1Folder_Click);
            // 
            // OutputLocation1Folder
            // 
            this.OutputLocation1Folder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OutputLocation1Folder.Location = new System.Drawing.Point(124, 199);
            this.OutputLocation1Folder.Name = "OutputLocation1Folder";
            this.OutputLocation1Folder.Size = new System.Drawing.Size(263, 20);
            this.OutputLocation1Folder.TabIndex = 13;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 128);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Root Namespace:";
            // 
            // RootNamespace
            // 
            this.RootNamespace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RootNamespace.Location = new System.Drawing.Point(124, 125);
            this.RootNamespace.Name = "RootNamespace";
            this.RootNamespace.Size = new System.Drawing.Size(295, 20);
            this.RootNamespace.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 202);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Output Locations:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 267);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "SQL Project Folder:";
            // 
            // SelectDatabaseProjectFolder
            // 
            this.SelectDatabaseProjectFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SelectDatabaseProjectFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SelectDatabaseProjectFolder.Location = new System.Drawing.Point(393, 264);
            this.SelectDatabaseProjectFolder.Name = "SelectDatabaseProjectFolder";
            this.SelectDatabaseProjectFolder.Size = new System.Drawing.Size(26, 21);
            this.SelectDatabaseProjectFolder.TabIndex = 19;
            this.SelectDatabaseProjectFolder.Text = "..";
            this.SelectDatabaseProjectFolder.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.SelectDatabaseProjectFolder.UseVisualStyleBackColor = true;
            this.SelectDatabaseProjectFolder.Click += new System.EventHandler(this.SelectDatabaseProjectFolder_Click);
            // 
            // DatabaseProjectFolder
            // 
            this.DatabaseProjectFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DatabaseProjectFolder.Location = new System.Drawing.Point(124, 264);
            this.DatabaseProjectFolder.Name = "DatabaseProjectFolder";
            this.DatabaseProjectFolder.Size = new System.Drawing.Size(263, 20);
            this.DatabaseProjectFolder.TabIndex = 18;
            // 
            // LoginDialog
            // 
            this.AcceptButton = this.Open;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(754, 488);
            this.Controls.Add(this.ProjectGroupBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.RecentProjectList);
            this.Controls.Add(this.OpenExistingProject);
            this.Controls.Add(this.CreateNewProject);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Open);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(760, 420);
            this.Name = "LoginDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.LoginDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ProjectGroupBox.ResumeLayout(false);
            this.ProjectGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Open;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Button SelectConfigFolder;
        private System.Windows.Forms.TextBox ConfigFolder;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.FolderBrowserDialog folderDialog;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListBox RecentProjectList;
        private System.Windows.Forms.LinkLabel OpenExistingProject;
        private System.Windows.Forms.LinkLabel CreateNewProject;
        private System.Windows.Forms.GroupBox ProjectGroupBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox RootNamespace;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button SelectOutputLocation2Folder;
        private System.Windows.Forms.TextBox OutputLocation2Folder;
        private System.Windows.Forms.Button SelectOutputLocation1Folder;
        private System.Windows.Forms.TextBox OutputLocation1Folder;
        private System.Windows.Forms.Button SelectProjectFolder;
        private System.Windows.Forms.TextBox ProjectFolder;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox ProjectTypeList;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox ProjectName;
        private System.Windows.Forms.OpenFileDialog fileDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SelectDatabaseProjectFolder;
        private System.Windows.Forms.TextBox DatabaseProjectFolder;
    }
}