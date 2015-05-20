using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    public class EditGridColumn: GridColumn
    {
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        [Category("Data")]
        public string DataTextField { get; set; }
        
        [Category("Data")]
        public string Text { get; set; }

        ButtonType _ButtonType = ButtonType.Link;
        public ButtonType ButtonType
        {
            get
            {
                return _ButtonType;
            }
            set
            {
                _ButtonType = value;
            }
        }

        public override GridColumnType ColumnType
        {
            get
            {
                return GridColumnType.EditGridColumn;
            }
            set
            {
                base.ColumnType = value;
            }
        }

    }
}
