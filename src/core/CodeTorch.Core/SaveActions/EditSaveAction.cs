using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class EditSaveAction
    {
        bool _RedirectAfterInsert = true;
        bool _RedirectAfterUpdate = false;
        bool _RedirectAfterCancel = false;

        string _AfterInsertConfirmationMessage = "Record was added successfully";
        string _AfterUpdateConfirmationMessage = "Record was updated successfully";
        string _AfterCancelConfirmationMessage = String.Empty;

        public string AfterInsertConfirmationMessage { get { return _AfterInsertConfirmationMessage; } set { _AfterInsertConfirmationMessage = value; } }
        public string AfterUpdateConfirmationMessage { get { return _AfterUpdateConfirmationMessage; } set { _AfterUpdateConfirmationMessage = value; } }
        public string AfterCancelConfirmationMessage { get { return _AfterCancelConfirmationMessage; } set { _AfterCancelConfirmationMessage = value; } }

        public bool RedirectAfterInsert { get { return _RedirectAfterInsert; } set { _RedirectAfterInsert = value; } }
        public bool RedirectAfterUpdate { get { return _RedirectAfterUpdate; } set { _RedirectAfterUpdate = value; } }
        public bool RedirectAfterCancel { get { return _RedirectAfterCancel; } set { _RedirectAfterCancel = value; } }
        
        public string AfterUpdateRedirectUrl { get; set; }
        public string AfterInsertRedirectUrl { get; set; }
        public string AfterCancelRedirectUrl { get; set; }

        public string AfterUpdateRedirectUrlContext { get; set; }
        public string AfterInsertRedirectUrlContext { get; set; }
        public string AfterCancelRedirectUrlContext { get; set; }


        public override string ToString()
        {
            string retVal = "";
            
            if(RedirectAfterInsert || RedirectAfterUpdate)
            {
                retVal = "Redirects provided";
            }
            else{
                if ((String.IsNullOrEmpty(AfterInsertConfirmationMessage)) && (String.IsNullOrEmpty(AfterUpdateConfirmationMessage)))
                {
                    retVal = "No confirmation or redirect provided";
                }
                else
                {
                    retVal = "Confirmation provided";
                }
            }

            return retVal;
        }


    }
}
