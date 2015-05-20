using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class EmailAttachment
    {
        public string ID { get; set; }
        public string DocumentID { get; set; }
    }
}
