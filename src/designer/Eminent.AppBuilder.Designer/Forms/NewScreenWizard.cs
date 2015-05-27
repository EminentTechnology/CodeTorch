using CodeTorch.Core;
using CodeTorch.Core.Commands;
using CodeTorch.Designer.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;
using System.Text.RegularExpressions;

namespace CodeTorch.Designer.Forms
{
    public partial class NewScreenWizard : Telerik.WinControls.UI.RadForm
    {
        const int PAGE_GENERAL = 0;
        const int PAGE_TITLES = 1;
        const int PAGE_ENTITY = 2;
        const int PAGE_DATACOMMANDS = 3;
        const int PAGE_ACTIONLINKS = 4;
        const int PAGE_CRITERIA = 5;
        const int PAGE_GRID = 6;
        const int PAGE_SECTIONS = 7;
        const int PAGE_SECTION_CONTROLS = 8;
        const int PAGE_LOGIN = 9;
        const int PAGE_REDIRECT = 10;
        const int PAGE_FINISH = 11;

        enum ScreenType
        { 
            Blank,
            Comments,
            Content,
            CriteriaListEdit,
            Documents,
            Edit,
            List,
            ListEdit,
            Login,
            Logout,
            Overview,
            Picker,
            Redirect,
            ReportExport,
            ReportViewer,
            Search
        }



        Stack<PageState> pages = new Stack<PageState>();
        WizardPage current = null;
        ScreenType screenType = ScreenType.Blank;
        

        public DockWindow DockWindow {get;set;}
        public RadTreeView Solution { get; set; }
        public MainForm2 MainForm { get; set; }

        Dictionary<string, ScreenDataCommand> DataCommands = new Dictionary<string, ScreenDataCommand>();
        Dictionary<string, Section> Sections = new Dictionary<string, Section>();

        
        
        GridSection currentGridSection = null;
        
        DetailsSection currentDetailsSection = null;
        EditSection currentEditSection = null;
        CriteriaSection currentCriteriaSection = null;
        bool UseDetailsSection = false;

        bool InSectionsLoop = false;
        int SectionIndex = -1;

        public NewScreenWizard()
        {
            InitializeComponent();
            this.radContextMenuManager.SetRadContextMenu(this.wizard, this.radContextMenu);
            Setup();
            SetupConvertControls();
            SetupConvertGridColumns();
        }

        class PageState
        {
            public WizardPage Page { get; set; }
            public bool IsSection { get; set; }
            public int SectionIndex { get; set; }
        }

        void Setup()
        {
            ScreenTypeList.DataSource = null;
            ScreenTypeList.DataSource = Enum.GetNames(typeof(ScreenType));
            ScreenTypeList.Text = "";

            //populate folder list 
            FolderList.DataSource = null;
            FolderList.DataSource = CodeTorch.Core.Screen.GetDistinctFolders();
            FolderList.Text = "";

            //page templates
            PageTemplateList.DataSource = null;
            PageTemplateList.DisplayMember = "Name";
            PageTemplateList.DataSource = CodeTorch.Core.Configuration.GetInstance().PageTemplates;
            PageTemplateList.Text = "";
            

            //page templates
            MenuList.DataSource = null;
            MenuList.DisplayMember = "Name";
            MenuList.DataSource = CodeTorch.Core.Configuration.GetInstance().Menus;
            MenuList.Text = "";

            //permissions
            PermissionList.DataSource = null;
//            PermissionList.DisplayMember = String.Empty;
            PermissionList.DisplayMember = "Name";
            PermissionList.DataSource = CodeTorch.Core.Configuration.GetInstance().Permissions;
            PermissionList.Text = "";

            TitleCommandList.DataSource = null;
            TitleCommandList.DisplayMember = "Name";
            TitleCommandList.DataSource = GetDataCommandsReturningDataTablesList(); 
            TitleCommandList.Text = "";

            SubtitleCommandList.DataSource = null;
            SubtitleCommandList.DisplayMember = "Name";
            SubtitleCommandList.DataSource = GetDataCommandsReturningDataTablesList(); 
            SubtitleCommandList.Text = "";


            EntityInputTypeList.DataSource = null;
            EntityInputTypeList.DataSource = Enum.GetNames(typeof(CodeTorch.Core.ScreenInputType));
            EntityInputTypeList.Text = "";



            ActionLinkPermissionList.DataSource = null;
            ActionLinkPermissionList.DisplayMember = "Name";
            ActionLinkPermissionList.DataSource = CodeTorch.Core.Configuration.GetInstance().Permissions;
            ActionLinkPermissionList.Text = "";

            SectionLayoutList.DataSource = null;
            SectionLayoutList.DisplayMember = "Name";
            SectionLayoutList.DataSource = GetSectionLayoutsList();
            SectionLayoutList.Text = "";

            RedirectDataCommandList.DataSource = null;
            RedirectDataCommandList.DisplayMember = "Name";
            RedirectDataCommandList.DataSource = GetDataCommandsReturningDataTablesList();
            RedirectDataCommandList.Text = "";

            

            

            SetupSectionScreen();
            
        }

        private void SetupConvertControls()
        {
            foreach (ControlType c in CodeTorch.Core.Configuration.GetInstance().ControlTypes)
            {
                RadMenuItem item = new RadMenuItem();
                item.Tag = c;
                item.Text = String.Format("{0}", c.Name);
                item.Click += ConvertCriteriaControlItem_Click;
                ConvertCriteriaControl.Items.Add(item);

                item = new RadMenuItem();
                item.Tag = c;
                item.Text = String.Format("{0}", c.Name);
                item.Click += ConvertSectionControlItem_Click;
                this.ConvertSectionControl.Items.Add(item);


            }
        }

        private void SetupSectionScreen()
        {
            

            FillWithSectionDataCommands(Section1DataCommandList);
            FillWithSectionDataCommands(Section2DataCommandList);
            FillWithSectionDataCommands(Section3DataCommandList);
            FillWithSectionDataCommands(Section4DataCommandList);
            FillWithSectionDataCommands(Section5DataCommandList);
            FillWithSectionDataCommands(Section6DataCommandList);
            FillWithSectionDataCommands(Section7DataCommandList);
            FillWithSectionDataCommands(Section8DataCommandList);
            FillWithSectionDataCommands(Section9DataCommandList);
            FillWithSectionDataCommands(Section10DataCommandList);

            
        }


        private void SetupConvertGridColumns()
        {
            RadMenuItem item = null;
            
            item = new RadMenuItem();
            item.Text = "BinaryImageGridColumn";
            item.Click += ConvertGridColumItem_Click;
            this.ConvertGridColumn.Items.Add(item);

            item = new RadMenuItem();
            item.Text = "BoundGridColumn";
            item.Click += ConvertGridColumItem_Click;
            this.ConvertGridColumn.Items.Add(item); 

            item = new RadMenuItem();
            item.Text = "DeleteGridColumn";
            item.Click += ConvertGridColumItem_Click;
            this.ConvertGridColumn.Items.Add(item);

            item = new RadMenuItem();
            item.Text = "EditGridColumn";
            item.Click += ConvertGridColumItem_Click;
            this.ConvertGridColumn.Items.Add(item);

            item = new RadMenuItem();
            item.Text = "HyperLinkGridColumn";
            item.Click += ConvertGridColumItem_Click;
            this.ConvertGridColumn.Items.Add(item);

            item = new RadMenuItem();
            item.Text = "PickerHyperLinkGridColumn";
            item.Click += ConvertGridColumItem_Click;
            this.ConvertGridColumn.Items.Add(item);

            item = new RadMenuItem();
            item.Text = "PickerLinkButtonGridColumn";
            item.Click += ConvertGridColumItem_Click;
            this.ConvertGridColumn.Items.Add(item);
        }
        

        private List<DataCommand> GetDataCommandsReturningDataTablesList()
        {
            List<DataCommand> retVal = ObjectCopier.Clone<List<DataCommand>>(CodeTorch.Core.Configuration.GetInstance().DataCommands);

            var b = retVal.Where(d =>
                                     (
                                         (d.ReturnType == DataCommandReturnType.DataTable)
                                     )
                                 );

            return b.ToList<DataCommand>();

        }

        private List<DataCommand> GetDataCommandsForExecuteList()
        {
            List<DataCommand> retVal = ObjectCopier.Clone<List<DataCommand>>(CodeTorch.Core.Configuration.GetInstance().DataCommands);

            var b = retVal.Where(d =>
                                     (
                                         (d.ReturnType == DataCommandReturnType.Integer)
                                     )
                                 );

            return b.ToList<DataCommand>();

        }

        private List<SectionZoneLayout> GetSectionLayoutsList()
        {
            List<SectionZoneLayout> retVal = ObjectCopier.Clone<List<SectionZoneLayout>>(CodeTorch.Core.Configuration.GetInstance().SectionZoneLayouts);



            return retVal;

        }

        private void FillWithSectionTypesList(RadDropDownList list)
        {
            list.Items.Clear();

            if (screenType == ScreenType.Overview)
            {
                list.Items.Add("Details");
                list.Items.Add("Grid");
            }
            else
            {

                list.Items.Add("Edit");
                list.Items.Add("Grid");
            }
            
            list.Text = String.Empty;

        }

        private void FillWithSectionDataCommands(RadDropDownList list)
        {


            list.DataSource = null;
            list.DisplayMember = "Name";
            list.ValueMember = "Name";
            list.DataSource = GetDataCommandsReturningDataTablesList();
            list.Text = String.Empty;

        }

        private void wizard_Next(object sender, Telerik.WinControls.UI.WizardCancelEventArgs e)
        {

            current = wizard.SelectedPage;

            bool IsValid = PerformValidations();

            if (IsValid)
            {
                PageState state = new PageState();

                state.Page = current;
                state.IsSection = InSectionsLoop;
                state.SectionIndex = SectionIndex;

                pages.Push(state);

                if (current == wizard.Pages[PAGE_SECTIONS])
                { 
                    //this is a special page that creates a dynamic loop

                    //build array of sections - taking whatever sections we may already have into consideration

                    AddSectionScreen(0, Section1TypeList.Text, Section1Name.Text, Section1ZoneList.Text, Section1DataCommandList.Text);
                    AddSectionScreen(1, Section2TypeList.Text, Section2Name.Text, Section2ZoneList.Text, Section2DataCommandList.Text);
                    AddSectionScreen(2, Section3TypeList.Text, Section3Name.Text, Section3ZoneList.Text, Section3DataCommandList.Text);
                    AddSectionScreen(3, Section4TypeList.Text, Section4Name.Text, Section4ZoneList.Text, Section4DataCommandList.Text);
                    AddSectionScreen(4, Section5TypeList.Text, Section5Name.Text, Section5ZoneList.Text, Section5DataCommandList.Text);
                    AddSectionScreen(5, Section6TypeList.Text, Section6Name.Text, Section6ZoneList.Text, Section6DataCommandList.Text);
                    AddSectionScreen(6, Section7TypeList.Text, Section7Name.Text, Section7ZoneList.Text, Section7DataCommandList.Text);
                    AddSectionScreen(7, Section8TypeList.Text, Section8Name.Text, Section8ZoneList.Text, Section8DataCommandList.Text);
                    AddSectionScreen(8, Section9TypeList.Text, Section9Name.Text, Section9ZoneList.Text, Section9DataCommandList.Text);
                    AddSectionScreen(9, Section10TypeList.Text, Section10Name.Text, Section10ZoneList.Text, Section10DataCommandList.Text);

                    InSectionsLoop = true;
                    SectionIndex = -1;
                }

                if (InSectionsLoop)
                {
                    SectionIndex++;
                    current = GetNextSectionPage(SectionIndex);

                    if (current == null)
                    {
                        current = wizard.Pages[PAGE_FINISH];
                    }
                }
                else
                {
                    current = GetNextPage();
                }
                wizard.SelectedPage = current;
            }
            
            e.Cancel = true;

            
        }

        private WizardPage GetNextSectionPage(int index)
        {
            WizardPage p = null;

            if(Sections.ContainsKey("SECTION_" + index.ToString()))
            {
                Section section = Sections["SECTION_" + index.ToString()];

                switch (section.Type.ToLower())
                { 
                    case "details":
                        currentDetailsSection = (DetailsSection)section;
                        p = wizard.Pages[PAGE_SECTION_CONTROLS];
                        UseDetailsSection = true;
                        break;
                    case "edit":
                        currentEditSection = (EditSection)section;
                        p = wizard.Pages[PAGE_SECTION_CONTROLS];
                        UseDetailsSection = false;
                        break;
                    case "grid":
                        currentGridSection = (GridSection)section;
                        p = wizard.Pages[PAGE_GRID];
                        break;
                }

                SectionIndex = index;
            }
            else
            {
                index++;

                if(index < 10)
                {
                    p = GetNextSectionPage(index);
                }
                
            }

            return p;

        }

        private void AddSectionScreen(int keyIndex, string Type, string Name, string Zone, string SelectDataCommand)
        {
            if (!String.IsNullOrEmpty(Type))
            {
                string SectionKey = "SECTION_" + keyIndex.ToString();

                Section section = null;

                bool CreateNewSection = true;

                if (Sections.ContainsKey(SectionKey))
                {
                    section = Sections[SectionKey];

                    if (section.Type.ToLower() == Type.ToLower())
                    {
                        CreateNewSection = false;
                    }
                }
                
                
                if(CreateNewSection)
                { 
                    //create section of that type and add to sections collection
                    section = Section.GetNewSection(Type);
                    Sections[SectionKey] = section;
                }

                //set attributes

                if (!String.IsNullOrEmpty(Name))
                {
                    section.Name = Name;

                    if (section is GridSection)
                    {
                        ((GridSection)section).Grid.Name = Name;
                    }

                }

                if (!String.IsNullOrEmpty(SelectDataCommand))
                {
                    section.SelectDataCommand = SelectDataCommand;

                    if (section is GridSection)
                    {
                        ((GridSection)section).Grid.SelectDataCommand = SelectDataCommand;
                    }

                }

                if (section is EditSection)
                {
                    section.ContainerElement = "fieldset";
                    section.ContainerCssClass = "form-horizontal";
                }

                if (String.IsNullOrEmpty(Zone))
                {
                    section.ContentPane = "Left";
                }
                else 
                {
                    section.ContentPane = Zone;
                }

                section.ContainerMode = SectionContainer.Panel;
            }
            
        }

        private void wizard_Previous(object sender, Telerik.WinControls.UI.WizardCancelEventArgs e)
        {
            

            if (pages.Count > 0)
            {
                PageState state = pages.Pop();

                SectionIndex = state.SectionIndex;
                InSectionsLoop = state.IsSection;

                wizard.SelectedPage = state.Page;
                

            }
            else
            {
                wizard.SelectedPage = wizard.Pages[PAGE_GENERAL];
                InSectionsLoop = false;
            }

            

            e.Cancel = true;
            
           
        }

