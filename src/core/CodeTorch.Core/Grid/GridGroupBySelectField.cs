using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace CodeTorch.Core
{

    [Serializable]
    public class GridGroupBySelectField : GridGroupByField
    {
        public GridAggregateFunction Aggregate { get; set; }

        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string FieldAlias { get; set; }

        public string FormatString { get; set; }

        public string HeaderText { get; set; }
        public string HeaderValueSeparator { get; set; }

        

       
    }
}
