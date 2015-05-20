using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ListEditSaveAction
    {
        public string AfterInsertConfirmationMessage { get; set; }
        public string AfterUpdateConfirmationMessage { get; set; }

        public override string ToString()
        {
            string retVal = "";
            
            if((String.IsNullOrEmpty(AfterInsertConfirmationMessage)) &&  (String.IsNullOrEmpty(AfterUpdateConfirmationMessage)))
            {
                retVal = "No confirmation provided";
            }
            else
            {
                retVal = "Confirmation provided";
            }

            return retVal;
        }
    }
}
