using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public class EmailAddressControl: BaseControl
    {

        public override string Type
        {
            get
            {
                return "EmailAddress";
            }
            set
            {
                base.Type = value;
            }
        }

        public static IEnumerable<ResourceItem> GetResourceKeys(Screen Screen, EmailAddressControl Control, String Prefix)
        {
            List<ResourceItem> retVal = new List<ResourceItem>();

            //add title
            AddResourceKey(retVal, Screen, Control, Prefix, "EmailAddress.Validation.ErrorMessage", "Please enter a valid email address");
            



            return retVal;
        }
    }
}
