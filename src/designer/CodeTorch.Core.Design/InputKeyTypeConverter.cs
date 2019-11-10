using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core.Design
{
    class InputKeyTypeConverter : StringConverter
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

            if (context.Instance is IScreenParameter) 
            {
                IScreenParameter parameter = (IScreenParameter)context.Instance;

                if (parameter.InputType == ScreenInputType.Special)
                {
                    List<string> listItems = GetSpecialInputKeys();
                    

                    string[] items = listItems.ToArray<string>();

                    list = new StandardValuesCollection(items);
                }

                if (parameter.InputType == ScreenInputType.User)
                {
                    string[] items = Configuration.GetInstance().App.ProfileProperties.ToArray<string>();

                    list = new StandardValuesCollection(items);
                }

            }

            return list;


        }

        private static List<string> GetSpecialInputKeys()
        {
            List<string> listItems = new List<string>();

            listItems.Add("AbsoluteApplicationPath");
            listItems.Add("ApplicationPath");

            listItems.Add("ControlValue");
            listItems.Add("ControlText");
            listItems.Add("DBNull");
            listItems.Add("Grid.SelectedItems");
            listItems.Add("HostHeader");
            listItems.Add("IPAddress");
            listItems.Add("NewID");
            listItems.Add("Null");
            listItems.Add("RelatedControlValue");
            listItems.Add("UrlSegment");
            listItems.Add("UserName");
            return listItems;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}
