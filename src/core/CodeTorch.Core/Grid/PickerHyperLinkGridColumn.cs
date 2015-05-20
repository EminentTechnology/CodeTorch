using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public class PickerHyperLinkGridColumn:GridColumn
    {
        public string PickerType { get; set; }

        public override GridColumnType ColumnType
        {
            get
            {
                return GridColumnType.PickerHyperLinkGridColumn;
            }
            set
            {
                base.ColumnType = value;
            }
        }
    }
}
