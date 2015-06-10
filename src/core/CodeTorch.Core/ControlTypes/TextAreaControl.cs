using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace CodeTorch.Core
{
    [Serializable]
    public class TextAreaControl: Widget
    {

        public override string Type
        {
            get
            {
                return "TextArea";
            }
            set
            {
                base.Type = value;
            }
        }

        int _Rows = 5;
        public int Rows
        {
            get
            {
                return _Rows;
            }
            set
            {
                _Rows = value;
            }
        }

        int _Columns = 20;
        public int Columns 
        {
            get
            {
                return _Columns;
            }
            set
            {
                _Columns = value;
            }
        }

        
    }
}