        private WizardPage GetNextPage()
        {
            WizardPage nextPage = null;

            switch (screenType)
            {
                case ScreenType.Blank:
                    if (current == wizard.Pages[PAGE_GENERAL])
                    {
                        nextPage = wizard.Pages[PAGE_TITLES];
                    }

                    if (current == wizard.Pages[PAGE_TITLES])
                    {
                        if (
                            ((this.TitleUseCommand.Enabled) && (!String.IsNullOrEmpty(this.TitleCommandList.Text))) ||
                            ((this.SubtitleUseCommand.Enabled) && (!String.IsNullOrEmpty(this.SubtitleCommandList.Text)))
                        )
                        {
                            nextPage = wizard.Pages[PAGE_DATACOMMANDS];
                        }
                    }
                    break;
                case ScreenType.Comments:
                    if (current == wizard.Pages[PAGE_GENERAL])
                    {
                        nextPage = wizard.Pages[PAGE_TITLES];
                    }

                    if (current == wizard.Pages[PAGE_TITLES])
                    {
                        if (
                            ((this.TitleUseCommand.Enabled) && (!String.IsNullOrEmpty(this.TitleCommandList.Text))) ||
                            ((this.SubtitleUseCommand.Enabled) && (!String.IsNullOrEmpty(this.SubtitleCommandList.Text)))
                        )
                        {
                            nextPage = wizard.Pages[PAGE_DATACOMMANDS];
                        }
                        else
                        {
                            nextPage = wizard.Pages[PAGE_ENTITY];
                        }
                    }

                    if (current == wizard.Pages[PAGE_DATACOMMANDS])
                    {
                        nextPage = wizard.Pages[PAGE_ENTITY];
                    }
                    break;
                case ScreenType.Content:
                    if (current == wizard.Pages[PAGE_GENERAL])
                    {
                        nextPage = wizard.Pages[PAGE_TITLES];
                    }

                    if (current == wizard.Pages[PAGE_TITLES])
                    {
                        if (
                            ((this.TitleUseCommand.Enabled) && (!String.IsNullOrEmpty(this.TitleCommandList.Text))) ||
                            ((this.SubtitleUseCommand.Enabled) && (!String.IsNullOrEmpty(this.SubtitleCommandList.Text)))
                        )
                        {
                            nextPage = wizard.Pages[PAGE_DATACOMMANDS];
                        }
                    }

                    break;
                case ScreenType.CriteriaListEdit:
                    if (current == wizard.Pages[PAGE_GENERAL])
                    {
                        nextPage = wizard.Pages[PAGE_TITLES];
                    }

                    if (current == wizard.Pages[PAGE_TITLES])
                    {
                        nextPage = wizard.Pages[PAGE_DATACOMMANDS];
                    }

                    if (current == wizard.Pages[PAGE_DATACOMMANDS])
                    {
                        nextPage = wizard.Pages[PAGE_ACTIONLINKS];
                    }

                    if (current == wizard.Pages[PAGE_ACTIONLINKS])
                    {
                        nextPage = wizard.Pages[PAGE_CRITERIA];
                    }

                    if (current == wizard.Pages[PAGE_CRITERIA])
                    {
                        nextPage = wizard.Pages[PAGE_GRID];
                    }

                    if (current == wizard.Pages[PAGE_GRID])
                    {
                        nextPage = wizard.Pages[PAGE_SECTIONS];
                    }

                    if (current == wizard.Pages[PAGE_SECTIONS])
                    {
                        nextPage = wizard.Pages[PAGE_SECTION_CONTROLS];
                    }
                  
                    break;
                case ScreenType.Documents:
                    if (current == wizard.Pages[PAGE_GENERAL])
                    {
                        nextPage = wizard.Pages[PAGE_TITLES];
                    }

                    if (current == wizard.Pages[PAGE_TITLES])
                    {
                        if (
                            ((this.TitleUseCommand.Enabled) && (!String.IsNullOrEmpty(this.TitleCommandList.Text))) ||
                            ((this.SubtitleUseCommand.Enabled) && (!String.IsNullOrEmpty(this.SubtitleCommandList.Text)))
                        )
                        {
                            nextPage = wizard.Pages[PAGE_DATACOMMANDS];
                        }
                        else
                        {
                            nextPage = wizard.Pages[PAGE_ENTITY];
                        }
                    }

                    if (current == wizard.Pages[PAGE_DATACOMMANDS])
                    {
                        nextPage = wizard.Pages[PAGE_ENTITY];
                    }
                    break;
                case ScreenType.Edit:
                    if (current == wizard.Pages[PAGE_GENERAL])
                    {
                        nextPage = wizard.Pages[PAGE_TITLES];
                    }

                    if (current == wizard.Pages[PAGE_TITLES])
                    {
                        nextPage = wizard.Pages[PAGE_ENTITY];
                    }

                    if (current == wizard.Pages[PAGE_ENTITY])
                    {
                        nextPage = wizard.Pages[PAGE_DATACOMMANDS];
                    }

                    if (current == wizard.Pages[PAGE_DATACOMMANDS])
                    {
                        nextPage = wizard.Pages[PAGE_SECTIONS];
                    }

                    if (current == wizard.Pages[PAGE_SECTIONS])
                    {
                        nextPage = wizard.Pages[PAGE_SECTION_CONTROLS];
                    }
                    break;
                
                case ScreenType.List:
                    if (current == wizard.Pages[PAGE_GENERAL])
                    {
                        nextPage = wizard.Pages[PAGE_TITLES];
                    }

                    if (current == wizard.Pages[PAGE_TITLES])
                    {
                        nextPage = wizard.Pages[PAGE_DATACOMMANDS];
                    }

                    if (current == wizard.Pages[PAGE_DATACOMMANDS])
                    {
                        nextPage = wizard.Pages[PAGE_ACTIONLINKS];
                    }

                    if (current == wizard.Pages[PAGE_ACTIONLINKS])
                    {
                        nextPage = wizard.Pages[PAGE_GRID];
                    }
                    break;
                case ScreenType.ListEdit:
                    if (current == wizard.Pages[PAGE_GENERAL])
                    {
                        nextPage = wizard.Pages[PAGE_TITLES];
                    }

                    if (current == wizard.Pages[PAGE_TITLES])
                    {
                        nextPage = wizard.Pages[PAGE_DATACOMMANDS];
                    }

                    if (current == wizard.Pages[PAGE_DATACOMMANDS])
                    {
                        nextPage = wizard.Pages[PAGE_ACTIONLINKS];
                    }

                    if (current == wizard.Pages[PAGE_ACTIONLINKS])
                    {
                        nextPage = wizard.Pages[PAGE_GRID];
                    }

                    if (current == wizard.Pages[PAGE_GRID])
                    {
                        nextPage = wizard.Pages[PAGE_SECTIONS];
                    }

                    if (current == wizard.Pages[PAGE_SECTIONS])
                    {
                        nextPage = wizard.Pages[PAGE_SECTION_CONTROLS];
                    }
                    break;
                case ScreenType.Login:
                    if (current == wizard.Pages[PAGE_GENERAL])
                    {
                        nextPage = wizard.Pages[PAGE_TITLES];
                    }

                    if (current == wizard.Pages[PAGE_TITLES])
                    {
                        nextPage = wizard.Pages[PAGE_DATACOMMANDS];
                    }

                    if (current == wizard.Pages[PAGE_DATACOMMANDS])
                    {
                        nextPage = wizard.Pages[PAGE_LOGIN];
                    }
                    break;
                case ScreenType.Logout:
                    
                    break;
                case ScreenType.Overview:
                    if (current == wizard.Pages[PAGE_GENERAL])
                    {
                        nextPage = wizard.Pages[PAGE_TITLES];
                    }

                    if (current == wizard.Pages[PAGE_TITLES])
                    {
                        nextPage = wizard.Pages[PAGE_ENTITY];
                    }

                    if (current == wizard.Pages[PAGE_ENTITY])
                    {
                        nextPage = wizard.Pages[PAGE_DATACOMMANDS];
                    }

                    if (current == wizard.Pages[PAGE_DATACOMMANDS])
                    {
                        nextPage = wizard.Pages[PAGE_SECTIONS];
                    }

                    if (current == wizard.Pages[PAGE_SECTIONS])
                    {
                        nextPage = wizard.Pages[PAGE_SECTION_CONTROLS];
                    }
                    break;
                case ScreenType.Picker:
                    if (current == wizard.Pages[PAGE_GENERAL])
                    {
                        nextPage = wizard.Pages[PAGE_TITLES];
                    }

                    if (current == wizard.Pages[PAGE_TITLES])
                    {
                        nextPage = wizard.Pages[PAGE_DATACOMMANDS];
                    }

                    if (current == wizard.Pages[PAGE_DATACOMMANDS])
                    {
                        nextPage = wizard.Pages[PAGE_ACTIONLINKS];
                    }

                    if (current == wizard.Pages[PAGE_ACTIONLINKS])
                    {
                        nextPage = wizard.Pages[PAGE_CRITERIA];
                    }

                    if (current == wizard.Pages[PAGE_CRITERIA])
                    {
                        nextPage = wizard.Pages[PAGE_GRID];
                    }

                    if (current == wizard.Pages[PAGE_GRID])
                    {
                        nextPage = wizard.Pages[PAGE_SECTIONS];
                    }

                    if (current == wizard.Pages[PAGE_SECTIONS])
                    {
                        nextPage = wizard.Pages[PAGE_SECTION_CONTROLS];
                    }
                    break;
                case ScreenType.Redirect:
                    if (current == wizard.Pages[PAGE_GENERAL])
                    {
                        nextPage = wizard.Pages[PAGE_REDIRECT];
                    }

                    if (current == wizard.Pages[PAGE_REDIRECT])
                    {
                        if (this.RedirectUsingDataCommand.IsChecked)
                        {
                            nextPage = wizard.Pages[PAGE_DATACOMMANDS];
                        }
                    }
                    
                  
                    break;
                case ScreenType.ReportExport:
                    
                    break;
                case ScreenType.ReportViewer:
                    if (current == wizard.Pages[PAGE_GENERAL])
                    {
                        nextPage = wizard.Pages[PAGE_TITLES];
                    }

                    if (current == wizard.Pages[PAGE_TITLES])
                    {
                        nextPage = wizard.Pages[PAGE_DATACOMMANDS];
                    }

                    if (current == wizard.Pages[PAGE_DATACOMMANDS])
                    {
                        nextPage = wizard.Pages[PAGE_ACTIONLINKS];
                    }

                    if (current == wizard.Pages[PAGE_ACTIONLINKS])
                    {
                        nextPage = wizard.Pages[PAGE_CRITERIA];
                    }
                    break;
                case ScreenType.Search:
                    if (current == wizard.Pages[PAGE_GENERAL])
                    {
                        nextPage = wizard.Pages[PAGE_TITLES];
                    }

                    if (current == wizard.Pages[PAGE_TITLES])
                    {
                        nextPage = wizard.Pages[PAGE_DATACOMMANDS];
                    }

                    if (current == wizard.Pages[PAGE_DATACOMMANDS])
                    {
                        nextPage = wizard.Pages[PAGE_ACTIONLINKS];
                    }

                    if (current == wizard.Pages[PAGE_ACTIONLINKS])
                    {
                        nextPage = wizard.Pages[PAGE_CRITERIA];
                    }

                    if (current == wizard.Pages[PAGE_CRITERIA])
                    {
                        nextPage = wizard.Pages[PAGE_GRID];
                    }
                    break;
                
            }

            if (nextPage == null)
            {
                nextPage = wizard.Pages[PAGE_FINISH];
            }

            return nextPage;
        }

        private bool PerformValidations()
        {
            bool retVal = true;

            if (current == wizard.Pages[PAGE_GENERAL])
            {
                retVal = PerformValidationsForGeneralInfoScreen();
            }

            if (current == wizard.Pages[PAGE_TITLES])
            {
                retVal = PerformValidationsForTitleScreen();
            }

            if (current == wizard.Pages[PAGE_ACTIONLINKS])
            {
                retVal = PerformValidationsForActionLinksScreen();
            }

            return retVal;
        }

        private void PerformNextStep()
        {
            
        }

        private bool PerformValidationsForGeneralInfoScreen()
        {
            bool retVal = false;

            List<string> errors = new List<string>();

            //required
            if (String.IsNullOrEmpty(ScreenTypeList.Text))
            {
                errors.Add("Screen type is required");
            }

            

            if (String.IsNullOrEmpty(FolderList.Text))
            {
                errors.Add("Folder is required");
            }

            if (String.IsNullOrEmpty(ScreenName.Text))
            {
                errors.Add("Screen name is required");
            }

            if (!String.IsNullOrEmpty(ScreenName.Text) && !String.IsNullOrEmpty(FolderList.Text))
            {
                Core.Screen e = CodeTorch.Core.Screen.GetByFolderAndName(FolderList.Text, ScreenName.Text);
                if (e != null)
                {
                    errors.Add("Screen with same name and folder already exists");
                }
            }

            if (CheckPermission.Enabled && CheckPermission.Checked)
            {
                if (String.IsNullOrEmpty(PermissionList.Text))
                {
                    errors.Add("Permission is required");
                }
            }

            retVal = ProcessErrors(retVal, errors);

            return retVal;
        }

        private bool PerformValidationsForTitleScreen()
        {
            bool retVal = false;

            List<string> errors = new List<string>();

            //required
            if (
                this.TitleUseCommand.Enabled &&
                this.TitleUseCommand.Checked &&
                String.IsNullOrEmpty(this.Title.Text)
                )
            {
                errors.Add("Title format is required");
            }

            if (
                this.TitleUseCommand.Enabled &&
                this.TitleUseCommand.Checked &&
                String.IsNullOrEmpty(this.TitleCommandList.Text)
                )
            {
                errors.Add("Title command is required");
            }


            if (
                this.SubtitleUseCommand.Enabled &&
                this.SubtitleUseCommand.Checked &&
                String.IsNullOrEmpty(this.Subtitle.Text)
                )
            {
                errors.Add("Subtitle format is required");
            }

            if (
                this.SubtitleUseCommand.Enabled &&
                this.SubtitleUseCommand.Checked &&
                String.IsNullOrEmpty(this.SubtitleCommandList.Text)
                )
            {
                errors.Add("Subtitle command is required");
            }

            retVal = ProcessErrors(retVal, errors);

            return retVal;
        }

        private bool PerformValidationsForActionLinksScreen()
        {
            bool retVal = false;

            List<string> errors = new List<string>();



            if (this.ActionLinksCheckPermission.Enabled && ActionLinksCheckPermission.Checked)
            {
                if (String.IsNullOrEmpty(this.ActionLinkPermissionList.Text))
                {
                    errors.Add("Permission is required");
                }
            }

            retVal = ProcessErrors(retVal, errors);

            return retVal;
        }

        private static bool ProcessErrors(bool retVal, List<string> errors)
        {
            if (errors.Count > 0)
            {
                retVal = false;

                StringBuilder sb = new StringBuilder();

                foreach (string s in errors)
                {
                    sb.AppendLine(s);
                }

                MessageBox.Show(sb.ToString(), "The following error(s) occurred:", MessageBoxButtons.OK, MessageBoxIcon.Error);


            }
            else
            {
                retVal = true;
            }
            return retVal;
        }

        

        private void wizard_Finish(object sender, EventArgs e)
        {
            CreateScreen();
        }

