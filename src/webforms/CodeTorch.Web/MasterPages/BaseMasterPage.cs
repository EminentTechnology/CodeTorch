using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Reflection;
using System.Web.UI;

namespace CodeTorch.Web.MasterPages
{
    public class BaseMasterPage : System.Web.UI.MasterPage
    {
        protected Label currentYear;
        protected Label currentAppVersion;
        protected Label copyrightCompanyName;


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (currentYear != null)
                this.currentYear.Text = DateTime.Now.Year.ToString();

            if (currentAppVersion != null)
                this.currentAppVersion.Text = GetCurrentVersion();

            if (copyrightCompanyName != null)
                this.copyrightCompanyName.Text = CodeTorch.Core.Configuration.GetInstance().App.CopyrightCompanyName;

        }
      

        protected string GetCurrentVersion()
        {
            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            Version appVersion = currentAssembly.GetName().Version;
            return string.Format("{0}.{1}.{2}", appVersion.Major.ToString(), appVersion.Minor.ToString(), appVersion.Build.ToString());
        }

       

    }
}
