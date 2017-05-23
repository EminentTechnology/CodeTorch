using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    public enum RepeatDirectionMode
    {
        Horizontal = 0,
        Vertical = 1
    }

    [Serializable]
    public class RadioButtonListControl : Widget
    {
        public override string Type
        {
            get
            {
                return "RadioButtonList";
            }
            set
            {
                base.Type = value;
            }
        }

        public bool AutoPostBack { get; set; }

        public bool CausesValidation { get; set; }

        public string EmptyMessage { get; set; }
    

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.ScreenDataCommandTypeConverter,CodeTorch.Core.Design")]
        public string SelectDataCommand { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string DataTextField { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string DataValueField { get; set; }

        [Category("Appearance")]
        public string Skin { get; set; }

        private Action _SelectedIndexChanged = new Action();

        public RepeatDirectionMode RepeatDirection { get; set; }

        [Category("Actions")]
        public Action SelectedIndexChanged
        {
            get
            {
                return _SelectedIndexChanged;
            }
            set
            {
                _SelectedIndexChanged = value;
            }

        }

        public static IEnumerable<ResourceItem> GetResourceKeys(Screen Screen, DropDownListControl Control, String Prefix)
        {
            List<ResourceItem> retVal = new List<ResourceItem>();

            //add title
            AddResourceKey(retVal, Screen, Control, Prefix, "EmptyMessage", Control.EmptyMessage);
            AddResourceKey(retVal, Screen, Control, Prefix, "AdditionalListItemText", Control.AdditionalListItemText);


            return retVal;
        }

    }
}
