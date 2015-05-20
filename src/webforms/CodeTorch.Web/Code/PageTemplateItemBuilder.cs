using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeTorch.Core;
using System.Web.UI;
using System.Runtime.Remoting;
using System.Reflection;

namespace CodeTorch.Web
{
    public class PageTemplateItemBuilder
    {
        PageTemplateItem _item = null;
        Page _page = null;

        public PageTemplateItemBuilder(Page page, PageTemplateItem item)
        {
            _page = page;
            _item = item;
        }

        public void BuildPageTemplateItem(Control container)
        {

            Control ctrl = LoadControle(_item);

            if (ctrl != null)
            {
                container.Controls.Add(ctrl);
            }
        }

        private Control LoadControle(PageTemplateItem controlType)
        {
            Control ctrl = null;

            if (!String.IsNullOrEmpty(controlType.Assembly) && !String.IsNullOrEmpty(controlType.Class))
            {
                var instance = Activator.CreateInstance(controlType.Assembly, controlType.Class);
                ctrl = (Control)instance.Unwrap();

                if (!String.IsNullOrEmpty(controlType.Property) && !String.IsNullOrEmpty(controlType.Value))
                {
                    PropertyInfo[] properties = ctrl.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    foreach (var property in properties)
                    {
                        if (property.Name.ToLower() == controlType.Property.ToLower())
                        {

                            property.SetValue(ctrl, controlType.Value, null);
                        }
                    }
                }

                
            }
            else
            {
                if (!String.IsNullOrEmpty(controlType.Path))
                {
                    ctrl = _page.LoadControl(controlType.Path);
                }
            }

            return ctrl;
        }
    }
}
