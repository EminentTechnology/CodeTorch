using CodeTorch.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Linq;
using System.Xml.Serialization;

namespace CodeTorch.Core
{
    [Serializable]
    public class ContentSection : Section
    {
        public override string Type
        {
            get
            {
                return "Content";
            }
            set
            {
                base.Type = value;
            }
        }

        [CustomEditor("MarkupEditor")]
#if NETFRAMEWORK
        [Editor("CodeTorch.Core.Design.ContentEditor,CodeTorch.Core.Design", typeof(UITypeEditor))]
#endif
        public string Content { get; set; }

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
