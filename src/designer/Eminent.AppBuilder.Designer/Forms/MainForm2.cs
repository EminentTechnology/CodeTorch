using CodeTorch.Core;
using CodeTorch.Core.Interfaces;
using CodeTorch.Core.Services;
using CodeTorch.Designer.Code;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Deployment.Application;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml.Xsl;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;

namespace CodeTorch.Designer.Forms
{
    public partial class MainForm2 : Form
    {
       

        Project Project = new Project();
        string ProjectFile { get; set; }
        string ConfigurationPath { get; set; }

        public MainForm2()
        {
            
            InitializeComponent();
        }

        private void MainForm2_Load(object sender, EventArgs e)
        {
            try
            {
                //Show the version in a simple manner
                this.Text = string.Format("CodeTorch Designer - v {0}", GetDisplayVersion());
                this.Visible = false;

                

                LoadProjectSelectionScreen(true);

                this.Visible = true;
                //add shortcuts
                this.cmdSave.Shortcuts.Add(new Telerik.WinControls.RadShortcut(Keys.Control, Keys.S));
                this.cmdSaveAll.Shortcuts.Add(new Telerik.WinControls.RadShortcut(Keys.Control, Keys.Shift, Keys.S));

            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);

            }
        }

        private void LoadProjectSelectionScreen(bool ExitOnCancel)
        {
            LoginDialog login = new LoginDialog();
            login.Text = string.Format("CodeTorch Designer - v {0}", GetDisplayVersion());

            DialogResult result = login.ShowDialog();

            if (result == DialogResult.OK)
            {



                ConfigurationPath = Configuration.GetInstance().ConfigurationPath;

                Project = login.Project;
                ProjectFile = login.ProjectFile;

                try
                {
                    

                    if (!Directory.Exists(ConfigurationPath))
                    {
                        Directory.CreateDirectory(ConfigurationPath);
                    }

                    if(!File.Exists(String.Format("{0}\\App\\App.xml", ConfigurationPath)))
                    {
                        //Create Configuration Project
                        CreateDefaultConfigurationProject();
                    }

                    this.Text = string.Format("CodeTorch Designer - v {0} - {1}", GetDisplayVersion(), Project.Name);
                    PerformSchemaVersionChecks();

                   ConfigurationLoader.LoadFromConfigurationFolder(ConfigurationPath);

                    
                    LoadStartPage();
                    LoadSolutionExplorer();
                    LoadDataCommandProviders();

                }
                catch (Exception ex)
                {
                    ErrorManager.HandleError(ex);
                    Application.Exit();
                }

            }
            else
            {
                if(ExitOnCancel)
                    Application.Exit();
            }
        }

        private void CreateDefaultConfigurationProject()
        {
            App app = new App();
            
            app.AdminRole = "Administrator";

            

            app.DateMode = DateMode.LocalDate;
            app.DefaultMenu = "MainMenu";
            app.DefaultPageTemplate = "Default";
            app.DefaultScreen = "~/App/Home/Default.aspx";
            app.DefaultZoneLayout = "Default";
            app.EnableLocalization = false;
            app.LoginScreen = "~/App/Security/Login.aspx";
            app.Name = Project.Name;
            app.SchemaVersion = Constants.SCHEMA_VERSION;
            

            App.Save(app);


        }

