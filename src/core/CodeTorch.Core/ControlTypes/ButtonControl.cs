using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public class ButtonControl: Widget
    {

        public override string Type
        {
            get
            {
                return "Button";
            }
            set
            {
                base.Type = value;
            }
        }

        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        [Category("Data")]
        public string DataTextField { get; set; }

        [Category("Data")]
        public string DataTextFormatString { get; set; }

        public string Text { get; set; }
    
        public string CommandName { get; set; }
        public string CommandArgument { get; set; }

        public string OnClientClick { get; set; }
        public bool CausesValidation { get; set; }

 

        [Category("Common")]
        public string Context { get; set; }

        

        private Action _OnClick = new Action();

        [Category("Actions")]
        public Action OnClick
        {
            get
            {
                return _OnClick;
            }
            set
            {
                _OnClick = value;
            }

        }

        public override string ToString()
        {
            string retVal = Text;

            if (!Visible)
            {
                retVal = "Not Displayed";
            }
            else
            {
                if (String.IsNullOrEmpty(Text))
                    retVal = "No Button Text";
            }

            return retVal;
        }

        public static IEnumerable<ResourceItem> GetResourceKeys(Screen Screen, LabelControl Control, String Prefix)
        {
            List<ResourceItem> retVal = new List<ResourceItem>();

            //add title
            //AddResourceKey(retVal, Screen, Control, Prefix, "FormatString", Control.FormatString);



            return retVal;
        }
    }
}
