using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace CodeTorch.Core
{

    [Serializable]
    public class GridGroupByGroupField : GridGroupByField
    {


        public GridSortOrder SortOrder { get; set; }

      
    }
}