        private void PerformSchemaVersionChecks()
        {
            App app = ReloadApp();

            if (app != null)
            {
                

                if (app.SchemaVersion < Constants.SCHEMA_VERSION)
                {
                    //we need to upgrade
                    DialogResult result = MessageBox.Show("WARNING: Your project files need to be upgraded before you can use them with this version of CodeTorch Designer.\n\nPress YES to backup and upgrade your project files.\nPress NO to leave your project files alone", "Incompatible Schema Version", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button2);

                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        //perform upgrade
                        PerformUpgrade(app);
                    }
                    else
                    {
                        LoadProjectSelectionScreen(true);
                    }

                }
                else
                {
                    if (app.SchemaVersion > Constants.SCHEMA_VERSION)
                    { 
                        //we need to warn as you can mss up config
                        MessageBox.Show("WARNING: Project files were edited in a newer version of CodeTorch Designer.\n\nPlease upgrade your CodeTorch Designer","Incompatible Schema Version",MessageBoxButtons.OK, MessageBoxIcon.Error);

                        LoadProjectSelectionScreen(true);
                    }
                }

            }
        }

        private App ReloadApp()
        {
            XDocument doc = XDocument.Load(String.Format("{0}{1}\\{2}", ConfigurationPath, "App", "App.xml"));
            App app = App.Populate(doc);
            return app;
        }

        private void PerformUpgrade(App app)
        {
            string backupFolder = null;
            //MessageBox.Show("Performing Upgrade");

            //check to see if backup folder exists
            if (!Directory.Exists(String.Format("{0}Backups", ConfigurationPath)))
            {
                backupFolder = String.Format("{0}Backups\\1\\", ConfigurationPath);
                Directory.CreateDirectory(String.Format("{0}Backups", ConfigurationPath));
                Directory.CreateDirectory(backupFolder);
            }
            else
            {
                int folderCounter = 1;
                while(backupFolder == null)
                {
                    if(folderCounter > 1000)
                    {
                        throw new ApplicationException("Your backup folder has over 1000 copies - please clean your backup folder");
                    }

                    if(!Directory.Exists(String.Format("{0}Backups\\{1}", ConfigurationPath, folderCounter)))
                    {
                        backupFolder = String.Format("{0}Backups\\{1}\\", ConfigurationPath, folderCounter);
                    }
                    folderCounter++;
                }
            }
            

            //Now Create all of the directories
            foreach (string dirPath in Directory.GetDirectories(ConfigurationPath, "*", 
                SearchOption.AllDirectories))
            {
                if(!dirPath.ToLower().Contains("backups"))
                {
                    Directory.CreateDirectory(dirPath.Replace(ConfigurationPath, backupFolder));
                }
            }

            //Copy all the files
            foreach (string newPath in Directory.GetFiles(ConfigurationPath, "*.*", 
                SearchOption.AllDirectories))
            {
                if(!newPath.ToLower().Contains("backups"))
                {
                    File.Copy(newPath, newPath.Replace(ConfigurationPath, backupFolder));
                }
            }


            for (int i = app.SchemaVersion + 1; i <= Constants.SCHEMA_VERSION; i++)
            {
                UpgradeToSchemaVersion(app, i);
            }


        }

        private void UpgradeToSchemaVersion(App app, int SchemaVersion)
        {
            PerformUpgradeToSchemaVersionUsingCopy(SchemaVersion);
            PerformUpgradeToSchemaVersionUsingXslt(SchemaVersion);
            PerformUpgradeToSchemaVersionUsingCode(SchemaVersion);
            
            
            

            
            app = ReloadApp();
            app.SchemaVersion = SchemaVersion;
            App.Save(app);
        }

        private void PerformUpgradeToSchemaVersionUsingXslt(int SchemaVersion)
        {
            System.Reflection.Assembly configAssembly = Assembly.GetExecutingAssembly();

            var retVal = from item in configAssembly.GetManifestResourceNames()
                         where
                         (
                            (item.ToLower().StartsWith(String.Format("CodeTorch.Designer.schemaupgrades._{0}", SchemaVersion.ToString()))) &&
                            (item.ToLower().EndsWith(".xslt"))
                         )
                         orderby item
                         select item
                         ;

            List<string> items = retVal.ToList<string>();

            foreach (string xslt in items)
            {
                //determine entity type


                int endIndex = xslt.LastIndexOf('.', xslt.Length - 1);
                int startIndex = xslt.LastIndexOf('.', endIndex - 1);
                int length = endIndex - startIndex;
                string entityType = xslt.Substring(startIndex + 1, length - 1);

                //for each file in that entity type - apply xslt and save file locally
                string entityFolderPath = String.Format("{0}{1}s", ConfigurationPath, entityType);
                if (entityType.ToLower() == "app")
                {
                    entityFolderPath = String.Format("{0}{1}", ConfigurationPath, entityType);
                }



                XslCompiledTransform transformer = new XslCompiledTransform();
                using (Stream fileStream = configAssembly.GetManifestResourceStream(xslt))
                {
                    using (XmlTextReader xreader = new XmlTextReader(fileStream))
                    {
                        transformer.Load(xreader);
                    }
                }

                foreach (string configFile in Directory.GetFiles(entityFolderPath, "*.xml", SearchOption.AllDirectories))
                {
                    XPathDocument doc = new XPathDocument(configFile);



                    //XmlTextWriter writer = new XmlTextWriter(configFile, null);


                    using (XmlWriter writer = XmlWriter.Create(configFile, transformer.OutputSettings))
                    {
                        //writer.Settings.Indent = true;
                        //writer.Settings.CloseOutput = true;

                        transformer.Transform(doc, null, writer);
                        writer.Close();
                    }



                }



            }
        }

        private void PerformUpgradeToSchemaVersionUsingCopy(int SchemaVersion)
        {
            string resourcePath = String.Format("codetorch.designer.schemaupgrades._{0}.", SchemaVersion.ToString());
            System.Reflection.Assembly configAssembly = Assembly.GetExecutingAssembly();

            var retVal = from item in configAssembly.GetManifestResourceNames()
                         where
                         (
                            (item.ToLower().StartsWith(resourcePath)) &&
                            (!item.ToLower().EndsWith(".xslt"))
                         )
                         orderby item
                         select item
                         ;

            List<string> items = retVal.ToList<string>();

            foreach (string item in items)
            {
                //determine entity type
                string itemPath = item.Substring(resourcePath.Length);
                itemPath = itemPath.Replace(".", "\\");
                if(itemPath.ToLower().EndsWith("\\xml"))
                {
                    itemPath = itemPath.Replace("\\xml", ".xml");
                }


                string destFile = String.Format("{0}{1}", ConfigurationPath, itemPath);
                
                string destFolder = Path.GetDirectoryName(destFile);
                if (!Directory.Exists(destFolder))
                {
                    Directory.CreateDirectory(destFolder);
                }

                if (File.Exists(destFile))
                {
                    File.Delete(destFile);
                }
                
                using (Stream resourceStream = configAssembly.GetManifestResourceStream(item))
                {
                    using (Stream output = File.OpenWrite(destFile))
                    {
                        resourceStream.CopyTo(output);
                    }
                }

            }
        }

        private void PerformUpgradeToSchemaVersionUsingCode(int SchemaVersion)
        {
            List<ICodeTransformer> transformers = new List<ICodeTransformer>();
            switch (SchemaVersion)
            { 
                case 3:
                    
                    CodeTorch.Designer.SchemaUpgrades._3.ScreenTransformer v3Transformer = new CodeTorch.Designer.SchemaUpgrades._3.ScreenTransformer();
                    v3Transformer.GenerateRequiredObjects(ConfigurationPath);

                    transformers.Add(v3Transformer);
                    break;
                case 4:

                    transformers.Add(new CodeTorch.Designer.SchemaUpgrades._4.AppTransformer());
                    transformers.Add(new CodeTorch.Designer.SchemaUpgrades._4.MenuTransformer());
                    transformers.Add(new CodeTorch.Designer.SchemaUpgrades._4.ScreenTransformer());
                    transformers.Add(new CodeTorch.Designer.SchemaUpgrades._4.WorkflowTransformer());
                    break;
                case 6:

                    transformers.Add(new CodeTorch.Designer.SchemaUpgrades._6.AppTransformer());
                    transformers.Add(new CodeTorch.Designer.SchemaUpgrades._6.SectionTypeTransformer());
                    transformers.Add(new CodeTorch.Designer.SchemaUpgrades._6.ControlTypeTransformer());
                    transformers.Add(new CodeTorch.Designer.SchemaUpgrades._6.ScreenTransformer());
                    break;

            }

            foreach (ICodeTransformer transformer in transformers)
            {
                foreach (string entityType in transformer.GetSupportedEntityTypes())
                {
                    transformer.EntityType = entityType;
                    PerformSchemaCodeTransformation(SchemaVersion, transformer);
                }
            }
            
        }

        private void PerformSchemaCodeTransformation(int SchemaVersion, ICodeTransformer transformer)
        {
            string entityFolderPath = String.Format("{0}{1}s", ConfigurationPath, transformer.EntityType);
            if (transformer.EntityType.ToLower() == "app")
            {
                entityFolderPath = String.Format("{0}{1}", ConfigurationPath, transformer.EntityType);
            }

            if (Directory.Exists(entityFolderPath))
            {
                foreach (string configFile in Directory.GetFiles(entityFolderPath, "*.xml", SearchOption.AllDirectories))
                {
                    try
                    {
                        XDocument doc = XDocument.Load(configFile);

                        transformer.Document = doc;
                        bool retVal = transformer.Execute();

                        if (retVal)
                        {
                            doc.Save(configFile);
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException(String.Format("Version {1} Code Transformation Error in file {0}", configFile, SchemaVersion), ex);
                    }
                }
            }
        }

        private void LoadStartPage()
        {
            DockWindow d = null;

            try
            {
                foreach (DockWindow window in dock.DocumentManager.DocumentArray)
                {
                    if (window.Text.ToLower() == "start")
                    {
                        d = window; 
                    }
                }
            }
            catch { }

            if (d == null)
            {

                string url = String.Format("http://portal.codetorch.com/docs/guides/getting-started/what-is-codetorch");

                WebBrowser web = new WebBrowser();
                web.Dock = DockStyle.Fill;
                web.ScriptErrorsSuppressed = true;
                web.Navigate(url);
               


                DocumentWindow doc = new DocumentWindow();
                doc.Text = "Start";

                doc.Controls.Add(web);

                this.dock.AddDocument(doc);
            }
        }

        private void LoadSolutionExplorer()
        {
            solutionTree.Nodes.Clear();
            cmdNew.Items.Clear();

            SetupSolutionExplorerRootNodes("Screens", "Screen", "Screen", "Screens", Configuration.GetInstance().Screens);
            SetupSolutionExplorerRootNodes("Data Commands", "Data Command", "DataCommand", "DataCommands", Configuration.GetInstance().DataCommands);

            SetupSolutionExplorerRootNodes("Lookups", "Lookup", "Lookup", "Lookups", Configuration.GetInstance().Lookups);
            SetupSolutionExplorerRootNodes("Menus", "Menu", "Menu", "Menus", Configuration.GetInstance().Menus);
            SetupSolutionExplorerRootNodes("Permissions", "Permission", "Permission", "Permissions", Configuration.GetInstance().Permissions);
            SetupSolutionExplorerRootNodes("Pickers", "Picker", "Picker", "Pickers", Configuration.GetInstance().Pickers);
            SetupSolutionExplorerRootNodes("Sequences", "Sequence", "Sequence", "Sequences", Configuration.GetInstance().Sequences);
            SetupSolutionExplorerRootNodes("Templates", "Template", "Template", "Templates", Configuration.GetInstance().Templates);
            SetupSolutionExplorerRootNodes("Page Templates", "Page Template", "PageTemplate", "PageTemplates", Configuration.GetInstance().PageTemplates);
            SetupSolutionExplorerRootNodes("Workflows", "Workflow", "Workflow", "Workflows", Configuration.GetInstance().Workflows);
            SetupSolutionExplorerRootNodes("Section Zone Layouts", "Section Zone Layout", "SectionZoneLayout", "SectionZoneLayouts", Configuration.GetInstance().SectionZoneLayouts);
            SetupSolutionExplorerRootNodes("Rest Services", "Rest Service", "RestService", "RestServices", Configuration.GetInstance().RestServices);

            
            //items below typically when changed required code changes

            //SetupSolutionExplorerRootNodes("Control Types", "Control Type", "ControlType", "ControlTypes", Configuration.GetInstance().ControlTypes);
            SetupSolutionExplorerRootNodes("Data Connection Types", "Data Connection Type", "DataConnectionType", "DataConnectionTypes", Configuration.GetInstance().DataConnectionTypes);
            SetupSolutionExplorerRootNodes("Data Connections", "Data Connection", "DataConnection", "DataConnections", Configuration.GetInstance().DataConnections);
            SetupSolutionExplorerRootNodes("Document Repository Types", "Document Repository Type", "DocumentRepositoryType", "DocumentRepositoryTypes", Configuration.GetInstance().DocumentRepositoryTypes);
            SetupSolutionExplorerRootNodes("Document Repositories", "Document Repository", "DocumentRepository", "DocumentRepositories", Configuration.GetInstance().DocumentRepositories);
            SetupSolutionExplorerRootNodes("Workflow Types", "Workflow Type", "WorkflowType", "WorkflowTypes", Configuration.GetInstance().WorkflowTypes);
            SetupSolutionExplorerRootNodes("Email Connections", "Email Connection", "EmailConnection", "EmailConnections", Configuration.GetInstance().EmailConnections);
            SetupSolutionExplorerRootNodes("Email Connection Types", "Email Connection Type", "EmailConnectionType", "EmailConnectionTypes", Configuration.GetInstance().EmailConnectionTypes);
            //SetupSolutionExplorerRootNodes("Dashboard Types", "Dashboard Type", "DashboardComponentType", "DashboardComponentTypes", Configuration.GetInstance().DashboardComponentTypes);
            //SetupSolutionExplorerRootNodes("Screen Types", "Screen Type", "ScreenType", "ScreenTypes", Configuration.GetInstance().ScreenTypes);
            //SetupSolutionExplorerRootNodes("Section Types", "Section Type", "SectionType", "SectionTypes", Configuration.GetInstance().SectionTypes);


            //SetupSolutionExplorerRootNodes("App", "App", "App", "App", Configuration.GetInstance().ControlTypes);
        }

        private void SetupSolutionExplorerRootNodes(string PluralTitle, string SingleTitle, string EntityType, string ConfigFolderPath, IEnumerable list)
        {
            RadTreeNode node = new RadTreeNode();

            node.Text = PluralTitle;
            node.ContextMenu = GetRootNodesContextMenu(EntityType);
            node.ImageKey = GetRootNodeImageKey(EntityType);


            switch (EntityType)
            {
                case Constants.ENTITY_TYPE_SCREEN:
                    SetupSolutionExplorerScreenNodes(node, PluralTitle, SingleTitle, EntityType, ConfigFolderPath, list);
                    break;
                
                default:
                    PopulateNodes(node, PluralTitle, SingleTitle, EntityType, ConfigFolderPath, list);
                    break;
            }

            RadMenuItem item = new RadMenuItem();
            item.Tag = EntityType;
            item.Text = String.Format("New {0}", SingleTitle);
            item.Click += newItem_Click;
            //RadItem
            cmdNew.Items.Add(item);


            this.solutionTree.Nodes.Add(node);
        }

        private string GetRootNodeImageKey(string EntityType)
        {
            string retVal = null;

            switch (EntityType)
            {
                case Constants.ENTITY_TYPE_SCREEN:
                    retVal = "web";
                    break;
                case Constants.ENTITY_TYPE_DASHBOARD_COMPONENT:
                    retVal = "dashboard";
                    break;
                case Constants.ENTITY_TYPE_DATA_COMMAND:
                    retVal = "datacommand";
                    break;
               
                case Constants.ENTITY_TYPE_LOOKUP:
                    retVal = "lookup";
                    break;
                case Constants.ENTITY_TYPE_MENU:
                    retVal = "menu";
                    break;
                case Constants.ENTITY_TYPE_PAGE_TEMPLATE:
                    retVal = "pagetemplate";
                    break;
                case Constants.ENTITY_TYPE_PERMISSION:
                    retVal = "permission";
                    break;
                case Constants.ENTITY_TYPE_PICKER:
                    retVal = "picker";
                    break;
                case Constants.ENTITY_TYPE_REST_SERVICE:
                    retVal = "services";
                    break;
                case Constants.ENTITY_TYPE_SECTION_ZONE_LAYOUT:
                    retVal = "layout";
                    break;
                case Constants.ENTITY_TYPE_SEQUENCE:
                    retVal = "sequence";
                    break;
                case Constants.ENTITY_TYPE_TEMPLATE:
                    retVal = "template";
                    break;
                case Constants.ENTITY_TYPE_WORKFLOW:
                    retVal = "workflow";
                    break;
               
            }

            return retVal;
        }

        void newItem_Click(object sender, EventArgs e)
        {
            try
            {
                RadItem item = (RadItem)sender;
                DialogResult result = GetNewFormDialog(item.Tag.ToString());

                if (result == DialogResult.OK)
                {
                    //TODO: hack for now but need to do the following
                    //add node to  tree
                    //display new document
                    LoadSolutionExplorer();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
        }

        private DialogResult GetNewFormDialog(string EntityType)
        {

            
            switch (EntityType.ToLower())
            {
                case "controltype":
                    NewControlTypeDialog ctDlg = new NewControlTypeDialog();
                    return ctDlg.ShowDialog();
               
               
                case "datacommand":
                    NewDataCommandDialog dcDlg = new NewDataCommandDialog();
                    dcDlg.Project = this.Project;
                    return dcDlg.ShowDialog();
                case "dataconnection":
                    NewItemDialog dataConnectionDialog = new NewItemDialog();
                    dataConnectionDialog.ItemType = EntityType;
                    dataConnectionDialog.ItemDisplayName = "Data Connection";
                    return dataConnectionDialog.ShowDialog();
                case "dataconnectiontype":
                    NewItemDialog dataConnectionTypeDialog = new NewItemDialog();
                    dataConnectionTypeDialog.ItemType = EntityType;
                    dataConnectionTypeDialog.ItemDisplayName = "Data Connection Type";
                    return dataConnectionTypeDialog.ShowDialog();
                case "documentrepository":
                    NewItemDialog documentRepositoryDialog = new NewItemDialog();
                    documentRepositoryDialog.ItemType = EntityType;
                    documentRepositoryDialog.ItemDisplayName = "Document Repository";
                    return documentRepositoryDialog.ShowDialog();
                case "documentrepositorytype":
                    NewItemDialog documentRepositoryTypeDialog = new NewItemDialog();
                    documentRepositoryTypeDialog.ItemType = EntityType;
                    documentRepositoryTypeDialog.ItemDisplayName = "Document Repository Type";
                    return documentRepositoryTypeDialog.ShowDialog();
                case "emailconnection":
                    NewItemDialog emailConnectionDialog = new NewItemDialog();
                    emailConnectionDialog.ItemType = EntityType;
                    emailConnectionDialog.ItemDisplayName = "Email Connection";
                    return emailConnectionDialog.ShowDialog();
                case "emailconnectiontype":
                    NewItemDialog emailConnectionTypeDialog = new NewItemDialog();
                    emailConnectionTypeDialog.ItemType = EntityType;
                    emailConnectionTypeDialog.ItemDisplayName = "Email Connection Type";
                    return emailConnectionTypeDialog.ShowDialog();
                case "group":
                    NewItemDialog groupDialog = new NewItemDialog();
                    groupDialog.ItemType = EntityType;
                    groupDialog.ItemDisplayName = "Group";
                    return groupDialog.ShowDialog();
                case "lookup":
                    NewItemDialog lookupDialog = new NewItemDialog();
                    lookupDialog.ItemType = EntityType;
                    lookupDialog.ItemDisplayName = "Lookup";
                    return lookupDialog.ShowDialog();
                case "menu":
                    NewMenuDialog mdlg = new NewMenuDialog();
                    return mdlg.ShowDialog();
                case "pagetemplate":
                    NewItemDialog pagetemplateDialog = new NewItemDialog();
                    pagetemplateDialog.ItemType = EntityType;
                    pagetemplateDialog.ItemDisplayName = EntityType;
                    return pagetemplateDialog.ShowDialog();
                case "permission":
                    NewPermissionDialog pdlg = new NewPermissionDialog();
                    return pdlg.ShowDialog();
                case "picker":
                    NewItemDialog pickerDialog = new NewItemDialog();
                    pickerDialog.ItemType = EntityType;
                    pickerDialog.ItemDisplayName = "Picker";
                    return pickerDialog.ShowDialog();
                case "restservice":
                    NewItemDialog restServiceDialog = new NewItemDialog();
                    restServiceDialog.ItemType = EntityType;
                    restServiceDialog.ItemDisplayName = "RestService";
                    return restServiceDialog.ShowDialog();
                case "screen":
                    //NewScreenDialog2 sdlg = new NewScreenDialog2();
                    //return sdlg.ShowDialog();
                    NewScreenWizard sdlg = new NewScreenWizard();
                    sdlg.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
                    sdlg.TopLevel = false;
                    sdlg.Dock = DockStyle.Fill;

                    DocumentWindow wizardWindow = new DocumentWindow();
                    wizardWindow.Text = "New Screen";
                    wizardWindow.CloseAction = DockWindowCloseAction.CloseAndDispose;
                    wizardWindow.Controls.Add(sdlg);
                    this.dock.AddDocument(wizardWindow);

                    sdlg.DockWindow = wizardWindow;
                    sdlg.Solution = this.solutionTree;
                    sdlg.MainForm = this;
                    sdlg.Show();

                    this.dock.ActiveWindow = wizardWindow;
                    //not used here
                    return System.Windows.Forms.DialogResult.Cancel;


                case "screentype":
                    NewItemDialog screentypeDialog = new NewItemDialog();
                    screentypeDialog.ItemType = EntityType;
                    screentypeDialog.ItemDisplayName = EntityType;
                    return screentypeDialog.ShowDialog();
                case "sectiontype":
                    NewItemDialog sectiontypeDialog = new NewItemDialog();
                    sectiontypeDialog.ItemType = EntityType;
                    sectiontypeDialog.ItemDisplayName = EntityType;
                    return sectiontypeDialog.ShowDialog();
                case "sectionzonelayout":
                    NewItemDialog sectionZoneLayoutDialog = new NewItemDialog();
                    sectionZoneLayoutDialog.ItemType = EntityType;
                    sectionZoneLayoutDialog.ItemDisplayName = EntityType;
                    return sectionZoneLayoutDialog.ShowDialog();
                case "sequence":
                    NewItemDialog sequenceDialog = new NewItemDialog();
                    sequenceDialog.ItemType = EntityType;
                    sequenceDialog.ItemDisplayName = EntityType;
                    return sequenceDialog.ShowDialog();
                case "template":
                    NewItemDialog templateDialog = new NewItemDialog();
                    templateDialog.ItemType = EntityType;
                    templateDialog.ItemDisplayName = EntityType;
                    return templateDialog.ShowDialog();
                case "workflow":
                    NewItemDialog workflowDialog = new NewItemDialog();
                    workflowDialog.ItemType = EntityType;
                    workflowDialog.ItemDisplayName = EntityType;
                    return workflowDialog.ShowDialog();
                case "workflowtype":
                    NewItemDialog workflowtypeDialog = new NewItemDialog();
                    workflowtypeDialog.ItemType = EntityType;
                    workflowtypeDialog.ItemDisplayName = EntityType;
                    return workflowtypeDialog.ShowDialog();
                default:
                    return DialogResult.Cancel;
            }
        }

        private void SetupSolutionExplorerScreenNodes(RadTreeNode rootNode, string PluralTitle, string SingleTitle, string EntityType, string ConfigFolderPath, IEnumerable list)
        {

            List<string> folders = CodeTorch.Core.Screen.GetDistinctFolders();

            foreach (string folder in folders)
            {
                RadTreeNode f = new RadTreeNode(folder);
                f.ImageKey = "vsfolder_closed.bmp";
                
                f.Tag = "FOLDER";

                IEnumerable pages = CodeTorch.Core.Screen.GetByFolder(folder);
                PopulateNodes(f, PluralTitle, SingleTitle, EntityType, ConfigFolderPath,  pages);

                rootNode.Nodes.Add(f);
            }

            
            
        }

        private void SetupSolutionExplorerDataCommandNodes(RadTreeNode rootNode, string PluralTitle, string SingleTitle, string EntityType, string ConfigFolderPath, IEnumerable list)
        {

            List<string> folders = CodeTorch.Core.Screen.GetDistinctFolders();

            foreach (string folder in folders)
            {
                RadTreeNode f = new RadTreeNode(folder);
                f.ImageKey = "folder";

                f.Tag = "FOLDER";

                IEnumerable pages = CodeTorch.Core.Screen.GetByFolder(folder);
                PopulateNodes(f,  PluralTitle,  SingleTitle,EntityType,ConfigFolderPath, pages);

                rootNode.Nodes.Add(f);
            }



        }

        private  void PopulateNodes(RadTreeNode rootNode, string PluralTitle, string SingleTitle, string EntityType, string ConfigFolderPath, IEnumerable list)
        {
            

            foreach (object o in list)
            {
                SolutionTreeNode p = new SolutionTreeNode();
                p.Text = GetEntityInstanceName(EntityType, o);
                p.ImageKey = "screen";
                p.Tag = "OBJECT";
                p.Object = o;

                p.EntityType = EntityType;
                p.SingleType = SingleTitle;
                p.PluralTitle = PluralTitle;
                p.ConfigFolderPath = ConfigFolderPath;

                p.ContextMenu = GetItemContextMenu(EntityType);
                

                rootNode.Nodes.Add(p);
            }
        }

        private static string GetEntityInstanceName(string EntityType, object o)
        {
            string retVal = String.Empty;

            if (o != null)
            {
                switch (EntityType)
                {
                    case Constants.ENTITY_TYPE_SCREEN:
                        retVal = ((CodeTorch.Core.Screen)o).Name;
                        break;
                    
                    case Constants.ENTITY_TYPE_DATA_COMMAND:
                        retVal = ((CodeTorch.Core.DataCommand)o).Name;
                        break;
                    case Constants.ENTITY_TYPE_DATA_CONNECTION:
                        retVal = ((CodeTorch.Core.DataConnection)o).Name;
                        break;
                    case Constants.ENTITY_TYPE_DATA_CONNECTION_TYPE:
                        retVal = ((CodeTorch.Core.DataConnectionType)o).Name;
                        break;
                    case Constants.ENTITY_TYPE_EMAIL_CONNECTION:
                        retVal = ((CodeTorch.Core.EmailConnection)o).Name;
                        break;
                    case Constants.ENTITY_TYPE_EMAIL_CONNECTION_TYPE:
                        retVal = ((CodeTorch.Core.EmailConnectionType)o).Name;
                        break;
                    case Constants.ENTITY_TYPE_DOCUMENT_REPOSITORY:
                        retVal = ((CodeTorch.Core.DocumentRepository)o).Name;
                        break;
                    case Constants.ENTITY_TYPE_DOCUMENT_REPOSITORY_TYPE:
                        retVal = ((CodeTorch.Core.DocumentRepositoryType)o).Name;
                        break;
                   
                    case Constants.ENTITY_TYPE_LOOKUP:
                        retVal = ((CodeTorch.Core.Lookup)o).Name;
                        break;
                    case Constants.ENTITY_TYPE_MENU:
                        retVal = ((CodeTorch.Core.Menu)o).Name;
                        break;
                    case Constants.ENTITY_TYPE_PAGE_TEMPLATE:
                        retVal = ((CodeTorch.Core.PageTemplate)o).Name;
                        break;
                    case Constants.ENTITY_TYPE_PERMISSION:
                        retVal = ((CodeTorch.Core.Permission)o).Name;
                        break;
                    case Constants.ENTITY_TYPE_PICKER:
                        retVal = ((CodeTorch.Core.Picker)o).Name;
                        break;
                    case Constants.ENTITY_TYPE_REST_SERVICE:
                        retVal = ((CodeTorch.Core.RestService)o).Name;
                        break;
                    case Constants.ENTITY_TYPE_SECTION_ZONE_LAYOUT:
                        retVal = ((CodeTorch.Core.SectionZoneLayout)o).Name;
                        break;
                    case Constants.ENTITY_TYPE_SEQUENCE:
                        retVal = ((CodeTorch.Core.Sequence)o).Name;
                        break;
                    case Constants.ENTITY_TYPE_TEMPLATE:
                        retVal = ((CodeTorch.Core.Template)o).Name;
                        break;
                    case Constants.ENTITY_TYPE_WORKFLOW:
                        retVal = ((CodeTorch.Core.Workflow)o).Name;
                        break;
                    case Constants.ENTITY_TYPE_WORKFLOW_TYPE:
                        retVal = ((CodeTorch.Core.WorkflowType)o).Name;
                        break;
                    default:
                        retVal = o.ToString();
                        break;
                }
            }

            return retVal;
        }

        

        private string GetDisplayVersion()
        {
            Version v = Assembly.GetExecutingAssembly().GetName().Version;
            //Check to see if we are ClickOnce Deployed.
            //i.e. the executing code was installed via ClickOnce
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                //Collect the ClickOnce Current Version
                v = ApplicationDeployment.CurrentDeployment.CurrentVersion;


            }
            //Show the version in a simple manner
            return string.Format("{0}", v);
        }

        private void solutionTree_DoubleClick(object sender, EventArgs e)
        {
            MouseEventArgs args = e as MouseEventArgs;
            RadTreeNode clickedNode = this.solutionTree.GetNodeAt(args.X, args.Y);
            if (clickedNode != null)
            {
                if (clickedNode is SolutionTreeNode)
                {

                    OpenDocument(((SolutionTreeNode)clickedNode));
                }
            }
        }

        public void OpenDocument(SolutionTreeNode node)
        {
            OpenDocument(node, null);
        }

        public void OpenDocument(SolutionTreeNode node, DockWindow toClose)
        {

            DockWindow doc;
            doc = ProjectItemWindow.GetInstance(node);   

            

            this.dock.AddDocument(doc);

            if (toClose != null)
            {
                toClose.Close();
            }

            this.dock.ActiveWindow = doc;
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (dock.DocumentManager.ActiveDocument != null)
                {
                    if (SaveDocument(dock.DocumentManager.ActiveDocument)) 
                    {
                        statusBarLabel.Text = String.Format("{0} saved at {1} {2}", dock.DocumentManager.ActiveDocument.Text, DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
                        MessageBox.Show(dock.DocumentManager.ActiveDocument.Text + " Saved Successfully");
                    }
                }
                else
                {
                    MessageBox.Show("Nothing saved");
                }
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
        }

        private bool SaveDocument(DockWindow doc)
        {
            bool retVal = false;
            try
            {
                if (doc.Tag != null)
                {
                    SolutionTreeNode node = (SolutionTreeNode)doc.Tag;
                    object o = node.Object;
                    string objectTypeName = o.GetType().Name;
                    

                    IConfigurationObject2 config = ConfigurationObjectFactory.CreateConfigurationObject(node.EntityType);
                    config.Save(o);

                    retVal = true;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }

            return retVal;
            
            
        }

        private void cmdSaveAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (dock.DockWindows.DocumentWindows.Length > 0)
                {
                    foreach (DocumentWindow doc in dock.DockWindows.DocumentWindows)
                    {
                        SaveDocument(doc);
                    }

                    statusBarLabel.Text = String.Format("Documents saved at {0} {1}..", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
                    MessageBox.Show("All open files Saved Successfully");
                }
                else
                {
                    MessageBox.Show("Nothing saved");
                }
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

       

        private RadContextMenu GetRootNodesContextMenu(string EntityType)
        {
            RadContextMenu retval = new RadContextMenu();

            RadMenuItem refreshConfig = new RadMenuItem("Refresh");
            refreshConfig.Click += new EventHandler(refreshAll_Click);
            retval.Items.Add(refreshConfig);

            

            return retval;
        }

        private RadContextMenu GetItemContextMenu(string EntityType)
        {
            RadContextMenu retval = null;
            

            switch (EntityType)
            {
                case Constants.ENTITY_TYPE_LOOKUP:
                    retval = new RadContextMenu();

                    RadMenuItem lookupSingleDatabase = new RadMenuItem("Update This Lookup to Database");
                    lookupSingleDatabase.Click += new EventHandler(MenuItem_Lookup_UpdateSingleLookupToDataBase_Click);
                    retval.Items.Add(lookupSingleDatabase);


                    RadMenuItem lookupAllDatabase = new RadMenuItem("Update All Lookups to Database");
                    lookupAllDatabase.Click += new EventHandler(MenuItem_Lookup_UpdateAllLookupsToDataBase_Click);
                    retval.Items.Add(lookupAllDatabase);


                    break;
                case Constants.ENTITY_TYPE_PERMISSION:
                    retval = new RadContextMenu();

                    RadMenuItem permissionSingleDatabase = new RadMenuItem("Update This Permission to Database");
                    permissionSingleDatabase.Click += new EventHandler(MenuItem_Permission_UpdateSinglePermissionToDataBase_Click);
                    retval.Items.Add(permissionSingleDatabase);


                    RadMenuItem permissionAllDatabase = new RadMenuItem("Update All Permissions to Database");
                    permissionAllDatabase.Click += new EventHandler(MenuItem_Permission_UpdateAllPermissionsToDataBase_Click);
                    retval.Items.Add(permissionAllDatabase);


                    break;
                case Constants.ENTITY_TYPE_SEQUENCE:
                    retval = new RadContextMenu();

                    RadMenuItem sequenceSingleDatabase = new RadMenuItem("Update This Sequence to Database");
                    sequenceSingleDatabase.Click += new EventHandler(MenuItem_Sequence_UpdateSingleSequenceToDataBase_Click);
                    retval.Items.Add(sequenceSingleDatabase);

                    RadMenuItem sequenceAllDatabase = new RadMenuItem("Update All Sequences to Database");
                    sequenceAllDatabase.Click += new EventHandler(MenuItem_Sequence_UpdateAllSequencesToDataBase_Click);
                    retval.Items.Add(sequenceAllDatabase);


                    break;
                case Constants.ENTITY_TYPE_DATA_COMMAND:
                    retval = new RadContextMenu();

                    RadMenuItem datacommandRefreshSchema = new RadMenuItem("Refresh Schema From DataBase");
                    datacommandRefreshSchema.Click += new EventHandler(MenuItem_DataCommand_RefreshSchem_Click);
                    retval.Items.Add(datacommandRefreshSchema);




                    break;

                case Constants.ENTITY_TYPE_SCREEN:
                    retval = new RadContextMenu();

                    RadMenuItem screenBrowseToScreen = new RadMenuItem("Browse To Screen");
                    screenBrowseToScreen.Click += new EventHandler(MenuItem_Screen_BrowseToScreen_Click);
                    retval.Items.Add(screenBrowseToScreen);


                    //RadMenuItem AddGridColumnsToGrid = new RadMenuItem("Add Grid Columns to Grid");
                    //AddGridColumnsToGrid.Click += AddGridColumnsToGrid_Click;
                    //retval.Items.Add(AddGridColumnsToGrid);

                    break;
                case Constants.ENTITY_TYPE_WORKFLOW:
                    retval = new RadContextMenu();

                    RadMenuItem workflowSingleDatabase = new RadMenuItem("Update This Workflow to Database");
                    workflowSingleDatabase.Click += new EventHandler(MenuItem_Workflow_UpdateSingleWorkflowToDataBase_Click);
                    retval.Items.Add(workflowSingleDatabase);


                    break;
                

            }

            if (retval == null)
            {
                retval = new RadContextMenu();
            }

            RadMenuItem deleteItem = new RadMenuItem("Delete");
            deleteItem.Click += new EventHandler(deleteItem_Click);
            retval.Items.Add(deleteItem);

            return retval;
        }

     
        void deleteItem_Click(object sender, EventArgs e)
        {
            try
            {

                RadTreeView tree = this.solutionTree;
                SolutionTreeNode node = (SolutionTreeNode)tree.SelectedNode;
                string nodeText = node.Text;

                DialogResult result = MessageBox.Show(String.Format("WARNING: You are about to delete {0}.\n\nItems tied to this {1} (if any) will need to be manually updated.\nTo LEAVE the selected {1} alone click CANCEL.\nTo continue with this DELETE operation click OK.", node.Text, node.SingleType), "Delete Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                if (result == DialogResult.OK)
                {
                    //TODO: close this document ifit is open
                    IConfigurationObject2 config = ConfigurationObjectFactory.CreateConfigurationObject(node.EntityType);
                    config.Delete(node.Object);

                    
                    
                    ConfigurationLoader.ReloadConfigurationItems(node.ConfigFolderPath, node.EntityType);
                    
                    LoadSolutionExplorer();

                    

                    MessageBox.Show(String.Format("{0} has been deleted successfully", nodeText));

                }
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
        }

        void refreshAll_Click(object sender, EventArgs e)
        {
            try
            {
                //TODO: need to load only seection requested..currently performing entire reload
                RefreshAll();

            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
        }

        public void RefreshAll()
        {
            ConfigurationLoader.LoadFromConfigurationFolder(ConfigurationPath);
            LoadSolutionExplorer();
        }
        void MenuItem_Screen_BrowseToScreen_Click(object sender, EventArgs e)
        {
            try
            {

                RadTreeView tree = this.solutionTree;
                SolutionTreeNode node = (SolutionTreeNode)tree.SelectedNode;

                
                string baseUrl = Configuration.GetInstance().App.DevelopmentBaseUrl;

                if (String.IsNullOrEmpty(baseUrl))
                {
                    MessageBox.Show("DevelopmentBaseUrl in app.config has not been setup properly");
                }
                else
                {
                    if (!baseUrl.EndsWith("/"))
                        baseUrl += "/";

                    if (tree.SelectedNode.Tag.ToString() == "OBJECT")
                    {
                        CodeTorch.Core.Screen screen = ((CodeTorch.Core.Screen)((SolutionTreeNode)tree.SelectedNode).Object);



                        string url = String.Format("{0}App/{1}/{2}", baseUrl, screen.Folder, screen.Name);

                        System.Diagnostics.Process.Start(url);

                    }
                    else
                    {
                        MessageBox.Show("Select screen to browse to.");
                    }
                }


                


            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }

        }

        void MenuItem_DataCommand_RefreshSchem_Click(object sender, EventArgs e)
        {
            try
            {
                RadTreeView tree = this.solutionTree;
                SolutionTreeNode node = (SolutionTreeNode)tree.SelectedNode;
                DataCommand item = (DataCommand)node.Object;

                DataConnection connection = Project.GetDataConnection(item);
                IDataCommandProvider DataSource = DataCommandService.GetInstance().GetProvider(connection);
                DataSource.RefreshSchema(connection, item);

                DataCommand.Save(item);

                MessageBox.Show(String.Format("Data Command {0} has been refreshed successfully", item.Name));
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }

        }

        void MenuItem_Workflow_UpdateSingleWorkflowToDataBase_Click(object sender, EventArgs e)
        {
            try
            {
                RadTreeView tree = this.solutionTree;
                SolutionTreeNode node = (SolutionTreeNode)tree.SelectedNode;
                Workflow item = (Workflow)node.Object;

                DataConnection connection = Project.GetDefaultDataConnection();
                var provider = WorkflowService.GetInstance().GetProvider(item);
                provider.Connection = connection;
                    
                provider.Save(item);
                

                MessageBox.Show(String.Format("Workflow {0} was saved successfully to the database", item.Code));
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
        }

        void MenuItem_Lookup_UpdateSingleLookupToDataBase_Click(object sender, EventArgs e)
        {
            try
            {
                RadTreeView tree = this.solutionTree;
                SolutionTreeNode node = (SolutionTreeNode)tree.SelectedNode;
                Lookup item = (Lookup)node.Object;

                DataConnection connection = Project.GetDefaultDataConnection();
                ILookupProvider lookup = LookupService.GetInstance().LookupProvider;
                lookup.Connection = connection;

                lookup.Save(item);

                MessageBox.Show(String.Format("Lookup {0} was saved successfully to the database", item.Name));
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }

        }

        void MenuItem_Lookup_UpdateAllLookupsToDataBase_Click(object sender, EventArgs e)
        {
            try
            {
                RadTreeView tree = this.solutionTree;
                SolutionTreeNode node = (SolutionTreeNode)tree.SelectedNode;

                ILookupProvider lookup = LookupService.GetInstance().LookupProvider;
                
                int i = 0;
                DataConnection connection = Project.GetDefaultDataConnection();
                lookup.Connection = connection;
                foreach (Lookup item in Configuration.GetInstance().Lookups)
                {
                    i++;
                    lookup.Save( item);
                }

                MessageBox.Show(String.Format("{0} lookup(s) were saved successfully to the database", i));

            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }

        }

        void MenuItem_Permission_UpdateSinglePermissionToDataBase_Click(object sender, EventArgs e)
        {
            try
            {
                IAuthorizationProvider auth = AuthorizationService.GetInstance().AuthorizationProvider;
                

                RadTreeView tree = this.solutionTree;
                SolutionTreeNode node = (SolutionTreeNode)tree.SelectedNode;
                Permission item = (Permission)node.Object;

                DataConnection connection = Project.GetDefaultDataConnection();

                auth.SavePermission(connection, item);

                if (!String.IsNullOrEmpty(Configuration.GetInstance().App.AdminRole))
                    auth.AddAllPermissionsToRole(connection, Configuration.GetInstance().App.AdminRole);

                MessageBox.Show(String.Format("Permission {0} was saved successfully to the database", item.Name));
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }

        }

        void MenuItem_Permission_UpdateAllPermissionsToDataBase_Click(object sender, EventArgs e)
        {
            try
            {
                IAuthorizationProvider auth = AuthorizationService.GetInstance().AuthorizationProvider;

                RadTreeView tree = this.solutionTree;
                SolutionTreeNode node = (SolutionTreeNode)tree.SelectedNode;

                 DataConnection connection = Project.GetDefaultDataConnection();

                int i = 0;
                foreach (Permission item in Configuration.GetInstance().Permissions)
                {
                    i++;
                    auth.SavePermission(connection,item);
                }

                if (!String.IsNullOrEmpty(Configuration.GetInstance().App.AdminRole))
                    auth.AddAllPermissionsToRole(connection, Configuration.GetInstance().App.AdminRole);

                MessageBox.Show(String.Format("{0} permission(s) were saved successfully to the database", i));

            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }

        }

        void MenuItem_Sequence_UpdateSingleSequenceToDataBase_Click(object sender, EventArgs e)
        {
            try
            {
                RadTreeView tree = this.solutionTree;
                SolutionTreeNode node = (SolutionTreeNode)tree.SelectedNode;
                Sequence item = (Sequence)node.Object;



                SequenceService sequence = new SequenceService();
                

                sequence.Save(item);



                MessageBox.Show(String.Format("Sequence {0} was saved successfully to the database", item.Name));
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }

        }

        void MenuItem_Sequence_UpdateAllSequencesToDataBase_Click(object sender, EventArgs e)
        {
            try
            {
                RadTreeView tree = this.solutionTree;
                SolutionTreeNode node = (SolutionTreeNode)tree.SelectedNode;

                SequenceService sequence = new SequenceService();
                
                int i = 0;
                foreach (Sequence item in Configuration.GetInstance().Sequences)
                {
                    i++;
                    sequence.Save(item);
                }


                MessageBox.Show(String.Format("{0} sequence(s) were saved successfully to the database", i));

            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }

        }

        

        
      

        private void solutionFilter_TextChanged(object sender, EventArgs e)
        {
            this.solutionTree.Filter = this.solutionFilter.Text;
        }

      

       

        private void menuGenerateGridUniqueNames_Click(object sender, EventArgs e)
        {
            try
            {
                GridUniqueNamerForm form = new GridUniqueNamerForm();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
        }

        private void menuGenerateMenuItemUniqueCodes_Click(object sender, EventArgs e)
        {
            try
            {
                MenuCodeNamerForm form = new MenuCodeNamerForm();
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
        }

        private void menuResourceTranslations_Click(object sender, EventArgs e)
        {
            try
            {
                DataConnection connection = Project.GetDefaultDataConnection();
                ResourceTranslationForm form = new ResourceTranslationForm(connection);
                
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
        }

        private void menuEditConnections_Click(object sender, EventArgs e)
        {
            try
            {
                ProjectConnections form = new ProjectConnections();
                form.Project = this.Project;
                DialogResult result = form.ShowDialog();
                
                if (result == System.Windows.Forms.DialogResult.OK)
                { 
                    //save project
                    ConfigurationLoader.SerializeObjectToFile(this.Project, this.ProjectFile);
                    LoadDataCommandProviders();
                }
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
        }

        private void radMenuViewSolutionExplorer_Click(object sender, EventArgs e)
        {
            try
            {
                DockWindow toolWindow = dock.DockWindows["solutionWindow"];
                if (toolWindow != null)
                {
                    toolWindow.Show();
                    //dock.ShowAutoHidePopup((ToolWindow)toolWindow);
                }
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
        }

        private void dock_DockWindowClosed(object sender, DockWindowEventArgs e)
        {
            CloseDocument(e);
        }

        private static void CloseDocument(DockWindowEventArgs e)
        {
            if (e.DockWindow is ProjectItemWindow)
            {
                ProjectItemWindow.Remove((ProjectItemWindow)e.DockWindow);
            }

            //if (e.DockWindow is EditScreenWindow)
            //{
            //    EditScreenWindow.Remove((EditScreenWindow)e.DockWindow);
            //}
        }

        private void menuOpen_Click(object sender, EventArgs e)
        {
            try
            {
                LoadProjectSelectionScreen(false);
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
        }

        private void cmdBuild_Click(object sender, EventArgs e)
        {
            try
            {
                Build();

            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
        }

        public void Build()
        {
            statusBarLabel.Text = String.Format("Saving All Open Documents at {0} {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());

            if (dock.DockWindows.DocumentWindows.Length > 0)
            {
                foreach (DocumentWindow doc in dock.DockWindows.DocumentWindows)
                {
                    SaveDocument(doc);
                }


            }

            statusBarLabel.Text = String.Format("Build started at {0} {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());

            CompilerOptions options = new CompilerOptions();
            options.ConfigurationFolder = Project.ConfigurationFolder;
            options.RootNamespace = Project.RootNamespace;
            options.OutputLocations = Project.OutputLocations;

            CompilerResults results = Compiler.GenerateConfigurationAssembly(options);

            if (results.Errors.Count == 0)
            {

                statusBarLabel.Text = String.Format("Build successful at {0} {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
            }
            else
            {

                statusBarLabel.Text = String.Format("Build failed at {0} {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
            }
        }

        private void LoadDataCommandProviders()
        {
            //clear all cached data command providers
            DataCommandService service = DataCommandService.GetInstance();
            if (service != null)
            {
                service.Connections.Clear();

                //loop through all project connections and load data command providers into cache

                foreach (DataConnection connection in this.Project.Connections)
                {
                    try
                    {
                        IDataCommandProvider provider = DataCommandService.GetInstance().GetProvider(connection);
                    }
                    catch (Exception ex)
                    {
                        ErrorManager.HandleError(ex);
                    }
                }
                //all providers are now loaded
            }
        }

        
    }
}
