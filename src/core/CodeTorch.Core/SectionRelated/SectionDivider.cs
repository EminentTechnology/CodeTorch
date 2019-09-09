using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CodeTorch.Core
{
    [Serializable]
    public class SectionDivider
    {
        public string Name { get; set; }
        public string CssClass { get; set; }
        public string Style { get; set; }
        public string StartMarkup { get; set; }
        public string EndMarkup { get; set; }

        List<SectionDivider> _dividers = new List<SectionDivider>();

        [XmlArray("Dividers")]
        [XmlArrayItem("Divider")]
        [Description("List of  divs contained in this div")]
        public virtual List<SectionDivider> Dividers
        {
            get
            {
                return _dividers;
            }
            set
            {
                _dividers = value;
            }

        }
    }
}
