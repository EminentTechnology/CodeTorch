using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace CodeTorch.Core
{

    public class ScrollViewControl: BaseControl
    {

        public override string Type
        {
            get
            {
                return "ScrollView";
            }
            set
            {
                base.Type = value;
            }
        }

        public ScrollOrientation Orientation = ScrollOrientation.Vertical;

    }
}
