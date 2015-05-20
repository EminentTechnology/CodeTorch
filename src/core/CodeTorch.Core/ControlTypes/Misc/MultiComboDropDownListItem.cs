using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public class MultiComboDropDownListColumn
    {
        public string HeaderText { get; set; }
        public string Width { get; set; }
        public string DataField { get; set; }

        public override string ToString()
        {
            return String.IsNullOrEmpty(HeaderText) ? DataField : HeaderText;
        }
    }
}

