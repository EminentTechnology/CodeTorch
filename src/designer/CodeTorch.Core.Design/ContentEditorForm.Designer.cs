
using Telerik.WinControls.UI;
//using Telerik.Windows.Controls;
namespace CodeTorch.Core.Design
{
    partial class ContentEditorForm
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
            this.markup = new RadTextBoxControl();
            this.Cancel = new Telerik.WinControls.UI.RadButton();
            this.Apply = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.markup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Apply)).BeginInit();
            this.SuspendLayout();
            // 
            // markup
            // 
            this.markup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
           
            this.markup.Location = new System.Drawing.Point(0, 0);
            this.markup.Multiline = true;

            this.markup.Name = "markup";
            this.markup.Size = new System.Drawing.Size(686, 432);
            this.markup.TabIndex = 0;
            this.markup.Click += new System.EventHandler(this.markup_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(567, 454);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(110, 24);
            this.Cancel.TabIndex = 2;
            this.Cancel.Text = "Cancel";
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // Apply
            // 
            this.Apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Apply.Location = new System.Drawing.Point(425, 454);
            this.Apply.Name = "Apply";
            this.Apply.Size = new System.Drawing.Size(110, 24);
            this.Apply.TabIndex = 3;
            this.Apply.Text = "Apply";
            this.Apply.Click += new System.EventHandler(this.Apply_Click);
            // 
            // ContentEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 490);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Apply);
            this.Controls.Add(this.markup);
            this.Name = "ContentEditorForm";
            this.Text = "Content Editor";
            ((System.ComponentModel.ISupportInitialize)(this.markup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Apply)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private RadTextBoxControl markup;
        private RadButton Cancel;
        private RadButton Apply;
    }
}