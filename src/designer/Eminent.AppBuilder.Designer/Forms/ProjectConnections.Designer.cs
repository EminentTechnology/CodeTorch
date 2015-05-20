namespace CodeTorch.Designer.Forms
{
    partial class ProjectConnections
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
            this.ConnectionsList = new System.Windows.Forms.ListBox();
            this.PropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.ConnectionsLabel = new System.Windows.Forms.Label();
            this.DataConnectionLabel = new System.Windows.Forms.Label();
            this.Save = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ConnectionsList
            // 
            this.ConnectionsList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ConnectionsList.FormattingEnabled = true;
            this.ConnectionsList.Location = new System.Drawing.Point(12, 42);
            this.ConnectionsList.Name = "ConnectionsList";
            this.ConnectionsList.Size = new System.Drawing.Size(288, 407);
            this.ConnectionsList.TabIndex = 0;
            this.ConnectionsList.SelectedIndexChanged += new System.EventHandler(this.ConnectionsList_SelectedIndexChanged);
            // 
            // PropertyGrid
            // 
            this.PropertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PropertyGrid.Enabled = false;
            this.PropertyGrid.Location = new System.Drawing.Point(323, 42);
            this.PropertyGrid.Name = "PropertyGrid";
            this.PropertyGrid.Size = new System.Drawing.Size(415, 407);
            this.PropertyGrid.TabIndex = 1;
            // 
            // ConnectionsLabel
            // 
            this.ConnectionsLabel.AutoSize = true;
            this.ConnectionsLabel.Location = new System.Drawing.Point(13, 23);
            this.ConnectionsLabel.Name = "ConnectionsLabel";
            this.ConnectionsLabel.Size = new System.Drawing.Size(92, 13);
            this.ConnectionsLabel.TabIndex = 2;
            this.ConnectionsLabel.Text = "Data Connections";
            // 
            // DataConnectionLabel
            // 
            this.DataConnectionLabel.AutoSize = true;
            this.DataConnectionLabel.Location = new System.Drawing.Point(320, 23);
            this.DataConnectionLabel.Name = "DataConnectionLabel";
            this.DataConnectionLabel.Size = new System.Drawing.Size(137, 13);
            this.DataConnectionLabel.TabIndex = 3;
            this.DataConnectionLabel.Text = "Data Connection Properties";
            // 
            // Save
            // 
            this.Save.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Save.Location = new System.Drawing.Point(567, 467);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 4;
            this.Save.Text = "Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(663, 467);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 5;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // ProjectConnections
            // 
            this.AcceptButton = this.Save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(750, 502);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.DataConnectionLabel);
            this.Controls.Add(this.ConnectionsLabel);
            this.Controls.Add(this.PropertyGrid);
            this.Controls.Add(this.ConnectionsList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(750, 500);
            this.Name = "ProjectConnections";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Project Connections";
            this.Load += new System.EventHandler(this.ProjectConnections_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox ConnectionsList;
        private System.Windows.Forms.PropertyGrid PropertyGrid;
        private System.Windows.Forms.Label ConnectionsLabel;
        private System.Windows.Forms.Label DataConnectionLabel;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Button Cancel;
    }
}