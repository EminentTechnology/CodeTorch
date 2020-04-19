using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Drawing.Design;

namespace CodeTorch.Core
{
    [Serializable]
    public class DeleteGridColumn: GridColumn
    {
        [Category("Data")]
        public string Text { get; set; }
        
        [Category("Data")]
        public string ConfirmText { get; set; }

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


        public string ImageUrl { get; set; }

        public override GridColumnType ColumnType
        {
            get
            {
                return GridColumnType.DeleteGridColumn;
            }
            set
            {
                base.ColumnType = value;
            }
        }
    }
}
