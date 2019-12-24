using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CodeTorch.Core.Services;
using CodeTorch.Web.Data;
using CodeTorch.Web.UserControls;
using CodeTorch.Core;
using System.Web.Security;
using CodeTorch.Core.Messages;
using CodeTorch.Web.SectionControls;

namespace CodeTorch.Web.Templates
{
    public class BasePage : Page
    {
        string _SubTitle = "";
        bool _RequiresAuthentication = true;


        //used in edit and overview
        protected PlaceHolder SectionLayout;
       

        private const string MODE = "MODE";
        private const string PAGEMODE_INSERT = "insert";
        private const string PAGEMODE_EDIT = "edit";
        private const string ADD = "Add";
        private const string EDIT = "Edit";

        DataCommandService dataCommandDB = DataCommandService.GetInstance();
        PageDB pageDB = new PageDB();
        public List<BaseSectionControl> SectionControls = new List<BaseSectionControl>();

        public Screen Screen { get; set; }
        PageTemplate PageTemplate { get; set; }

        
        App app;


        MessageBus _MessageBus = null;

        public MessageBus MessageBus
        {
            get { return _MessageBus; }
            
        }

        public FormViewMode PageMode
        {
            get
            {
                if (ViewState["PageMode"] == null)
                {
                    return FormViewMode.Insert;
                }
                else
                {
                    return (FormViewMode)ViewState["PageMode"];
                }

            }
            set
            {
                ViewState["PageMode"] = value;
            }
        }


        public string SubTitle
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
       

        public bool RequiresAuthentication
        {
            get
            {
                return _RequiresAuthentication;
            }
            set
            {
                _RequiresAuthentication = value;
            }
        }

        public BasePage()
        {
            this.Load += new EventHandler(BasePage_Load);
            this.Page.Init += new EventHandler(BasePage_Init);
            this.Page.PreInit += new EventHandler(Page_PreInit);
        }

        void Page_PreInit(object sender, EventArgs e)
        {
            _MessageBus = new Core.MessageBus();
            LoadMasterPage();
        }

        

        void BasePage_Init(object sender, EventArgs e)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                LoadPageClientScripts();
                LoadPageSettings();

