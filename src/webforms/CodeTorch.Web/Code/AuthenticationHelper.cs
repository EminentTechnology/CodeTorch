using CodeTorch.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace CodeTorch.Web
{
    public class AuthenticationHelper
    {

        public static HttpCookie CreateFormAuthenticationCookie(string UserName, DateTime TicketExpirationDate, bool RememberMe, string FormsAuthenticationCookieName, string FormsAuthenticationCookiePath, DataRow profile)
        {
            return CreateFormAuthenticationCookie(UserName, TicketExpirationDate, RememberMe, FormsAuthenticationCookieName, FormsAuthenticationCookiePath, profile, CodeTorch.Core.Configuration.GetInstance().App.ProfileProperties);
        }

        public static HttpCookie CreateFormAuthenticationCookie(string UserName, DateTime TicketExpirationDate, bool RememberMe, string FormsAuthenticationCookieName, string FormsAuthenticationCookiePath, DataRow profile, List<String> profileProperties)
        {
            HttpCookie retVal = null;
            string profileData = null;

            if (profile != null)
            {
                profileData = FormsAuthenticationMode.BuildProfileString(profile, profileProperties);
            }

            FormsAuthenticationTicket tkt;
            string cookiestr;

            tkt = new FormsAuthenticationTicket(1, UserName, DateTime.Now,
                    TicketExpirationDate, RememberMe, profileData);
            cookiestr = FormsAuthentication.Encrypt(tkt);
            retVal = new HttpCookie(FormsAuthenticationCookieName, cookiestr);
            retVal.Path = FormsAuthenticationCookiePath;

            return retVal;
        }

    }
}
