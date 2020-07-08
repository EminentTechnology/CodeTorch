using CodeTorch.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;

using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public class ContentControl: Widget
    {

        public override string Type
        {
            get
            {
                return "Content";
            }
            set
            {
                base.Type = value;
            }
        }

        [Category("Data")]
        [CustomEditor("MarkupEditor")]
#if NETFRAMEWORK
        [Editor("CodeTorch.Core.Design.ContentEditor,CodeTorch.Core.Design", typeof(UITypeEditor))]
#endif
        public string Content { get; set; }

        public static IEnumerable<ResourceItem> GetResourceKeys(Screen Screen, LabelControl Control, String Prefix)
        {
            List<ResourceItem> retVal = new List<ResourceItem>();


            return retVal;
        }

        
    }
}
