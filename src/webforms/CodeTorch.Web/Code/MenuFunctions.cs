using CodeTorch.Core;
using CodeTorch.Web.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;


namespace CodeTorch.Web
{
    public class MenuFunctions
    {
        public static string GetMenuItemText(App app, Page page, string ResourceSet, string resourceKeyPrefix, string menuItemCode, string menuItemText)
        {
            string retVal = menuItemText;

            if (app.EnableLocalization)
            {
                if (page is BasePage)
                {

                    string resourcekey = String.Format("{0}{1}{2}.MenuItem.Name", resourceKeyPrefix, ((String.IsNullOrEmpty(resourceKeyPrefix)) ? "" : "."), menuItemCode);

                    retVal = ((BasePage)page).GetGlobalResourceString(ResourceSet, resourcekey, menuItemText);
                }

            }

            return retVal;
        }
    }
}
