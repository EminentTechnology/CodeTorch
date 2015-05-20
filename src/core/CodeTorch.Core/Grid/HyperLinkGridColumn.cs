using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    public class HyperLinkGridColumn: GridColumn
    {
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        [Category("Data")]
        public string DataNavigateUrlFields { get; set; }

        [Category("Data")]
        public string DataNavigateUrlFormatString { get; set; }

        [Category("Common")]
        public string Context { get; set; }

        
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        [Category("Data")]
        public string DataTextField { get; set; }

        [Category("Data")]
        public string DataTextFormatString { get; set; }

        [Category("Data")]
        public string Text { get; set; }

        [Category("Data")]
        public string Target { get; set; }

        public override GridColumnType ColumnType
        {
            get
            {
                return GridColumnType.HyperLinkGridColumn;
            }
            set
            {
                base.ColumnType = value;
            }
        }
    }
}
