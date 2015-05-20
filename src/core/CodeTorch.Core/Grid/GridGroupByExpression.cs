using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Drawing.Design;

namespace CodeTorch.Core
{
    [Serializable]
    public class GridGroupByExpression
    {
        private List<GridGroupByField> _Fields = new List<GridGroupByField>();
        


        [XmlArray("GroupByFields")]
        [XmlArrayItem(ElementName = "SelectField", Type = typeof(GridGroupBySelectField))]
        [XmlArrayItem(ElementName = "GroupField", Type = typeof(GridGroupByGroupField))]
        [Editor("CodeTorch.Core.Design.GridGroupByFieldCollectionEditor,CodeTorch.Core.Design", typeof(UITypeEditor))]  
        [Description("List of grid group by fields")]
        public List<GridGroupByField> Fields
        {
            get
            {
                return _Fields;
            }
            set
            {
                _Fields = value;
            }

        }

        

        [Browsable(false)]
        [XmlIgnore()]
        public Grid Grid { get; set; }
    }
}