        private void CreateScreen()
        {
            CodeTorch.Core.Screen screen = new CodeTorch.Core.Screen();

            SetCommonScreenAttributes(screen);
            

            switch (screenType)
            { 

                case ScreenType.Blank:
                    screen.SectionZoneLayout = "SingleColumn";
                    ProcessRenderPageSections(screen, true);
                    ProcessTitles(screen);
                    ProcessDataCommands(screen);
                    
                    break;

                case ScreenType.Comments:
                    screen.SectionZoneLayout = "SingleColumn";
                    ProcessRenderPageSections(screen, false);
                    ProcessTitles(screen);
                    ProcessDataCommands(screen);
                    ProcessComments(screen);
                    //todo  - how to handle data commands in title
                    //cancel redirect page
                    //defaults for subtitle, input types to avoid typing - maybe dont add redirect url command
                    break;
                case ScreenType.Content:
                    screen.SectionZoneLayout = "SingleColumn";
                    ProcessRenderPageSections(screen, true);
                    ProcessTitles(screen);
                    ProcessDataCommands(screen);
                    ProcessContent(screen);
                    break;
                case ScreenType.CriteriaListEdit:
                    screen.SectionZoneLayout = "SingleColumn";
                    ProcessRenderPageSections(screen, true);
                    ProcessTitles(screen);
                    ProcessDataCommands(screen);
                    AddAlertSection(screen, true);
                    ProcessCriteriaSection(screen);
                    ProcessCriteriaListEditScreen(screen);
                    break;
                case ScreenType.Documents:
                    screen.SectionZoneLayout = "SingleColumn";
                    ProcessRenderPageSections(screen, false);
                    ProcessTitles(screen);
                    ProcessDataCommands(screen);
                    ProcessDocuments(screen);
                    break;
                case ScreenType.Edit:
                    screen.SectionZoneLayout = this.SectionLayoutList.Text;

                    ProcessRenderPageSections(screen, false);
                    ProcessDefaultOrPopulateScreenCommand(screen);

                    ProcessTitles(screen);
                    ProcessDataCommands(screen);

                    AddAlertSection(screen, true);
                    ProcessSections(screen);
                    ProcessSaveButtons(screen);
                    
                    break;

                case ScreenType.List:
                    screen.SectionZoneLayout = "SingleColumn";
                    ProcessRenderPageSections(screen, true);
                    ProcessDataCommands(screen);
                    ProcessTitles(screen);
                    AddAlertSection(screen, false);
                    ProcessActionLink(screen);
                    ProcessSections(screen);
                    break;
                case ScreenType.ListEdit:
                    screen.SectionZoneLayout = "SingleColumn";
                    ProcessRenderPageSections(screen, true);
                    ProcessTitles(screen);
                    ProcessDataCommands(screen);
                    AddAlertSection(screen, true);

                    ProcessListEditScreen(screen);
                    
                    break;
                case ScreenType.Login:
                    screen.SectionZoneLayout = "SingleColumn";
                    ProcessRenderPageSections(screen, true);
                    ProcessTitles(screen);
                    ProcessDataCommands(screen);
                    AddAlertSection(screen, true);
                    ProcessLoginScreen(screen);
                    
                    break;
                case ScreenType.Logout:
                    LogoutCommand logout = new LogoutCommand();
                    logout.Name = "Logout";

                    screen.OnPageLoad.Commands.Add(logout);
                    break;
                case ScreenType.Overview:
                    screen.SectionZoneLayout = this.SectionLayoutList.Text;

                    ProcessRenderPageSections(screen, true);
                    ProcessDefaultOrPopulateScreenCommand(screen);

                    ProcessTitles(screen);
                    ProcessDataCommands(screen);

                    AddAlertSection(screen, true);
                    ProcessSections(screen);

                    break;
                case ScreenType.Picker:
                    screen.SectionZoneLayout = "SingleColumn";
                    ProcessRenderPageSections(screen, false);
                    ProcessTitles(screen);
                    ProcessDataCommands(screen);
                    break;
                case ScreenType.Redirect:
                    ProcessRedirect(screen);
                    ProcessDataCommands(screen);
                    break;
                case ScreenType.ReportExport:
                    ProcessDataCommands(screen);
                    break;
                case ScreenType.ReportViewer:
                    screen.SectionZoneLayout = "SingleColumn";
                    ProcessRenderPageSections(screen, true);
                    ProcessTitles(screen);
                    ProcessDataCommands(screen);
                    AddAlertSection(screen, false);
                    ProcessActionLink(screen);
                    ProcessCriteriaSection(screen);
                    ProcessDefaultOrPopulateScreenCommand(screen);
                    break;
                case ScreenType.Search:
                    screen.SectionZoneLayout = "SingleColumn";
                    ProcessRenderPageSections(screen, true);
                    ProcessDataCommands(screen);
                    ProcessTitles(screen);
                    AddAlertSection(screen, false);
                    ProcessActionLink(screen);

                    ProcessCriteriaSection(screen);

                    ProcessSections(screen);

                    ProcessDefaultOrPopulateScreenCommandForSearchScreen(screen);

                    break;
            }


            CodeTorch.Core.Screen.Save(screen);

            if ((Solution != null) && (MainForm != null))
            {
                
                MainForm.RefreshAll();
                MainForm.Build();
                //find screen
                Predicate<RadTreeNode> match = new Predicate<RadTreeNode>(delegate(RadTreeNode node)
                {
                    return
                        (
                            (node.Tag != null) &&
                            (node.Tag.ToString().ToUpper() == "OBJECT")  &&
                            (node is SolutionTreeNode) &&
                            (((SolutionTreeNode)node).Key.ToLower() == String.Format("SCREEN|{0}|{1}", screen.Folder, screen.Name).ToLower())
                        )
                        ? true : false;
                });


                //OpenDocument
                RadTreeNode[] nodes = Solution.FindNodes(match);

                if ((nodes != null) && (nodes.Length > 0))
                {
                    //take 1st match
                    nodes[0].Selected = true;
                    MainForm.OpenDocument( (SolutionTreeNode)nodes[0], this.DockWindow);
                }
            }
        }

        private void ProcessCriteriaListEditScreen(Core.Screen screen)
        {
            EditableGridSection section = new EditableGridSection();

            //get grid and create editable grid section
            if (this.Sections.ContainsKey("Grid"))
            {
                GridSection grid = (GridSection)this.Sections["Grid"];


                section.SectionZoneLayout = this.SectionLayoutList.Text;

                section.ID = grid.ID;
                section.Name = grid.Name;
                section.IntroText = grid.IntroText;
                section.Visible = grid.Visible;
                section.Mode = grid.Mode;
                section.ContainerCssClass = grid.ContainerCssClass;
                section.ContainerElement = grid.ContainerElement;
                section.ContainerMode = grid.ContainerMode;
                section.ContentPane = grid.ContentPane;
                section.Controls = grid.Controls;
                section.CssClass = grid.CssClass;
                section.Permission = grid.Permission;
                section.Grid = grid.Grid;
                section.LoadDataOnPageLoad = grid.LoadDataOnPageLoad;


                //TODO - blank; might need a screen for this
                section.Grid.DataKeyNames = "";
                section.Grid.DataKeyParameterNames = "";

                //populate from action section
                section.ActionLink = grid.ActionLink;

                //data commands
                section.SelectDataCommand = grid.SelectDataCommand;

                section.InsertCommand = GetDataCommandNameByKey("Insert");
                section.UpdateCommand = GetDataCommandNameByKey("Update");
                section.DefaultCommand = GetDataCommandNameByKey("Default");

                //lop through all sectins that start with section and add them to editable grid 
                foreach (KeyValuePair<string, Section> s in this.Sections)
                {
                    if (s.Key.ToLower().StartsWith("section_"))
                    {
                        section.Sections.Add(s.Value);
                    }
                }

                //add button section for save and cancel 
                ProcessSaveButtons(section);

                ProcessActionLink(section);

                screen.Sections.Add(section);

            }




        }

        private void ProcessListEditScreen(Core.Screen screen)
        {
            EditableGridSection section = new EditableGridSection();
            
            //get grid and create editable grid section
            if (this.Sections.ContainsKey("Grid"))
            {
                GridSection grid = (GridSection)this.Sections["Grid"];

                
                section.SectionZoneLayout = this.SectionLayoutList.Text;

                section.ID = grid.ID;
                section.Name = grid.Name;
                section.IntroText = grid.IntroText;
                section.Visible = grid.Visible;
                section.Mode = grid.Mode;
                section.ContainerCssClass = grid.ContainerCssClass;
                section.ContainerElement = grid.ContainerElement;
                section.ContainerMode = grid.ContainerMode;
                section.ContentPane = grid.ContentPane;
                section.Controls = grid.Controls;
                section.CssClass = grid.CssClass;
                section.Permission = grid.Permission;
                section.Grid = grid.Grid;
                section.LoadDataOnPageLoad = grid.LoadDataOnPageLoad;


                //TODO - blank; might need a screen for this
                section.Grid.DataKeyNames = "";
                section.Grid.DataKeyParameterNames = "";

                //populate from action section
                section.ActionLink = grid.ActionLink;

                //data commands
                section.SelectDataCommand = grid.SelectDataCommand;

                section.InsertCommand = GetDataCommandNameByKey("Insert");
                section.UpdateCommand = GetDataCommandNameByKey("Update");
                section.DefaultCommand = GetDataCommandNameByKey("Default");

                //lop through all sectins that start with section and add them to editable grid 
                foreach (KeyValuePair<string, Section> s in this.Sections)
                {
                    if (s.Key.ToLower().StartsWith("section_"))
                    {
                        section.Sections.Add(s.Value);
                    }
                }

                //add button section for save and cancel 
                ProcessSaveButtons(section);

                ProcessActionLink(section);

                screen.Sections.Add(section);

            }

            

            
        }

        private string GetDataCommandNameByKey(string DataCommandKey)
        {
            string retVal = null;

            if (this.DataCommands.ContainsKey(DataCommandKey))
            {
                ScreenDataCommand iCommand = this.DataCommands[DataCommandKey];
                if (!String.IsNullOrEmpty(iCommand.Name))
                {
                    retVal = iCommand.Name;
                }
            }

            return retVal;
        }

        private void ProcessLoginScreen(CodeTorch.Core.Screen screen)
        {
            ScreenDataCommand profile = new ScreenDataCommand();

            if (this.DataCommands.ContainsKey("Profile"))
            {
                profile = this.DataCommands["Profile"];
            }

            if (String.IsNullOrEmpty(profile.Name))
            {
                profile = new ScreenDataCommand("User_GetByUserName");

                ScreenDataCommandParameter p = new ScreenDataCommandParameter("UserName", ScreenInputType.Control);
                p.Name = "@UserName";

                profile.Parameters.Add(p);

                screen.DataCommands.Add(profile);
            }

            EditSection edit = new EditSection();
            edit.ID = "LoginSection";
            edit.Name = "Login";
            edit.Mode = SectionMode.All;
            edit.Visible = true;
            edit.ContentPane = "Left";
            edit.Permission = new PermissionCheck(false);
            edit.ContainerElement = "fieldset";
            edit.ContainerCssClass = "form-horizontal";

            TextBoxControl control = new TextBoxControl();
            if (profile.Parameters.Count > 0)
            {
                control.Name = profile.Parameters[0].Name.Replace("@", "");
            }
            else
            {
                control.Name = "UserName";             
            }
            control.Label = Common.ConvertWordToProperCase(control.Name);
            control.IsRequired = true;

            control.ControlGroupElement = "div";
            control.ControlGroupCssClass = "form-group";
            control.LabelRendersBeforeControl = true;
            control.LabelWrapsControl = false;
            control.ControlContainerElement = "div";
            control.HelpTextElement = "span";
            control.HelpTextCssClass = "help-block";
            control.LabelCssClass = "col-md-12";
            control.ControlContainerCssClass = "col-md-12";

            edit.Controls.Add(control);

            PasswordControl password = new PasswordControl();
            password.Name = "Password"; 
            password.Label = "Password";
            password.IsRequired = true;

            password.PasswordAlgorithm = "SaltHashProvider";
            password.PasswordMode = PasswordMode.PlainText;


            password.ControlGroupElement = "div";
            password.ControlGroupCssClass = "form-group";
            password.LabelRendersBeforeControl = true;
            password.LabelWrapsControl = false;
            password.ControlContainerElement = "div";
            password.HelpTextElement = "span";
            password.HelpTextCssClass = "help-block";
            password.LabelCssClass = "col-md-12";
            password.ControlContainerCssClass = "col-md-12";

            edit.Controls.Add(password);

            screen.Sections.Add(edit);

            //add buttons
            ButtonListSection buttons = new ButtonListSection();
            buttons.Name = "Buttons";
            buttons.Mode = SectionMode.All;
            buttons.ContainerMode = SectionContainer.Plain;
            buttons.CssClass = "text-center";
            buttons.Visible = true;
            buttons.ContentPane = "Bottom";
            buttons.Permission = new PermissionCheck(false);

            ButtonControl b = new ButtonControl();
            b.Name = "Login";
            b.Text = "Login";
            b.CssClass = "btn btn-lg btn-success btn-block";
            b.Visible = true;
            b.CausesValidation = true;

            ValidateUserCommand loginCommand = new ValidateUserCommand();
            loginCommand.Name = "LoginCommand";
            loginCommand.ProfileCommand = profile.Name;


            loginCommand.UserNameParameter = "@" + control.Name;

            loginCommand.PasswordEntityID = "Password";
            loginCommand.PasswordEntityInputType = ScreenInputType.Control;

            loginCommand.PasswordField = "Password";
            loginCommand.PasswordAlgorithm = "SaltHashProvider";

            loginCommand.PasswordMode = PasswordMode.Hash;
            loginCommand.LogoutTimeout = 30;


            b.OnClick.Commands.Add(loginCommand);

            
            buttons.Buttons.Add(b);
            screen.Sections.Add(buttons);

            if (DisplayForgotPasswordLink.Checked || DisplaySignupLink.Checked)
            {
                ContentSection section = new ContentSection();
                section.Name = "extra";
                section.ContainerMode = SectionContainer.None;
                section.Mode = SectionMode.All;
                section.Visible = true;
                section.ContentPane = "Left";
                section.Permission = new PermissionCheck(false);

                string forgotContent = "";

                if(DisplayForgotPasswordLink.Checked)
                    forgotContent = String.Format("<div class='pull-left'><a href='{0}'>{1}</a></div>", this.ForgotPasswordUrl.Text, this.ForgotPasswordText.Text);


                string signupContent = "";
                if (DisplaySignupLink.Checked)
                    signupContent = String.Format("<div class='pull-right'><a href='{0}'>{1}</a></div>", this.SignupUrl.Text, this.SignupText.Text);

                string content = String.Format("<div class='clearfix' style='padding-top:20px;'>{0}{1}</div>", forgotContent, signupContent);
                section.Content = content;

                screen.Sections.Add(section);
            
            }

            
        }

        private void ProcessSaveButtons(EditableGridSection grid)
        {
            ButtonListSection section = new ButtonListSection();
            section.Name = "Buttons";
            section.CssClass = "text-center";
            section.ContainerMode = SectionContainer.Plain;
            section.Mode = SectionMode.All;
            section.Visible = true;
            section.ContentPane = "Bottom";
            section.Permission = new PermissionCheck(false);

            



            ButtonControl save = new ButtonControl();
            save.Name = "Save";
            save.CssClass = "btn btn-primary";
            save.Text = "Save";
            save.CausesValidation = true;
            save.CommandName = "PerformInsert";

            section.Buttons.Add(save);

            

            ButtonControl cancel = new ButtonControl();
            cancel.Name = "Cancel";
            cancel.CssClass = "btn btn-default";
            cancel.Text = "Cancel";
            cancel.CausesValidation = false;
            cancel.CommandName = "Cancel";

            section.Buttons.Add(cancel);

            grid.Sections.Add(section);
        }

        private void ProcessSaveButtons(CodeTorch.Core.Screen screen)
        {
            ButtonListSection section = new ButtonListSection();
            section.Name = "Buttons";
            section.CssClass = "text-center";
            section.ContainerMode = SectionContainer.Plain;
            section.Mode = SectionMode.All;
            section.Visible = true;
            section.ContentPane = "Bottom";
            section.Permission = new PermissionCheck(false);

            InsertUpdateSaveCommand insertCommand = new InsertUpdateSaveCommand();
            insertCommand.Name = "SaveCommand";

            if (!String.IsNullOrEmpty(this.EntityInputTypeList.Text))
                insertCommand.EntityInputType = (ScreenInputType)Enum.Parse(typeof(ScreenInputType), this.EntityInputTypeList.Text);

            if (!String.IsNullOrEmpty(this.EntityInputTypeKey.Text))
                insertCommand.EntityID = this.EntityInputTypeKey.Text;
            


            ButtonControl save = new ButtonControl();
            save.Name = "Save";
            save.CssClass = "btn btn-primary";
            save.Text = "Save";
            save.CausesValidation = true;
            save.OnClick.Commands.Add(insertCommand);

            section.Buttons.Add(save);

            NavigateToUrlCommand nCommand = new NavigateToUrlCommand();
            nCommand.Name = "NavigateOnCancel";

            if (!String.IsNullOrEmpty(this.EntityInputTypeKey.Text))
                nCommand.EntityID = this.EntityInputTypeKey.Text;

            ButtonControl cancel = new ButtonControl();
            cancel.Name = "Cancel";
            cancel.CssClass = "btn btn-default";
            cancel.Text = "Cancel";
            cancel.CausesValidation = false;
            cancel.OnClick.Commands.Add(nCommand);

            section.Buttons.Add(cancel);

            screen.Sections.Add(section);
        }

