namespace CodeTorch.Designer.Forms
{
    partial class NewScreenDialog2
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
            this.Cancel = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.PageName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PageTitle = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Folder = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.PageSubTitle = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.PageTypeList = new System.Windows.Forms.ComboBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(205, 145);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 11;
            this.Cancel.Text = "&Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(124, 145);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 10;
            this.Save.Text = "&Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Screen Type:";
            // 
            // PageName
            // 
            this.PageName.Location = new System.Drawing.Point(95, 6);
            this.PageName.Name = "PageName";
            this.PageName.Size = new System.Drawing.Size(185, 20);
            this.PageName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Screen Name:";
            // 
            // PageTitle
            // 
            this.PageTitle.Location = new System.Drawing.Point(95, 84);
            this.PageTitle.Name = "PageTitle";
            this.PageTitle.Size = new System.Drawing.Size(185, 20);
            this.PageTitle.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Screen Title:";
            // 
            // Folder
            // 
            this.Folder.AcceptsReturn = true;
            this.Folder.Location = new System.Drawing.Point(95, 58);
            this.Folder.Name = "Folder";
            this.Folder.Size = new System.Drawing.Size(185, 20);
            this.Folder.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(39, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Folder:";
            // 
            // PageSubTitle
            // 
            this.PageSubTitle.Location = new System.Drawing.Point(95, 110);
            this.PageSubTitle.Name = "PageSubTitle";
            this.PageSubTitle.Size = new System.Drawing.Size(185, 20);
            this.PageSubTitle.TabIndex = 9;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 113);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(82, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Screen Subtitle:";
            // 
            // PageTypeList
            // 
            this.PageTypeList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PageTypeList.FormattingEnabled = true;
            this.PageTypeList.Location = new System.Drawing.Point(95, 32);
            this.PageTypeList.Name = "PageTypeList";
            this.PageTypeList.Size = new System.Drawing.Size(185, 21);
            this.PageTypeList.TabIndex = 3;
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // NewScreenDialog
            // 
            this.AcceptButton = this.Save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(295, 179);
            this.Controls.Add(this.PageTypeList);
            this.Controls.Add(this.PageSubTitle);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.PageTitle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Folder);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PageName);
            this.Controls.Add(this.label1);
            this.Name = "NewScreenDialog";
            this.Text = "New Screen Dialog";
            this.Load += new System.EventHandler(this.NewPageDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox PageName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PageTitle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Folder;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox PageSubTitle;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox PageTypeList;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}