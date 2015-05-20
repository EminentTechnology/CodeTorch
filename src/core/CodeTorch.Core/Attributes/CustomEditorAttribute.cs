using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core.Attributes
{
    [AttributeUsage(AttributeTargets.All)]
    public class CustomEditorAttribute: Attribute
    {
        public string Editor { get; set; }

        public CustomEditorAttribute(string Editor)
        {
            this.Editor = Editor;
        }
    }
}