        private void ProcessCriteriaSection(Core.Screen screen)
        {
            CriteriaSection criteriaSection = null;
            if (this.Sections.ContainsKey("Criteria"))
            {
                criteriaSection = (CriteriaSection)Sections["Criteria"];

                criteriaSection.ColumnElement = "div";
              
                int columnWidth = (12 / criteriaSection.ControlsPerRow);
                if (columnWidth < 1)
                    columnWidth = 1;
                if (columnWidth > 12)
                    columnWidth = 12;

                criteriaSection.ColumnCssClass = String.Format("col-md-{0}", columnWidth);

                criteriaSection.RowElement = "div";
                criteriaSection.RowCssClass = "row";

                if (criteriaSection.Controls.Count > 0)
                {
                    if (criteriaSection.Controls[criteriaSection.Controls.Count - 1].Name.ToLower() != "searchbutton")
                    {
                        ButtonControl button = new ButtonControl();

                        button.Name = "ButtonSearch";
                        button.Text = "Search";
                        button.CssClass = "btn btn-primary criteria-button";
                        button.Visible = true;
                        button.ControlGroupElement = "div";
                        button.ControlGroupCssClass = "form-group";
                        button.CausesValidation = false;
                        button.LabelRendersBeforeControl = true;
                        button.HelpTextElement = "span";
                        button.HelpTextCssClass = "help-block";

                        InvokeSearchMessageCommand command = new InvokeSearchMessageCommand();
                        command.Name = "CriteriaClick";

                        button.OnClick.Commands.Add(command);

                        criteriaSection.Controls.Add(button);
                    }
                }
            }
        }

        private void ProcessDataCommands(Core.Screen screen)
        {
            foreach (ScreenDataCommand command in this.DataCommands.Values)
            {
                if (!String.IsNullOrEmpty(command.Name))
                {
                    //do not add existing commands - eg case of insert and update using same datacommand in wizard
                    List<ScreenDataCommand> exist = 
                        screen.DataCommands.Where
                        (x => 
                            (x.Name.ToLower() == command.Name.ToLower())
                        ).ToList<ScreenDataCommand>();

                    if (exist.Count == 0)
                    {
                        screen.DataCommands.Add(command);
                    }
                }
            }
        }

        private void ProcessActionLink(EditableGridSection section)
        {
            if (!String.IsNullOrEmpty(ActionLinksText.Text))
            {
                section.ActionLink.Text = ActionLinksText.Text;
                section.ActionLink.ShowLink = true;
            }

        }

        private void ProcessActionLink(Core.Screen screen)
        {
            if (!String.IsNullOrEmpty(ActionLinksText.Text))
            {
                LinkListSection section = new LinkListSection();
                section.ID = "Links";
                section.ContainerElement = "ul";
                section.ItemElement = "li";
                section.Mode = SectionMode.All;
                section.ContainerMode = SectionContainer.Plain;
                section.CssClass = "section-links";
                section.Visible = true;
                section.ContentPane = "Top";

                section.Permission = new PermissionCheck(ActionLinksCheckPermission.Checked);
                section.Permission.Name = ActionLinkPermissionList.Text;

                LinkListItem i = new LinkListItem();

                i.Text = "<span class='glyphicon glyphicon-plus'/> " + ActionLinksText.Text;
                i.Url = ActionLinksUrl.Text;
                i.Visible = true;

                section.Items.Add(i);

                screen.Sections.Add(section);
            }

        }

        private void ProcessTitles(Core.Screen screen)
        {
            ScreenTitle title = null;

            if (!String.IsNullOrEmpty(Title.Text))
            {
                title = new ScreenTitle();
                if (TitleUseCommand.Enabled && TitleUseCommand.Checked)
                {
                    title.UseCommand = true;
                    title.CommandFormatString = Title.Text;
                    title.CommandName = TitleCommandList.Text;
                }
                else
                {
                    title.Name = Title.Text;
                }

                screen.Title = title;
            }

            if (!String.IsNullOrEmpty(Subtitle.Text))
            {
                title = new ScreenTitle();
                if (SubtitleUseCommand.Enabled && SubtitleUseCommand.Checked)
                {
                    title.UseCommand = true;
                    title.CommandFormatString = Subtitle.Text;
                    title.CommandName = SubtitleCommandList.Text;
                }
                else
                {
                    title.Name = Subtitle.Text;
                }

                screen.SubTitle = title;
            }

        }

        private void ProcessRenderPageSections(Core.Screen screen, bool Simple)
        {
            RenderPageSectionsCommand command = new RenderPageSectionsCommand();

            command.Name = "RenderPageSections";
            if (Simple)
            {
                command.Mode = RenderPageSectionsCommand.SectionRenderMode.Simple;
                
            }
            else
            {
                command.Mode = RenderPageSectionsCommand.SectionRenderMode.InsertEdit;

                if (!String.IsNullOrEmpty(this.EntityInputTypeList.Text))
                    command.EntityInputType = (ScreenInputType)Enum.Parse(typeof(ScreenInputType), this.EntityInputTypeList.Text);

                if (!String.IsNullOrEmpty(this.EntityInputTypeKey.Text))
                    command.EntityID = this.EntityInputTypeKey.Text;
            }

            screen.OnPageInit.Commands.Add(command);
            

        }

        private void ProcessDefaultOrPopulateScreenCommand(Core.Screen screen)
        {
            DefaultOrPopulateScreenCommand command = new DefaultOrPopulateScreenCommand();

            command.Name = "DefaultOrPopulateScreen";

            if (this.DataCommands.ContainsKey("Default"))
            {
                command.DefaultCommand = DataCommands["Default"].Name;    
            }

            if (this.DataCommands.ContainsKey("Retrieve"))
            {
                command.RetrieveCommand = DataCommands["Retrieve"].Name;
            }

            

            if (!String.IsNullOrEmpty(this.EntityInputTypeList.Text))
                command.EntityInputType = (ScreenInputType)Enum.Parse(typeof(ScreenInputType), this.EntityInputTypeList.Text);

            if (!String.IsNullOrEmpty(this.EntityInputTypeKey.Text))
                command.EntityID = this.EntityInputTypeKey.Text;

            

            screen.OnPageLoad.Commands.Add(command);


        }

        private void ProcessDefaultOrPopulateScreenCommandForSearchScreen(Core.Screen screen)
        {
            if (this.DataCommands.ContainsKey("Default"))
            {
                DefaultOrPopulateScreenCommand command = new DefaultOrPopulateScreenCommand();

                command.Name = "DefaultOrPopulateScreen";

                command.DefaultCommand = DataCommands["Default"].Name;






                if (!String.IsNullOrEmpty(this.EntityInputTypeList.Text))
                    command.EntityInputType = (ScreenInputType)Enum.Parse(typeof(ScreenInputType), this.EntityInputTypeList.Text);

                if (!String.IsNullOrEmpty(this.EntityInputTypeKey.Text))
                    command.EntityID = this.EntityInputTypeKey.Text;



                screen.OnPageLoad.Commands.Add(command);
            }


        }

        private void ProcessComments(Core.Screen screen)
        {

            ScreenDataCommand command = null;
            ScreenDataCommandParameter p = null;
            
            //add data command to retrieve comments
            command = new ScreenDataCommand();
            command.Name = "Comment_GetCommentsByEntityID";

            p = new ScreenDataCommandParameter();
            p.Name = "@EntityID";
            
            if(!String.IsNullOrEmpty(this.EntityInputTypeList.Text))
                p.InputType = (ScreenInputType)Enum.Parse(typeof(ScreenInputType), this.EntityInputTypeList.Text);

            if (!String.IsNullOrEmpty(this.EntityInputTypeKey.Text))
                p.InputKey = this.EntityInputTypeKey.Text;

            command.Parameters.Add(p);

            p = new ScreenDataCommandParameter();
            p.Name = "@EntityType";
            p.InputType = ScreenInputType.Constant;

            if (!String.IsNullOrEmpty(this.Entity.Text))
                p.InputKey = this.Entity.Text;

            command.Parameters.Add(p);

            screen.DataCommands.Add(command);

            //add data command to save comments
            command = new ScreenDataCommand();
            command.Name = "Comment_SaveComment";

            p = new ScreenDataCommandParameter();
            p.Name = "@EntityID";

            if (!String.IsNullOrEmpty(this.EntityInputTypeList.Text))
                p.InputType = (ScreenInputType)Enum.Parse(typeof(ScreenInputType), this.EntityInputTypeList.Text);

            if (!String.IsNullOrEmpty(this.EntityInputTypeKey.Text))
                p.InputKey = this.EntityInputTypeKey.Text;

            command.Parameters.Add(p);

            p = new ScreenDataCommandParameter();
            p.Name = "@EntityType";
            p.InputType = ScreenInputType.Constant;

            if (!String.IsNullOrEmpty(this.Entity.Text))
                p.InputKey = this.Entity.Text;

            command.Parameters.Add(p);

            p = new ScreenDataCommandParameter();
            p.Name = "@Comments";
            p.InputType = ScreenInputType.Control;
            p.InputKey = "Comments";
            command.Parameters.Add(p);

            p = new ScreenDataCommandParameter();
            p.Name = "@CreatedBy";
            p.InputType = ScreenInputType.Special;
            p.InputKey = "UserName";
            command.Parameters.Add(p);

            screen.DataCommands.Add(command);

            screen.SectionZoneLayout = "SingleColumn";

            //add alert

            AddAlertSection(screen, true);

            //add comments edit section
            EditSection edit = new EditSection();
            edit.Name = "Enter your comments here";
            edit.Mode = SectionMode.All;
            edit.Visible = true;
            edit.ContentPane = "Left";
            edit.Permission = new PermissionCheck(false);
            edit.ContainerElement = "fieldset";
            edit.ContainerCssClass = "form-horizontal";

            TextAreaControl control = new TextAreaControl();
            control.Name = "Comments";
            control.IsRequired = true;
            control.Rows = 6;
            control.ControlGroupElement = "div";
            control.ControlGroupCssClass = "form-group";
            control.LabelRendersBeforeControl = true;
            control.LabelWrapsControl = false;
            control.ControlContainerElement = "div";
            control.HelpTextElement = "span";
            control.HelpTextCssClass = "help-block";
            control.LabelCssClass = "col-md-12";
            control.ControlContainerCssClass = "col-md-12";

            edit.Controls.Add(control);
            screen.Sections.Add(edit);

            //add buttons
            ButtonListSection buttons = new ButtonListSection();
            buttons.Name = "Buttons";
            buttons.Mode = SectionMode.All;
            buttons.ContainerMode = SectionContainer.Plain;
            buttons.CssClass = "text-center";
            buttons.Visible = true;
            buttons.ContentPane = "Bottom";
            buttons.Permission = new PermissionCheck(false);

            ButtonControl b = new ButtonControl();
            b.Name = "Save";
            b.Text = "Save";
            b.CssClass = "btn btn-primary";
            b.Visible = true;
            b.CausesValidation = true;

            InsertUpdateSaveCommand iCommand = new InsertUpdateSaveCommand();
            iCommand.Name = "SaveCommand";
            iCommand.InsertCommand = "Comment_SaveComment";
            iCommand.UpdateCommand = "Comment_SaveComment";
            iCommand.AfterInsertConfirmationMessage = "Comment added";
            iCommand.AfterUpdateConfirmationMessage = "Comment added";
            iCommand.RedirectAfterInsert = false;
            iCommand.RedirectAfterUpdate = false;
            if (!String.IsNullOrEmpty(this.EntityInputTypeList.Text))
                iCommand.EntityInputType = (ScreenInputType)Enum.Parse(typeof(ScreenInputType), this.EntityInputTypeList.Text);

            if (!String.IsNullOrEmpty(this.EntityInputTypeKey.Text))
                iCommand.EntityID = this.EntityInputTypeKey.Text;

            b.OnClick.Commands.Add(iCommand);

            SetControlPropertyCommand sCommand = new SetControlPropertyCommand();
            sCommand.Name = "ClearComments";
            sCommand.ControlName = "Comments";
            sCommand.PropertyName = "Value";
            sCommand.PropertyValue = String.Empty;
            b.OnClick.Commands.Add(sCommand);
            
            buttons.Buttons.Add(b);

            b = new ButtonControl();
            b.Name = "Cancel";
            b.Text = "Cancel";
            b.CssClass = "btn btn-default";
            b.Visible = true;
            b.CausesValidation = false;

            //NavigateToUrlCommand nCommand = new NavigateToUrlCommand();
            //nCommand.Name = "NavigateOnCancel";
            //if (!String.IsNullOrEmpty(this.EntityInputTypeKey.Text))
            //    nCommand.EntityID = this.EntityInputTypeKey.Text;

            //b.OnClick.Commands.Add(nCommand);
            buttons.Buttons.Add(b);
            screen.Sections.Add(buttons);

            //add grid
            GridSection gridSection = new GridSection();
            gridSection.ID = "Existing_Comments";
            gridSection.Name = "Existing Comments";
            gridSection.Mode = SectionMode.All;
            gridSection.ContainerMode = SectionContainer.Panel;
            gridSection.Visible = true;
            gridSection.ContentPane = "Left";
            gridSection.Permission = new PermissionCheck(false);
            //gridSection.SelectDataCommand = "Comment_GetCommentsByEntityID";

            Grid g = new Grid();
            g.Name = "Existing Comments";
            g.SelectDataCommand = "Comment_GetCommentsByEntityID";
            g.AllowPaging = true;
            g.PageSize = 20;
            g.AllowSorting = true;
            g.SortOrder = GridSortOrder.Descending;

            BoundGridColumn col = new BoundGridColumn();
            col.HeaderText = "Comments";
            col.DataField = "Comments";
            col.SortExpression = "Comments";
            col.IncludeInExport = true;
            g.Columns.Add(col);

            col = new BoundGridColumn();
            col.HeaderText = "Date/Time";
            col.DataField = "CreatedOn";
            col.SortExpression = "CreatedOn";
            col.DataFormatString = "{0:MMM dd yyyy HH:MM:ss}";
            col.IncludeInExport = true;
            g.Columns.Add(col);

            col = new BoundGridColumn();
            col.HeaderText = "Created By";
            col.DataField = "FullName";
            col.SortExpression = "FullName";
            col.IncludeInExport = true;
            g.Columns.Add(col);

            gridSection.Grid = g;

            screen.Sections.Add(gridSection);

        }

        private void ProcessContent(Core.Screen screen)
        {

            screen.SectionZoneLayout = "SingleColumn";

            //add alert

            AddAlertSection(screen, false);

            //add comments edit section
            ContentSection content = new ContentSection();
            content.ID = "ContentSection";
            content.Name = "Content";
            content.Mode = SectionMode.All;
            content.Visible = true;
            content.ContentPane = "Left";
            content.Permission = new PermissionCheck(false);
            content.Content = "Hello World - don't forget to edit me.";

            
            screen.Sections.Add(content);

           

        }

