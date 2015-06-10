using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CodeTorch.Core
{
    [Serializable]
    public class ButtonListSection : Section
    {
        

        public override string Type
        {
            get
            {
                return "ButtonList";
            }
            set
            {
                base.Type = value;
            }
        }




        private List<ButtonControl> _Buttons = new List<ButtonControl>();


        [XmlArray("Buttons")]
        [XmlArrayItem(ElementName = "Button")]
        [Category("Buttons")]
        [Description("List of buttons")]
        public virtual List<ButtonControl> Buttons
        {
            get
            {
                return _Buttons;
            }
            set
            {
                _Buttons = value;
            }

        }

        public string ContainerElement { get; set; }
        public string ItemElement { get; set; }

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
