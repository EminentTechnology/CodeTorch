using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    public class PickerLinkButtonGridColumn : GridColumn
    {
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        [Category("Data")]
        public string DataTextField { get; set; }

        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        [Category("Data")]
        public string DataField { get; set; }

        public override GridColumnType ColumnType
        {
            get
            {
                return GridColumnType.PickerLinkButtonGridColumn;
            }
            set
            {
                base.ColumnType = value;
            }
        }

    }
}