        private void ProcessRedirect(Core.Screen screen)
        {



            RedirectCommand command = new RedirectCommand();

            command.Name = "Redirect";

            
            
            

            if (this.RedirectToReferrer.IsChecked)
            {
                command.RedirectMode = RedirectCommand.RedirectModeEnum.Referrer;
            }

            if (this.RedirectToSpecificUrl.IsChecked)
            {
                command.RedirectMode = RedirectCommand.RedirectModeEnum.Constant;
                command.RedirectUrl = RedirectUrl.Text;
            }

            if (this.RedirectUsingDataCommand.IsChecked)
            {
                command.RedirectMode = RedirectCommand.RedirectModeEnum.DataCommand;
                command.DataCommand = RedirectDataCommandList.Text;
                command.RedirectUrlField = RedirectField.Text;
            }
            

            screen.OnPageLoad.Commands.Add(command);



        }

        private void ProcessSections(Core.Screen screen)
        {
            
            foreach (Section section in this.Sections.Values)
            {
                //add screen data command for grids in sections when it does not exist
                if (section is GridSection)
                {
                    GridSection s = (GridSection)section;

                    if (!String.IsNullOrEmpty(s.Grid.SelectDataCommand))
                    {
                        bool DataCommandDoesNotExist = true;

                        foreach (ScreenDataCommand command in this.DataCommands.Values)
                        {
                            
                            if (
                                (!String.IsNullOrEmpty(command.Name)) && 
                                (command.Name.ToLower() == s.Grid.SelectDataCommand.ToLower())
                               )
                            {
                                DataCommandDoesNotExist = false;
                                break;
                            }
                        }

                        if (DataCommandDoesNotExist)
                        {
                            screen.DataCommands.Add(new ScreenDataCommand(s.Grid.SelectDataCommand));
                        }
                    }
                }

                screen.Sections.Add(section);
            }



        }

        private void ProcessDocuments(Core.Screen screen)
        {

            ScreenDataCommand command = null;
            ScreenDataCommandParameter p = null;

            //add data command to retrieve comments
            command = new ScreenDataCommand();
            command.Name = "Document_GetDocumentsByEntityID";

            p = new ScreenDataCommandParameter();
            p.Name = "@EntityID";

            if (!String.IsNullOrEmpty(this.EntityInputTypeList.Text))
                p.InputType = (ScreenInputType)Enum.Parse(typeof(ScreenInputType), this.EntityInputTypeList.Text);

            if (!String.IsNullOrEmpty(this.EntityInputTypeKey.Text))
                p.InputKey = this.EntityInputTypeKey.Text;

            command.Parameters.Add(p);

            p = new ScreenDataCommandParameter();
            p.Name = "@EntityType";
            p.InputType = ScreenInputType.Constant;

            if (!String.IsNullOrEmpty(this.Entity.Text))
                p.InputKey = this.Entity.Text;

            command.Parameters.Add(p);

            screen.DataCommands.Add(command);

            //add data command to save comments
            command = new ScreenDataCommand();
            command.Name = "Document_UpdateEntity";

            p = new ScreenDataCommandParameter();
            p.Name = "@DocumentID";
            p.InputType = ScreenInputType.Control;
            p.InputKey = "FilesToUpload";
            command.Parameters.Add(p);

            p = new ScreenDataCommandParameter();
            p.Name = "@EntityID";

            if (!String.IsNullOrEmpty(this.EntityInputTypeList.Text))
                p.InputType = (ScreenInputType)Enum.Parse(typeof(ScreenInputType), this.EntityInputTypeList.Text);

            if (!String.IsNullOrEmpty(this.EntityInputTypeKey.Text))
                p.InputKey = this.EntityInputTypeKey.Text;

            command.Parameters.Add(p);

            p = new ScreenDataCommandParameter();
            p.Name = "@EntityType";
            p.InputType = ScreenInputType.Constant;

            if (!String.IsNullOrEmpty(this.Entity.Text))
                p.InputKey = this.Entity.Text;

            command.Parameters.Add(p);

           

            p = new ScreenDataCommandParameter();
            p.Name = "@ModifiedBy";
            p.InputType = ScreenInputType.Special;
            p.InputKey = "UserName";
            command.Parameters.Add(p);

            screen.DataCommands.Add(command);

            screen.SectionZoneLayout = "SingleColumn";

            //add alert

            AddAlertSection(screen, true);

            //add grid
            GridSection gridSection = new GridSection();
            gridSection.ID = "Existing_Documents_Section";
            gridSection.Name = "Existing Documents";
            gridSection.Mode = SectionMode.All;
            gridSection.ContainerMode = SectionContainer.Panel;
            gridSection.Visible = true;
            gridSection.ContentPane = "Left";
            gridSection.Permission = new PermissionCheck(false);
            //gridSection.SelectDataCommand = "Document_GetDocumentsByEntityID";

            Grid g = new Grid();
            g.Name = "Existing Documents";
            g.SelectDataCommand = "Document_GetDocumentsByEntityID";
            g.DataKeyNames = "DocumentID";
            g.DataKeyParameterNames = "@DocumentID";
            g.AllowPaging = true;
            g.AllowDelete = true;
            g.DeleteDataCommand = "Document_Inactivate";
            g.PageSize = 20;
            g.AllowSorting = true;
            g.SortOrder = GridSortOrder.Descending;

            HyperLinkGridColumn hcol = new HyperLinkGridColumn();
            hcol.DataNavigateUrlFields = "DocumentID";
            hcol.DataNavigateUrlFormatString = "~/App/Document/Download.aspx?DocumentID={0}";
            hcol.Text = "Download";
            hcol.Target = "_blank";
            hcol.IncludeInExport = false;
            g.Columns.Add(hcol);

            BoundGridColumn col = new BoundGridColumn();
            col.HeaderText = "Name";
            col.DataField = "DocumentName";
            col.SortExpression = "DocumentName";
            col.IncludeInExport = true;
            g.Columns.Add(col);

            col = new BoundGridColumn();
            col.HeaderText = "Type";
            col.DataField = "Description";
            col.SortExpression = "Description";
            col.IncludeInExport = true;
            g.Columns.Add(col);

            col = new BoundGridColumn();
            col.HeaderText = "Uploaded On";
            col.DataField = "CreatedOn";
            col.SortExpression = "CreatedOn";
            col.DataFormatString = "{0:MMM dd yyyy HH:MM:ss}";
            col.IncludeInExport = true;
            g.Columns.Add(col);

            col = new BoundGridColumn();
            col.HeaderText = "Uploaded By";
            col.DataField = "FullName";
            col.SortExpression = "FullName";
            col.IncludeInExport = true;
            g.Columns.Add(col);

            DeleteGridColumn dcol = new DeleteGridColumn();
            dcol.HeaderText = "Delete";
            dcol.ConfirmText = "WARNING: You are about to delete this document\r\nPress OK to DELETE this document\r\nPress CANCEL to LEAVE this document alone";
            dcol.IncludeInExport = false;
            g.Columns.Add(dcol);

            gridSection.Grid = g;

            screen.Sections.Add(gridSection);

            //add  edit section
            EditSection edit = new EditSection();
            edit.ID = "Documents_Upload_Section";
            edit.Name = "Select documents to upload here";
            edit.Mode = SectionMode.All;
            edit.Visible = true;
            edit.ContentPane = "Left";
            edit.Permission = new PermissionCheck(false);
            edit.ContainerElement = "fieldset";
            edit.ContainerCssClass = "form-horizontal";

            LookupDropDownListControl control = new LookupDropDownListControl();
            control.Name = "DocType";
            control.IsRequired = true;
            control.Label = "Document Type";
            control.IncludeAdditionalListItem = true;
            control.LookupType = "DOCUMENT_TYPE";
            control.AdditionalListItemText = "-- Select --";
            control.ControlGroupElement = "div";
            control.ControlGroupCssClass = "form-group";
            control.LabelRendersBeforeControl = true;
            control.LabelWrapsControl = false;
            control.ControlContainerElement = "div";
            control.HelpTextElement = "span";
            control.HelpTextCssClass = "help-block";
            control.LabelCssClass = "col-md-2";
            control.ControlContainerCssClass = "col-md-10";
            control.Visible = true;

            edit.Controls.Add(control);

            FileUploadControl fcontrol = new FileUploadControl();
            fcontrol.Name = "FilesToUpload";
            fcontrol.IsRequired = true;
            fcontrol.Label = "Documents";
            fcontrol.ControlGroupElement = "div";
            fcontrol.ControlGroupCssClass = "form-group";
            fcontrol.LabelRendersBeforeControl = true;
            fcontrol.LabelWrapsControl = false;
            fcontrol.ControlContainerElement = "div";
            fcontrol.HelpTextElement = "span";
            fcontrol.HelpTextCssClass = "help-block";
            fcontrol.LabelCssClass = "col-md-2";
            fcontrol.ControlContainerCssClass = "col-md-10";
            
            fcontrol.MaxFileInputsCount = 0;


            if (!String.IsNullOrEmpty(this.Entity.Text))
                fcontrol.Document.EntityType = Entity.Text;

            if(!String.IsNullOrEmpty(this.EntityInputTypeList.Text))
                fcontrol.EntityInputType = (ScreenInputType) Enum.Parse(typeof(ScreenInputType), this.EntityInputTypeList.Text);

            if (!String.IsNullOrEmpty(this.EntityInputTypeKey.Text))
                fcontrol.Document.EntityID = Entity.Text;

            fcontrol.Visible = true;;

            edit.Controls.Add(fcontrol);

            screen.Sections.Add(edit);

            //add buttons
            ButtonListSection buttons = new ButtonListSection();
            buttons.Name = "Buttons";
            buttons.Mode = SectionMode.All;
            buttons.ContainerMode = SectionContainer.Plain;
            buttons.CssClass = "text-center";
            buttons.Visible = true;
            buttons.ContentPane = "Bottom";
            buttons.Permission = new PermissionCheck(false);

            ButtonControl b = new ButtonControl();
            b.Name = "Save";
            b.Text = "Save";
            b.CssClass = "btn btn-primary";
            b.Visible = true;
            b.CausesValidation = true;

            InsertUpdateSaveCommand iCommand = new InsertUpdateSaveCommand();
            iCommand.Name = "SaveCommand";
            iCommand.InsertCommand = "Document_UpdateEntity";
            iCommand.UpdateCommand = "Document_UpdateEntity";
            iCommand.AfterInsertConfirmationMessage = "Document uploaded";
            iCommand.AfterUpdateConfirmationMessage = "Document uploaded";
            iCommand.RedirectAfterInsert = false;
            iCommand.RedirectAfterUpdate = false;
            if (!String.IsNullOrEmpty(this.EntityInputTypeList.Text))
                iCommand.EntityInputType = (ScreenInputType)Enum.Parse(typeof(ScreenInputType), this.EntityInputTypeList.Text);

            if (!String.IsNullOrEmpty(this.EntityInputTypeKey.Text))
                iCommand.EntityID = this.EntityInputTypeKey.Text;

            b.OnClick.Commands.Add(iCommand);

            SetControlPropertyCommand sCommand = new SetControlPropertyCommand();
            sCommand.Name = "ClearFiles";
            sCommand.ControlName = "FilesToUpload";
            sCommand.PropertyName = "Value";
            sCommand.PropertyValue = String.Empty;
            b.OnClick.Commands.Add(sCommand);

            buttons.Buttons.Add(b);

            b = new ButtonControl();
            b.Name = "Cancel";
            b.Text = "Cancel";
            b.CssClass = "btn btn-default";
            b.Visible = true;
            b.CausesValidation = false;

            //NavigateToUrlCommand nCommand = new NavigateToUrlCommand();
            //nCommand.Name = "NavigateOnCancel";
            //if (!String.IsNullOrEmpty(this.EntityInputTypeKey.Text))
            //    nCommand.EntityID = this.EntityInputTypeKey.Text;

            //b.OnClick.Commands.Add(nCommand);
            buttons.Buttons.Add(b);
            screen.Sections.Add(buttons);

            

        }

        private static void AddAlertSection(Core.Screen screen, bool IncludeValidationSummary)
        {
            AlertSection alert = new AlertSection();
            alert.Name = "Alert";
            alert.Mode = SectionMode.All;
            alert.ContainerMode = SectionContainer.None;
            alert.Visible = true;
            alert.ContentPane = "Top";
            alert.IncludeValidationSummary = IncludeValidationSummary;
            alert.Permission = new PermissionCheck(false);
            screen.Sections.Add(alert);
        }

        private void SetCommonScreenAttributes(CodeTorch.Core.Screen screen)
        {
            screen.Type = "Screen";
            screen.Name = ScreenName.Text;
            screen.Folder = FolderList.Text;

            ScreenMenu menu = new ScreenMenu();
            if (!String.IsNullOrEmpty(MenuList.Text))
            {
                menu.Name = MenuList.Text;
                menu.DisplayMenu = true;
            }
            else
            {
                menu.DisplayMenu = false;
            }
            screen.Menu = menu;

            if (!String.IsNullOrEmpty(PageTemplateList.Text))
            {
                ScreenPageTemplate pt = new ScreenPageTemplate();
                pt.Mode = ScreenPageTemplateMode.Static;
                pt.Name = PageTemplateList.Text;
                screen.PageTemplate = pt;
            }

            screen.RequireAuthentication = RequireAuthentication.Checked;

            PermissionCheck sp = new PermissionCheck();
            if (CheckPermission.Enabled && CheckPermission.Checked)
            {
                sp.CheckPermission = true;

                if (PermissionList.Enabled && !String.IsNullOrEmpty(PermissionList.Text))
                {
                    sp.Name = PermissionList.Text;
                }
            }
            else
            {
                sp.CheckPermission = false;
            }
            screen.ScreenPermission = sp;

            screen.ValidateRequest = true;
        }

        private void wizard_HelpRequested(object sender, HelpEventArgs hlpevent)
        {
            string URL = "http://support.codetorch.com";
            System.Diagnostics.Process.Start(URL);
        }

        private void wizard_Cancel(object sender, EventArgs e)
        {
            //do work then close
            if (this.DockWindow != null)
            {
                this.DockWindow.Close();
            }
            else
            {
                this.Close();
            }
        }

        private void wizard_SelectedPageChanging(object sender, Telerik.WinControls.UI.SelectedPageChangingEventArgs e)
        {

        }

        private void wizard_SelectedPageChanged(object sender, Telerik.WinControls.UI.SelectedPageChangedEventArgs e)
        {
            InitPage(e);

        }

        private void InitPage(Telerik.WinControls.UI.SelectedPageChangedEventArgs e)
        {
            if (e.SelectedPage == wizard.Pages[PAGE_DATACOMMANDS])
            {
                if (
                    (e.PreviousPage == wizard.Pages[PAGE_GENERAL]) || 
                    (e.PreviousPage == wizard.Pages[PAGE_TITLES]) ||
                    (e.PreviousPage == wizard.Pages[PAGE_ENTITY]) ||
                    (e.PreviousPage == wizard.Pages[PAGE_REDIRECT]) 
                   ) 
                {
                    LoadDataCommandsPage();
                }
            }

            if (e.SelectedPage == wizard.Pages[PAGE_CRITERIA])
            {

                CriteriaSection criteriaSection = InitCriteriaPage();

                LoadCriteriaPage(criteriaSection);

            }

            if (e.SelectedPage == wizard.Pages[PAGE_GRID])
            {
                GridSection gridSection = null;

                if (InSectionsLoop)
                {
                    gridSection = InitGridPage("SECTION_" + SectionIndex.ToString());
                }
                else
                { 
                    gridSection = InitGridPage("Grid");
                }
                LoadGridPage(gridSection);

            }

            if (e.SelectedPage == wizard.Pages[PAGE_SECTIONS])
            {
                if (e.PreviousPage == wizard.Pages[PAGE_DATACOMMANDS])
                {
                    LoadSectionsPage();
                }
                

            }

            if (e.SelectedPage == wizard.Pages[PAGE_SECTION_CONTROLS])
            {
                DetailsSection detailsSection = null;
                EditSection editSection = null;

                if (InSectionsLoop)
                {
                    InitSectionControlsPage(this.UseDetailsSection, out detailsSection, out editSection, "SECTION_" + SectionIndex.ToString());
                    currentDetailsSection = detailsSection;
                    currentEditSection = editSection;
                    LoadSectionControlsPage(this.UseDetailsSection, detailsSection, editSection);
                
                }
                

            }

            if (e.SelectedPage == wizard.Pages[PAGE_FINISH])
            {
                LoadFinishPage();
            }
        }

