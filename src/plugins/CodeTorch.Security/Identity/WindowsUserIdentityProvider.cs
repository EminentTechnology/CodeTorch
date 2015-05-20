using CodeTorch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CodeTorch.Security.Identity
{
    public enum WindowsAuthenticationUserNameFormat
    { 
        UserNameOnly = 0,
        FullUserName
    }

    public class WindowsUserIdentityProvider: IUserIdentityProvider
    {
        WindowsAuthenticationUserNameFormat UserNameFormat = WindowsAuthenticationUserNameFormat.UserNameOnly;
        
        public void Initialize(string config)
        {
            //determine username format     
        }

        public string GetUserName()
        {
            string retVal = HttpContext.Current.User.Identity.Name;
            
            if (UserNameFormat == WindowsAuthenticationUserNameFormat.UserNameOnly)
            {
                int separator = retVal.ToString().LastIndexOf("\\");

                if (separator > 0)
                {
                    retVal = retVal.ToString().Substring(separator + 1);
                }
            }

            return retVal;
        }
    }
}
