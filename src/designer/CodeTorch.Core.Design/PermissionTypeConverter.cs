using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core.Design
{
    public class PermissionTypeConverter: StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            //this means a standard list of values are supported
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            //the actual list of standard items to return

            var retVal = from permission in Configuration.GetInstance().Permissions
                         select permission.Name;

            var list = retVal.ToList<String>();
            list.Insert(0, String.Empty);

            string[] items = list.ToArray<string>();

            return new StandardValuesCollection(items);
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            
            return false;
        }
    }
}
