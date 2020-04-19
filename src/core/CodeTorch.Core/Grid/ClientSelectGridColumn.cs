using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace CodeTorch.Core
{


    [Serializable]
    public class ClientSelectGridColumn : GridColumn
    {

        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        [Category("Data")]
        public string EnabledDataField { get; set; }

        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        [Category("Data")]
        public string TooltipDataField { get; set; }

        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        [Category("Data")]
        public string SelectedDataField { get; set; }


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
