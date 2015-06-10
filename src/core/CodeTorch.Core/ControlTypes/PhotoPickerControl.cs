using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public class PhotoPickerControl: Widget
    {
        public string EntityID { get; set; }
        public string EntityType { get; set; }


        public override string Type
        {
            get
            {
                return "PhotoPicker";
            }
            set
            {
                base.Type = value;
            }
        }
    }
}
