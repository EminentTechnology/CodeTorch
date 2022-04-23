using CodeTorch.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeTorch.Samples.Web.Pages
{
    public class CodeTorchPage : ComponentBase
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; }

        public CodeTorch.Core.Configuration config;
        public string url;
        public string folder;
        public string pagename;

        public CodeTorch.Core.Screen Screen = null;
        public bool IsLoading = true;

        protected override void OnInitialized()
        {
            //get page url
            url = NavigationManager.Uri;
            var uri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri);

            //determine folder and page name
            var pageMeta = DetermineFolderAndPageName(uri);
            this.folder = pageMeta.Folder;
            this.pagename = pageMeta.PageName;

            this.Screen = CodeTorch.Core.Screen.GetByFolderAndName(pageMeta.Folder, pageMeta.PageName);


            //get config
            config = CodeTorch.Core.Configuration.GetInstance();

            
        }

        public (string Folder, string PageName) DetermineFolderAndPageName(Uri uri)
        {
            string folder=null;
            string pagename=null;
            int appSegmentIndex = -1;

            if (uri != null)
            {
                for (int segmentIndex = 0; segmentIndex < uri.Segments.Length; segmentIndex++)
                {
                    var segment = uri.Segments[segmentIndex];
                    if (segment.ToLower() == "app/")
                    {
                        appSegmentIndex = segmentIndex;
                        break;
                    }
                }

                if ((appSegmentIndex + 1) <= uri.Segments.Length)
                    folder = uri.Segments[appSegmentIndex + 1].Replace("/", "");

                if ((appSegmentIndex + 2) <= uri.Segments.Length)
                {
                    pagename = uri.Segments[appSegmentIndex + 2].Replace("/", "");
                    if (!pagename.ToLower().EndsWith(".aspx"))
                    {
                        pagename += ".aspx";
                    }
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(uri), $"{nameof(uri)} is required");
            }

            return (folder, pagename);
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            //builder.AddMarkupContent(3, $"folder:{this.Screen?.Folder} page:{this.Screen?.Name}  ");

            //builder.OpenComponent<CodeTorch.Samples.Web.Layouts.PageTemplates.LoginPageTemplate>(0);
            string pageTemplateTypeName = GetPageTemplateTypeName();
            
            builder.OpenComponent(0, Type.GetType(pageTemplateTypeName));
            builder.AddAttribute(1, "Title", GetPageTitle());
            builder.AddAttribute(2, "SubTitle", GetPageSubTitle());

            builder.AddAttribute(3, "Body", 
            (Microsoft.AspNetCore.Components.RenderFragment)((body) => 
            {
                body.AddMarkupContent(4, pageTemplateTypeName);

                //add page layout

            }));

            builder.CloseComponent();
        }

        private string GetPageTemplateTypeName()
        {
            //TODO - need to read directly from config once config is transformed
            string pageTemplateType = "CodeTorch.Samples.Web.Layouts.PageTemplates.PlainPageTemplate";
            if (Screen.PageTemplate != null)
            {
                pageTemplateType = $"CodeTorch.Samples.Web.Layouts.PageTemplates.{Screen.PageTemplate.Name}PageTemplate";
            }
            else
            {
                if (!String.IsNullOrEmpty(config.App.DefaultPageTemplate))
                {
                    pageTemplateType = $"CodeTorch.Samples.Web.Layouts.PageTemplates.{Screen.PageTemplate.Name}PageTemplate";
                }
            }
            return pageTemplateType;
        }

        private string GetPageTitle()
        {
            //TODO - need to support dynamic page titles
            return this.Screen.Title.Name;
        }

        private string GetPageSubTitle()
        {
            //TODO - need to support dynamic page titles
            return this.Screen.SubTitle.Name;
        }

        protected void BuildRenderTree2(RenderTreeBuilder builder)
        {
            RenderTreeBuilder __builder = builder;
           
            __builder.OpenComponent<CodeTorch.Samples.Web.Layouts.PageTemplates.LoginPageTemplate>(0);
            __builder.AddAttribute(1, "Title", "RxTemps");
            __builder.AddAttribute(2, "Body", (Microsoft.AspNetCore.Components.RenderFragment)((__builder2) => {
                __builder2.AddMarkupContent(3, "\r\n        ");
                __builder2.OpenComponent<CodeTorch.Samples.Web.Layouts.Content.LoginLayout>(4);
                __builder2.AddAttribute(5, "Left", (Microsoft.AspNetCore.Components.RenderFragment)((__builder3) => {
                    __builder3.AddMarkupContent(6, "\r\n                ");
                    __builder3.OpenComponent<CodeTorch.Blazor.Sections.Alert>(7);
                    __builder3.CloseComponent();
                    __builder3.AddMarkupContent(8, "\r\n                ");
                    __builder3.OpenComponent<CodeTorch.Blazor.Sections.Edit>(9);
                    __builder3.AddAttribute(10, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder4) => {
                        __builder4.AddMarkupContent(11, "\r\n                    ");
                        __builder4.OpenComponent<CodeTorch.Blazor.Controls.Textbox>(12);
                        __builder4.AddAttribute(13, "Name", "EmailAddress");
                        __builder4.AddAttribute(14, "Label", "Email Address");
                        __builder4.AddAttribute(15, "IsRequired", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Boolean>(
#nullable restore
#line 12 "C:\Sandbox\CodeTorch2\samples\CodeTorch.Samples.Web\Pages\CodeTorchPage.razor"
                                                                                   true

#line default
#line hidden
#nullable disable
                        ));
                        __builder4.CloseComponent();
                        __builder4.AddMarkupContent(16, "\r\n                    ");
                        __builder4.OpenComponent<CodeTorch.Blazor.Controls.Password>(17);
                        __builder4.AddAttribute(18, "Name", "Password");
                        __builder4.AddAttribute(19, "Label", "Password");
                        __builder4.AddAttribute(20, "IsRequired", Microsoft.AspNetCore.Components.CompilerServices.RuntimeHelpers.TypeCheck<System.Boolean>(
#nullable restore
#line 13 "C:\Sandbox\CodeTorch2\samples\CodeTorch.Samples.Web\Pages\CodeTorchPage.razor"
                                                                           true

#line default
#line hidden
#nullable disable
                        ));
                        __builder4.CloseComponent();
                        __builder4.AddMarkupContent(21, "\r\n                ");
                    }
                    ));
                    __builder3.CloseComponent();
                    __builder3.AddMarkupContent(22, "\r\n                ");
                    __builder3.OpenComponent<CodeTorch.Blazor.Sections.Buttons>(23);
                    __builder3.AddAttribute(24, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder4) => {
                        __builder4.AddMarkupContent(25, "\r\n                        ");
                        __builder4.OpenComponent<CodeTorch.Blazor.Controls.ButtonControl>(26);
                        __builder4.AddAttribute(27, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder5) => {
                            __builder5.AddContent(28, "Login");
                        }
                        ));
                        __builder4.CloseComponent();
                        __builder4.AddMarkupContent(29, "\r\n                    ");
                    }
                    ));
                    __builder3.CloseComponent();
                    __builder3.AddMarkupContent(30, "\r\n            ");
                }
                ));
                __builder2.AddAttribute(31, "Bottom", (Microsoft.AspNetCore.Components.RenderFragment)((__builder3) => {
                    __builder3.AddMarkupContent(32, "\r\n                ");
                    __builder3.OpenComponent<CodeTorch.Blazor.Sections.Content>(33);
                    __builder3.AddAttribute(34, "ChildContent", (Microsoft.AspNetCore.Components.RenderFragment)((__builder4) => {
                        __builder4.AddMarkupContent(35, "\r\n                    ");
                        __builder4.AddMarkupContent(36, "<div class=\"text-center text-muted\">Don\'t have a Pharmacist/Technician account yet? <a href=\"../Signup/Default\">Sign Up</a></div>\r\n                    ");
                        __builder4.AddMarkupContent(37, "<div class=\"text-center text-muted\">Forgot your password? <a href=\"ForgotPasswordRequest\">Reset Password</a></div>\r\n                    ");
                        __builder4.AddMarkupContent(38, "<div class=\"text-center text-muted\">Are you a Pharmacy looking to hire Pharmacists/Technicians? <a href=\"RedirectToEmployerLogin\">Login Here</a></div>\r\n                ");
                    }
                    ));
                    __builder3.CloseComponent();
                    __builder3.AddMarkupContent(39, "\r\n            ");
                }
                ));
                __builder2.CloseComponent();
                __builder2.AddMarkupContent(40, "\r\n    ");
            }
            ));
            __builder.CloseComponent();

            base.BuildRenderTree(builder);
        }

    }
}
