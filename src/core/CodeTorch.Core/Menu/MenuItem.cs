using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;


namespace CodeTorch.Core
{
    [Serializable]
    public class MenuItem
    {
        private List<MenuItem> _Items = new List<MenuItem>();
        bool _UseCommand = false;

        [Category("Appearance")]
        public string CssClass { get; set; }

        [Category("Common")]
        public string Name { get; set; }

        [Category("Common")]
        public string Code { get; set; }

        [Category("Common")]
        public string Location { get; set; }

        [Category("Common")]
        public string Context { get; set; }

        [Category("Common")]
        public string Target { get; set; }

        [Category("Common")]
        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        [Description("List of children menu items")]
        public List<MenuItem> Items
        {
            get
            {
                return _Items;
            }

        }

        [Category("Data")]
        public bool UseCommand
        {
            get { return _UseCommand; }
            set { _UseCommand = value; }
        }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandTypeConverter,CodeTorch.Core.Design")]
        public string CommandName { get; set; }

        PermissionCheck _Permission = new PermissionCheck();
        [Category("Security")]
        public PermissionCheck Permission
        {
            get { return _Permission; }
            set { _Permission = value; }
        }
    }
}