                //call all page load actions
                ExecuteAction(Screen.OnPageInit);
            }
            catch (Exception ex)
            {
                DisplayErrorAlert(ex);

                log.Error(ex);
            }
        }

        protected override void InitializeCulture()
        {
            base.InitializeCulture();

            app = CodeTorch.Core.Configuration.GetInstance().App;
            if (app.EnableLocalization)
            {
                string cultureCode = Common.CultureCode;

                if(!String.IsNullOrEmpty(cultureCode))
                    SetUserLocale(cultureCode, null, true);
                
             
            }

        }

        public void SetUserLocale(string CurrencySymbol, bool SetUiCulture)
        {
            HttpRequest Request = HttpContext.Current.Request;
            if (Request.UserLanguages == null)
                return;

            string Lang = Request.UserLanguages[0];
            if (Lang != null)
            {
                // *** Problems with Turkish Locale and upper/lower case
                // *** DataRow/DataTable indexes
                if (Lang.StartsWith("tr"))
                    return;

                if (Lang.Length < 3)
                    Lang = Lang + "-" + Lang.ToUpper();
                try
                {
                    System.Globalization.CultureInfo Culture =
                           new System.Globalization.CultureInfo(Lang);
                    System.Threading.Thread.CurrentThread.CurrentCulture = Culture;

                    if (!string.IsNullOrEmpty(CurrencySymbol))
                        System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol =
                          CurrencySymbol;

                    if (SetUiCulture)
                        System.Threading.Thread.CurrentThread.CurrentUICulture = Culture;
                }
                catch
                { ;}
            }
        }


        public void SetUserLocale()
        {
            this.SetUserLocale(null, true);
        }


        public void SetUserLocale(string LCID, string CurrencySymbol, bool SetUiCulture)
        {
            try
            {
                if (LCID.IndexOf("-") < 0)
                    LCID += "-" + LCID;

                System.Globalization.CultureInfo Culture = new System.Globalization.CultureInfo(LCID);
                System.Threading.Thread.CurrentThread.CurrentCulture = Culture;

                if (!string.IsNullOrEmpty(CurrencySymbol))
                    System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol =
                      CurrencySymbol;

                if (SetUiCulture)
                    System.Threading.Thread.CurrentThread.CurrentUICulture = Culture;
            }
            catch (Exception ex)
            { string t = ex.Message; }
        }

        private void LoadPageClientScripts()
        {
            if (this.Screen != null)
            {

                if (this.Screen.Scripts.Count > 0)
                {

                    ScriptManager Smgr = ScriptManager.GetCurrent(Page);
                    if (Smgr == null) throw new Exception("ScriptManager not found.");

                    int scriptIndex = 0;
                    foreach (Script s in this.Screen.Scripts)
                    {
                        scriptIndex++;

                        if (s.RenderAtTopOfPage || !String.IsNullOrEmpty(s.Assembly))
                        {
                            ScriptReference SRef = new ScriptReference();

                            if (String.IsNullOrEmpty(s.Assembly))
                            {
                                SRef.Path = s.Path;

                            }
                            else
                            {
                                SRef.Name = s.Name;
                                SRef.Assembly = s.Assembly;
                            }

                            Smgr.Scripts.Add(SRef);
                        }
                        else
                        {
                            string scriptName = String.IsNullOrEmpty(s.Name) ? $"script{scriptIndex}" : s.Name;
                            string scriptContent = null;
                            string scriptUrl = null;
                            bool addScriptTags = String.IsNullOrEmpty(s.Contents) ? false : true;

                            if (String.IsNullOrEmpty(s.Contents))
                            {
                                if (!String.IsNullOrEmpty(s.Path))
                                {
                                    scriptContent = $"<script src='{Page.ResolveClientUrl(s.Path)}' language='text/javascript'></script>";
                                }
                                else
                                {
                                    scriptContent = $"document.write('script {0} is not configured correctly - check codetorch configuration');";
                                }

                            }
                            else 
                            {
                                scriptContent = s.Contents;
                            }


                            if (s.IsOnSubmitScript)
                            {
                                //specific script to target on submit for a page
                                if (!Page.ClientScript.IsOnSubmitStatementRegistered(scriptName))
                                    Page.ClientScript.RegisterOnSubmitStatement(typeof(string), scriptName, scriptContent);
                            }
                            else
                            {
                                //render at bottom of page
                                if (String.IsNullOrEmpty(s.Assembly))
                                {
                                    if (!Page.ClientScript.IsStartupScriptRegistered(scriptName))
                                        Page.ClientScript.RegisterStartupScript(typeof(string), scriptName, scriptContent, addScriptTags);
                                }
                            }
                        }
                    }

           

                }


            }
        }

        void BasePage_Load(object sender, EventArgs e)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            try
            {

                bool AuthenticateScreen = PerformScreenAuthenticationCheck();

                if (AuthenticateScreen)
                {
                    if (this.Screen != null)
                    {
                        if (this.Screen.RequireAuthentication)
                        {
                            if (!Page.User.Identity.IsAuthenticated)
                            {
                                App app = CodeTorch.Core.Configuration.GetInstance().App;

                                if (app.AuthenticationMode is FormsAuthenticationMode)
                                {
                                    Common.RedirectToLoginPage(Request.RawUrl);
                                }
                                else
                                {
                                    Common.RedirectToAccessDenied("AUTHENTICATION_ERROR");
                                }
                            }
                            else
                            {
                                if (this.Screen.ScreenPermission.CheckPermission)
                                {
                                    if (!Common.HasPermission(this.Screen.ScreenPermission.Name))
                                    {
                                        Common.RedirectToAccessDenied(this.Screen.ScreenPermission.Name);
                                    }

                                }
                            }
                        }


                        
                    }
                }

                //moved from edit globally
                if (!IsPostBack)
                {
                    PopulateSections();

                    


                }

                //call all page load actions
                ExecuteAction(Screen.OnPageLoad);
            }
            catch (Exception ex)
            {
                DisplayErrorAlert(ex);

                log.Error(ex);
            }

        }

        public virtual bool PerformScreenAuthenticationCheck()
        {
            return true;
        }

        private void LoadPageSettings()
        {
            if (DesignMode)
                return;

            if (Screen != null)
            {
                this.RequiresAuthentication = Screen.RequireAuthentication;

                if (Screen.ValidateRequest)
                {
                    Request.ValidateInput();
                }
            }

            


            SetPageTitle();
            SetPageSubTitle();

            InitializePage();
        }

        private void LoadMasterPage()
        {
            if (DesignMode)
                return;

            Screen = PopulateScreenObject();

            string PageTemplateToLoad = null;
            ScreenPageTemplate pageTemplate = GetScreenPageTemplate();
            if (pageTemplate != null)
            {
                //we have config item - lets attempt to get PageTemplateToLoad
                if (pageTemplate.Mode == ScreenPageTemplateMode.Static)
                {
                    PageTemplateToLoad = GetPageTemplate();
                }
                else
                {
                    //dynamic
                    PageTemplateToLoad = GetDynamicPageTemplate(pageTemplate);
                }
            }
            else
            {
                //handles custom pages
                PageTemplateToLoad = GetPageTemplate();
            }

            

            if(String.IsNullOrEmpty(PageTemplateToLoad))
            {
                //no page specific template so check to see if there is an application defined default template
                if (!String.IsNullOrEmpty(CodeTorch.Core.Configuration.GetInstance().App.DefaultPageTemplate))
                {
                    PageTemplateToLoad = CodeTorch.Core.Configuration.GetInstance().App.DefaultPageTemplate;
                }
            }

            if (!String.IsNullOrEmpty(PageTemplateToLoad))
            {
                PageTemplate = PageTemplate.GetByName(PageTemplateToLoad);

                if (PageTemplate == null)
                {
                    throw new ApplicationException(String.Format("Page Template {0} could not be found in configuration", PageTemplateToLoad));
                }

                this.MasterPageFile = PageTemplate.Path;

                foreach (PageTemplateItem item in PageTemplate.Items)
                {
                    LoadMasterPageContentPlaceHolder(item);

                }
            }
        }

        private string GetDynamicPageTemplate(ScreenPageTemplate pageTemplate)
        {
            string retVal = null;
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            try
            {
                DataCommandService dataCommandDB = DataCommandService.GetInstance();
                PageDB pageDB = new PageDB();

                List<ScreenDataCommandParameter> parameters = pageDB.GetPopulatedCommandParameters(pageTemplate.DataCommand, this);

                DataTable dt = dataCommandDB.GetDataForDataCommand(pageTemplate.DataCommand, parameters);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains(pageTemplate.DataField))
                    {
                        retVal = dt.Rows[0][pageTemplate.DataField].ToString();
                    }
                }


            }
            catch (Exception ex)
            {
                DisplayErrorAlert(ex);

                log.Error(ex);
            }

            return retVal;
        }

        protected virtual string GetPageTemplate()
        {
            string PageTemplateToLoad = null;

            if (Screen != null)
            {
                PageTemplateToLoad = Screen.PageTemplate.Name;
            }

            return PageTemplateToLoad;
        }

        protected virtual ScreenPageTemplate GetScreenPageTemplate()
        {
            ScreenPageTemplate PageTemplateToLoad = null;

            if (Screen != null)
            {
                PageTemplateToLoad = Screen.PageTemplate;
            }

            return PageTemplateToLoad;
        }

        protected virtual string GetSectionZoneLayout()
        {
            return Screen.SectionZoneLayout;
        }

        private void LoadMasterPageContentPlaceHolder(PageTemplateItem item)
        {

            PageTemplateItemBuilder builder = new PageTemplateItemBuilder(this, item);
            AddContentTemplate(item.Name, new CompiledTemplateBuilder(new BuildTemplateMethod(builder.BuildPageTemplateItem)));
        }


        protected virtual Core.Screen PopulateScreenObject()
        {
            Core.Screen retVal = null;

            //string[] urlSegments = this.Request.Url.Segments;

            //string rootFolder = null;

            ////if (urlSegments.Length > 4)
            ////{
            ////    rootFolder = urlSegments[urlSegments.Length - 3].Replace("/", "");
            ////}
            ////else
            ////{
            ////    rootFolder = urlSegments[urlSegments.Length - 2].Replace("/", "");
            ////}

            
            


            //if (rootFolder.ToLower() == "app")
            if(this.Request.Url.AbsolutePath.ToLower().Contains("/app/"))
            {
                string pageName = this.RouteData.GetRequiredString("page");
                string folder = this.RouteData.GetRequiredString("folder");

                retVal = Screen.GetByFolderAndName(folder, pageName);
                this.EnableViewState = true;
            }

            return retVal;
        }



        public Control FindControlRecursive(Control root, string id)
        {
            return Common.FindControlRecursive(root, id);
        }

        public Control FindControlRecursive(string id)
        {
            return Common.FindControlRecursive(this, id);
        }

        public CodeTorch.Web.FieldTemplates.BaseFieldTemplate FindFieldRecursive(string id)
        {
            return Common.FindFieldRecursive(this, null, id);
        }

        public CodeTorch.Web.FieldTemplates.BaseFieldTemplate FindFieldRecursive(Control Container, string id)
        {
            return Common.FindFieldRecursive(this, Container, id);
        }

       

        public BaseSectionControl FindSection(string id)
        {
            return Common.FindSection(this, null, id);
        }

        public BaseSectionControl FindSection(Control Container, string id)
        {
            return Common.FindSection(this, Container, id);
        }


        protected virtual void SetPermissions()
        {
        }

        protected virtual void SetPageDefaults()
        {

        }

        protected void InitializePage()
        {


            SetPageDefaults();

            //SetPageInternalDefaults();
            SetPermissions();

        }

        public string GetSetting(string SettingName)
        {
            string retVal = String.Empty;
            
                return retVal;
    
        }

        public string GetSectionSetting(string SettingName)
        {
            string retVal = String.Empty;

            return retVal;
        }

        public void RenderPageSections(string screenLayout, Screen screen, List<Section> sections, bool isNewRecord)
        {
            RenderPageSections(this.SectionLayout, screenLayout, screen, sections, isNewRecord, SectionMode.All, "Screen.Sections");
        }

        public void RenderPageSections(string screenLayout, Screen screen, List<Section> sections, bool isNewRecord, SectionMode Mode, string ResourceKeyPrefix)
        {
            RenderPageSections(this.SectionLayout, screenLayout, screen, sections, isNewRecord, Mode, ResourceKeyPrefix);
        }

        public void RenderPageSections(PlaceHolder holder, string screenLayout, Screen screen, List<Section> sections, bool isNewRecord, SectionMode Mode, string ResourceKeyPrefix)
        {

            SectionZoneLayout layout = null;
            


            layout = GetZoneLayout(screenLayout, layout);


            if (isNewRecord)
            {

                holder.Controls.Clear();
            }


            List<SectionDivider> dividers = layout.Dividers;

            GenerateSectionDivs(holder, null, sections, Mode, ResourceKeyPrefix, dividers);

        


        }

        private void GenerateSectionDivs(PlaceHolder holder, HtmlGenericControl parent, List<Section> sections, SectionMode Mode, string ResourceKeyPrefix, List<SectionDivider> dividers)
        {
            foreach (SectionDivider d in dividers)
            {


                HtmlGenericControl div = new HtmlGenericControl("div");

                if (!String.IsNullOrEmpty(d.Name))
                {
                    div.ID = String.Format("{0}Zone", d.Name);
                }

                if (!String.IsNullOrEmpty(d.CssClass))
                    div.Attributes.Add("class", d.CssClass);

                if (!String.IsNullOrEmpty(d.Style))
                    div.Attributes.Add("style", d.Style);

                if (!String.IsNullOrEmpty(d.StartMarkup))
                {
                    div.Controls.Add(new LiteralControl(d.StartMarkup));
                }

                if (!String.IsNullOrEmpty(d.Name))
                {
                    IterateSectionsToRender(sections, Mode, ResourceKeyPrefix, d, div);
                }


                if (d.Dividers.Count > 0)
                {
                    GenerateSectionDivs(holder, div, sections, Mode, ResourceKeyPrefix, d.Dividers);
                }

                if (!String.IsNullOrEmpty(d.EndMarkup))
                {
                    div.Controls.Add(new LiteralControl(d.EndMarkup));
                }

                if (parent == null)
                {
                    holder.Controls.Add(div);
                }
                else
                {
                    parent.Controls.Add(div);
                }
                
            }
        }

        private void IterateSectionsToRender(List<Section> sections, SectionMode Mode, string ResourceKeyPrefix, SectionDivider d, HtmlGenericControl div)
        {

            foreach (Section section in sections)
            {
                if (section.ContentPane.ToLower() == d.Name.ToLower())
                {
                    try
                    {
                        bool render = true;

                        if (section is EditSection)
                        {
                            render = false;



                            switch (section.Mode)
                            {
                                case SectionMode.Edit:
                                    if (Mode == SectionMode.Edit)
                                        render = true;
                                    break;
                                case SectionMode.Insert:
                                    if (Mode == SectionMode.Insert)
                                        render = true;
                                    break;
                                default:
                                    render = true;
                                    break;
                            }
                        }


                        if (render)
                        {
                            string sectionResourcePrefixName = (ResourceKeyPrefix);
                            RenderSection(div, Screen, section, sectionResourcePrefixName);
                        }


                    }
                    catch (Exception ex)
                    {
                        string ErrorMessageFormat = "<span style='color:red'>ERROR - {0} - {1} - {2}</span>";
                        string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, section.Name, "BasePage.RenderPageSections");


                        div.Controls.Add(new LiteralControl(ErrorMessages));
                    }
                }
            }
        }

       
        private static SectionZoneLayout GetZoneLayout(string screenLayout, SectionZoneLayout layout)
        {
            if (String.IsNullOrEmpty(screenLayout))
            {
                if (String.IsNullOrEmpty(CodeTorch.Core.Configuration.GetInstance().App.DefaultZoneLayout))
                {
                    throw new ApplicationException("No default section zone layout to use");
                }
                else
                {
                    layout = SectionZoneLayout.GetByName(CodeTorch.Core.Configuration.GetInstance().App.DefaultZoneLayout);
                }

            }
            else
            {
                layout = SectionZoneLayout.GetByName(screenLayout);
            }
            return layout;
        }

        private void RenderSection(HtmlGenericControl div, Screen screen, Section section, string ResourceKeyPrefix)
        {


            bool RenderSection = section.Visible;

            try
            {

                if (section.Permission.CheckPermission)
                {
                    RenderSection = Common.HasPermission(section.Permission.Name);
                }

                
                BaseSectionControl sectionControl = GetSectionControl(screen, section, ResourceKeyPrefix);

                if (!sectionControl.Visible)
                {
                    RenderSection = false;
                }

                if (sectionControl != null)
                {
                    if (String.IsNullOrEmpty(section.ID))
                    {
                        if (!String.IsNullOrEmpty(section.Name))
                        {
                            sectionControl.ID = section.Name.Trim().Replace(' ', '_').Replace('&', '_').Replace("'", "_");
                        }
                    }
                    else
                    {
                        sectionControl.ID = section.ID;
                    }

                    

                    sectionControl.Visible = RenderSection;

                    div.Controls.Add(sectionControl);

                    this.SectionControls.Add(sectionControl);
                }
                else
                {
                    throw new ApplicationException("GetSectionControl() could not load usercontrol");

                }

               
            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "<span style='color:red'>ERROR - Could not load section {0} - {1} - {2}</span>";
                string ErrorMessages = String.Format(ErrorMessageFormat, section.Name, section.Type, ex.Message);

                div.Controls.Add(new LiteralControl(ErrorMessages));
               
                
            }
        }



        private BaseSectionControl GetSectionControl(Screen screen, Section section, string ResourceKeyPrefix)
        {
            BaseSectionControl sectionControl = null;
            SectionType sectionType = SectionType.GetSectionType(section);

            if (sectionType != null)
            {
                if (!String.IsNullOrEmpty(sectionType.Assembly) && !String.IsNullOrEmpty(sectionType.Class))
                {
                    sectionControl = (BaseSectionControl)Activator.CreateInstance(sectionType.Assembly, sectionType.Class).Unwrap();
                }
                

                if (sectionControl != null)
                {
                    sectionControl.Screen = screen;
                    sectionControl.Section = section;
                    sectionControl.ResourceKeyPrefix = ResourceKeyPrefix;

                    sectionControl.RenderControl();
                }
            }

            return sectionControl;
        }


        protected string GetSectionHeaderName(string sectionName)
        {


            StringBuilder tokens = new StringBuilder();

            string sep = "";
            string[] token = sectionName.Split(' ');

            for (int i = 0; i < token.Length; i++)
            {

                if (token[i].StartsWith("{"))
                {
                    string modeType = token[i].Substring(1, (token[i].Length - 2));
                    if (modeType.ToUpper() == MODE)
                    {

                        switch (this.PageMode.ToString().ToLower())
                        {
                            case PAGEMODE_INSERT:
                                token[i] = ADD;
                                break;
                            case PAGEMODE_EDIT:
                                token[i] = EDIT;
                                break;
                        }
                    }

                }
                else if (token[i].ToLower() == ADD || token[i].ToLower() == EDIT)
                {
                    switch (this.PageMode.ToString().ToLower())
                    {
                        case PAGEMODE_INSERT:
                            token[i] = ADD;
                            break;
                        case PAGEMODE_EDIT:
                            token[i] = EDIT;
                            break;
                    }

                }


                tokens.Append(sep);
                tokens.Append(token[i]);


                sep = " ";
            }


            return tokens.ToString();
        }

        #region PopulateFormByDataTable
        public void PopulateFormByDataTable(List<Widget> controls, DataTable data)
        {

            PopulateFormByDataTable(null, controls, data, false);
        }

        public void PopulateFormByDataTable(List<Widget> controls, DataTable data, bool RefreshControls)
        {
            PopulateFormByDataTable(null, controls, data, RefreshControls);
        }

        public void PopulateFormByDataTable(Control container, List<Widget> controls, DataTable data, bool RefreshControls)
        {

            DataRow row = null;

            if (data.Rows.Count > 0)
                row = data.Rows[0];

            if (row != null)
            {
                PopulateFormByDataRow(container, controls, row, RefreshControls);
            }

        }

        public void PopulateFormByDataRowView(Control container, List<Widget> controls, DataRowView data, bool RefreshControls)
        {
            DataRow row = data.Row;
            PopulateFormByDataRow(container, controls, row, RefreshControls);
        }

        public void PopulateFormByDataRow(Control container, List<Widget> controls, DataRow row, bool RefreshControls)
        {
            int index = 0;
            

            

            foreach (Widget control in controls)
            {
                index++;
                if (!String.IsNullOrEmpty(control.Name))
                {
                    CodeTorch.Web.FieldTemplates.BaseFieldTemplate c = this.FindFieldRecursive(container, control.Name);

                    if (c != null)
                    {
                        try
                        {
                            if (RefreshControls)
                            {
                                c.Refresh();
                            }


                            if (!String.IsNullOrEmpty(control.DataField))
                            {
                                if (row.Table.Columns.Contains(control.DataField))
                                {
                                    c.ValueObject = row[control.DataField];
                                    c.Value = row[control.DataField].ToString();
                                }
                            }

                            c.RecordObject = row;
                        }
                        catch { }
                    }
                    else
                    {
                        //likely a read only control - lets try seach again
                        c = this.FindFieldRecursive(container, (control.Name + "_ReadOnly_Label"));

                        if (c != null)
                        {
                            try
                            {
                                if (RefreshControls)
                                {
                                    c.Refresh();
                                }

                                if (!String.IsNullOrEmpty(control.ReadOnlyDataField))
                                {
                                    if (row.Table.Columns.Contains(control.ReadOnlyDataField))
                                    {
                                        c.ValueObject = row[control.ReadOnlyDataField];
                                        c.Value = row[control.ReadOnlyDataField].ToString();
                                    }
                                }

                                c.RecordObject = row;
                            }
                            catch { }
                        }
                    }
                }
                else
                {
                    throw new ApplicationException($"Control {index} is missing ControlName - {control.GetType().FullName}");
                }

            }
        }
        #endregion

 

        public void RefeshForm(Control container, List<Widget> controls)
        {
            int index = 0;
            foreach (Widget control in controls)
            {
                index++;
                if (!String.IsNullOrEmpty(control.Name))
                {
                    CodeTorch.Web.FieldTemplates.BaseFieldTemplate c = this.FindFieldRecursive(container, control.Name);

                    if (c != null)
                    {
                        try
                        {
                            c.Refresh();
                        }
                        catch { }
                    }
                }
                else
                {
                    throw new ApplicationException($"Control {index} is missing ControlName - {control.GetType().FullName}");
                }

            }
        }

        protected string BuildTitle(DataRow data, string TitleFormatString)
        {
            StringBuilder tokens = new StringBuilder();

            //tokenize format string
            string sep = "";
            string[] token = TitleFormatString.Split(' ');

            for (int i = 0; i < token.Length; i++)
            {

                if (token[i].StartsWith("{"))
                {
                    string ColumnName = token[i].Substring(1, (token[i].Length - 2));
                    if (data.Table.Columns.Contains(ColumnName))
                    {
                        token[i] = data[ColumnName].ToString();
                    }

                }

                tokens.Append(sep);
                tokens.Append(token[i]);

                sep = " ";
            }

            return tokens.ToString();
        }

        public virtual void SetPageTitle()
        {
            if (this.UseTitleCommand())
            {
                try
                {
                    if (String.IsNullOrEmpty(this.Screen.Title.CommandName))
                    {
                        throw new ApplicationException("Title.CommandName is missing");
                    }

                    List<ScreenDataCommandParameter> parameters = pageDB.GetPopulatedCommandParameters(this.Screen.Title.CommandName, this);
                    DataTable data = dataCommandDB.GetDataForDataCommand(this.Screen.Title.CommandName, parameters);

                    if (data.Rows.Count > 0)
                    {
                        this.Title = BuildTitle(data.Rows[0], GetTitleFormatString());
                    }
                }
                catch (Exception ex)
                {
                    this.Title = String.Format("Error while setting page title - {0}", ex.Message);
                }

            }
            else
            {
                if (Screen != null)
                {
                    this.Title = GetGlobalResourceString("Screen.Title.Name", Screen.Title.Name); 
                }
            }
        }

        public string GetLocalResourceString(string ResourceKey, string DefaultValue)
        {
            string retVal = DefaultValue;

            if (app.EnableLocalization)
            {
                object resourceValue = GetLocalResourceObject(ResourceKey);
                //object resourceValue = HttpContext.GetLocalResourceObject("", ResourceKey);

                if (resourceValue == null)
                {
                    retVal = DefaultValue;
                }
                else
                {
                    retVal = resourceValue.ToString();
                }
            }

            return retVal;
        }

        public string GetGlobalResourceString(string ResourceSet, string ResourceKey, string DefaultValue)
        {
            string retVal = DefaultValue;

            if (app.EnableLocalization)
            {
                try
                {
                    object resourceValue = GetGlobalResourceObject(ResourceSet, ResourceKey);
                    //object resourceValue = HttpContext.GetLocalResourceObject("", ResourceKey);

                    if (resourceValue == null)
                    {
                        retVal = DefaultValue;
                    }
                    else
                    {
                        retVal = resourceValue.ToString();
                    }
                }
                catch { }
            }
            

            return retVal;
        }

        public string GetGlobalResourceString(string ResourceKey, string DefaultValue)
        {
            string retVal = null;
            string ResourceSet = Common.StripVirtualPath(Request.Url.AbsolutePath);


            retVal = GetGlobalResourceString(ResourceSet, ResourceKey, DefaultValue);

            return retVal;
        }

        

        public virtual void SetPageSubTitle()
        {
            if (this.UseSubTitleCommand())
            {
                try
                {

                    if (String.IsNullOrEmpty(this.Screen.SubTitle.CommandName))
                    {
                        throw new ApplicationException("SubTitle.CommandName is missing");
                    }

                    List<ScreenDataCommandParameter> parameters = pageDB.GetPopulatedCommandParameters(this.Screen.SubTitle.CommandName, this);
                    DataTable data = dataCommandDB.GetDataForDataCommand(this.Screen.SubTitle.CommandName, parameters);

                    if (data.Rows.Count > 0)
                    {
                        this.SubTitle = BuildTitle(data.Rows[0], GetSubTitleFormatString());
                    }
                }
                catch (Exception ex)
                {
                    this.Title = String.Format("Error while setting page sub title - {0}", ex.Message);
                }

            }
            else
            {
                if (Screen != null)
                {
                    this.SubTitle = Screen.SubTitle.Name;
                    this.SubTitle = GetGlobalResourceString("Screen.SubTitle.Name", Screen.SubTitle.Name); 
                }
            }
        }

        public virtual string GetTitleFormatString()
        {
            string retVal = String.Empty;

            if (String.IsNullOrEmpty(this.Screen.Title.CommandFormatString))
            {
                retVal = this.Screen.Title.Name;
            }
            else
            {
                retVal = this.Screen.Title.CommandFormatString;
            }
            return retVal;
        }

        public virtual string GetSubTitleFormatString()
        {
            string retVal = String.Empty;

            if (String.IsNullOrEmpty(this.Screen.SubTitle.CommandFormatString))
            {
                retVal = this.Screen.SubTitle.Name;
            }
            else
            {
                retVal = this.Screen.SubTitle.CommandFormatString;
            }
            return retVal;
        }

        public virtual bool UseTitleCommand()
        {

            bool retVal = false;

            try
            {
                if(this.Screen != null)
                    retVal = this.Screen.Title.UseCommand;
            }
            catch { }

            return retVal;
        }

        public virtual bool UseSubTitleCommand()
        {
            bool retVal = false;

            try
            {
                if (this.Screen != null)
                    retVal = this.Screen.SubTitle.UseCommand;
            }
            catch { }

            return retVal;

        }

        public string GetEntityIDValue(Screen screen, string EntityID, ScreenInputType EntityInputType)
        {
            return GetEntityIDValue(screen, EntityID, EntityInputType, this);
        }

        public string GetEntityIDValue(Screen screen, string EntityID, ScreenInputType EntityInputType, Control container)
        {
            string retVal = null;
            ScreenDataCommandParameter p = new ScreenDataCommandParameter(EntityID, EntityInputType);
            object EntityIDValue = Common.GetParameterInputValue(this, p, container);

            if (EntityIDValue != null)
                retVal = EntityIDValue.ToString();
            

            //switch (EntityInputType)
            //{
            //    case ScreenInputType.QueryString:
            //        EntityIDValue = Request.QueryString[EntityID];
            //        break;
            //    case ScreenInputType.User:
            //        try
            //        {
            //            List<string> profileProperties = CodeTorch.Core.Configuration.GetInstance().App.ProfileProperties;
            //            int propertyIndex = Enumerable.Range(0, profileProperties.Count).First(i => profileProperties[i].ToLower() == EntityID.ToLower());

            //            FormsIdentity identity = (FormsIdentity)HttpContext.Current.User.Identity;
            //            FormsAuthenticationTicket ticket = identity.Ticket;

            //            EntityIDValue = ticket.UserData.Split('|')[propertyIndex];

            //        }
            //        catch { }
            //        break;
            //}


            return retVal;
        }


        public virtual void ExecuteAction(CodeTorch.Core.Action sourceAction)
        {
            if (sourceAction != null)
            {
                ActionRunner runner = new ActionRunner();

                Core.Action action = ObjectCopier.Clone<Core.Action>(sourceAction);

                runner.Page = (BasePage)this.Page;
                runner.Action = action;

                runner.Execute();

            }
        }

        public virtual void DisplaySuccessAlert(string Message)
        {
            DisplayAlertMessage message = new DisplayAlertMessage();

            message.IsDismissable = true;
            message.AlertType = DisplayAlertMessage.ALERT_SUCCESS;
            message.Text = String.Format("{0}", Message);
            this.MessageBus.Publish(message);
        }

        public virtual void DisplayWarningAlert(string Message)
        {
            DisplayAlertMessage message = new DisplayAlertMessage();

            message.IsDismissable = true;
            message.AlertType = DisplayAlertMessage.ALERT_WARNING;
            message.Text = String.Format("{0}", Message);
            this.MessageBus.Publish(message);
        }

        public virtual void DisplayInfoAlert(string Message)
        {
            DisplayAlertMessage message = new DisplayAlertMessage();

            message.IsDismissable = true;
            message.AlertType = DisplayAlertMessage.ALERT_INFO;
            message.Text = String.Format("{0}", Message);
            this.MessageBus.Publish(message);
        }

        public virtual void DisplayErrorAlert(string Message, bool htmlEncodeMessage = false)
        {
            //todo - implement htmlEncodeMessage
            DisplayAlertMessage message = new DisplayAlertMessage();

            message.IsDismissable = true;
            message.AlertType = DisplayAlertMessage.ALERT_DANGER;
            message.Text = String.Format("{0}", Message);
            this.MessageBus.Publish(message);
        }

        public virtual void DisplayErrorAlert(Exception ex, bool htmlEncodeMessage = false)
        {
            DisplayAlertMessage message = new DisplayAlertMessage();

            string errorMessageFormat = "<strong>The following error(s) occurred</strong>:<ul><li>{0}</li></ul>";

            if (app != null)
            {
                if (app.DefaultErrorMessageFormatString != null)
                {
                    errorMessageFormat = app.DefaultErrorMessageFormatString;
                }
            }

            string errorMessage = null;

            if(htmlEncodeMessage)
                errorMessage = String.Format(errorMessageFormat, System.Net.WebUtility.HtmlEncode(ex.Message));
            else
                errorMessage = String.Format(errorMessageFormat, System.Net.WebUtility.HtmlEncode(ex.Message));

            errorMessage = errorMessage.Replace("\r\n", "<br/>");
            

            message.IsDismissable = false;
            message.AlertType = DisplayAlertMessage.ALERT_DANGER;
            message.Text = errorMessage;
            this.MessageBus.Publish(message);
        }

        public void PopulateSections()
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                foreach (BaseSectionControl section in this.SectionControls)
                {
                    try
                    {
                        section.PopulateControl();

                        ExecuteAction(section.Section.AfterPopulateSection);


                    }
                    catch (Exception ex)
                    {
                        string ErrorMessageFormat = "<span class='ErrorMessages'>ERROR - {0} - {2} Section - {1})</span>";
                        string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, section.Section.Name, section.Section.Type);

                        section.Controls.Add(new LiteralControl(ErrorMessages));
                    }
                }


            }
            catch (Exception ex)
            {
                DisplayErrorAlert(ex);

                log.Error(ex);
            }
        }



        public virtual FormViewMode DetermineMode(string EntityID, ScreenInputType EntityInputType)
        {
            FormViewMode retVal = FormViewMode.ReadOnly;


            string EntityIDValue = this.GetEntityIDValue(this.Screen, EntityID, EntityInputType);

            if (String.IsNullOrEmpty(EntityIDValue))
            {
                retVal = FormViewMode.Insert;
            }
            else
            {
                retVal = FormViewMode.Edit;
            }

            return retVal;
        }
    }
}
