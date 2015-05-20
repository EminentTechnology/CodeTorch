namespace CodeTorch.Designer.Editors
{
    partial class MarkupEditorForm
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
            this.Apply = new Telerik.WinControls.UI.RadButton();
            this.Cancel = new Telerik.WinControls.UI.RadButton();
            this.markup = new Telerik.WinControls.RichTextBox.RadRichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.Apply)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.markup)).BeginInit();
            this.SuspendLayout();
            // 
            // Apply
            // 
            this.Apply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Apply.Location = new System.Drawing.Point(396, 438);
            this.Apply.Name = "Apply";
            this.Apply.Size = new System.Drawing.Size(110, 24);
            this.Apply.TabIndex = 1;
            this.Apply.Text = "Apply";
            this.Apply.Click += new System.EventHandler(this.Apply_Click);
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(534, 438);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(110, 24);
            this.Cancel.TabIndex = 0;
            this.Cancel.Text = "Cancel";
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // markup
            // 
            this.markup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.markup.HyperlinkToolTipFormatString = null;
            this.markup.Location = new System.Drawing.Point(0, 0);
            this.markup.Name = "markup";
            this.markup.Size = new System.Drawing.Size(653, 423);
            this.markup.TabIndex = 2;
            this.markup.Text = "radRichTextBox1";
            // 
            // MarkupEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(656, 473);
            this.Controls.Add(this.markup);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.Apply);
            this.Name = "MarkupEditorForm";
            this.Text = "MarkupEditorForm";
            ((System.ComponentModel.ISupportInitialize)(this.Apply)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.markup)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadButton Apply;
        private Telerik.WinControls.UI.RadButton Cancel;
        private Telerik.WinControls.RichTextBox.RadRichTextBox markup;
    }
}