        private void InitSectionControlsPage(bool UseDetailsSection, out DetailsSection detailsSection, out EditSection editSection, string SectionKey)
        {
            detailsSection = null;
            editSection = null;

            if (UseDetailsSection)
            {
                if (this.Sections.ContainsKey(SectionKey))
                {
                    if (this.Sections[SectionKey] is DetailsSection)
                    {
                        detailsSection = (DetailsSection)Sections[SectionKey];
                    }
                    
                }

            }
            else
            {
                if (this.Sections.ContainsKey(SectionKey))
                {
                    if (this.Sections[SectionKey] is EditSection)
                    {
                        editSection = (EditSection)Sections[SectionKey];
                    }

                }
            }

        }

        private void LoadSectionControlsPage(bool UseDetailsSection, DetailsSection detailsSection, EditSection editSection)
        {
            List<BaseControl> assignedControls = null;

            if (UseDetailsSection)
            {
                this.SectionName.DataBindings.Clear();
                this.SectionName.DataBindings.Add("Text", detailsSection, "Name");

                this.SectionIntroText.DataBindings.Clear();
                this.SectionIntroText.DataBindings.Add("Text", detailsSection, "IntroText");

                assignedControls = detailsSection.Controls;
            }
            else
            {
                this.SectionName.DataBindings.Clear();
                this.SectionName.DataBindings.Add("Text", editSection, "Name");

                this.SectionIntroText.DataBindings.Clear();
                this.SectionIntroText.DataBindings.Add("Text", editSection, "IntroText");

                assignedControls = editSection.Controls;
            }

            PopulateAvailableSectionControls();

            this.AssignedSectionControlsList.DataSource = null;
            this.AssignedSectionControlsList.DisplayMember = "";
            this.AssignedSectionControlsList.DisplayMember = "Name";
            this.AssignedSectionControlsList.ValueMember = "Name";
            this.AssignedSectionControlsList.DataSource = assignedControls;
            this.AssignedSectionControlsList.Refresh();
            this.AssignedSectionControlsList.Update();

            



        }

        private void PopulateAvailableSectionControls()
        {
            //get list of available controls
            List<string> availableControls = new List<string>();

            ScreenDataCommand command = null;

            if (this.DataCommands.ContainsKey("Insert"))
            {
                command = this.DataCommands["Insert"];
                if ((command != null) && (!String.IsNullOrEmpty(command.Name)))
                {
                    foreach (var p in command.Parameters)
                    {
                        if ((p.InputType == ScreenInputType.Control) || (p.InputType == ScreenInputType.ControlText))
                        {
                            string parameterName = p.Name.Replace("@", "");

                            if (!availableControls.Contains(parameterName))
                            {
                                availableControls.Add(parameterName);
                            }
                        }
                    }
                }
            }

            if (this.DataCommands.ContainsKey("Update"))
            {
                command = this.DataCommands["Update"];
                if ((command != null) && (!String.IsNullOrEmpty(command.Name)))
                {
                    foreach (var p in command.Parameters)
                    {
                        if ((p.InputType == ScreenInputType.Control) || (p.InputType == ScreenInputType.ControlText))
                        {
                            string parameterName = p.Name.Replace("@", "");

                            if (!availableControls.Contains(parameterName))
                            {
                                availableControls.Add(parameterName);
                            }
                        }
                    }
                }
            }

            if (this.DataCommands.ContainsKey("Retrieve"))
            {
                command = this.DataCommands["Retrieve"];
                if ((command != null) && (!String.IsNullOrEmpty(command.Name)))
                {
                    DataCommand dc = DataCommand.GetDataCommand(command.Name);

                    if (dc != null)
                    {
                        foreach (var c in dc.Columns)
                        {
                            if (!availableControls.Contains(c.Name))
                            {
                                availableControls.Add(c.Name);
                            }
                        }
                    }

                }
            }

            List<string> remove = new List<string>();

            //loop through every control in existing details or edit sections
            foreach (Section section in Sections.Values)
            {
                if ((section is DetailsSection) || (section is EditSection))
                {
                    foreach (var c in section.Controls)
                    {
                        if (availableControls.Contains(c.Name))
                        {
                            remove.Add(c.Name);
                        }
                    }
                }
            }



            foreach (string s in remove)
            {
                availableControls.Remove(s);
            }

            availableControls.Sort();

            this.AvailableSectionControlsList.Items.Clear();

            foreach (string s in availableControls)
            {
                AvailableSectionControlsList.Items.Add(s);
            }
        }

        private GridSection InitGridPage(string SectionKey)
        {
            GridSection gridSection = null;
            ScreenDataCommand dc = this.DataCommands["Retrieve"];

            if (this.Sections.ContainsKey(SectionKey))
            {
                gridSection = (GridSection)Sections[SectionKey];

            }
            else
            {
                gridSection = new GridSection();
                gridSection.ContainerMode = SectionContainer.Panel;
                gridSection.ContentPane = "Left";
                gridSection.Mode = SectionMode.All;
                gridSection.IntroText = "Select ENTITY from the following list:";
                gridSection.LoadDataOnPageLoad = true;

                if (dc != null)
                {
                    gridSection.Grid.SelectDataCommand = dc.Name;
                    gridSection.Grid.AllowDelete = false;
                    gridSection.Grid.AllowPaging = true;
                    gridSection.Grid.PageSize = 20;
                    gridSection.Grid.AllowSorting = true;
                    gridSection.Grid.ShowRefreshButton = true;
                    gridSection.Grid.ExportIgnorePaging = true;
                    gridSection.Grid.ExportOpenInNewWindow = true;
                    gridSection.Grid.ExportHideStructureColumns = true;
                    gridSection.Grid.CsvColumnDelimiter = GridCsvDelimiter.Comma;
                    gridSection.Grid.CsvRowDelimiter = GridCsvDelimiter.NewLine;
                }

                this.Sections.Add(SectionKey, gridSection);


            }

            return gridSection;
        }

        private CriteriaSection InitCriteriaPage()
        {
            CriteriaSection criteriaSection = null;
            if (this.Sections.ContainsKey("Criteria"))
            {
                criteriaSection = (CriteriaSection)Sections["Criteria"];

                //need to add controls back --
            }
            else
            {
                criteriaSection = new CriteriaSection();
                criteriaSection.ContentPane = "Left";
                criteriaSection.ControlsPerRow = 4;
                criteriaSection.ColumnCssClass = "col-md-3";
                criteriaSection.RowElement = "div";
                criteriaSection.RowCssClass = "row";
                criteriaSection.Visible = true;
                criteriaSection.Mode = SectionMode.All;
                criteriaSection.IntroText = "Locate any ENTITY in the system by entering data into the fields below:";

                this.Sections.Add("Criteria", criteriaSection);


            }

            ScreenDataCommand dc = this.DataCommands["Retrieve"];

            if (dc != null)
            {
                List<ScreenDataCommandParameter> parameters = dc.Parameters.Where(x =>
                    (
                        (x.InputType == ScreenInputType.Control) ||
                        (x.InputType == ScreenInputType.ControlText)
                    )
                ).ToList<ScreenDataCommandParameter>();

                List<BaseControl> removeControl = new List<BaseControl>();
                //remove parameters that are no longer there - from controls
                foreach (BaseControl control in criteriaSection.Controls)
                {
                    if (!parameters.Exists(x => x.Name.Replace("@", "").ToLower() == control.Name.ToLower()))
                    {
                        removeControl.Add(control);
                    }
                }

                foreach (BaseControl control in removeControl)
                {
                    criteriaSection.Controls.Remove(control);
                }

                //remove parameters that are already there - from parameters
                List<ScreenDataCommandParameter> removeParameter = new List<ScreenDataCommandParameter>();
                foreach (ScreenDataCommandParameter p in parameters)
                {
                    if (criteriaSection.Controls.Exists(x => x.Name.ToLower() == p.Name.Replace("@", "").ToLower()))
                    {
                        removeParameter.Add(p);
                    }
                }
                foreach (ScreenDataCommandParameter p in removeParameter)
                {
                    parameters.Remove(p);
                }


                //add remaining parameters - to conrl

                foreach (ScreenDataCommandParameter p in parameters)
                {
                    TextBoxControl control = new TextBoxControl();

                    control.Name = p.Name.Replace("@", "");
                    control.DataField = p.Name.Replace("@", "");
                    control.Label = Common.ConvertWordToProperCase(p.Name.Replace("@", ""));
                    control.IsRequired = false;
                    control.Visible = true;
                    control.ControlGroupElement = "div";
                    control.ControlCssClass = "form-group";
                    control.LabelRendersBeforeControl = true;
                    control.LabelWrapsControl = false;
                    control.HelpTextElement = "span";
                    control.HelpTextCssClass = "help-block";
                    control.Parent = criteriaSection;

                    criteriaSection.Controls.Add(control);
                }

            }
            else
            {
                criteriaSection.Controls.Clear();
            }
            return criteriaSection;
        }

        

        private void LoadDataCommandsPage()
        {
            Dictionary<string, ScreenDataCommand> dc = new Dictionary<string, ScreenDataCommand>();

            if (this.TitleUseCommand.Enabled && this.TitleUseCommand.Checked && !String.IsNullOrEmpty(this.TitleCommandList.Text))
            {
                dc.Add("Title", new ScreenDataCommand(this.TitleCommandList.Text));
            }

            if (this.SubtitleUseCommand.Enabled && this.SubtitleUseCommand.Checked && !String.IsNullOrEmpty(this.SubtitleCommandList.Text))
            {
                dc.Add("Subtitle", new ScreenDataCommand(this.SubtitleCommandList.Text));
            }

            switch (screenType)
            {
                case ScreenType.Blank:
                    break;

                case ScreenType.Comments:
                    break;
                case ScreenType.Content:
                    break;
                case ScreenType.CriteriaListEdit:
                    dc.Add("Default", new ScreenDataCommand());
                    dc.Add("Retrieve", new ScreenDataCommand());
                    dc.Add("Insert", new ScreenDataCommand());
                    dc.Add("Update", new ScreenDataCommand());
                    dc.Add("Delete", new ScreenDataCommand());
                    break;
                case ScreenType.Documents:
                    break;
                case ScreenType.Edit:
                    dc.Add("Default", new ScreenDataCommand());
                    dc.Add("Retrieve", new ScreenDataCommand());
                    dc.Add("Insert", new ScreenDataCommand());
                    dc.Add("Update", new ScreenDataCommand());
                    dc.Add("Delete", new ScreenDataCommand());
                    break;
                case ScreenType.List:
                    dc.Add("Retrieve", new ScreenDataCommand());
                    break;
                case ScreenType.ListEdit:
                    dc.Add("Default", new ScreenDataCommand());
                    dc.Add("Retrieve", new ScreenDataCommand());
                    dc.Add("Insert", new ScreenDataCommand());
                    dc.Add("Update", new ScreenDataCommand());
                    dc.Add("Delete", new ScreenDataCommand());
                    break;
                case ScreenType.Login:
                    dc.Add("Profile", new ScreenDataCommand());
                    break;
                case ScreenType.Logout:
                    break;
                case ScreenType.Overview:
                    dc.Add("Retrieve", new ScreenDataCommand());
                    break;
                case ScreenType.Picker:
                    dc.Add("Default", new ScreenDataCommand());
                    dc.Add("Retrieve", new ScreenDataCommand());
                    dc.Add("Insert", new ScreenDataCommand());
                    dc.Add("Update", new ScreenDataCommand());
                    dc.Add("Delete", new ScreenDataCommand());
                    break;
                case ScreenType.Redirect:

                    if (String.IsNullOrEmpty(RedirectDataCommandList.Text))
                    {
                        dc.Add("Redirect", new ScreenDataCommand());
                    }
                    else
                    {
                        dc.Add("Redirect", new ScreenDataCommand(RedirectDataCommandList.Text));

                        if (this.DataCommands.ContainsKey("Redirect"))
                        {
                            if (this.DataCommands["Redirect"].Name.ToLower() != RedirectDataCommandList.Text.ToLower())
                            {
                                this.DataCommands.Remove("Redirect");
                            }
                        }
                    }

                    
                    break;
                case ScreenType.ReportExport:
                    break;
                case ScreenType.ReportViewer:
                    dc.Add("Default", new ScreenDataCommand());
                    break;
                case ScreenType.Search:
                    dc.Add("Default", new ScreenDataCommand());
                    dc.Add("Retrieve", new ScreenDataCommand());
                    break;
            }


            RefreshDataCommandsList(dc);

            

            this.dataCommandsPanel.Controls.Clear();

            screenDataCommandGrid.SelectedObject = null;
            screenDataCommandGrid.Visible = false;
            ParameterList.Visible = false;
            DataCommandsSelectedParameterLabel.Visible = false;
            DataCommandSelectedLabel.Visible = false;

            ScreenDataCommand command1 = null;

            if (this.DataCommands.Keys.Count > 0)
            {
                foreach (string item in DataCommands.Keys)
                {


                    ScreenDataCommand command = this.DataCommands[item];

                    if (command1 == null)
                        command1 = command;

                    //add label
                    RadLabel label = new RadLabel();
                    label.ForeColor = Color.Blue;
                    label.Font = new Font(label.Font, FontStyle.Underline);
                    label.Text = item + ":";
                    

                    this.dataCommandsPanel.Controls.Add(label);

                    //add dropdown control set to list of data commands
                    RadDropDownList list = new RadDropDownList();
                    list.Name = item + "_DataCommand";
                    list.Tag = command;

                    label.Tag = list;


                    this.dataCommandsPanel.Controls.Add(list);

                    list.DropDownStyle = RadDropDownStyle.DropDown;

                    list.AutoCompleteMode = AutoCompleteMode.Suggest;
                    //list.DropDownListElement.AutoCompleteAppend.LimitToList = true;
                    //list.DisplayMember = "Name";
                    //list.ValueMember = "Name";
                    //list.AutoCompleteDataSource = GetDataCommandsReturningDataTablesList();

                    list.DropDownListElement.AutoCompleteSuggest = new AutoCompleteHelper(list.DropDownListElement);
                    
                    list.DisplayMember = "Name";
                    list.ValueMember = "Name";
                    list.Width = this.dataCommandsPanel.Width - 5;
                    
                    switch(item.ToLower())
                    {
                        case "insert":
                            list.DataSource = GetDataCommandsForExecuteList();
                            break;
                        case "update":
                            list.DataSource = GetDataCommandsForExecuteList();
                            break;
                        case "delete":
                            list.DataSource = GetDataCommandsForExecuteList();
                            break;
                        default:
                            list.DataSource = GetDataCommandsReturningDataTablesList();
                            break;
                    }
                    

                    if (!String.IsNullOrEmpty(this.DataCommands[item].Name))
                    {
                        list.Text = this.DataCommands[item].Name;
                    }
                    else
                    {
                        list.Text = "";
                    }

                    //list.

                    list.SelectedValueChanged += list_SelectedValueChanged;
                    list.SelectedIndexChanged += list_SelectedIndexChanged;
                    label.Click += label_Click;


                }

                SelectDataCommandForGrid(command1);
            }



        }

        

        public class AutoCompleteHelper: AutoCompleteSuggestHelper
        {
            public AutoCompleteHelper(RadDropDownListElement owner) : base(owner)
            { 
                
            }

            public override void AutoComplete(KeyPressEventArgs e)
            {
                base.AutoComplete(e);
                if (this.DropDownList.Items.Count > 0)
                {
                    this.DropDownList.SelectedIndex = this.DropDownList.FindString(this.Filter);
                }
            }

