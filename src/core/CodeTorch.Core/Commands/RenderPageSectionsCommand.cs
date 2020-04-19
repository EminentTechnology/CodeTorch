using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CodeTorch.Core.Commands
{
    [Serializable]
    public class RenderPageSectionsCommand : ActionCommand
    {

        ScreenInputType _EntityInputType = ScreenInputType.QueryString;
       

        public override string Type
        {
            get
            {
                return "RenderPageSectionsCommand";
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

        public SectionRenderMode Mode { get; set; }


        public enum SectionRenderMode
        {
            Simple,
            InsertEdit
        }

        

    }
}
