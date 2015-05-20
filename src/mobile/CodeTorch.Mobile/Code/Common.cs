using CodeTorch.Core;
using CodeTorch.Core.Interfaces;
using CodeTorch.Core.Services;
using CodeTorch.Mobile.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CodeTorch.Mobile
{
    public class Common
    {
        public static void LoadMobileConfiguration(string assemblyName)
        {


            CodeTorch.Core.ConfigurationLoader.LoadMobileConfiguration(assemblyName);



            




        }

        internal static object GetParameterInputValue(Templates.IMobilePage page, Core.ScreenDataCommandParameter parameter)
        {
            object retVal = null;
            App app = CodeTorch.Core.Configuration.GetInstance().App;
            Element e = null;

            switch (parameter.InputType)
            {
                case ScreenInputType.AppSetting:
                   // retVal = ConfigurationManager.AppSettings[parameter.InputKey];
                    break;
                case ScreenInputType.Control:

                    if(page.Elements.ContainsKey(parameter.InputKey))
                    {
                        e = page.Elements[parameter.InputKey];
                    }

                    if (e != null)
                    {
                        if (e is IView)
                        {
                            retVal = (e as IView).Value;
                        }
                    }

                    break;
                case ScreenInputType.ControlText:
                    if(page.Elements.ContainsKey(parameter.InputKey))
                    {
                        e = page.Elements[parameter.InputKey];
                    }

                    if (e != null)
                    {
                        if (e is IView)
                        {
                            retVal = (e as IView).Value;
                            //retVal = (e as IView).Text;
                        }
                    }

                    break;
                case ScreenInputType.Cookie:
                    //retVal = page.Request.Cookies[parameter.InputKey].Value;
                    //no mobile concept
                    break;
                case ScreenInputType.Form:
                    //retVal = page.Request.Form[parameter.InputKey];
                    //no mobile concept
                    break;
                case ScreenInputType.QueryString:
                    //retVal = page.Request.QueryString[parameter.InputKey];
                    //no mobile concept
                    break;

                case ScreenInputType.Session:
                    //retVal = page.Session[parameter.InputKey];
                    //no mobile concept
                    break;
                case ScreenInputType.Special:
                    switch (parameter.InputKey.ToLower())
                    {

                        case "null":
                            retVal = null;
                            break;
                        //case "dbnull":
                        //    retVal = DBNull.Value;
                        //    break;
                        //case "username":

                        //    retVal = UserIdentityService.GetInstance().IdentityProvider.GetUserName();

                        //    break;
                        //case "hostheader":
                        //    retVal = HttpContext.Current.Request.ServerVariables["HTTP_HOST"];
                        //    break;
                        //case "applicationpath":
                        //    retVal = HttpContext.Current.Request.ApplicationPath;
                        //    break;
                        //case "absoluteapplicationpath":
                        //    retVal = String.Format("{0}://{1}{2}",
                        //        HttpContext.Current.Request.Url.Scheme,
                        //        HttpContext.Current.Request.ServerVariables["HTTP_HOST"],
                        //        ((HttpContext.Current.Request.ApplicationPath == "/") ? String.Empty : HttpContext.Current.Request.ApplicationPath));

                        //    break;

                    }
                    break;

                //case ScreenInputType.User:
                //    try
                //    {

                //        retVal = GetProfileProperty(parameter.InputKey);
                        
                //    }
                //    catch { }
                //    break;
                case ScreenInputType.Constant:
                    retVal = parameter.InputKey;
                    break;
                //case ScreenInputType.ServerVariables:
                //    retVal = HttpContext.Current.Request.ServerVariables[parameter.InputKey];
                //    break;
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


        public static void DataRowToSettings(DataRow row, string columns)
        {
            string[] settings_to_store = columns.Split(';');

            ISettings settings = SettingService.GetInstance().Provider;

            foreach (string s in settings_to_store)
            {
                string[] items = s.Split('=');

                if (items.Length == 2)
                {
                    
                    string key = items[0].Trim();
                    object val = row[items[1].Trim()];

                    if (val != null)
                    {
                        settings.AddOrUpdateValue<object>(key, val);
                    }
                }
            }
        }

        public static void RemoveSettings(string columns)
        {
            string[] settings_to_remove = columns.Split(',');
            ISettings settings = SettingService.GetInstance().Provider;

            foreach (string s in settings_to_remove)
            {

                settings.DeleteValue(s);
            }
        }

        public static List<ScreenDataCommandParameter> GetPopulatedCommandParameters(string DataCommandName, IMobilePage page)
        {
            return GetPopulatedCommandParameters(DataCommandName, page, page.Screen.DataCommands);
        }

       

        public static  List<ScreenDataCommandParameter> GetPopulatedCommandParameters(string DataCommandName, IMobilePage page, List<ScreenDataCommand> datacommands)
        {
            string ErrorFormat = "Invalid {0} propery for Data Command {1} Parameter {2} - {3}";
            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();

            ScreenDataCommand screenCommand = ScreenDataCommand.GetDataCommand(datacommands, DataCommandName);
            if ((screenCommand != null) && (screenCommand.Parameters != null))
            {
                


                foreach (ScreenDataCommandParameter existingP in screenCommand.Parameters)
                {
                    ScreenDataCommandParameter p = existingP.Clone();
                    try
                    {
                        

                        p.Value = Common.GetParameterInputValue(page, p);

                        parameters.Add(p);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(
                            String.Format(ErrorFormat, "Value", DataCommandName, p.Name, ex.Message),
                            ex);
                    }

                }
            }




            return parameters;

        }
        
    }
}
