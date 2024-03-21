using CodeTorch.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CodeTorch.Core
{
    public enum SectionZoneLayoutMode
    {
        Static,
        Dynamic
    }

    public class Screen
    {
        bool _ValidateRequest = true;
        private List<Section> _Sections = new List<Section>();
        private CodeTorch.Core.Action _OnPageLoad = new CodeTorch.Core.Action();
        private CodeTorch.Core.Action _OnPageInit = new CodeTorch.Core.Action();

        [ReadOnly(true)]
        [Category("Common")]
        public string Name { get; set; }

       

        ScreenPageTemplate _PageTemplate = new ScreenPageTemplate();
        [Category("Common")]
        [PropertyValueChangedCommand("PageTemplate.Mode", "ScreenPageTemplateChangedCommand")]
        public virtual ScreenPageTemplate PageTemplate
        {
            get
            {
                return _PageTemplate;
            }
            set
            {
                _PageTemplate = value;
            }
        }
        
        [ReadOnly(true)]
        [Category("Common")]
        public string Type { get; set; }

        [Browsable(false)]
        public string Folder { get; set; }

        [Category("Security")]
        public virtual bool RequireAuthentication { get; set; }

        bool _SupportsTransactions = true;
        [Category("Misc")]
        public bool SupportsTransactions {
            get { return _SupportsTransactions; }
            set { _SupportsTransactions = value; }
        }

        [Category("Security")]
        public bool ValidateRequest
        {
            get { return _ValidateRequest; }
            set { _ValidateRequest = value; }
        }

        ScreenMenu _Menu = new ScreenMenu();
        [Category("Common")]
        public virtual ScreenMenu Menu
        {
            get
            {
                return _Menu;
            }
            set
            {
                _Menu = value;
            }
        }

        ScreenTitle _Title = new ScreenTitle();
        [Category("Common")]
        public virtual ScreenTitle Title
        {
            get 
            {
                return _Title;
            }
            set 
            {
                _Title = value;
            }
        }

        ScreenTitle _SubTitle = new ScreenTitle();
        [Category("Common")]
        public virtual ScreenTitle SubTitle
        {
            get
            {
                return _SubTitle;
            }
            set
            {
                _SubTitle = value;
            }
        }


        PermissionCheck _ScreenPermission = new PermissionCheck();

        [Category("Security")]
        public virtual PermissionCheck ScreenPermission
        {
            get
            {
                return _ScreenPermission;
            }
            set
            {
                _ScreenPermission = value;
            }
        }

        

        [Category("Sections")]
        [Description("List of page sections")]
        [XmlArray("Sections")]
        [Editor("CodeTorch.Core.Design.SectionCollectionEditor,CodeTorch.Core.Design", typeof(UITypeEditor))]
        public virtual List<Section> Sections
        {
            get
            {
                return _Sections;
            }
            set
            {
                _Sections = value;
            }

        }

        [Category("Sections")]
        [TypeConverter("CodeTorch.Core.Design.SectionZoneLayoutTypeConverter,CodeTorch.Core.Design")]
        public string SectionZoneLayout { get; set; }

        [Category("Sections")]
        public SectionZoneLayoutMode SectionZoneLayoutMode { get; set; } = SectionZoneLayoutMode.Static;

        [Category("Sections")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandTypeConverter,CodeTorch.Core.Design")]
        public string SectionZoneLayoutDataCommand { get; set; }

        [Category("Sections")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string SectionZoneLayoutDataField { get; set; }

        List<Setting> _settings = new List<Setting>();

        [XmlArray("Settings")]
        [XmlArrayItem("Setting")]
        [Description("List of page specific settings")]
        [Category("Settings")]
        [Browsable(false)]
        public virtual List<Setting> Settings
        {
            get
            {
                return _settings;
            }

        }

        List<Script> _scripts = new List<Script>();

        [XmlArray("Scripts")]
        [XmlArrayItem("Script")]
        [Description("List of page client side script to include")]
        public virtual List<Script> Scripts
        {
            get
            {
                return _scripts;
            }
            set
            {
                _scripts = value;
            }

        }

        List<ScreenDataCommand> _commands = new List<ScreenDataCommand>();

        [XmlArray("DataCommands")]
        [XmlArrayItem("DataCommand")]
        [Description("List of page specific datacommands and their page input settings")]
        [Category("Data")]
        [Editor("CodeTorch.Core.Design.ScreenDataCommandCollectionEditor,CodeTorch.Core.Design", typeof(UITypeEditor))]
        public virtual List<ScreenDataCommand> DataCommands
        {
            get
            {
                return _commands;
            }
            set
            {
                _commands = value;
            }

        }

        private static XmlSerializer serializer = null;
        public static void Load(XDocument doc, string FolderName)
        {

            string pageType = doc.Root.Element("Type").Value;

            var combinedTypes = GetExtraTypes();

            
            if (serializer == null)
            {
                serializer = new XmlSerializer(typeof(Screen),
                combinedTypes
                );
            }


            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            Screen screen = null;

            try
            {

                screen = (Screen)serializer.Deserialize(reader);
                screen.Folder = FolderName;
                Screen.SetupGridColumns(screen);
                Screen.SetupControls(screen);


                Configuration.GetInstance().Screens.Add(screen);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Error occurred while processing Screen - {0}", doc.Root.FirstNode.ToString()), ex);
            }

        }

        public static Screen GetScreenObjectByType(string TypeName)
        {
            Screen screen = new Screen();
            
            

            return screen;
        }

        private static void SetupControls(Screen screen)
        {
            TieControlsToSections(screen, screen.Sections);

        }

        

        private static void TieControlsToSections(Screen screen, List<EditSection> sections)
        {
            if(sections != null)
            {
                foreach (Section sectionObj in sections)
                {
                    if (sectionObj is EditSection)
                    {
                        EditSection section = (EditSection)sectionObj;
                        foreach (Widget control in section.Widgets)
                        {
                            control.Parent = section;
                        }
                    }

                    sectionObj.Parent = screen;
                }
            }
        }

        private static void TieControlsToSections(Screen screen, List<Section> sections)
        {
            if (sections != null)
            {
                foreach (Section section in sections)
                {
                    foreach (Widget control in section.Widgets)
                    {
                        control.Parent = section;
                    }

                    section.Parent = screen;
                }
            }
        }

        private static void SetupGridColumns(Screen screen)
        {
            foreach (Section sectionObj in screen.Sections)
            {
                if (sectionObj is GridSection)
                {
                    GridSection section = (GridSection)sectionObj;

                    TieGridMembersToGrid(screen, section.Grid);
                }

                if (sectionObj is EditableGridSection)
                {
                    EditableGridSection section = (EditableGridSection)sectionObj;

                    TieGridMembersToGrid(screen, section.Grid);
                }
            }

        }

        private static void TieGridMembersToGrid(Screen screen, Grid grid)
        {
            if (grid != null)
            {
                foreach (GridColumn col in grid.Columns)
                {
                    col.Parent = grid;
                }

                foreach (GridGroupByExpression expression in grid.GroupByExpressions)
                {
                    expression.Grid = grid;

                    foreach (GridGroupByField f in expression.Fields)
                    {
                        f.Expression = expression;
                    }
                }

                grid.Parent = screen;
            }
        }

        public static void Save(Screen item)
        {
            string ConfigPath = ConfigurationLoader.GetFileConfigurationPath();

            if(!Directory.Exists(String.Format("{0}Screens\\{1}", ConfigPath, item.Folder)))
            {
                Directory.CreateDirectory(String.Format("{0}Screens\\{1}", ConfigPath, item.Folder));
            }

            string filePath = String.Format("{0}Screens\\{1}\\{2}.xml", ConfigPath, item.Folder, item.Name);

            ConfigurationLoader.SerializeObjectToFile(item, filePath, GetExtraTypes());

        }

        private static Type[] GetExtraTypes()
        {
            var sectionTypes = SectionType.GetTypeArray();
            var widgetTypes = ControlType.GetTypeArray();

            var combinedTypes = sectionTypes.Concat(widgetTypes).ToArray();
            return combinedTypes;
        }

        public static List<Screen> GetByFolder(string FolderName)
        {
            var retVal = from item in Configuration.GetInstance().Screens
                         where item.Folder.ToLower() == FolderName.ToLower()
                         select item;
            return retVal.ToList<Screen>();
        }

        public static List<string> GetDistinctFolders()
        {
            var retVal = (
                            from item in Configuration.GetInstance().Screens
                            select item.Folder
                         )
                         .Distinct()
                         ;

            return retVal.ToList<string>();
        }

        public override string ToString()
        {
            return this.Name.ToString();
        }

        public static Screen GetByFolderAndName(string FolderName, string ScreenName)
        {
            var supportExtensionlessScreens = Configuration.GetInstance().App.SupportExtensionlessScreens;
            
            if (!supportExtensionlessScreens)
            {
                //if file does not end in .aspx add .aspx
                //this is to allow using Microsoft.AspNet.FriendlyUrls nuget package

                if (!ScreenName.ToLower().EndsWith(".aspx"))
                {
                    ScreenName += ".aspx";
                }
            }

            Screen screen = Configuration.GetInstance().Screens
                            .Where(s => 
                                (
                                    (s.Folder.ToLower()==FolderName.ToLower()) &&
                                    (s.Name.ToLower() == ScreenName.ToLower())
                                )
                            )
                            .SingleOrDefault();

            return screen;
        }

        public static ScreenDataCommand GetDataCommand(Screen screen, string DataCommandName)
        {
            ScreenDataCommand command = screen.DataCommands
                            .Where(d =>
                                (
                                    (d.Name.ToLower() == DataCommandName.ToLower())
                                )
                            )
                            .SingleOrDefault();

            return command;
        }


        internal static int GetItemCount(string Folder, string Name)
        {
            int retVal = 0;


            retVal = Configuration.GetInstance().Screens
                            .Where(i =>
                                (
                                    (((!String.IsNullOrEmpty(Name)) && (i.Name.ToLower() == Name.ToLower())) || (String.IsNullOrEmpty(Name))) &&
                                    (((!String.IsNullOrEmpty(Folder)) && (i.Folder.ToLower() == Folder.ToLower())) || (String.IsNullOrEmpty(Folder)))
                                )
                            ).Count();
     

            return retVal;
        }

        public static IEnumerable<ResourceItem> GetResourceKeys(Screen Screen)
        {
            List<ResourceItem> retVal = new List<ResourceItem>();

            //add title
            AddResourceKey(retVal, Screen, "Screen.Title.Name", Screen.Title.Name);
            //sub titles
            AddResourceKey(retVal, Screen, "Screen.SubTitle.Name", Screen.SubTitle.Name);

            
            
          

            return retVal;
        }

        protected static void AddResourceKey(List<ResourceItem> keys, Screen screen, string ResourceKey, string DefaultValue)
        {
            
            ResourceItem key = new Core.ResourceItem();

            key.ResourceSet = String.Format("App/{0}/{1}", screen.Folder, screen.Name);
            key.Key = ResourceKey;
            key.Value = DefaultValue;

            if (!String.IsNullOrEmpty(DefaultValue))
            {
                keys.Add(key);
            }
        }

       

        [Category("Actions")]
        public Action OnPageLoad
        {
            get
            {
                return _OnPageLoad;
            }
            set
            {
                _OnPageLoad = value;
            }

        }

        

        [Category("Actions")]
        public Action OnPageInit
        {
            get
            {
                return _OnPageInit;
            }
            set
            {
                _OnPageInit = value;
            }

        }

        

        
    }
}
