using System;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeTorch.Web.Data;
using CodeTorch.Web.Templates;
using CodeTorch.Web.HttpHandlers;
using System.Web.Routing;
using System.Reflection;
using System.Drawing;
using System.Drawing.Imaging;
using log4net;
using CodeTorch.Core;
using System.Web.Security;
using System.Configuration;
using CodeTorch.Web.SectionControls;
using System.Web.Hosting;
using CodeTorch.Web.Providers.EmbeddedResourceVirtualPathProvider;
using CodeTorch.Core.Services;

namespace CodeTorch.Web
{
    public class Common
    {
        

        public static void RedirectToHomePage()
        {
            
            HttpContext.Current.Response.Redirect(String.Format("{0}",CodeTorch.Core.Configuration.GetInstance().App.DefaultScreen), false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        public static void Redirect(string url)
        {
            Redirect(url, false);
        }

        public static void Redirect(string url, bool ForceEnd)
        {
            HttpContext.Current.Response.Redirect(url, false);
            HttpContext.Current.ApplicationInstance.CompleteRequest();

            if (ForceEnd)
            {
                HttpContext.Current.Response.End();
            }
        }


        

        public static Control FindControlRecursive(Control root, string id)
        {
            Control retVal = root.FindControl(id);

            if (retVal == null)
            {
                foreach (Control c in root.Controls)
                {
                    retVal = FindControlRecursive(c, id);
                    if (retVal != null)
                    {
                        return retVal;
                    }
                }
            }

            return retVal;
        }

        public static string StripVirtualPath(string FullVirtualPath)
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
   


        
        public static CodeTorch.Web.FieldTemplates.BaseFieldTemplate FindFieldRecursive(BasePage page, Control container, string id)
        {
            CodeTorch.Web.FieldTemplates.BaseFieldTemplate retVal = null;

            ControlCollection controls = null;

            if (container == null)
            {
                controls = page.Controls;
            }
            else
            {
                controls = container.Controls;
            }


            foreach (Control c in controls)
            {
                if ((c.ID != null) && (c.ID.ToLower() == id.ToLower()))
                {
                    if (c is CodeTorch.Web.FieldTemplates.BaseFieldTemplate)
                    {
                        retVal = (CodeTorch.Web.FieldTemplates.BaseFieldTemplate)c;
                        return retVal;
                    }
                }
                else
                {
                    if (c.HasControls())
                    {
                        Control child = FindControlRecursive(c, id);
                        if (child != null)
                        {
                            if (child is CodeTorch.Web.FieldTemplates.BaseFieldTemplate)
                            {
                                retVal = (CodeTorch.Web.FieldTemplates.BaseFieldTemplate)child;
                                return retVal;
                            }
                        }
                    }
                }
            }

            return retVal;
        }

        public static BaseSectionControl FindSection(BasePage page, Control container, string id)
        {
            BaseSectionControl retVal = null;

            ControlCollection controls = null;

            if (container == null)
            {
                controls = page.Controls;
            }
            else
            {
                controls = container.Controls;
            }


            foreach (Control c in controls)
            {
                if ((c.ID != null) && (c.ID.ToLower() == id.ToLower()))
                {
                    if (c is BaseSectionControl)
                    {
                        retVal = (BaseSectionControl)c;
                        return retVal;
                    }
                }
                else
                {
                    if (c.HasControls())
                    {
                        Control child = FindControlRecursive(c, id);
                        if (child != null)
                        {
                            if (child is BaseSectionControl)
                            {
                                retVal = (BaseSectionControl)child;
                                return retVal;
                            }
                        }
                    }
                }
            }

            return retVal;
        }

        public static Bitmap ResizeImage(Bitmap OriginalImage, int? width, int? height, Brush brush, bool ConstrainPhotoDimensions)
        {
            System.Drawing.Bitmap bmpOut = null;
            const int defaultWidth = 800;
            const int defaultHeight = 600;
            int lnWidth = width == null ? defaultWidth : (int)width;
            int lnHeight = height == null ? defaultHeight : (int)height;
            try
            {

                ImageFormat loFormat = OriginalImage.RawFormat;

                decimal lnRatio;
                int lnNewWidth = 0;
                int lnNewHeight = 0;

                if (ConstrainPhotoDimensions)
                {
                    //*** If the image is smaller than a thumbnail just return it
                    if ((OriginalImage.Width < lnWidth) && (OriginalImage.Height < lnHeight))
                    {
                        return (Bitmap)(OriginalImage.Clone());
                    }

                    if (OriginalImage.Width > OriginalImage.Height)
                    {
                        lnRatio = (decimal)lnWidth / OriginalImage.Width;
                        lnNewWidth = lnWidth;
                        decimal lnTemp = OriginalImage.Height * lnRatio;
                        lnNewHeight = (int)lnTemp;
                    }
                    else
                    {
                        lnRatio = (decimal)lnHeight / OriginalImage.Height;
                        lnNewHeight = lnHeight;
                        decimal lnTemp = OriginalImage.Width * lnRatio;
                        lnNewWidth = (int)lnTemp;
                    }
                }
                else
                {
                    lnNewWidth = lnWidth;
                    lnNewHeight = lnHeight;
                }

                bmpOut = new Bitmap(lnNewWidth, lnNewHeight);

                Graphics g = Graphics.FromImage(bmpOut);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.FillRectangle(brush, 0, 0, lnNewWidth, lnNewHeight);
                g.DrawImage(OriginalImage, 0, 0, lnNewWidth, lnNewHeight);

            }
            catch
            {
                return null;
            }
            return bmpOut;
        }
        

 
        public static string UserName
        {
            get
            {
                string retVal = "";


                UserIdentityService identityService = UserIdentityService.GetInstance();
                retVal = identityService.IdentityProvider.GetUserName();

                return retVal;
            }
        }

        public static string CultureCode
        {
            get
            {
                string retVal = null;


                HttpCookie c = HttpContext.Current.Request.Cookies["CultureCode"];

                if (c != null)
                {
                    retVal=c.Value;
                }


                return retVal;
            }
        }
        

        public static string HostHeader
        {
            get
            {
                string retVal = "";


                retVal = HttpContext.Current.Request.ServerVariables["HTTP_HOST"];


                return retVal;
            }
        }

        

        public static string GetProfileProperty(string PropertyName)
        {
            string retVal = "";
            
            try
            {
                App app = CodeTorch.Core.Configuration.GetInstance().App;

                List<string> profileProperties = app.ProfileProperties;

                retVal = GetProfileProperty(PropertyName, profileProperties);

            }
            catch { }

            return retVal;
        }

        public static string GetProfileProperty(string PropertyName, List<string> profileProperties)
        {
            string profileSessionKey = "CodeTorch.Profile";
            App app = CodeTorch.Core.Configuration.GetInstance().App;
            string retVal = "";
            int propertyIndex = Enumerable.Range(0, profileProperties.Count).First(i => profileProperties[i].ToLower() == PropertyName.ToLower());

            if (app.AuthenticationMode is FormsAuthenticationMode)
            {
                //get profile property from form authentication cookie
                FormsIdentity identity = (FormsIdentity)HttpContext.Current.User.Identity;
                FormsAuthenticationTicket ticket = identity.Ticket;

                retVal = ticket.UserData.Split('|')[propertyIndex];
            }
            else
            {
                throw new NotImplementedException();
                

                //get profile property from session object
                DataTable dtProfile = null;
                if (HttpContext.Current != null)
                {
                    if (HttpContext.Current.Session[profileSessionKey] != null)
                    {
                        dtProfile = (DataTable)HttpContext.Current.Session[profileSessionKey];
                    }
                    else
                    {
                       
                        //TODO - needs to be moved to app authenticate
                        DataCommandService dataCommandDB = DataCommandService.GetInstance();
                        PageDB pageDB = new PageDB();

                       
                        List<ScreenDataCommandParameter> parameters = pageDB.GetPopulatedCommandParameters(app.ProfileCommand, null);

                        dtProfile = dataCommandDB.GetDataForDataCommand(app.ProfileCommand, parameters);

                        HttpContext.Current.Session[profileSessionKey] = dtProfile;
                    }

                    if ((dtProfile != null) && (dtProfile.Rows.Count == 1))
                    {
                        retVal = dtProfile.Rows[0][PropertyName].ToString();
                    }
                }
                
            }
            return retVal;
        }


        public static void GenerateCultureTrackingCookie()
        {
            App app = CodeTorch.Core.Configuration.GetInstance().App;

            if (app.EnableLocalization)
            { 
                //get localization data command for retrieval

                DataCommandService dataCommandDB = DataCommandService.GetInstance();

                if (!String.IsNullOrEmpty(app.LocalizationCommand))
                {
                    DataCommand command = DataCommand.GetDataCommand(app.LocalizationCommand);

                    if (command == null)
                    {
                        throw new ApplicationException(String.Format("DataCommand {0} could not be found in configuration - Common.GenerateCultureTrackingCookie", app.LocalizationCommand));
                    }

                    //set any parameters needed
                    List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
                    ScreenDataCommandParameter parameter = null;

                    if (!String.IsNullOrEmpty(app.LocalizationUserNameParameter))
                    {
                        parameter = new ScreenDataCommandParameter();
                        if (app.LocalizationUserNameParameter.StartsWith("@"))
                        {
                            parameter.Name = app.LocalizationUserNameParameter;
                        }
                        else
                        {
                            parameter.Name = "@" + app.LocalizationUserNameParameter;
                        }
                        parameter.Value = Common.UserName;
                        parameters.Add(parameter);
                    }


                    //execute command
                    
                    DataTable retVal = dataCommandDB.GetData(null, command, parameters,  command.Text);


                    string culture = "en-US";
                    //get data from culture field
                    if (retVal != null)
                    {
                        if (retVal.Rows.Count > 0)
                        {
                            if (retVal.Columns.Contains(app.LocalizationCultureField))
                            {
                                if (retVal.Rows[0][app.LocalizationCultureField] != DBNull.Value)
                                {
                                    culture = retVal.Rows[0][app.LocalizationCultureField].ToString();
                                }
                            }
                        }
                    }

                    //generate culture cookie
                    HttpCookie c;

                    if (HttpContext.Current.Request.Cookies["CultureCode"] == null)
                    {
                        c = new HttpCookie("CultureCode");

                    }
                    else
                    {
                        c = HttpContext.Current.Request.Cookies["CultureCode"];
                    }

                    c.Value = culture;
                    c.Path = "/";

                    HttpContext.Current.Response.Cookies.Add(c);
                }
                
            
            }
        }
       


        public static void RedirectToAccessDenied(string Permission)
        {
            HttpContext.Current.Response.Redirect("~/App/Security/AccessDenied.aspx?p=" + Permission,true);
            //HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        internal static void RedirectToLoginPage(string PageUrl)
        {
            if (!String.IsNullOrEmpty(PageUrl))
            {
                PageUrl = HttpContext.Current.Server.UrlEncode(PageUrl);
            }
            HttpContext.Current.Response.Redirect(String.Format("{0}?ReturnUrl={1}", CodeTorch.Core.Configuration.GetInstance().App.LoginScreen, PageUrl),true);
            //HttpContext.Current.ApplicationInstance.CompleteRequest();
        }

        internal static void RedirectTo404Page(string PageUrl)
        {
            if (!String.IsNullOrEmpty(PageUrl))
            {
                PageUrl = HttpContext.Current.Server.UrlEncode(PageUrl);
            }
            HttpContext.Current.Response.Redirect("~/App/Security/PageNotFound.aspx?From=" + PageUrl,true);
            //HttpContext.Current.ApplicationInstance.CompleteRequest();
            
        }

        public static void LoadWebConfiguration(RouteCollection routes)
        {
            //load virtual path provider
            HostingEnvironment.RegisterVirtualPathProvider(new EmbeddedResourceProvider() 
            { 
                {Assembly.GetExecutingAssembly()}
            });

            CodeTorch.Core.ConfigurationLoader.LoadWebConfiguration();

            

            var routeHandler = new AppBuilderRouteHandler<Page>();

            Route appbuilderScreenRoute = new Route("App/{folder}/{page}", routeHandler);
            appbuilderScreenRoute.Defaults = new RouteValueDictionary{{"page", "default.aspx"}};
            routes.Add(appbuilderScreenRoute);

            var restServicesStatic = from svc in CodeTorch.Core.Configuration.GetInstance().RestServices
                    where 
                    (
                        (!String.IsNullOrEmpty(svc.Resource)) &&
                        (!svc.Resource.Contains("{"))
                    )
                    orderby svc.Name
                    select svc;

            var restServicesDynamic = from svc in CodeTorch.Core.Configuration.GetInstance().RestServices
                where
                (
                    (!String.IsNullOrEmpty(svc.Resource)) &&
                    (svc.Resource.Contains("{"))
                )
                orderby svc.Name
                select svc;

            AddRestRoutes(routes, routeHandler, restServicesStatic);
            AddRestRoutes(routes, routeHandler, restServicesDynamic);


        }

        private static void AddRestRoutes(RouteCollection routes, AppBuilderRouteHandler<Page> routeHandler, IEnumerable<RestService> restServices)
        {
            foreach (RestService s in restServices)
            {
                BuildRoute(routes, routeHandler, s);
            }
        }

        private static void BuildRoute(RouteCollection routes, AppBuilderRouteHandler<Page> routeHandler, RestService s)
        {
            Route r;
            string routeUrl = null;
            string routeName = null;

            

            if (s.SupportJSON)
            {
                routeName = String.Format("{0}_{1}_json", s.Folder, s.Name);
                routeUrl = String.Format("{{folder}}/{1}.json", s.Folder, s.Resource);
                r = new Route(routeUrl, routeHandler);
                if (r.DataTokens == null)
                    r.DataTokens = new RouteValueDictionary();
                r.DataTokens["rest-service-name"] = s.Name;
                routes.Add(routeName, r);
            }

            if (s.SupportXML)
            {
                routeName = String.Format("{0}_{1}_xml", s.Folder, s.Name);
                routeUrl = String.Format("{{folder}}/{1}.xml", s.Folder, s.Resource);
                r = new Route(routeUrl, routeHandler);
                if (r.DataTokens == null)
                    r.DataTokens = new RouteValueDictionary();
                r.DataTokens["rest-service-name"] = s.Name;
                routes.Add(routeName, r);
            }

            routeName = String.Format("{0}_{1}_default", s.Folder, s.Name);
            routeUrl = String.Format("{{folder}}/{1}", s.Folder, s.Resource);
            r = new Route(routeUrl, routeHandler);
            if (r.DataTokens == null)
                r.DataTokens = new RouteValueDictionary();
            r.DataTokens["rest-service-name"] = s.Name;
            routes.Add(routeName, r);
        }


        public static string CoalesceStr(string Setting, object Value)
        {
            string retVal = Setting;

            if (Value != DBNull.Value)
            {
                if(Value != null)
                    retVal = Value.ToString();
            }

            return retVal;
        }

        public static string[] CoalesceDelimArr(string[] Setting, object Value, char Delimiter)
        {
            string[] retVal = Setting;

            if (Value != DBNull.Value)
            {
                if (Value != null)
                    retVal = Value.ToString().Split(Delimiter);
            }

            return retVal;
        }

       
       
       

        public static void LogException(Exception ex)
        {
            LogException(ex, true);
        }


        public static void LogException(Exception ex, bool rethrow)
        {

           
                ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                log.Error(ex);

                if (rethrow)
                {
                    throw ex;
                }
           


        }

        static public string Replace(string original, string pattern, string replacement, StringComparison comparisonType)
        {

            if (original == null)
            {

                return null;

            }

            if (String.IsNullOrEmpty(pattern))
            {

                return original;

            }

            int lenPattern = pattern.Length;

            int idxPattern = -1;

            int idxLast = 0;

            StringBuilder result = new StringBuilder();

            while (true)
            {

                idxPattern = original.IndexOf(pattern, idxPattern + 1, comparisonType);

                if (idxPattern < 0)
                {

                    result.Append(original, idxLast, original.Length - idxLast);

                    break;

                }

                result.Append(original, idxLast, idxPattern - idxLast);

                result.Append(replacement);

                idxLast = idxPattern + lenPattern;

            }

            return result.ToString();

        }



        public static bool HasPermission(string PermissionName)
        {
            bool retVal = false;

            try
            {
                AuthorizationService auth = AuthorizationService.GetInstance();

                retVal = auth.AuthorizationProvider.HasPermission(PermissionName);
            }
            catch (Exception ex)
            {
                LogException(ex, false);
            }

            return retVal;
        }

        public static string GetExceptionMessage(Exception ex, int LevelCount)
        {
            string retval = ex.Message;

            if ((ex.InnerException != null) && (LevelCount >= 0))
            {
                retval += "\n" + GetExceptionMessage(ex.InnerException, LevelCount - 1);
            }

            return retval;
        }

        public static string CreateUrlWithQueryStringContext(string url, string context)
        {
            string sep = "";

            if (!String.IsNullOrEmpty(url) && !String.IsNullOrEmpty(context))
            {
                string[] keys = context.Split(',');

                if (keys.Length > 0)
                {
                    if (url.IndexOf('?') == -1)
                    {
                        url += "?";

                    }
                    else
                    {
                        sep = "&";
                    }


                    for (int i = 0; i < keys.Length; i++)
                    {
                        string key = keys[i];
                        string val = HttpContext.Current.Request.QueryString[key];

                        if (val != null)
                        {
                            url += String.Format("{0}{1}={2}", sep, key, val);
                        }

                        sep = "&";
                    }
                }
            }

            return url;
        }

        public static string ReplaceText(string originalString, string oldValue, string newValue, StringComparison comparisonType)
        {
            int startIndex = 0;
            while (true)
            {
                startIndex = originalString.IndexOf(oldValue, startIndex, comparisonType);
                if (startIndex == -1)
                    break;

                originalString = originalString.Substring(0, startIndex) + newValue + originalString.Substring(startIndex + oldValue.Length);

                startIndex += newValue.Length;
            }

            return originalString;
        }

        public static object GetParameterInputValue(BasePage page, IScreenParameter parameter, Control container)
        {
            object retVal = null;
            App app = CodeTorch.Core.Configuration.GetInstance().App;
            CodeTorch.Web.FieldTemplates.BaseFieldTemplate f = null;

            switch (parameter.InputType)
            {
                case ScreenInputType.AppSetting:
                    retVal = ConfigurationManager.AppSettings[parameter.InputKey];
                    break;
                case ScreenInputType.Control:


                    if (container == null)
                    {
                        f = page.FindFieldRecursive(parameter.InputKey);
                    }
                    else
                    {
                        f = page.FindFieldRecursive(container, parameter.InputKey);
                    }

                    if (f != null)
                    {
                        retVal = f.Value;
                    }

                    break;
                case ScreenInputType.ControlText:

                    if (container == null)
                    {
                        f = page.FindFieldRecursive(parameter.InputKey);
                    }
                    else
                    {
                        f = page.FindFieldRecursive(container, parameter.InputKey);
                    }

                    if (f != null)
                    {
                        retVal = f.DisplayText;
                    }

                    break;
                case ScreenInputType.Cookie:
                    retVal = page.Request.Cookies[parameter.InputKey]?.Value;
                    break;
                case ScreenInputType.Form:
                    retVal = page.Request.Form[parameter.InputKey];
                    break;
                case ScreenInputType.Header:
                    retVal = page.Request.Headers[parameter.InputKey];
                    break;
                case ScreenInputType.QueryString:
                    retVal = page.Request.QueryString[parameter.InputKey];
                    break;

                case ScreenInputType.Session:
                    retVal = page.Session[parameter.InputKey];
                    break;
                case ScreenInputType.Special:
                    switch (parameter.InputKey.ToLower())
                    {

                        case "null":
                            retVal = null;
                            break;
                        case "dbnull":
                            retVal = DBNull.Value;
                            break;
                        case "username":
                            retVal = UserIdentityService.GetInstance().IdentityProvider.GetUserName();
                            break;
                        case "hostheader":
                            retVal = HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
                            break;
                        case "applicationpath":
                            retVal = HttpContext.Current.Request.ApplicationPath;
                            break;
                        case "grid.selecteditems":

                            if (!String.IsNullOrEmpty(parameter.Default))
                            {
                                var tokens = parameter.Default.Split('.');
                                if (tokens.Length == 3)
                                {
                                    string sectionId = tokens[0];
                                    string columnUniqueName = tokens[1];
                                    string DataKeyName = tokens[2];

                                    var sectionControl = FindSection(page, null, sectionId);
                                    if (sectionControl != null)
                                    {
                                        if (sectionControl is GridSectionControl)
                                        {
                                            var gridSectionControl = sectionControl as GridSectionControl;
                                            retVal = gridSectionControl.GetSelectedDataKeyValues(columnUniqueName, DataKeyName);
                                        }

                                        if (sectionControl is EditableGridSectionControl)
                                        {
                                            var editableGridSectionControl = sectionControl as EditableGridSectionControl;
                                            retVal = editableGridSectionControl.GetSelectedDataKeyValues(columnUniqueName, DataKeyName);
                                        }

                                    }
                                }
                                else
                                {
                                    //this is an incorrectly configured parameter - throw error to help developer
                                    throw new ApplicationException($"{parameter.Name} parameter is not configured correctly for special.grid.selecteditems input. It requires a default value in the following format 'gridsectionid.uniquecolumnname.datakey'.");
                                }
                            }
                            else
                            {
                                //this is an incorrectly configured parameter - throw error to help developer
                                throw new ApplicationException($"{parameter.Name} parameter is not configured correctly for special.grid.selecteditems input. It requires a default value in the following format 'gridsectionid.uniquecolumnname.datakey'.");
                            }

                            

                            break;
                        case "absoluteapplicationpath":
                            retVal = String.Format("{0}://{1}{2}",
                                HttpContext.Current.Request.Url.Scheme,
                                HttpContext.Current.Request.ServerVariables["HTTP_HOST"],
                                ((HttpContext.Current.Request.ApplicationPath == "/") ? String.Empty : HttpContext.Current.Request.ApplicationPath));

                            break;

                    }
                    break;

                case ScreenInputType.User:
                    try
                    {

                        retVal = GetProfileProperty(parameter.InputKey);
                        
                    }
                    catch { }
                    break;
                case ScreenInputType.Constant:
                    retVal = parameter.InputKey;
                    break;
                case ScreenInputType.ServerVariables:
                    retVal = HttpContext.Current.Request.ServerVariables[parameter.InputKey];
                    break;
            }

            if (parameter.InputType != ScreenInputType.Special)
            {
                if (retVal == null)
                {
                    retVal = parameter.Default;
                }
            }

            return retVal;
        }

        


    }
}
