namespace CodeTorch.Designer.UserControls
{
    partial class SectionDesigner
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SectionsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.AddControl = new Telerik.WinControls.UI.RadDropDownButton();
            this.SectionLabel = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AddControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SectionLabel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            this.SuspendLayout();
            // 
            // SectionsPanel
            // 
            this.SectionsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SectionsPanel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.SectionsPanel.Location = new System.Drawing.Point(3, 43);
            this.SectionsPanel.Name = "SectionsPanel";
            this.SectionsPanel.Size = new System.Drawing.Size(669, 25);
            this.SectionsPanel.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.SectionsPanel, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(675, 71);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.AddControl);
            this.panel1.Controls.Add(this.SectionLabel);
            this.panel1.Controls.Add(this.radLabel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(669, 34);
            this.panel1.TabIndex = 0;
            // 
            // AddControl
            // 
            this.AddControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddControl.Location = new System.Drawing.Point(559, 3);
            this.AddControl.Name = "AddControl";
            this.AddControl.Size = new System.Drawing.Size(107, 24);
            this.AddControl.TabIndex = 0;
            this.AddControl.Text = "Add Control";
            // 
            // SectionLabel
            // 
            this.SectionLabel.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.SectionLabel.Location = new System.Drawing.Point(75, 4);
            this.SectionLabel.Name = "SectionLabel";
            this.SectionLabel.Size = new System.Drawing.Size(110, 25);
            this.SectionLabel.TabIndex = 1;
            this.SectionLabel.Text = "{SectionLabel}";
            // 
            // radLabel1
            // 
            this.radLabel1.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.radLabel1.Location = new System.Drawing.Point(4, 4);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(65, 25);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "Section:";
            // 
            // SectionDesigner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SectionDesigner";
            this.Size = new System.Drawing.Size(675, 71);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AddControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SectionLabel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel SectionsPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private Telerik.WinControls.UI.RadDropDownButton AddControl;
        private Telerik.WinControls.UI.RadLabel SectionLabel;
        private Telerik.WinControls.UI.RadLabel radLabel1;

    }
}
