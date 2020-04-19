using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CodeTorch.Core.Commands
{
    [Serializable]
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

        [Category("Entity")]
        public string EntityID { get; set; }



        [Category("Entity")]
        public ScreenInputType EntityInputType
        {
            get { return _EntityInputType; }
            set { _EntityInputType = value; }
        }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.ScreenDataCommandTypeConverter,CodeTorch.Core.Design")]
        public string InsertCommand { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.ScreenDataCommandTypeConverter,CodeTorch.Core.Design")]
        public string UpdateCommand { get; set; }

       

        

        public string AfterInsertConfirmationMessage { get { return _AfterInsertConfirmationMessage; } set { _AfterInsertConfirmationMessage = value; } }
        public string AfterUpdateConfirmationMessage { get { return _AfterUpdateConfirmationMessage; } set { _AfterUpdateConfirmationMessage = value; } }

        public bool RedirectAfterInsert { get { return _RedirectAfterInsert; } set { _RedirectAfterInsert = value; } }
        public bool RedirectAfterUpdate { get { return _RedirectAfterUpdate; } set { _RedirectAfterUpdate = value; } }

        public string AfterUpdateRedirectUrl { get; set; }
        public string AfterInsertRedirectUrl { get; set; }


        public string AfterUpdateRedirectUrlContext { get; set; }
        public string AfterInsertRedirectUrlContext { get; set; }

       

    }
}
