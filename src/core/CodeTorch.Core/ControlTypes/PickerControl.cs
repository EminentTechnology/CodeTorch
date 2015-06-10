using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    public class PickerControl: Widget
    {
        [TypeConverter("CodeTorch.Core.Design.PickerTypeConverterConverter,CodeTorch.Core.Design")]
        public string Picker { get; set; }

        public bool AutoPostBack { get; set; }
        public bool Modal { get; set; }


        public override string Type
        {
            get
            {
                return "Picker";
            }
            set
            {
                base.Type = value;
            }
        }

        Picker _PickerObject = null;
        [XmlIgnore()]
        [Browsable(false)]
        public Picker PickerObject
        {
            get
            {
                if ((!String.IsNullOrEmpty(Picker)) && (_PickerObject == null))
                {
                    _PickerObject = CodeTorch.Core.Picker.GetPicker(this.Picker);

                    if (_PickerObject == null)
                    {
                        throw new ApplicationException(String.Format("Picker with name '{0}' does not exist", Picker));
                    }
                }
                return _PickerObject;
            }
        }

        public static IEnumerable<ResourceItem> GetResourceKeys(Screen Screen, PickerControl Control, String Prefix)
        {
            List<ResourceItem> retVal = new List<ResourceItem>();

            //add title
            AddResourceKey(retVal, Screen, Control, Prefix, "ResetButton.Label", "Reset");




            return retVal;
        }
        
    }
}