            protected override bool DefaultFilter(RadListDataItem item)
            {
                return item.Text.ToLower().Contains(this.Filter.ToLower());
            }
        }

        private void LoadCriteriaPage(CriteriaSection section)
        {
            currentCriteriaSection = section;

            CriteriaSectionName.DataBindings.Clear();
            CriteriaSectionName.DataBindings.Add("Text", section, "Name");

            CriteriaIntroText.DataBindings.Clear();
            CriteriaIntroText.DataBindings.Add("Text", section, "IntroText");

            ControlsPerRow.DataBindings.Clear();
            ControlsPerRow.DataBindings.Add("Value", section, "ControlsPerRow");

            CriteriaControlList.DataSource = null;
            CriteriaControlList.DisplayMember = "";
            CriteriaControlList.DisplayMember = "Name";
            CriteriaControlList.ValueMember = "Name";
            CriteriaControlList.DataSource = section.Controls;
            CriteriaControlList.Refresh();
            CriteriaControlList.Update();

            



        }

        private void LoadGridPage(GridSection section)
        {
            currentGridSection = section;
            Grid grid = currentGridSection.Grid;

            this.GridSectionName.DataBindings.Clear();
            this.GridSectionName.DataBindings.Add("Text", grid, "Name");

            this.GridIntroText.DataBindings.Clear();
            this.GridIntroText.DataBindings.Add("Text", section, "IntroText");

            this.GridLoadDataOnPageLoad.DataBindings.Clear();
            this.GridLoadDataOnPageLoad.DataBindings.Add("Checked", section, "LoadDataOnPageLoad");

            DataCommand command = null;
            
            if(!String.IsNullOrEmpty(grid.SelectDataCommand))
                command = DataCommand.GetDataCommand(grid.SelectDataCommand);

            if (command != null)
            {
                List<DataCommandColumn> columns = command.Columns;

                this.GridAvailableFields.DataSource = null;
                this.GridAvailableFields.DisplayMember = "";
                this.GridAvailableFields.DisplayMember = "Name";
                this.GridAvailableFields.ValueMember = "Name";
                this.GridAvailableFields.DataSource = columns;
                this.GridAvailableFields.Refresh();
                this.GridAvailableFields.Update();

            }
            else
            {
                this.GridAvailableFields.DataSource = null;
                this.GridAvailableFields.Refresh();
                this.GridAvailableFields.Update();
            }

            this.GridColumns.DataSource = null;
            this.GridColumns.DisplayMember = "";
            this.GridColumns.DisplayMember = "HeaderText";
            this.GridColumns.ValueMember = "HeaderText";
            this.GridColumns.DataSource = grid.Columns;
            this.GridColumns.Refresh();
            this.GridColumns.Update();

           





        }

        private void LoadSectionsPage()
        {

            

            







        }

        void ConvertCriteriaControlItem_Click(object sender, EventArgs e)
        {
            RadMenuItem item = (RadMenuItem)sender;
            if (CriteriaControlPropertyGrid.SelectedObject != null)
            { 
                BaseControl current = (BaseControl) CriteriaControlPropertyGrid.SelectedObject;

                if(current.Type.ToLower() != item.Text.ToLower())
                {
                    BaseControl newControl = GetNewControl(item, current);

                    int ControlIndex = 0;
                    for (int i = 0; i <= (currentCriteriaSection.Controls.Count - 1); i++)
                    {
                        if (currentCriteriaSection.Controls[i].Name == newControl.Name)
                        {
                            currentCriteriaSection.Controls[i] = newControl;
                            ControlIndex = i;
                            break;
                        }
                    }

                    LoadCriteriaPage(currentCriteriaSection);


                    CriteriaControlList.SelectedIndex = ControlIndex;
                    SelectCriteriaControl(ControlIndex);
                    

                }

            }
        }

        void ConvertSectionControlItem_Click(object sender, EventArgs e)
        {
            RadMenuItem item = (RadMenuItem)sender;
            if (this.SectionControlPropertyGrid.SelectedObject != null)
            {
                BaseControl current = (BaseControl)SectionControlPropertyGrid.SelectedObject;

                if (current.Type.ToLower() != item.Text.ToLower())
                {
                    BaseControl newControl = GetNewControl(item, current);
                    List<BaseControl> controls = UseDetailsSection ? currentDetailsSection.Controls : currentEditSection.Controls;

                    int ControlIndex = 0;
                    for (int i = 0; i <= (controls.Count - 1); i++)
                    {
                        if (controls[i].Name == newControl.Name)
                        {
                            if (UseDetailsSection)
                            {
                                currentDetailsSection.Controls[i] = newControl;
                            }
                            else
                            {
                                currentEditSection.Controls[i] = newControl;
                            }
                            ControlIndex = i;
                            break;
                        }
                    }

                    LoadSectionControlsPage(UseDetailsSection, currentDetailsSection, currentEditSection);

                    this.AssignedSectionControlsList.SelectedIndex = ControlIndex;
                    SelectAssignedSectionControl(ControlIndex);


                }

            }
        }

        private static BaseControl GetNewControl(RadMenuItem item, BaseControl current)
        {
            BaseControl newControl = BaseControl.GetNewControl(item.Text);

            newControl.ControlContainerCssClass = current.ControlContainerCssClass;
            newControl.ControlContainerElement = current.ControlContainerElement;
            newControl.ControlCssClass = current.ControlCssClass;
            newControl.ControlGroupCssClass = current.ControlGroupCssClass;
            newControl.ControlGroupElement = current.ControlGroupElement;
            newControl.CssClass = current.CssClass;
            newControl.DataField = current.DataField;
            newControl.HelpText = current.HelpText;
            newControl.HelpTextCssClass = current.HelpTextCssClass;
            newControl.HelpTextElement = current.HelpTextElement;
            newControl.IsRequired = current.IsRequired;
            newControl.Label = current.Label;
            newControl.LabelContainerCssClass = current.LabelContainerCssClass;
            newControl.LabelContainerElement = current.LabelContainerElement;
            newControl.LabelCssClass = current.LabelCssClass;
            newControl.LabelRendersBeforeControl = current.LabelRendersBeforeControl;
            newControl.LabelWrapsControl = current.LabelWrapsControl;
            newControl.Name = current.Name;
            newControl.ReadOnlyDataField = current.ReadOnlyDataField;
            newControl.SkinID = current.SkinID;
            newControl.Width = current.Width;

            newControl.Parent = current.Parent;
            return newControl;
        }

        void label_Click(object sender, EventArgs e)
        {
             RadLabel label = (RadLabel)sender;
             ScreenDataCommand item = ((ScreenDataCommand)((RadDropDownList)label.Tag).Tag);
             this.DataCommandSelectedLabel.Text = String.Format("{0} {1}", label.Text, item.Name);
             this.DataCommandSelectedLabel.Visible = true;
             SelectDataCommandForGrid(item);
        }

        void list_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            
            //RadDropDownList list = (RadDropDownList)sender;
            //ScreenDataCommand item = (ScreenDataCommand)list.Tag;

            //if ((list.SelectedItem != null) && (!String.IsNullOrEmpty(list.SelectedItem.Text)))
            //{
                
            //    if (list.SelectedItem.Text != item.Name)
            //    {
            //        item.Name = (list.SelectedItem.Text);
            //        //list.Tag = item;
            //    }
            //}

            //this.DataCommandSelectedLabel.Text = String.Format("{0}: {1}", list.Name.Replace("_DataCommand",""), item.Name);
            //this.DataCommandSelectedLabel.Visible = true;
            //SelectDataCommandForGrid((ScreenDataCommand)list.Tag);
        }

        void list_SelectedValueChanged(object sender, EventArgs e)
        {
            RadDropDownList list = (RadDropDownList)sender;
            ScreenDataCommand item = (ScreenDataCommand)list.Tag;

            if ((list.SelectedItem != null) && (!String.IsNullOrEmpty(list.SelectedItem.Text)))
            {

                if (list.SelectedItem.Text != item.Name)
                {
                    item.Name = (list.SelectedItem.Text);
                    //list.Tag = item;
                }
            }

            this.DataCommandSelectedLabel.Text = String.Format("{0}: {1}", list.Name.Replace("_DataCommand", ""), item.Name);
            this.DataCommandSelectedLabel.Visible = true;
            SelectDataCommandForGrid((ScreenDataCommand)list.Tag);
        }

        private void SelectDataCommandForGrid(ScreenDataCommand item)
        {
            

            if ((item != null) && (!String.IsNullOrEmpty(item.Name)))
            {

                RefreshDataCommandParameters(item);

                FillParameterList(item.Parameters);
                screenDataCommandGrid.Visible = true;
                ParameterList.Visible = true;
                DataCommandsSelectedParameterLabel.Visible = true;
            }
            else
            {
                screenDataCommandGrid.SelectedObject = null;
                screenDataCommandGrid.Visible = false;
                ParameterList.Visible = false;
                DataCommandsSelectedParameterLabel.Visible = false;
            }
        }

        

        private void RefreshDataCommandsList(Dictionary<string, ScreenDataCommand> dc)
        {
            //loop through screen data command parameters
            List<string> remove = null;

            remove = new List<string>();

            //remove commands that no longer exist for selected page
            foreach (KeyValuePair<string, ScreenDataCommand> d in this.DataCommands)
            {
                if (!dc.ContainsKey(d.Key))
                {
                    remove.Add(d.Key);
                }
            }


            foreach (string d in remove)
            {
                this.DataCommands.Remove(d);
            }

            remove = new List<string>();

            //remove those that already exist - so we dont override their settings
            foreach (KeyValuePair<string, ScreenDataCommand> d in dc)
            {
                
                if (this.DataCommands.ContainsKey(d.Key))
                {
                    remove.Add(d.Key);
                }
            }


            foreach (string d in remove)
            {
                dc.Remove(d);
            }

            //add remaining items - this should be new - in case of page change
            foreach (KeyValuePair<string, ScreenDataCommand> d in dc)
            {
                this.DataCommands.Add(d.Key, d.Value);
            }
        }

        

