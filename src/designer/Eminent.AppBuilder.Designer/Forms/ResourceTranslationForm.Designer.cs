namespace CodeTorch.Designer.Forms
{
    partial class ResourceTranslationForm
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
            this.DefaultCultureCode = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CultureCode = new System.Windows.Forms.TextBox();
            this.UpdateCultureFromConfig = new System.Windows.Forms.RadioButton();
            this.UpdateCultureFromGoogle = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.GoogleAPIKey = new System.Windows.Forms.TextBox();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.StartButton = new System.Windows.Forms.Button();
            this.ForceUpdate = new System.Windows.Forms.CheckBox();
            this.UpdateCultureFromGoogleUsingDB = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Default Culture Code:";
            // 
            // DefaultCultureCode
            // 
            this.DefaultCultureCode.AutoSize = true;
            this.DefaultCultureCode.Location = new System.Drawing.Point(123, 13);
            this.DefaultCultureCode.Name = "DefaultCultureCode";
            this.DefaultCultureCode.Size = new System.Drawing.Size(107, 13);
            this.DefaultCultureCode.TabIndex = 1;
            this.DefaultCultureCode.Text = "{DefaultCultureCode}";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Culture Code:";
            // 
            // CultureCode
            // 
            this.CultureCode.Location = new System.Drawing.Point(126, 34);
            this.CultureCode.Name = "CultureCode";
            this.CultureCode.Size = new System.Drawing.Size(100, 20);
            this.CultureCode.TabIndex = 3;
            // 
            // UpdateCultureFromConfig
            // 
            this.UpdateCultureFromConfig.AutoSize = true;
            this.UpdateCultureFromConfig.Checked = true;
            this.UpdateCultureFromConfig.Location = new System.Drawing.Point(126, 93);
            this.UpdateCultureFromConfig.Name = "UpdateCultureFromConfig";
            this.UpdateCultureFromConfig.Size = new System.Drawing.Size(273, 17);
            this.UpdateCultureFromConfig.TabIndex = 4;
            this.UpdateCultureFromConfig.TabStop = true;
            this.UpdateCultureFromConfig.Text = "Update culture with default values from configuration";
            this.UpdateCultureFromConfig.UseVisualStyleBackColor = true;
            this.UpdateCultureFromConfig.CheckedChanged += new System.EventHandler(this.UpdateCultureFromConfig_CheckedChanged);
            // 
            // UpdateCultureFromGoogle
            // 
            this.UpdateCultureFromGoogle.AutoSize = true;
            this.UpdateCultureFromGoogle.Location = new System.Drawing.Point(126, 116);
            this.UpdateCultureFromGoogle.Name = "UpdateCultureFromGoogle";
            this.UpdateCultureFromGoogle.Size = new System.Drawing.Size(299, 17);
            this.UpdateCultureFromGoogle.TabIndex = 5;
            this.UpdateCultureFromGoogle.Text = "Update culture with translations from Google (using config)";
            this.UpdateCultureFromGoogle.UseVisualStyleBackColor = true;
            this.UpdateCultureFromGoogle.CheckedChanged += new System.EventHandler(this.UpdateCultureFromGoogle_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Google API Key:";
            // 
            // GoogleAPIKey
            // 
            this.GoogleAPIKey.Enabled = false;
            this.GoogleAPIKey.Location = new System.Drawing.Point(126, 60);
            this.GoogleAPIKey.Name = "GoogleAPIKey";
            this.GoogleAPIKey.Size = new System.Drawing.Size(273, 20);
            this.GoogleAPIKey.TabIndex = 7;
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 192);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(413, 23);
            this.progressBar.TabIndex = 8;
            // 
            // StartButton
            // 
            this.StartButton.Location = new System.Drawing.Point(350, 221);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(75, 23);
            this.StartButton.TabIndex = 9;
            this.StartButton.Text = "Start";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.StartButton_Click);
            // 
            // ForceUpdate
            // 
            this.ForceUpdate.AutoSize = true;
            this.ForceUpdate.Location = new System.Drawing.Point(126, 159);
            this.ForceUpdate.Name = "ForceUpdate";
            this.ForceUpdate.Size = new System.Drawing.Size(198, 17);
            this.ForceUpdate.TabIndex = 10;
            this.ForceUpdate.Text = "Force update on existing translations";
            this.ForceUpdate.UseVisualStyleBackColor = true;
            // 
            // UpdateCultureFromGoogleUsingDB
            // 
            this.UpdateCultureFromGoogleUsingDB.AutoSize = true;
            this.UpdateCultureFromGoogleUsingDB.Location = new System.Drawing.Point(126, 137);
            this.UpdateCultureFromGoogleUsingDB.Name = "UpdateCultureFromGoogleUsingDB";
            this.UpdateCultureFromGoogleUsingDB.Size = new System.Drawing.Size(282, 17);
            this.UpdateCultureFromGoogleUsingDB.TabIndex = 11;
            this.UpdateCultureFromGoogleUsingDB.Text = "Update culture with translations from Google (using db)";
            this.UpdateCultureFromGoogleUsingDB.UseVisualStyleBackColor = true;
            this.UpdateCultureFromGoogleUsingDB.CheckedChanged += new System.EventHandler(this.UpdateCultureFromGoogleUsingDB_CheckedChanged);
            // 
            // ResourceTranslationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 266);
            this.Controls.Add(this.UpdateCultureFromGoogleUsingDB);
            this.Controls.Add(this.ForceUpdate);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.GoogleAPIKey);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.UpdateCultureFromGoogle);
            this.Controls.Add(this.UpdateCultureFromConfig);
            this.Controls.Add(this.CultureCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DefaultCultureCode);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ResourceTranslationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Resource Translation Form";
            this.Load += new System.EventHandler(this.ResourceTranslationForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label DefaultCultureCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox CultureCode;
        private System.Windows.Forms.RadioButton UpdateCultureFromConfig;
        private System.Windows.Forms.RadioButton UpdateCultureFromGoogle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox GoogleAPIKey;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.CheckBox ForceUpdate;
        private System.Windows.Forms.RadioButton UpdateCultureFromGoogleUsingDB;
    }
}