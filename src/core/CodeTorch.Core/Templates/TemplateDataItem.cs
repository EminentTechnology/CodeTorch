using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    public class TemplateDataItem
    {
        public string Name { get; set; }
        public TemplateDataItemType Type  { get; set; }
    }

    public enum TemplateDataItemType
    {
        DataRow,
        DataRowList
    }
}
