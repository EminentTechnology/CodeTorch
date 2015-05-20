using System;
using System.Linq;

namespace CodeTorch.Core.Commands
{

    public class InsertUpdateSaveCommand: ActionCommand
    {
        ScreenInputType _EntityInputType = ScreenInputType.QueryString;

        bool _RedirectAfterInsert = true;
        bool _RedirectAfterUpdate = false;

        string _AfterInsertConfirmationMessage = "Record was added successfully";
        string _AfterUpdateConfirmationMessage = "Record was updated successfully";

        public override string Type
        {
            get
            {
                return "InsertUpdateSaveCommand";
            }
            set
            {
                base.Type = value;
            }
        }

 
        public string EntityID { get; set; }



  
        public ScreenInputType EntityInputType
        {
            get { return _EntityInputType; }
            set { _EntityInputType = value; }
        }

 
        public string InsertCommand { get; set; }


        public string UpdateCommand { get; set; }


        public string AfterInsertSettings { get; set; }
        public string AfterUpdateSettings { get; set; }
        
        public string AfterInsertConfirmationMessage { get { return _AfterInsertConfirmationMessage; } set { _AfterInsertConfirmationMessage = value; } }
        public string AfterUpdateConfirmationMessage { get { return _AfterUpdateConfirmationMessage; } set { _AfterUpdateConfirmationMessage = value; } }

        public bool RedirectAfterInsert { get { return _RedirectAfterInsert; } set { _RedirectAfterInsert = value; } }
        public bool RedirectAfterUpdate { get { return _RedirectAfterUpdate; } set { _RedirectAfterUpdate = value; } }

        public string AfterUpdateRedirectScreen { get; set; }
        public string AfterInsertRedirectScreen { get; set; }


        public string AfterUpdateRedirectUrlContext { get; set; }
        public string AfterInsertRedirectUrlContext { get; set; }

       

    }
}
