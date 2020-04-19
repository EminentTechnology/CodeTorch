using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;


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