        private void ParameterList_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (ParameterList.SelectedItem != null)
            {
                
                screenDataCommandGrid.Enabled = true;
                ScreenDataCommandParameter item = (ScreenDataCommandParameter)ParameterList.SelectedItem;
                screenDataCommandGrid.SelectedObject = item;
                DataCommandsSelectedParameterLabel.Text = item.Name + " " + "Parameter";
               

            }
            else
            {
                screenDataCommandGrid.SelectedObject = null;
                screenDataCommandGrid.Enabled = false;
                
            }
        }

        private void RefreshDataCommandParameters(ScreenDataCommand item)
        {
            //get data command
            DataCommand c = DataCommand.GetDataCommand(item.Name);

            if (c != null)
            {

                //loop through screen data command parameters
                List<ScreenDataCommandParameter> remove = new List<ScreenDataCommandParameter>();
                foreach (ScreenDataCommandParameter p in item.Parameters)
                {
                    ScreenDataCommandParameter localP = p;
                    if (!c.Parameters.Exists(x => x.Name == localP.Name))
                    {
                        remove.Add(localP);
                    }
                }

                //are there any missing from data command - if so remove
                foreach (ScreenDataCommandParameter p in remove)
                {
                    item.Parameters.Remove(p);
                }


                //loop through data command parameters
                foreach (DataCommandParameter p in c.Parameters)
                {
                    DataCommandParameter localP = p;
                    if (!item.Parameters.Exists(x => x.Name == localP.Name))
                    {
                        //are there any missing from screen command parameters - if so add with control as default

                        ScreenDataCommandParameter sp = new ScreenDataCommandParameter();

                        sp.InputType = ScreenInputType.Control;
                        sp.Name = p.Name;
                        sp.InputKey = p.Name.Replace("@", "").Replace("'", "").Replace(" ", "");

                        item.Parameters.Add(sp);

                    }
                }


            }
        }

        private void FillParameterList(List<ScreenDataCommandParameter> items)
        {
            this.ParameterList.DisplayMember = "";
            this.ParameterList.DisplayMember = "Name";
            this.ParameterList.ValueMember = "Name";
            this.ParameterList.DataSource = items;
            this.ParameterList.Refresh();
        }

        private void LoadFinishPage()
        {
            CompletionScreenName.Text = ScreenName.Text;
            CompletionScreenFolder.Text = FolderList.Text;
            CompletionScreenType.Text = screenType.ToString();
        }

        private void ScreenTypeList_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(ScreenTypeList.Text))
            {
                screenType = ScreenType.Blank;
            }
            else
            {
                screenType = (ScreenType) Enum.Parse(typeof(ScreenType), ScreenTypeList.Text);

                if (
                        (screenType == ScreenType.Login) ||
                        (screenType == ScreenType.Logout)
                   )
                {
                    RequireAuthentication.Checked = false;
                }
                else
                {
                    RequireAuthentication.Checked = true;
                    CheckPermission.Checked = true;

                }
            }

            switch (screenType)
            { 
                case ScreenType.Comments:
                    Subtitle.Text = "Comments";
                    EntityLabel.Visible = true;
                    Entity.Visible = true;
                    break;
                case ScreenType.Documents:
                    Subtitle.Text = "Documents";
                    EntityLabel.Visible = true;
                    Entity.Visible = true;
                    break;
                case ScreenType.Edit:
                    break;
                case ScreenType.ListEdit:

                    ActionLinksUrl.Visible = false;
                    ActionLinksUrlLabel.Visible = false;
                    break;
                case ScreenType.CriteriaListEdit:
                    ActionLinksUrl.Visible = false;
                    ActionLinksUrlLabel.Visible = false;
                    break;
                case ScreenType.Picker:
                    ActionLinksUrl.Visible = false;
                    ActionLinksUrlLabel.Visible = false;
                    break;
                default:
                    EntityLabel.Visible = false;
                    Entity.Visible = false;

                    ActionLinksUrl.Visible = true;
                    ActionLinksUrlLabel.Visible = true;
                    break;
            }

            FillWithSectionTypesList(Section1TypeList);
            FillWithSectionTypesList(Section2TypeList);
            FillWithSectionTypesList(Section3TypeList);
            FillWithSectionTypesList(Section4TypeList);
            FillWithSectionTypesList(Section5TypeList);
            FillWithSectionTypesList(Section6TypeList);
            FillWithSectionTypesList(Section7TypeList);
            FillWithSectionTypesList(Section8TypeList);
            FillWithSectionTypesList(Section9TypeList);
            FillWithSectionTypesList(Section10TypeList);
        }

        private void CheckPermission_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            PermissionList.Enabled = (args.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On);
        }

        private void RequireAuthentication_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            CheckPermission.Enabled = (args.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On);
            PermissionList.Enabled = (args.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On);
        }

        private void TitleUseCommand_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            TitleCommandList.Enabled = (args.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On);
            TitleLabel.Text = (args.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On) ? "Title Format:" : "Title:";
        }

        private void SubtitleUseCommand_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            SubtitleCommandList.Enabled = (args.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On);
            SubtitleLabel.Text = (args.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On) ? "Subtitle Format:" : "Subtitle:";
        }

        private void ActionLinksCheckPermission_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            this.ActionLinkPermissionList.Enabled = (args.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On);
        }

       

        private void CriteriaControlList_SelectedIndexChanged(object sender, EventArgs e)
        {
            CriteriaControlList_Selected();
        }

        private void SelectCriteriaControl(int SelectedIndex)
        {
            CriteriaSelectedControlLabel.Visible = true;
            CriteriaControlPropertyGrid.Visible = true;
            ConvertCriteriaControl.Visible = true;

            BaseControl control = (BaseControl)CriteriaControlList.Items[SelectedIndex].DataBoundItem;
            CriteriaSelectedControlLabel.Text = String.Format("Control: {0} - {1}", control.Name, control.Type);
            CriteriaControlPropertyGrid.SelectedObject = control; // BaseControl.ConvertToSpecificControl(control);


        }

        private void CriteriaControlList_Click(object sender, EventArgs e)
        {
            CriteriaControlList_Selected();
        }

        private void CriteriaControlList_Selected()
        {
            CriteriaControlPropertyGrid.SelectedObject = null;

            if (CriteriaControlList.SelectedIndex >= 0)
            {
                SelectCriteriaControl(CriteriaControlList.SelectedIndex);
            }
            else
            {
                CriteriaSelectedControlLabel.Visible = false;
                CriteriaControlPropertyGrid.Visible = false;
                ConvertCriteriaControl.Visible = false;
            }
        }

        private void CriteriaControlsMoveUp_Click(object sender, EventArgs e)
        {
            if (this.CriteriaControlList.SelectedIndex > 0)
            {
                int oldIndex = CriteriaControlList.SelectedIndex;
                int newIndex = CriteriaControlList.SelectedIndex-1;

                var item = currentCriteriaSection.Controls[CriteriaControlList.SelectedIndex];
                currentCriteriaSection.Controls.RemoveAt(CriteriaControlList.SelectedIndex);

                if (newIndex > oldIndex) newIndex--;
                currentCriteriaSection.Controls.Insert(newIndex, item);

                LoadCriteriaPage(currentCriteriaSection);
                CriteriaControlList.SelectedIndex = newIndex;
                SelectCriteriaControl(newIndex);
            }
        }

        private void CriteriaControlsMoveDown_Click(object sender, EventArgs e)
        {
            if (this.CriteriaControlList.SelectedIndex < (this.CriteriaControlList.Items.Count - 1))
            {
                int oldIndex = CriteriaControlList.SelectedIndex;
                int newIndex = CriteriaControlList.SelectedIndex + 1;

                var item = currentCriteriaSection.Controls[CriteriaControlList.SelectedIndex];
                currentCriteriaSection.Controls.RemoveAt(CriteriaControlList.SelectedIndex);

                currentCriteriaSection.Controls.Insert(newIndex, item);

                LoadCriteriaPage(currentCriteriaSection);
                CriteriaControlList.SelectedIndex = newIndex;
                SelectCriteriaControl(newIndex);
                
            }
        }

        private void GridAddFieldAsBoundColumn_Click(object sender, EventArgs e)
        {
            if (this.GridAvailableFields.SelectedItems != null)
            {
                foreach (RadListDataItem item in this.GridAvailableFields.SelectedItems)
                {
                    DataCommandColumn field = (DataCommandColumn) item.DataBoundItem;

                    BoundGridColumn column = new BoundGridColumn();

                    column.HeaderText = Common.ConvertWordToProperCase(field.Name);
                    column.DataField = field.Name;
                    column.SortExpression = field.Name;
                    column.IncludeInExport = true;
                    column.Parent = currentGridSection.Grid;

                    currentGridSection.Grid.Columns.Add(column);

                
                }

                LoadGridPage(currentGridSection);
               

            }
        }

        private void GridColumns_Click(object sender, EventArgs e)
        {
            GridColumns_Select();
        }

        private void GridColumns_Select()
        {
            this.GridColumnPropertyGrid.SelectedObject = null;

            if (GridColumns.SelectedIndex >= 0)
            {
                SelectGridColumnControl(GridColumns.SelectedIndex);
            }
            else
            {
                this.GridSelectedColumnLabel.Visible = false;
                this.GridColumnPropertyGrid.Visible = false;
                this.ConvertGridColumn.Visible = false;
            }
        }

        private void GridColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridColumns_Select();
        }

        private void GridColumnMoveUp_Click(object sender, EventArgs e)
        {
            if (this.GridColumns.SelectedIndex > 0)
            {
                int oldIndex = GridColumns.SelectedIndex;
                int newIndex = GridColumns.SelectedIndex - 1;

                var item = currentGridSection.Grid.Columns[GridColumns.SelectedIndex];
                currentGridSection.Grid.Columns.RemoveAt(GridColumns.SelectedIndex);

                if (newIndex > oldIndex) newIndex--;
                currentGridSection.Grid.Columns.Insert(newIndex, item);

                LoadGridPage(currentGridSection);
                GridColumns.SelectedIndex = newIndex;


            }
        }

        private void GridColumnMoveDown_Click(object sender, EventArgs e)
        {
            if (this.GridColumns.SelectedIndex < (this.GridColumns.Items.Count - 1))
            {
                int oldIndex = GridColumns.SelectedIndex;
                int newIndex = GridColumns.SelectedIndex + 1;

                var item = currentGridSection.Grid.Columns[GridColumns.SelectedIndex];
                currentGridSection.Grid.Columns.RemoveAt(GridColumns.SelectedIndex);

                currentGridSection.Grid.Columns.Insert(newIndex, item);

                LoadGridPage(currentGridSection);
                GridColumns.SelectedIndex = newIndex;
               

            }
        }

        private void GridColumnRemove_Click(object sender, EventArgs e)
        {
            if (this.GridColumns.SelectedIndex >= 0)
            {
                currentGridSection.Grid.Columns.RemoveAt(GridColumns.SelectedIndex);
                LoadGridPage(currentGridSection);

                this.GridSelectedColumnLabel.Visible = false;
                this.GridColumnPropertyGrid.Visible = false;
                this.ConvertGridColumn.Visible = false;
                


            }
        }

        private void SelectGridColumnControl(int SelectedIndex)
        {
            this.GridSelectedColumnLabel.Visible = true;
            this.GridColumnPropertyGrid.Visible = true;
            this.ConvertGridColumn.Visible = true;

            GridColumn column = (GridColumn)this.GridColumns.Items[SelectedIndex].DataBoundItem;
            GridSelectedColumnLabel.Text = String.Format("Column: {0} - {1}", column.HeaderText, column.ColumnType);
            GridColumnPropertyGrid.SelectedObject = column; // BaseControl.ConvertToSpecificControl(control);


        }


        void ConvertGridColumItem_Click(object sender, EventArgs e)
        {
            RadMenuItem item = (RadMenuItem)sender;
            int currentIndex = this.GridColumns.SelectedIndex;
            if ((this.GridColumnPropertyGrid.SelectedObject != null) && (currentIndex >= 0))
            {
                
                GridColumn current = (GridColumn)GridColumnPropertyGrid.SelectedObject;

                if (current.ColumnType.ToString().ToLower() != item.Text.ToLower())
                {
                    GridColumn newColumn = GridColumn.GetNewColumn(item.Text);

                    GridColumn.Convert(current, newColumn);

                    currentGridSection.Grid.Columns[currentIndex] = newColumn;

                    LoadGridPage(currentGridSection);

                    this.GridColumns.SelectedIndex = currentIndex;
                    this.SelectGridColumnControl(currentIndex);


                }

            }
        }

        private void SectionLayoutList_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(SectionLayoutList.Text))
            {
                SectionZoneLayout l = SectionZoneLayout.GetByName(SectionLayoutList.Text);

                if (l != null)
                {
                    Section1ZoneList.DataSource = l.GetDividerNames();
                    Section2ZoneList.DataSource = l.GetDividerNames();
                    Section3ZoneList.DataSource = l.GetDividerNames();
                    Section4ZoneList.DataSource = l.GetDividerNames();
                    Section5ZoneList.DataSource = l.GetDividerNames();
                    Section6ZoneList.DataSource = l.GetDividerNames();
                    Section7ZoneList.DataSource = l.GetDividerNames();
                    Section8ZoneList.DataSource = l.GetDividerNames();
                    Section9ZoneList.DataSource = l.GetDividerNames();
                    Section10ZoneList.DataSource = l.GetDividerNames();

                    Section1ZoneList.Text = String.Empty;
                    Section2ZoneList.Text = String.Empty;
                    Section3ZoneList.Text = String.Empty;
                    Section4ZoneList.Text = String.Empty;
                    Section5ZoneList.Text = String.Empty;
                    Section6ZoneList.Text = String.Empty;
                    Section7ZoneList.Text = String.Empty;
                    Section8ZoneList.Text = String.Empty;
                    Section9ZoneList.Text = String.Empty;
                    Section10ZoneList.Text = String.Empty;
                }

            }
            else
            {
                Section1ZoneList.DataSource = null;
                Section2ZoneList.DataSource = null;
                Section3ZoneList.DataSource = null;
                Section4ZoneList.DataSource = null;
                Section5ZoneList.DataSource = null;
                Section6ZoneList.DataSource = null;
                Section7ZoneList.DataSource = null;
                Section8ZoneList.DataSource = null;
                Section9ZoneList.DataSource = null;
                Section10ZoneList.DataSource = null;
            }
        }

        private void SectionTypeList_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            bool DisplayDataCommand = false;
            RadDropDownList list = (RadDropDownList)sender;

            if (!String.IsNullOrEmpty(list.Text))
            {
                if (list.Text.ToLower() == "grid")
                {
                    DisplayDataCommand = true;
                }
            }

            Control[] controls = list.Parent.Controls.Find(list.Name.Replace("TypeList", "DataCommandList"), false);

            if ((controls != null) && (controls.Length > 0))
            {
                RadDropDownList dc = (RadDropDownList)controls[0];
                dc.Visible = DisplayDataCommand;
                dc.Text = String.Empty;
            }


        }

        private void SectionAddControl_Click(object sender, EventArgs e)
        {
            if (this.AvailableSectionControlsList.SelectedItems != null)
            {
                foreach (RadListDataItem item in this.AvailableSectionControlsList.SelectedItems)
                {


                    BaseControl control = null;

                    if (screenType == ScreenType.Overview)
                    {
                        control = new LabelControl();
                    }
                    else
                    {
                        control = new TextBoxControl();
                    }

                    control.Name = item.Text;
                    
                    control.Label = Common.ConvertWordToProperCase(item.Text);
                    control.LabelRendersBeforeControl = true;

                    control.IsRequired = false;
                    control.Visible = true;
                    control.DataField = item.Text;

                    control.ControlContainerElement = "div";

                    if (
                        (this.SectionLayoutList.Text.ToLower() == "singlecolumn") 
                        )
                    {
                        control.LabelCssClass = "col-md-2";
                        control.ControlContainerCssClass = "col-md-10";

                        
                    }
                    else
                    {
                        control.LabelCssClass = "col-md-4";
                        control.ControlContainerCssClass = "col-md-8";
                    }

                    control.ControlGroupElement = "div";
                    control.ControlGroupCssClass = "form-group";

                    
                    

                    control.HelpTextElement = "span";
                    control.HelpTextCssClass = "help-block";

                    if (UseDetailsSection)
                    {
                        currentDetailsSection.Controls.Add(control);
                    }
                    else
                    {
                        currentEditSection.Controls.Add(control);
                    }

                }

                LoadSectionControlsPage(UseDetailsSection, currentDetailsSection, currentEditSection);


            }
        }

        private void RemoveSectionControl_Click(object sender, EventArgs e)
        {
            if (this.AssignedSectionControlsList.SelectedIndex >= 0)
            {
                if (UseDetailsSection)
                {
                    currentDetailsSection.Controls.RemoveAt(AssignedSectionControlsList.SelectedIndex);
                }
                else
                {
                    currentEditSection.Controls.RemoveAt(AssignedSectionControlsList.SelectedIndex);
                }

                LoadSectionControlsPage(UseDetailsSection, currentDetailsSection, currentEditSection);

                this.SectionControlNameLabel.Visible = false;
                this.ConvertSectionControl.Visible = false;
                this.SectionControlPropertyGrid.Visible = false;



            }
        }

        private void MoveSectionControlDown_Click(object sender, EventArgs e)
        {
            if (this.AssignedSectionControlsList.SelectedIndex < (this.AssignedSectionControlsList.Items.Count - 1))
            {
                int oldIndex = AssignedSectionControlsList.SelectedIndex;
                int newIndex = AssignedSectionControlsList.SelectedIndex + 1;

                var item = UseDetailsSection ? currentDetailsSection.Controls[AssignedSectionControlsList.SelectedIndex] : currentEditSection.Controls[AssignedSectionControlsList.SelectedIndex];

                if (UseDetailsSection)
                {
                    currentDetailsSection.Controls.RemoveAt(oldIndex);

                    currentDetailsSection.Controls.Insert(newIndex, item);
                }
                else
                {
                    currentEditSection.Controls.RemoveAt(oldIndex);

                    currentEditSection.Controls.Insert(newIndex, item);
                }

                LoadSectionControlsPage(UseDetailsSection, currentDetailsSection, currentEditSection);
                AssignedSectionControlsList.SelectedIndex = newIndex;


            }
        }

        private void MoveSectionControlUp_Click(object sender, EventArgs e)
        {
            if (this.AssignedSectionControlsList.SelectedIndex > 0)
            {
                int oldIndex = AssignedSectionControlsList.SelectedIndex;
                int newIndex = AssignedSectionControlsList.SelectedIndex - 1;

                var item = UseDetailsSection ? currentDetailsSection.Controls[AssignedSectionControlsList.SelectedIndex] : currentEditSection.Controls[AssignedSectionControlsList.SelectedIndex];

                if (UseDetailsSection)
                {
                    currentDetailsSection.Controls.RemoveAt(oldIndex);

                    if (newIndex > oldIndex) newIndex--;

                    currentDetailsSection.Controls.Insert(newIndex, item);
                }
                else
                {
                    currentEditSection.Controls.RemoveAt(oldIndex);

                    if (newIndex > oldIndex) newIndex--;

                    currentEditSection.Controls.Insert(newIndex, item);
                }

                LoadSectionControlsPage(UseDetailsSection, currentDetailsSection, currentEditSection);
                AssignedSectionControlsList.SelectedIndex = newIndex;


            }

           
        }

        private void AssignedSectionControlsList_Select()
        {
            if (this.AssignedSectionControlsList.SelectedIndex >= 0)
            {
                BaseControl item = (BaseControl)AssignedSectionControlsList.SelectedItem.DataBoundItem;


                this.SectionControlPropertyGrid.SelectedObject = null;

                if (AssignedSectionControlsList.SelectedIndex >= 0)
                {
                    SelectAssignedSectionControl(AssignedSectionControlsList.SelectedIndex);
                }
                else
                {
                    this.SectionControlNameLabel.Visible = false;
                    this.ConvertSectionControl.Visible = false;
                    this.SectionControlPropertyGrid.Visible = false;
                }

            }
        }

        private void SelectAssignedSectionControl(int SelectedIndex)
        {
            this.SectionControlNameLabel.Visible = true;
            this.ConvertSectionControl.Visible = true;
            this.SectionControlPropertyGrid.Visible = true;

            BaseControl control = (BaseControl)this.AssignedSectionControlsList.Items[SelectedIndex].DataBoundItem;
            SectionControlNameLabel.Text = String.Format("Control: {0} - {1}", control.Name, control.Type);
            SectionControlPropertyGrid.SelectedObject = control;


        }

        private void AssignedSectionControlsList_Click(object sender, EventArgs e)
        {
            AssignedSectionControlsList_Select();
        }

        private void AssignedSectionControlsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            AssignedSectionControlsList_Select();
        }

        private void RedirectDataCommandList_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty(RedirectDataCommandList.Text))
            {
                DataCommand command = DataCommand.GetDataCommand(RedirectDataCommandList.Text);

                if (command != null)
                {
                    this.RedirectField.DisplayMember = "";
                    this.RedirectField.DisplayMember = "Name";
                    this.RedirectField.ValueMember = "Name";
                    this.RedirectField.DataSource = command.Columns;
                    this.RedirectField.Refresh();
                    this.RedirectField.Update();
                }
            }
        }

        private void radMenuItemRefresh_Click(object sender, EventArgs e)
        {
            Setup();
        }
        
        
    }
}
