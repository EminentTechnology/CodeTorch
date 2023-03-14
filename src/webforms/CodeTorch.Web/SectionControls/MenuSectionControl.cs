using System;
using System.Web;
using System.Web.UI;
using CodeTorch.Core;
using CodeTorch.Web.Templates;

namespace CodeTorch.Web.SectionControls
{
    public class MenuSectionControl: BaseSectionControl
    {
        CodeTorch.Web.Controls.Menu ctrl;

        MenuSection _Me = null;
        public MenuSection Me
        {
            get
            {
                if ((_Me == null) && (this.Section != null) && (this.Section is MenuSection))
                {
                    _Me = (MenuSection)this.Section;
                }
                return _Me;
            }
        }

        BasePage page = null;


        public override void RenderControl()
        {
            page = HttpContext.Current.Handler as BasePage;
            base.RenderControl();

            ctrl = new CodeTorch.Web.Controls.Menu();
            ctrl.MenuType = Me.MenuType;
            ctrl.MenuName = Me.Menu;
           
            this.ContentPlaceHolder.Controls.Add(ctrl);  
        }
    }
}