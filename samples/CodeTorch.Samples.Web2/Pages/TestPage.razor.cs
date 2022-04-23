using CodeTorch.Abstractions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodeTorch.Samples.Web.Pages
{
    public class TestPageBase : ComponentBase
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
            string folder = null;
            string pagename = null;
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
    }
}
