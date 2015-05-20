using CodeTorch.Core.Interfaces;
using CodeTorch.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Mobile.Security.Identity
{
    public class SettingsTokenIdentityProvider: IUserIdentityProvider
    {
        private string token = "Token";

        public void Initialize(string config)
        {
           
        }

        public string GetUserName()
        {
            string retVal = String.Empty;

            ISettings settings = SettingService.GetInstance().Provider;

            retVal = settings.GetValue<string>(token, String.Empty);

            return retVal;
        }
    }
}
