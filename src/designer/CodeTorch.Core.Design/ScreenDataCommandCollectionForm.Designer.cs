namespace CodeTorch.Core.Design
{
    partial class ScreenDataCommandCollectionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScreenDataCommandCollectionForm));
            this.AvailableDataCommandsLabel = new System.Windows.Forms.Label();
            this.AvailableCommandList = new System.Windows.Forms.ListBox();
            this.PageCommandList = new System.Windows.Forms.ListBox();
            this.PageDataCommandsLabel = new System.Windows.Forms.Label();
            this.DataCommandPanel = new System.Windows.Forms.Panel();
            this.parameterPropertyGrid = new System.Windows.Forms.PropertyGrid();
            this.AddDataCommand = new System.Windows.Forms.Button();
            this.RemoveDataCommand = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.ParameterList = new System.Windows.Forms.ListBox();
            this.SelectedDataCommandLabel = new System.Windows.Forms.Label();
            this.SelectedParameterLabel = new System.Windows.Forms.Label();
            this.Cancel = new System.Windows.Forms.Button();
            this.DataCommandPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // AvailableDataCommandsLabel
            // 
            this.AvailableDataCommandsLabel.AutoSize = true;
            this.AvailableDataCommandsLabel.Location = new System.Drawing.Point(13, 13);
            this.AvailableDataCommandsLabel.Name = "AvailableDataCommandsLabel";
            this.AvailableDataCommandsLabel.Size = new System.Drawing.Size(134, 13);
            this.AvailableDataCommandsLabel.TabIndex = 0;
            this.AvailableDataCommandsLabel.Text = "Available Data Commands:";
            // 
            // AvailableCommandList
            // 
            this.AvailableCommandList.FormattingEnabled = true;
            this.AvailableCommandList.Location = new System.Drawing.Point(13, 30);
            this.AvailableCommandList.Name = "AvailableCommandList";
            this.AvailableCommandList.Size = new System.Drawing.Size(243, 160);
            this.AvailableCommandList.Sorted = true;
            this.AvailableCommandList.TabIndex = 1;
            // 
            // PageCommandList
            // 
            this.PageCommandList.FormattingEnabled = true;
            this.PageCommandList.Location = new System.Drawing.Point(16, 221);
            this.PageCommandList.Name = "PageCommandList";
            this.PageCommandList.Size = new System.Drawing.Size(243, 251);
            this.PageCommandList.Sorted = true;
            this.PageCommandList.TabIndex = 4;
            this.PageCommandList.SelectedIndexChanged += new System.EventHandler(this.PageCommandList_SelectedIndexChanged);
            // 
            // PageDataCommandsLabel
            // 
            this.PageDataCommandsLabel.AutoSize = true;
            this.PageDataCommandsLabel.Location = new System.Drawing.Point(13, 204);
            this.PageDataCommandsLabel.Name = "PageDataCommandsLabel";
            this.PageDataCommandsLabel.Size = new System.Drawing.Size(130, 13);
            this.PageDataCommandsLabel.TabIndex = 3;
            this.PageDataCommandsLabel.Text = "Selected Data Commands";
            // 
            // DataCommandPanel
            // 
            this.DataCommandPanel.Controls.Add(this.parameterPropertyGrid);
            this.DataCommandPanel.Location = new System.Drawing.Point(320, 218);
            this.DataCommandPanel.Name = "DataCommandPanel";
            this.DataCommandPanel.Size = new System.Drawing.Size(268, 251);
            this.DataCommandPanel.TabIndex = 5;
            // 
            // parameterPropertyGrid
            // 
            this.parameterPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.parameterPropertyGrid.Enabled = false;
            this.parameterPropertyGrid.Location = new System.Drawing.Point(0, 0);
            this.parameterPropertyGrid.Name = "parameterPropertyGrid";
            this.parameterPropertyGrid.Size = new System.Drawing.Size(268, 251);
            this.parameterPropertyGrid.TabIndex = 1;
            // 
            // AddDataCommand
            // 
            this.AddDataCommand.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("AddDataCommand.BackgroundImage")));
            this.AddDataCommand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.AddDataCommand.FlatAppearance.BorderSize = 0;
            this.AddDataCommand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddDataCommand.Location = new System.Drawing.Point(262, 30);
            this.AddDataCommand.Name = "AddDataCommand";
            this.AddDataCommand.Size = new System.Drawing.Size(23, 22);
            this.AddDataCommand.TabIndex = 2;
            this.AddDataCommand.UseVisualStyleBackColor = true;
            this.AddDataCommand.Click += new System.EventHandler(this.AddDataCommand_Click);
            // 
            // RemoveDataCommand
            // 
            this.RemoveDataCommand.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("RemoveDataCommand.BackgroundImage")));
            this.RemoveDataCommand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.RemoveDataCommand.FlatAppearance.BorderSize = 0;
            this.RemoveDataCommand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.RemoveDataCommand.Location = new System.Drawing.Point(265, 221);
            this.RemoveDataCommand.Name = "RemoveDataCommand";
            this.RemoveDataCommand.Size = new System.Drawing.Size(23, 22);
            this.RemoveDataCommand.TabIndex = 5;
            this.RemoveDataCommand.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.RemoveDataCommand.UseVisualStyleBackColor = true;
            this.RemoveDataCommand.Click += new System.EventHandler(this.RemoveDataCommand_Click);
            // 
            // Close
            // 
            this.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CloseButton.Location = new System.Drawing.Point(432, 509);
            this.CloseButton.Name = "Close";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 9;
            this.CloseButton.Text = "Save";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.Close_Click);
            // 
            // ParameterList
            // 
            this.ParameterList.FormattingEnabled = true;
            this.ParameterList.Location = new System.Drawing.Point(320, 30);
            this.ParameterList.Name = "ParameterList";
            this.ParameterList.Size = new System.Drawing.Size(268, 160);
            this.ParameterList.TabIndex = 7;
            this.ParameterList.SelectedIndexChanged += new System.EventHandler(this.ParameterList_SelectedIndexChanged);
            // 
            // SelectedDataCommandLabel
            // 
            this.SelectedDataCommandLabel.AutoSize = true;
            this.SelectedDataCommandLabel.Location = new System.Drawing.Point(317, 13);
            this.SelectedDataCommandLabel.Name = "SelectedDataCommandLabel";
            this.SelectedDataCommandLabel.Size = new System.Drawing.Size(77, 13);
            this.SelectedDataCommandLabel.TabIndex = 6;
            this.SelectedDataCommandLabel.Text = "{0} Parameters";
            // 
            // SelectedParameterLabel
            // 
            this.SelectedParameterLabel.AutoSize = true;
            this.SelectedParameterLabel.Location = new System.Drawing.Point(317, 201);
            this.SelectedParameterLabel.Name = "SelectedParameterLabel";
            this.SelectedParameterLabel.Size = new System.Drawing.Size(72, 13);
            this.SelectedParameterLabel.TabIndex = 8;
            this.SelectedParameterLabel.Text = "{0} Parameter";
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(513, 509);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 10;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // ScreenDataCommandCollectionForm
            // 
            this.AcceptButton = this.CloseButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(609, 544);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.SelectedParameterLabel);
            this.Controls.Add(this.ParameterList);
            this.Controls.Add(this.SelectedDataCommandLabel);
            this.Controls.Add(this.CloseButton);
            this.Controls.Add(this.RemoveDataCommand);
            this.Controls.Add(this.AddDataCommand);
            this.Controls.Add(this.DataCommandPanel);
            this.Controls.Add(this.PageCommandList);
            this.Controls.Add(this.PageDataCommandsLabel);
            this.Controls.Add(this.AvailableCommandList);
            this.Controls.Add(this.AvailableDataCommandsLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ScreenDataCommandCollectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Data Commands Collection Editor";
            this.Load += new System.EventHandler(this.ScreenDataCommandCollectionForm_Load);
            this.DataCommandPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label AvailableDataCommandsLabel;
        private System.Windows.Forms.ListBox AvailableCommandList;
        private System.Windows.Forms.ListBox PageCommandList;
        private System.Windows.Forms.Label PageDataCommandsLabel;
        private System.Windows.Forms.Panel DataCommandPanel;
        private System.Windows.Forms.PropertyGrid parameterPropertyGrid;
        private System.Windows.Forms.Button AddDataCommand;
        private System.Windows.Forms.Button RemoveDataCommand;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.ListBox ParameterList;
        private System.Windows.Forms.Label SelectedDataCommandLabel;
        private System.Windows.Forms.Label SelectedParameterLabel;
        private System.Windows.Forms.Button Cancel;
        
    }
}