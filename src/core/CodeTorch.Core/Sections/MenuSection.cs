using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace CodeTorch.Core
{
    [Serializable]
    public class MenuSection : Section
    {
        public override string Type
        {
            get
            {
                return "Menu";
            }
            set
            {
                base.Type = value;
            }
        }

        [Description("Primary or Secondary menu")]
        [TypeConverter("CodeTorch.Core.Design.MenuTypeTypeConverter,CodeTorch.Core.Design")]
        [Category("Menu")]
        public string MenuType { get; set; }

        [Description("Menu to load")]
        [TypeConverter("CodeTorch.Core.Design.MenuTypeConverter,CodeTorch.Core.Design")]
        [Category("Menu")]
        public string Menu { get; set; }

        #region Hidden Overrides
        [XmlArray("Widgets")]
        [Category("Widgets")]
        [Description("List of section widgets")]
        [Browsable(false)]
        public override List<Widget> Widgets
        {
            get
            {
                return base.Widgets;
            }
            set
            {
                base.Widgets = value;
            }
        }
        #endregion
    }
}
