using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace CodeTorch.Core
{
    [Serializable]
    public class GridGroupByField
    {
      

        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        [Category("Data")]
        public string FieldName { get; set; }

        [Browsable(false)]
        [XmlIgnore()]
        public GridGroupByExpression Expression { get; set; }
    }
}
