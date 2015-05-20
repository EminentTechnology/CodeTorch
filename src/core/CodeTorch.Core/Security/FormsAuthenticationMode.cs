using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Web.Security;

namespace CodeTorch.Core
{
    [Serializable]
    public class FormsAuthenticationMode : BaseAuthenticationMode
    {
        public override string Type
        {
            get
            {
                return "Forms";
            }
            set
            {
                base.Type = value;
            }
        }

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
                profileData = BuildProfileString(profile, profileProperties);
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

        public static string BuildProfileString(DataRow profile)
        {

            return BuildProfileString(profile, CodeTorch.Core.Configuration.GetInstance().App.ProfileProperties);
            


        }

        public static string BuildProfileString(DataRow profile, List<String> properties)
        {
            System.Text.StringBuilder token = new System.Text.StringBuilder();

            foreach (string item in properties)
            {
                string itemValue = String.Empty;

                try
                {
                    itemValue = profile[item].ToString();
                }
                catch { }

                token.Append(itemValue);
                token.Append('|');
            }

            return token.ToString();


        }
    }
}
