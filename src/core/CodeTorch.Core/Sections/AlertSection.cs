using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CodeTorch.Core
{
    [Serializable]
    public class AlertSection : BaseSection
    {
        public override string Type
        {
            get
            {
                return "Alert";
            }
            set
            {
                base.Type = value;
            }
        }

        public bool IncludeValidationSummary { get; set; }


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
