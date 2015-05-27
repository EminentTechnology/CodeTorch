using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CodeTorch.Core
{
    [Serializable]
    public class TemplateSection : Section
    {
        public override string Type
        {
            get
            {
                return "Template";
            }
            set
            {
                base.Type = value;
            }
        }

        List<ContentDataItem> _DataItems = new List<ContentDataItem>();

        DynamicMode _ContentSelectionMode = DynamicMode.Static;
        [Category("Content")]
        public DynamicMode ContentSelectionMode
        {
            get
            {
                return _ContentSelectionMode;
            }
            set
            {
                _ContentSelectionMode = value;
            }
        }

        [Category("Content")]
        public DynamicMode TemplateSelectionMode { get; set; }

        [Category("Content")]
        [TypeConverter("CodeTorch.Core.Design.TemplateTypeConverter,CodeTorch.Core.Design")]
        public string ContentTemplate { get; set; }
        [Category("Content")]
        [TypeConverter("CodeTorch.Core.Design.ScreenDataCommandTypeConverter,CodeTorch.Core.Design")]
        public string ContentTemplateDataCommand { get; set; }

        [Category("Content")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string ContentTemplateField { get; set; }

        [Category("Content")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string ContentField { get; set; }

        [Category("Content")]
        public List<ContentDataItem> DataItems
        {
            get { return _DataItems; }
            set { _DataItems = value; }
        }


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
