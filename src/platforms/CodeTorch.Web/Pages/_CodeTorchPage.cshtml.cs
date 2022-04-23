using CodeTorch.Abstractions;
using CodeTorch.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CodeTorch.Web.Pages
{
    public class SectionViewModel
    {
        public SectionViewModel(CodeTorchPageModel pageModel, Section section)
        {
            PageModel = pageModel;
            Section = section;
        }

        public CodeTorchPageModel PageModel { get; private set; }
        public Section Section { get; private set; }
    }
    public class CodeTorchPageModel : PageModel
    {
        readonly IConfigurationStore config;
        public CodeTorchPageModel(IConfigurationStore configStore)
        {
            this.config = configStore;
        }

        public string? Layout { get; set; }
        public Screen Screen { get; set; } = null!;
        PageTemplate PageTemplate { get; set; } = null!;

        public App AppState { get; set; }

        public override Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            Screen = PopulateScreenObject();
            LoadMasterPage();

            return base.OnPageHandlerSelectionAsync(context);
        }

        public async Task OnGet()
        {
            var app = CodeTorch.Core.Configuration.GetInstance().App;
            this.AppState = app;

        }

        public async void OnPost(string command)
        {

        }

        protected virtual Core.Screen PopulateScreenObject()
        {
            //if (rootFolder.ToLower() == "app")
            if (this.Request.Path.HasValue)
            {
                if (this.Request.Path.Value.ToLower().StartsWith("/app/"))
                {
                    int lastSlash = this.Request.Path.Value.LastIndexOf('/');

                    string pageName = this.Request.Path.Value.Substring(lastSlash + 1);
                    string folder = this.Request.Path.Value.Substring(5, lastSlash - 5);

                    if (!pageName.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase))
                    {
                        //TODO: POCHU - need to remove - only here for backwards support
                        pageName += ".aspx";
                    }

                    var retVal = Screen.GetByFolderAndName(folder, pageName);
                    if (retVal == null)
                    {
                        throw new Exception("Screen could not be found");
                    }
                    return retVal;
                }
            }
            throw new Exception("Request Path does not have a value");
        }

        private void LoadMasterPage()
        {

            string? PageTemplateToLoad = null;
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



            if (String.IsNullOrEmpty(PageTemplateToLoad))
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

                //TODO: handle schema upgrade for master page - they should start with _

                this.Layout = PageTemplate.Path;
                if (!this.Layout.StartsWith("_"))
                {
                    this.Layout = $"_{System.IO.Path.GetFileNameWithoutExtension(this.Layout)}";
                }

                //foreach (PageTemplateItem item in PageTemplate.Items)
                //{
                //    LoadMasterPageContentPlaceHolder(item);

                //}
            }
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

        private string GetDynamicPageTemplate(ScreenPageTemplate pageTemplate)
        {
            throw new NotImplementedException();
            //TODO: implement GetDynamicPageTemplate
            //    string retVal = null;
            //    log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            //    try
            //    {
            //        DataCommandService dataCommandDB = DataCommandService.GetInstance();
            //        PageDB pageDB = new PageDB();

            //        List<ScreenDataCommandParameter> parameters = pageDB.GetPopulatedCommandParameters(pageTemplate.DataCommand, this);

            //        DataTable dt = dataCommandDB.GetDataForDataCommand(pageTemplate.DataCommand, parameters);
            //        if (dt.Rows.Count > 0)
            //        {
            //            if (dt.Columns.Contains(pageTemplate.DataField))
            //            {
            //                retVal = dt.Rows[0][pageTemplate.DataField].ToString();
            //            }
            //        }


            //    }
            //    catch (Exception ex)
            //    {
            //        DisplayErrorAlert(ex);

            //        log.Error(ex);
            //    }

            //    return retVal;
        }
    }
}
