using CodeTorch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace CodeTorch.Security.Identity
{
    public class FormsUserIdentityProvider : IUserIdentityProvider
    {
        public void Initialize(string config)
        {
            
        }

        public string GetUserName()
        {
            return HttpContext.Current.User.Identity.Name;
        }
    }
}
