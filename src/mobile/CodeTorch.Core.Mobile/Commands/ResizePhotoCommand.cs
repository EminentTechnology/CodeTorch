using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace CodeTorch.Core.Commands
{
#if !( PORTABLE)
    [Serializable]
#endif
    public class ResizePhotoCommand : ActionCommand
    {

        ScreenInputType _MaxHeightInputType = ScreenInputType.QueryString;
        ScreenInputType _MaxWidthInputType = ScreenInputType.QueryString;
       

        public override string Type
        {
            get
            {
                return "ResizePhotoCommand";
            }
            set
            {
                base.Type = value;
            }
        }


        

        public string RetrieveCommand { get; set; }

        public bool UseDiskCache { get; set; }
        public string DishCacheFolderPath { get; set; }
        


        [Category("Constraints")]
        public ScreenInputType MaxHeightInputType
        {
            get { return _MaxHeightInputType; }
            set { _MaxHeightInputType = value; }
        }

        [Category("Constraints")]
        public string MaxHeight { get; set; }



        [Category("Constraints")]
        public ScreenInputType MaxWidthInputType
        {
            get { return _MaxWidthInputType; }
            set { _MaxWidthInputType = value; }
        }

        [Category("Constraints")]
        public string MaxWidth { get; set; }
        



        

    }
}
