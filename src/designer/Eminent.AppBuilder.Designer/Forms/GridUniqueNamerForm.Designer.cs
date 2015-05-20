namespace CodeTorch.Designer.Forms
{
    partial class GridUniqueNamerForm
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
            this.ForceRename = new System.Windows.Forms.CheckBox();
            this.StartButton = new System.Windows.Forms.Button();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ForceRename
            // 
            this.ForceRename.AutoSize = true;
            this.ForceRename.Location = new System.Drawing.Point(11, 79);
            this.ForceRename.Name = "ForceRename";
            this.ForceRename.Size = new System.Drawing.Size(178, 17);
            this.ForceRename.TabIndex = 7;
            this.ForceRename.Text = "Force rename of all grid columns";
            this.ForceRename.UseVisualStyleBackColor = true;
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(374, 75);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(89, 23);
            this.StartButton.TabIndex = 6;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(10, 46);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(453, 23);
            this.progressBar.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(406, 26);
            this.label1.TabIndex = 4;
            this.label1.Text = "This screen will attempt to create a unique name for all grid columns that do not" +
    " have\r\na unique name set already.";
            // 
            // GridUniqueNamerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 103);
            this.Controls.Add(this.ForceRename);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.label1);
            this.Name = "GridUniqueNamerForm";
            this.Text = "Grid Unique Name Generator";
            this.Load += new System.EventHandler(this.GridUniqueNamerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox ForceRename;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label label1;
    }
}