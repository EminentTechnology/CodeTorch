using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core.Design
{
    class MenuTypeTypeConverter : StringConverter
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

            List<string> listItems = new List<string>();

            listItems.Add("Primary");
            listItems.Add("Secondary");

            string[] items = listItems.ToArray<string>();

            list = new StandardValuesCollection(items);

            return list;


        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
