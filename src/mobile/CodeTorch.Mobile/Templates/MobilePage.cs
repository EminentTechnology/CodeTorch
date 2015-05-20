using CodeTorch.Core;
using CodeTorch.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CodeTorch.Mobile.Templates
{
    public class MobilePage
    {
        public static IMobilePage    GetPage(string pageName)
        {
            IMobilePage  retVal = null;

            MobileScreen screen = MobileScreen.GetByName(pageName);

            switch (screen.Type.ToLower())
            { 
                case "mobilecontent":
                    retVal = new MobileContentPage(screen);
                    break;
                case "mobiletabbed":
                    retVal = new MobileTabbedPage(screen);
                    break;
                case "mobilenavigation":
                    Page root = GetPage(((MobileNavigationScreen)screen).GetRootScreen()).GetPage();
                    retVal = new MobileNavigationPage(screen, root);
                    break;
            }

            if (retVal != null)
            {
                string loginScreen = Configuration.GetInstance().App.LoginScreen;

                if (!String.IsNullOrEmpty(loginScreen))
                {
                    if (retVal.Screen != null)
                    {
                        if (retVal.Screen.RequireAuthentication)
                        {
                            UserIdentityService identity = UserIdentityService.GetInstance();
                            string userName = identity.IdentityProvider.GetUserName();

                            if (String.IsNullOrEmpty(userName))
                            {

                                retVal = GetPage(loginScreen);
                            }

                        }
                    }
                }
            }

            return retVal;
        }

    }
}
