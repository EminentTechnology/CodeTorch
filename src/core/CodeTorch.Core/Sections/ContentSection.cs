using CodeTorch.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;

namespace CodeTorch.Core
{
    [Serializable]
    public class ContentSection : Section
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

        [CustomEditor("MarkupEditor")]
        [Editor("CodeTorch.Core.Design.ContentEditor,CodeTorch.Core.Design", typeof(UITypeEditor))]
        public string Content { get; set; }

        #region Hidden Overrides
        [Browsable(false)]
        public override List<BaseControl> Controls
        {
            get
            {
                return base.Controls;
            }
            set
            {
                base.Controls = value;
            }
        }
        #endregion
    }
}
