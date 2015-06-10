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
    public class LinkListSection : Section
    {
        PermissionCheck _Permission = new PermissionCheck();

        public override string Type
        {
            get
            {
                return "LinkList";
            }
            set
            {
                base.Type = value;
            }
        }

        public string ContainerElement { get; set; }
        public string ItemElement { get; set; }
        

        private List<LinkListItem> _Items = new List<LinkListItem>();


        [XmlArray("Items")]
        [XmlArrayItem(ElementName = "Item")]
        [Category("Items")]
        [Description("List of items")]
        public virtual List<LinkListItem> Items
        {
            get
            {
                return _Items;
            }
            set
            {
                _Items = value;
            }

        }

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


    [Serializable]
    public class LinkListItem 
    {
        PermissionCheck _Permission = new PermissionCheck();

       

        public string Text { get; set; }
        public string Url { get; set; }
        public bool Visible { get; set; }

        [Category("Common")]
        public string Context { get; set; }

        [Category("Security")]
        public PermissionCheck Permission
        {
            get
            {
                return _Permission;
            }
            set
            {
                _Permission = value;
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
                    retVal = "No Link Text";
            }

            return retVal;
        }
    }
}
