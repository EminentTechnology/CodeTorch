using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;

namespace CodeTorch.Core
{
    [Serializable]
    public class BoundGridColumn: GridColumn
    {
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        [Category("Data")]
        public string DataField { get; set; }

        [Category("Data")]
        public string DataFormatString { get; set; }

        public override GridColumnType ColumnType
        {
            get
            {
                return GridColumnType.BoundGridColumn;
            }
            set
            {
                base.ColumnType = value;
            }
        }


    }
}
