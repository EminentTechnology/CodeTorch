namespace CodeTorch.Designer.Forms
{
    partial class MainForm2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm2));
            this.commandBar = new Telerik.WinControls.UI.RadCommandBar();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.commandBarRowElement1 = new Telerik.WinControls.UI.CommandBarRowElement();
            this.commandBarStripElement1 = new Telerik.WinControls.UI.CommandBarStripElement();
            this.cmdNew = new Telerik.WinControls.UI.CommandBarDropDownButton();
            this.cmdSave = new Telerik.WinControls.UI.CommandBarButton();
            this.cmdSaveAll = new Telerik.WinControls.UI.CommandBarButton();
            this.cmdBuild = new Telerik.WinControls.UI.CommandBarButton();
            this.radMenuItem1 = new Telerik.WinControls.UI.RadMenuItem();
            this.menuExit = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem2 = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem3 = new Telerik.WinControls.UI.RadMenuItem();
            this.menuGenerateGridUniqueNames = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem4 = new Telerik.WinControls.UI.RadMenuItem();
            this.menuGenerateMenuItemUniqueCodes = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem5 = new Telerik.WinControls.UI.RadMenuItem();
            this.menuResourceTranslations = new Telerik.WinControls.UI.RadMenuItem();
            this.statusBar = new Telerik.WinControls.UI.RadStatusStrip();
            this.statusBarLabel = new Telerik.WinControls.UI.RadLabelElement();
            this.documentContainer = new Telerik.WinControls.UI.Docking.DocumentContainer();
            this.solutionWindow = new Telerik.WinControls.UI.Docking.ToolWindow();
            this.solutionTree = new Telerik.WinControls.UI.RadTreeView();
            this.solutionFilter = new Telerik.WinControls.UI.RadTextBox();
            this.dock = new Telerik.WinControls.UI.Docking.RadDock();
            this.toolTabStrip1 = new Telerik.WinControls.UI.Docking.ToolTabStrip();
            this.toolWindow1 = new Telerik.WinControls.UI.Docking.ToolWindow();
            this.radMenu1 = new Telerik.WinControls.UI.RadMenu();
            this.radMenuItem6 = new Telerik.WinControls.UI.RadMenuItem();
            this.menuOpen = new Telerik.WinControls.UI.RadMenuItem();
            this.menuEditConnections = new Telerik.WinControls.UI.RadMenuItem();
            this.menuExitNew = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem12 = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuViewSolutionExplorer = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem7 = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem9 = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem10 = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem11 = new Telerik.WinControls.UI.RadMenuItem();
            this.menuGenerateUniqueNames = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem13 = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem14 = new Telerik.WinControls.UI.RadMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.commandBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentContainer)).BeginInit();
            this.solutionWindow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.solutionTree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.solutionFilter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dock)).BeginInit();
            this.dock.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toolTabStrip1)).BeginInit();
            this.toolTabStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radMenu1)).BeginInit();
            this.SuspendLayout();
            // 
            // commandBar
            // 
            this.commandBar.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.commandBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.commandBar.ImageList = this.imageList;
            this.commandBar.Location = new System.Drawing.Point(0, 25);
            this.commandBar.Name = "commandBar";
            // 
            // 
            // 
            this.commandBar.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 20, 25, 25);
            this.commandBar.Rows.AddRange(new Telerik.WinControls.UI.CommandBarRowElement[] {
            this.commandBarRowElement1});
            this.commandBar.Size = new System.Drawing.Size(994, 56);
            this.commandBar.TabIndex = 0;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Magenta;
            this.imageList.Images.SetKeyName(0, "new");
            this.imageList.Images.SetKeyName(1, "save");
            this.imageList.Images.SetKeyName(2, "save.all");
            this.imageList.Images.SetKeyName(3, "build");
            this.imageList.Images.SetKeyName(4, "folder");
            this.imageList.Images.SetKeyName(5, "folder.open");
            this.imageList.Images.SetKeyName(6, "web");
            this.imageList.Images.SetKeyName(7, "lookup");
            this.imageList.Images.SetKeyName(8, "permission");
            this.imageList.Images.SetKeyName(9, "datacommand");
            this.imageList.Images.SetKeyName(10, "sequence");
            this.imageList.Images.SetKeyName(11, "menu");
            this.imageList.Images.SetKeyName(12, "layout");
            this.imageList.Images.SetKeyName(13, "template");
            this.imageList.Images.SetKeyName(14, "dashboard");
            this.imageList.Images.SetKeyName(15, "group");
            this.imageList.Images.SetKeyName(16, "workflow");
            this.imageList.Images.SetKeyName(17, "picker");
            this.imageList.Images.SetKeyName(18, "services");
            this.imageList.Images.SetKeyName(19, "screen");
            // 
            // commandBarRowElement1
            // 
            this.commandBarRowElement1.MinSize = new System.Drawing.Size(25, 25);
            this.commandBarRowElement1.Name = "commandBarRowElement1";
            this.commandBarRowElement1.Strips.AddRange(new Telerik.WinControls.UI.CommandBarStripElement[] {
            this.commandBarStripElement1});
            // 
            // commandBarStripElement1
            // 
            this.commandBarStripElement1.DisplayName = "commandBarStripElement1";
            this.commandBarStripElement1.Items.AddRange(new Telerik.WinControls.UI.RadCommandBarBaseItem[] {
            this.cmdNew,
            this.cmdSave,
            this.cmdSaveAll,
            this.cmdBuild});
            this.commandBarStripElement1.Name = "commandBarStripElement1";
            // 
            // cmdNew
            // 
            this.cmdNew.DisplayName = "cmdNew";
            this.cmdNew.DrawText = true;
            this.cmdNew.EnableImageTransparency = true;
            this.cmdNew.Image = ((System.Drawing.Image)(resources.GetObject("cmdNew.Image")));
            this.cmdNew.ImageIndex = 0;
            this.cmdNew.Name = "cmdNew";
            this.cmdNew.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.cmdNew.Text = "New";
            this.cmdNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            // 
            // cmdSave
            // 
            this.cmdSave.BackColor2 = System.Drawing.Color.Transparent;
            this.cmdSave.DisplayName = "cmdSave";
            this.cmdSave.DrawFill = true;
            this.cmdSave.DrawText = true;
            this.cmdSave.EnableImageTransparency = true;
            this.cmdSave.Image = ((System.Drawing.Image)(resources.GetObject("cmdSave.Image")));
            this.cmdSave.ImageIndex = 1;
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.NumberOfColors = 4;
            this.cmdSave.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.cmdSave.Text = "Save";
            this.cmdSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cmdSave.ToolTipText = "Save (Ctrl+S)";
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // cmdSaveAll
            // 
            this.cmdSaveAll.DisplayName = "commandBarButton2";
            this.cmdSaveAll.DrawText = true;
            this.cmdSaveAll.EnableImageTransparency = true;
            this.cmdSaveAll.Image = ((System.Drawing.Image)(resources.GetObject("cmdSaveAll.Image")));
            this.cmdSaveAll.ImageIndex = 2;
            this.cmdSaveAll.Name = "cmdSaveAll";
            this.cmdSaveAll.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.cmdSaveAll.Text = "Save All";
            this.cmdSaveAll.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cmdSaveAll.ToolTipText = "Save All (Ctrl+Shift+S)";
            this.cmdSaveAll.Click += new System.EventHandler(this.cmdSaveAll_Click);
            // 
            // cmdBuild
            // 
            this.cmdBuild.DisplayName = "commandBarButton2";
            this.cmdBuild.DrawText = true;
            this.cmdBuild.EnableImageTransparency = true;
            this.cmdBuild.Image = ((System.Drawing.Image)(resources.GetObject("cmdBuild.Image")));
            this.cmdBuild.ImageIndex = 3;
            this.cmdBuild.Name = "cmdBuild";
            this.cmdBuild.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.cmdBuild.Text = "Build";
            this.cmdBuild.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cmdBuild.Click += new System.EventHandler(this.cmdBuild_Click);
            // 
            // radMenuItem1
            // 
            this.radMenuItem1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.menuExit});
            this.radMenuItem1.Name = "radMenuItem1";
            this.radMenuItem1.Text = "&File";
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Text = "&Exit";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // radMenuItem2
            // 
            this.radMenuItem2.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radMenuItem3,
            this.radMenuItem4,
            this.radMenuItem5});
            this.radMenuItem2.Name = "radMenuItem2";
            this.radMenuItem2.Text = "&Tools";
            // 
            // radMenuItem3
            // 
            this.radMenuItem3.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.menuGenerateGridUniqueNames});
            this.radMenuItem3.Name = "radMenuItem3";
            this.radMenuItem3.Text = "Grids";
            // 
            // menuGenerateGridUniqueNames
            // 
            this.menuGenerateGridUniqueNames.Name = "menuGenerateGridUniqueNames";
            this.menuGenerateGridUniqueNames.Text = "Generate Unique Names";
            this.menuGenerateGridUniqueNames.Click += new System.EventHandler(this.menuGenerateGridUniqueNames_Click);
            // 
            // radMenuItem4
            // 
            this.radMenuItem4.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.menuGenerateMenuItemUniqueCodes});
            this.radMenuItem4.Name = "radMenuItem4";
            this.radMenuItem4.Text = "Menus";
            // 
            // menuGenerateMenuItemUniqueCodes
            // 
            this.menuGenerateMenuItemUniqueCodes.Name = "menuGenerateMenuItemUniqueCodes";
            this.menuGenerateMenuItemUniqueCodes.Text = "Generate Menu Item Codes";
            this.menuGenerateMenuItemUniqueCodes.Click += new System.EventHandler(this.menuGenerateMenuItemUniqueCodes_Click);
            // 
            // radMenuItem5
            // 
            this.radMenuItem5.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.menuResourceTranslations});
            this.radMenuItem5.Name = "radMenuItem5";
            this.radMenuItem5.Text = "Localization";
            // 
            // menuResourceTranslations
            // 
            this.menuResourceTranslations.Name = "menuResourceTranslations";
            this.menuResourceTranslations.Text = "Resource Translations";
            this.menuResourceTranslations.Click += new System.EventHandler(this.menuResourceTranslations_Click);
            // 
            // statusBar
            // 
            this.statusBar.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.statusBar.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.statusBarLabel});
            this.statusBar.Location = new System.Drawing.Point(0, 385);
            this.statusBar.Name = "statusBar";
            // 
            // 
            // 
            this.statusBar.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 385, 300, 24);
            this.statusBar.RootElement.StretchVertically = true;
            this.statusBar.Size = new System.Drawing.Size(994, 24);
            this.statusBar.TabIndex = 1;
            // 
            // statusBarLabel
            // 
            this.statusBarLabel.Name = "statusBarLabel";
            this.statusBar.SetSpring(this.statusBarLabel, false);
            this.statusBarLabel.Text = "";
            this.statusBarLabel.TextWrap = true;
            // 
            // documentContainer
            // 
            this.documentContainer.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.documentContainer.Name = "documentContainer";
            this.documentContainer.Padding = new System.Windows.Forms.Padding(0);
            // 
            // 
            // 
            this.documentContainer.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 0, 200, 200);
            this.documentContainer.RootElement.MinSize = new System.Drawing.Size(0, 0);
            this.documentContainer.SizeInfo.SizeMode = Telerik.WinControls.UI.Docking.SplitPanelSizeMode.Fill;
            this.documentContainer.SplitterWidth = 8;
            this.documentContainer.TabIndex = 2;
            // 
            // solutionWindow
            // 
            this.solutionWindow.Caption = null;
            this.solutionWindow.Controls.Add(this.solutionTree);
            this.solutionWindow.Controls.Add(this.solutionFilter);
            this.solutionWindow.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.solutionWindow.Location = new System.Drawing.Point(4, 34);
            this.solutionWindow.Name = "solutionWindow";
            this.solutionWindow.PreviousDockState = Telerik.WinControls.UI.Docking.DockState.Docked;
            this.solutionWindow.Size = new System.Drawing.Size(192, 266);
            this.solutionWindow.Text = "Solution Explorer";
            // 
            // solutionTree
            // 
            this.solutionTree.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.solutionTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.solutionTree.ImageList = this.imageList;
            this.solutionTree.ItemHeight = 28;
            this.solutionTree.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.solutionTree.LineStyle = Telerik.WinControls.UI.TreeLineStyle.Solid;
            this.solutionTree.Location = new System.Drawing.Point(0, 20);
            this.solutionTree.Name = "solutionTree";
            // 
            // 
            // 
            this.solutionTree.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 20, 150, 250);
            this.solutionTree.Size = new System.Drawing.Size(192, 246);
            this.solutionTree.SpacingBetweenNodes = -1;
            this.solutionTree.TabIndex = 0;
            this.solutionTree.DoubleClick += new System.EventHandler(this.solutionTree_DoubleClick);
            // 
            // solutionFilter
            // 
            this.solutionFilter.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.solutionFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.solutionFilter.Location = new System.Drawing.Point(0, 0);
            this.solutionFilter.Name = "solutionFilter";
            this.solutionFilter.NullText = "Type here to filter";
            // 
            // 
            // 
            this.solutionFilter.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 0, 100, 20);
            this.solutionFilter.RootElement.StretchVertically = true;
            this.solutionFilter.Size = new System.Drawing.Size(192, 20);
            this.solutionFilter.TabIndex = 1;
            this.solutionFilter.TextChanged += new System.EventHandler(this.solutionFilter_TextChanged);
            // 
            // dock
            // 
            this.dock.ActiveWindow = this.solutionWindow;
            this.dock.AutoHideAnimation = Telerik.WinControls.UI.Docking.AutoHideAnimateMode.Both;
            this.dock.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.dock.CausesValidation = false;
            this.dock.Controls.Add(this.toolTabStrip1);
            this.dock.Controls.Add(this.documentContainer);
            this.dock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dock.IsCleanUpTarget = true;
            this.dock.Location = new System.Drawing.Point(0, 81);
            this.dock.MainDocumentContainer = this.documentContainer;
            this.dock.Name = "dock";
            this.dock.Padding = new System.Windows.Forms.Padding(0);
            // 
            // 
            // 
            this.dock.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 50, 200, 200);
            this.dock.RootElement.MinSize = new System.Drawing.Size(0, 0);
            this.dock.Size = new System.Drawing.Size(994, 304);
            this.dock.SplitterWidth = 8;
            this.dock.TabIndex = 0;
            this.dock.TabStop = false;
            this.dock.DockWindowClosed += new Telerik.WinControls.UI.Docking.DockWindowEventHandler(this.dock_DockWindowClosed);
            // 
            // toolTabStrip1
            // 
            this.toolTabStrip1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.toolTabStrip1.CausesValidation = false;
            this.toolTabStrip1.Controls.Add(this.solutionWindow);
            this.toolTabStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolTabStrip1.Name = "toolTabStrip1";
            // 
            // 
            // 
            this.toolTabStrip1.RootElement.ControlBounds = new System.Drawing.Rectangle(5, 5, 200, 200);
            this.toolTabStrip1.RootElement.MinSize = new System.Drawing.Size(0, 0);
            this.toolTabStrip1.SelectedIndex = 0;
            this.toolTabStrip1.Size = new System.Drawing.Size(200, 304);
            this.toolTabStrip1.TabIndex = 3;
            this.toolTabStrip1.TabStop = false;
            // 
            // toolWindow1
            // 
            this.toolWindow1.Caption = null;
            this.toolWindow1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolWindow1.Location = new System.Drawing.Point(0, 0);
            this.toolWindow1.Name = "toolWindow1";
            this.toolWindow1.PreviousDockState = Telerik.WinControls.UI.Docking.DockState.Docked;
            this.toolWindow1.Size = new System.Drawing.Size(200, 200);
            this.toolWindow1.Text = "toolWindow1";
            // 
            // radMenu1
            // 
            this.radMenu1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.radMenu1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radMenuItem6,
            this.radMenuItem12,
            this.radMenuItem7});
            this.radMenu1.Location = new System.Drawing.Point(0, 0);
            this.radMenu1.Name = "radMenu1";
            // 
            // 
            // 
            this.radMenu1.RootElement.ControlBounds = new System.Drawing.Rectangle(0, 0, 100, 24);
            this.radMenu1.Size = new System.Drawing.Size(994, 25);
            this.radMenu1.TabIndex = 2;
            // 
            // radMenuItem6
            // 
            this.radMenuItem6.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.menuOpen,
            this.menuEditConnections,
            this.menuExitNew});
            this.radMenuItem6.Name = "radMenuItem6";
            this.radMenuItem6.Text = "&File";
            // 
            // menuOpen
            // 
            this.menuOpen.Name = "menuOpen";
            this.menuOpen.Text = "&Open";
            this.menuOpen.Click += new System.EventHandler(this.menuOpen_Click);
            // 
            // menuEditConnections
            // 
            this.menuEditConnections.Name = "menuEditConnections";
            this.menuEditConnections.Text = "&Edit Connections";
            this.menuEditConnections.Click += new System.EventHandler(this.menuEditConnections_Click);
            // 
            // menuExitNew
            // 
            this.menuExitNew.AccessibleDescription = "radMenuItem8";
            this.menuExitNew.AccessibleName = "radMenuItem8";
            this.menuExitNew.Name = "menuExitNew";
            this.menuExitNew.Text = "E&xit";
            this.menuExitNew.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // radMenuItem12
            // 
            this.radMenuItem12.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radMenuViewSolutionExplorer});
            this.radMenuItem12.Name = "radMenuItem12";
            this.radMenuItem12.Text = "&View";
            // 
            // radMenuViewSolutionExplorer
            // 
            this.radMenuViewSolutionExplorer.Name = "radMenuViewSolutionExplorer";
            this.radMenuViewSolutionExplorer.Text = "Solution Explorer";
            this.radMenuViewSolutionExplorer.Click += new System.EventHandler(this.radMenuViewSolutionExplorer_Click);
            // 
            // radMenuItem7
            // 
            this.radMenuItem7.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radMenuItem9,
            this.radMenuItem10,
            this.radMenuItem11});
            this.radMenuItem7.Name = "radMenuItem7";
            this.radMenuItem7.Text = "&Tools";
            // 
            // radMenuItem9
            // 
            this.radMenuItem9.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.menuGenerateGridUniqueNames});
            this.radMenuItem9.Name = "radMenuItem9";
            this.radMenuItem9.Text = "Grids";
            // 
            // radMenuItem10
            // 
            this.radMenuItem10.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.menuGenerateMenuItemUniqueCodes});
            this.radMenuItem10.Name = "radMenuItem10";
            this.radMenuItem10.Text = "Menus";
            // 
            // radMenuItem11
            // 
            this.radMenuItem11.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.menuResourceTranslations});
            this.radMenuItem11.Name = "radMenuItem11";
            this.radMenuItem11.Text = "Localization";
            // 
            // menuGenerateUniqueNames
            // 
            this.menuGenerateUniqueNames.Name = "menuGenerateUniqueNames";
            this.menuGenerateUniqueNames.Text = "Generate Unique Names";
            // 
            // radMenuItem13
            // 
            this.radMenuItem13.Name = "radMenuItem13";
            this.radMenuItem13.Text = "Generate Menu Item Codes";
            // 
            // radMenuItem14
            // 
            this.radMenuItem14.Name = "radMenuItem14";
            this.radMenuItem14.Text = "Resource Translations";
            // 
            // MainForm2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 409);
            this.Controls.Add(this.dock);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.commandBar);
            this.Controls.Add(this.radMenu1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm2";
            this.Text = "MainForm2";
            this.Load += new System.EventHandler(this.MainForm2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.commandBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.documentContainer)).EndInit();
            this.solutionWindow.ResumeLayout(false);
            this.solutionWindow.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.solutionTree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.solutionFilter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dock)).EndInit();
            this.dock.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.toolTabStrip1)).EndInit();
            this.toolTabStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radMenu1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadCommandBar commandBar;
        private Telerik.WinControls.UI.CommandBarRowElement commandBarRowElement1;
        private Telerik.WinControls.UI.CommandBarStripElement commandBarStripElement1;
        private Telerik.WinControls.UI.CommandBarButton cmdSave;
        private Telerik.WinControls.UI.CommandBarButton cmdSaveAll;
        private Telerik.WinControls.UI.CommandBarButton cmdBuild;
        private Telerik.WinControls.UI.CommandBarDropDownButton cmdNew;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem1;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem2;
        private Telerik.WinControls.UI.RadMenuItem menuExit;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem3;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem4;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem5;
        private System.Windows.Forms.ImageList imageList;
        private Telerik.WinControls.UI.RadMenuItem menuGenerateGridUniqueNames;
        private Telerik.WinControls.UI.RadMenuItem menuGenerateMenuItemUniqueCodes;
        private Telerik.WinControls.UI.RadMenuItem menuResourceTranslations;
        private Telerik.WinControls.UI.RadStatusStrip statusBar;
        private Telerik.WinControls.UI.Docking.DocumentContainer documentContainer;
        private Telerik.WinControls.UI.Docking.ToolWindow solutionWindow;
        private Telerik.WinControls.UI.RadTreeView solutionTree;
        private Telerik.WinControls.UI.Docking.RadDock dock;
        private Telerik.WinControls.UI.Docking.ToolWindow toolWindow1;
        private Telerik.WinControls.UI.Docking.ToolTabStrip toolTabStrip1;
        private Telerik.WinControls.UI.RadTextBox solutionFilter;
        private Telerik.WinControls.UI.RadMenu radMenu1;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem6;
        private Telerik.WinControls.UI.RadMenuItem menuExitNew;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem7;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem9;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem10;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem11;
        private Telerik.WinControls.UI.RadMenuItem menuGenerateUniqueNames;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem13;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem14;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem12;
        private Telerik.WinControls.UI.RadMenuItem radMenuViewSolutionExplorer;
        private Telerik.WinControls.UI.RadMenuItem menuOpen;
        private Telerik.WinControls.UI.RadLabelElement statusBarLabel;
        private Telerik.WinControls.UI.RadMenuItem menuEditConnections;
    }
}