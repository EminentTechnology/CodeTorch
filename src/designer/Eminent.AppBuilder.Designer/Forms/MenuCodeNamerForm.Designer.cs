namespace CodeTorch.Designer.Forms
{
    partial class MenuCodeNamerForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.StartButton = new System.Windows.Forms.Button();
            this.ForceRename = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(456, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "This screen will attempt to create a code for all menu items that do not have a c" +
    "ode set already.\r\nThe code will be based of the menu name.";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(15, 50);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(453, 23);
            this.progressBar.TabIndex = 1;
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(379, 79);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(89, 23);
            this.StartButton.TabIndex = 2;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // ForceRename
            // 
            this.ForceRename.AutoSize = true;
            this.ForceRename.Location = new System.Drawing.Point(16, 83);
            this.ForceRename.Name = "ForceRename";
            this.ForceRename.Size = new System.Drawing.Size(199, 17);
            this.ForceRename.TabIndex = 3;
            this.ForceRename.Text = "Force rename of all menu item codes";
            this.ForceRename.UseVisualStyleBackColor = true;
            // 
            // MenuCodeNamerForm
            // 
            this.AcceptButton = this.StartButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 113);
            this.Controls.Add(this.ForceRename);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "MenuCodeNamerForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Menu Code Generator";
            this.Load += new System.EventHandler(this.MenuCodeNamerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.CheckBox ForceRename;
    }
}