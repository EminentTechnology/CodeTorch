using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    [DefaultProperty("Code")]    
    public class DocumentType
    {
        [TypeConverter("CodeTorch.Core.Design.DocumentTypeTypeConverter,CodeTorch.Core.Design")]
        [Description("The document code for a given document type")]
        public string Code { get; set; }

        public override string ToString()
        {
            return Code;
        }
    }
}
