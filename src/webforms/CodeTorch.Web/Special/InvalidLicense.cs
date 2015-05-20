using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeTorch.Web.Data;
using System.Web.Security;
using System.Management;

namespace CodeTorch.Web.Templates
{
    public class InvalidLicense: BasePage
    {
        protected Label ComputerName;
        protected Label LicenseID;

        public override bool PerformScreenAuthenticationCheck()
        {
            return false;
        }

        

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            this.Title = "Invalid License";
            this.SubTitle = "";

            this.ComputerName.Text = System.Environment.MachineName;

            //Get motherboard's serial number
            ManagementObjectCollection mbsList = null;
            ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select * From Win32_BaseBoard");
            mbsList = mbs.Get();
            foreach (ManagementObject mo in mbsList)
            {
                this.LicenseID.Text = mo["SerialNumber"].ToString();
            }

        }
    }
}
