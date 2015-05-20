using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Compilation;
using System.Web;

namespace CodeTorch.Web.DbResourceProvider
{
    [DesignTimeResourceProviderFactoryAttribute(typeof(DbResourceProviderFactory))]
    public class DbResourceProviderFactory : ResourceProviderFactory
    {
        public override IResourceProvider CreateGlobalResourceProvider(string classKey)
        {
            return new DbResourceProvider(null, classKey);
        }

        public override IResourceProvider CreateLocalResourceProvider(string virtualPath)
        {
            string pagePath = virtualPath;
            
            if (HttpContext.Current != null)
            { 
                //translate url to path because of dynamic pages
                pagePath = HttpContext.Current.Request.Url.AbsolutePath;
            }
            string ResourceSetName = StripVirtualPath(pagePath);
            return new DbResourceProvider(null, ResourceSetName.ToLower());
        }

        private string StripVirtualPath(string FullVirtualPath)
        {
            string StripVirtual = null; //todo: wwDbResourceConfiguration.Current.DesignTimeVirtualPath;
            if (HttpContext.Current != null)
                StripVirtual = HttpContext.Current.Request.ApplicationPath;

            if (!StripVirtual.EndsWith("/"))
                StripVirtual = StripVirtual.ToLower() + "/";

            // *** Root Webs are a special case
            if (StripVirtual == "/" && FullVirtualPath.StartsWith("/"))
                return FullVirtualPath.TrimStart('/');  // leave just the relative path

            // *** Stript out the virtual path leaving us just with page
            return FullVirtualPath.ToLower().Replace(StripVirtual, "");
        }
    }
}
