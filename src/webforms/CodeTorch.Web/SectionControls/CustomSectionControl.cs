using System;
using System.Web;
using System.Web.UI;
using CodeTorch.Core;
using CodeTorch.Web.Templates;

namespace CodeTorch.Web.SectionControls
{
    public class CustomSectionControl: BaseSectionControl
    {
        Control ctrl;

        CustomSection _Me = null;
        public CustomSection Me
        {
            get
            {
                if ((_Me == null) && (this.Section != null) && (this.Section is CustomSection))
                {
                    _Me = (CustomSection)this.Section;
                }
                return _Me;
            }
        }

        BasePage page = null;


        public override void RenderControl()
        {
            page = HttpContext.Current.Handler as BasePage;
            base.RenderControl();



           
            if (!String.IsNullOrEmpty(Me.Assembly) && !String.IsNullOrEmpty(Me.Class))
            {
                //attempt to load dynamic web control
                ctrl = (Control)Activator.CreateInstance(Me.Assembly, Me.Class).Unwrap();
            }
            else
            {
                //attempt to load dynamic user control
                ctrl = page.LoadControl(Me.UserControlPath);
            }
            this.ContentPlaceHolder.Controls.Add(ctrl);
            
        }

     

        

      

    }
}
