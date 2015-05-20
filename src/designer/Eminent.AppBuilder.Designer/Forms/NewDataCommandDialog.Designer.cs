namespace CodeTorch.Designer.Forms
{
    partial class NewDataCommandDialog
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
            this.CommandTypeList = new System.Windows.Forms.ComboBox();
            this.CommandText = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.Cancel = new System.Windows.Forms.Button();
            this.Save = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.CommandName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.CommandReturnTypeList = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.DataConnectionList = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // CommandTypeList
            // 
            this.CommandTypeList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CommandTypeList.FormattingEnabled = true;
            this.CommandTypeList.Location = new System.Drawing.Point(106, 70);
            this.CommandTypeList.Name = "CommandTypeList";
            this.CommandTypeList.Size = new System.Drawing.Size(185, 21);
            this.CommandTypeList.TabIndex = 15;
            // 
            // CommandText
            // 
            this.CommandText.Location = new System.Drawing.Point(15, 143);
            this.CommandText.Multiline = true;
            this.CommandText.Name = "CommandText";
            this.CommandText.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.CommandText.Size = new System.Drawing.Size(467, 125);
            this.CommandText.TabIndex = 19;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 18;
            this.label4.Text = "Command Text:";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // Cancel
            // 
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(407, 285);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 23;
            this.Cancel.Text = "&Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(326, 285);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(75, 23);
            this.Save.TabIndex = 22;
            this.Save.Text = "&Save";
            this.Save.UseVisualStyleBackColor = true;
            this.Save.Click += new System.EventHandler(this.Save_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 14;
            this.label2.Text = "Command Type:";
            // 
            // CommandName
            // 
            this.CommandName.Location = new System.Drawing.Point(106, 9);
            this.CommandName.Name = "CommandName";
            this.CommandName.Size = new System.Drawing.Size(376, 20);
            this.CommandName.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Command Name:";
            // 
            // CommandReturnTypeList
            // 
            this.CommandReturnTypeList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CommandReturnTypeList.FormattingEnabled = true;
            this.CommandReturnTypeList.Items.AddRange(new object[] {
            "DataTable",
            "Integer",
            "Xml"});
            this.CommandReturnTypeList.Location = new System.Drawing.Point(106, 95);
            this.CommandReturnTypeList.Name = "CommandReturnTypeList";
            this.CommandReturnTypeList.Size = new System.Drawing.Size(185, 21);
            this.CommandReturnTypeList.TabIndex = 25;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Return Type:";
            // 
            // DataConnectionList
            // 
            this.DataConnectionList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DataConnectionList.FormattingEnabled = true;
            this.DataConnectionList.Items.AddRange(new object[] {
            "StoredProcedure",
            "Text"});
            this.DataConnectionList.Location = new System.Drawing.Point(106, 43);
            this.DataConnectionList.Name = "DataConnectionList";
            this.DataConnectionList.Size = new System.Drawing.Size(185, 21);
            this.DataConnectionList.TabIndex = 27;
            this.DataConnectionList.SelectedIndexChanged += new System.EventHandler(this.DataConnectionList_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 26;
            this.label5.Text = "Connection:";
            // 
            // NewDataCommandDialog
            // 
            this.AcceptButton = this.Save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(529, 351);
            this.Controls.Add(this.DataConnectionList);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.CommandReturnTypeList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.CommandTypeList);
            this.Controls.Add(this.CommandText);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CommandName);
            this.Controls.Add(this.label1);
            this.Name = "NewDataCommandDialog";
            this.Text = "Data Command Dialog";
            this.Load += new System.EventHandler(this.DataCommandDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CommandTypeList;
        private System.Windows.Forms.TextBox CommandText;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button Save;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox CommandName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CommandReturnTypeList;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox DataConnectionList;
        private System.Windows.Forms.Label label5;
    }
}