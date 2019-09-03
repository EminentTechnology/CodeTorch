using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace CodeTorch.Core
{


    [Serializable]
    public class ClientSelectGridColumn : GridColumn
    {
        

        public override GridColumnType ColumnType
        {
            get
            {
                return GridColumnType.ClientSelectGridColumn;
            }
            set
            {
                base.ColumnType = value;
            }
        }
    }
}
