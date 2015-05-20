using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core.Design
{
    public class SectionZoneTypeConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            //this means a standard list of values are supported
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            //the actual list of standard items to return
            StandardValuesCollection list = null;
             SectionZoneLayout layout = null;


            if (context.Instance is BaseSection)
            {
                BaseSection section = (BaseSection)context.Instance;
               

                if (section.Parent != null)
                {
                    Screen screen = (Screen)section.Parent;
                    layout = SectionZoneLayout.GetByName(screen.SectionZoneLayout);
                }
                


            }

            if (layout == null)
            {
                if (!String.IsNullOrEmpty(Configuration.GetInstance().App.DefaultZoneLayout))
                {
                    layout = SectionZoneLayout.GetByName(Configuration.GetInstance().App.DefaultZoneLayout);
                }

                
            }

            if (layout != null)
            {
                var retVal = from item in layout.Dividers
                             where String.IsNullOrEmpty(item.Name) == false
                             select item.Name;

                var tempList = retVal.ToList<String>();
                tempList.Insert(0, String.Empty);

                string[] items = tempList.ToArray<string>();
                list = new StandardValuesCollection(items);

            }



            return list;


        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}
