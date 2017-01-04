using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web;
using System.Data;
using System.Data.Common;
using System.Web.UI;
using System.Web.Compilation;
using CodeTorch.Core;

namespace CodeTorch.Web.HttpHandlers
{
    public class AppBuilderRouteHandler<T>: IRouteHandler where T: IHttpHandler, new()
    {
        public AppBuilderRouteHandler()
        {
 
        }

        #region IRouteHandler Members

        public IHttpHandler GetHttpHandler( RequestContext requestContext )
        {
            IHttpHandler retVal = null;

            string virtualPath = null;




            string url = ((System.Web.Routing.Route)(requestContext.RouteData.Route)).Url;
            string[] urlSegments = url.Split('/');

            if (url.Length > 0)
            {
                switch (urlSegments[0].ToLower())
                {

                    case "app":
                        retVal = GetAppPagesHandler(requestContext, virtualPath, false);
                        break;
                    case "codetorch":
                        retVal = GetAppPagesHandler(requestContext, virtualPath, false);
                        break;
                    case "{folder}":
                        retVal = GetRestServiceHandler(requestContext, virtualPath, url, urlSegments);
                        break;
                    default:
                        virtualPath = "~/Templates/Pages/PageNotFound.aspx";
                        retVal = (IHttpHandler)BuildManager.CreateInstanceFromVirtualPath(virtualPath, typeof(T));
                        break;

                }
            }



            return retVal;
        }

        private  IHttpHandler GetAppPagesHandler(RequestContext requestContext, string virtualPath, bool IsBackend)
        {
            string pageName = requestContext.RouteData.GetRequiredString("page");
            string[] urlSegments = requestContext.HttpContext.Request.Url.Segments;
            string folder = requestContext.RouteData.GetRequiredString("folder"); //urlSegments[urlSegments.Length - 2].Replace("/", "");

            Screen screen = Screen.GetByFolderAndName(folder, pageName);

            if (screen != null)
            {
                ScreenType screenType = ScreenType.GetScreenType(screen);

                if (screenType == null)
                {
                    throw new ApplicationException(String.Format("Screen type {0} could not be found in configuration", screen.Type));
                }

                virtualPath = screenType.TemplateFile;

            }
            else
            {
                virtualPath = "~/Templates/Pages/PageNotFound.aspx";
            }
            

            return (virtualPath != null)
                ? (IHttpHandler)BuildManager.CreateInstanceFromVirtualPath(virtualPath, typeof(T))
                : new T();
        }

        private  IHttpHandler GetRestServiceHandler(RequestContext requestContext, string virtualPath, string urlPattern, string[] urlSegments)
        {
            
            RestService service = null;

            string folder = requestContext.RouteData.GetRequiredString("folder"); 
            string restServiceName = requestContext.RouteData.DataTokens["rest-service-name"].ToString();

            service = RestService.GetByFolderAndName(folder, restServiceName);

            RestServiceHandler retVal =  new RestServiceHandler();
            retVal.Me = service;
            retVal.RouteData = requestContext.RouteData;

            if (urlPattern.ToLower().EndsWith(".xml"))
            {
                retVal.Format = "xml";

            }
            else
            {
                retVal.Format = "json";
            }

            return retVal;
        }

        #endregion

        
    }
}
