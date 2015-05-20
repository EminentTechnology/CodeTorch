using CodeTorch.Core.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CodeTorch.Core
{
    public class TableSectionControl: BaseSection
    {
        public override string Type
        {
            get
            {
                return "TableSection";
            }
            set
            {
                base.Type = value;
            }
        }

        OnPlatformString _Title = new OnPlatformString();
        public OnPlatformString Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                this._Title = value;
            }
        }

        private List<BaseCell> _Cells = new List<BaseCell>();

        [XmlArray("Cells")]
        [XmlArrayItem(ElementName = "EntryCellControl", Type = typeof(EntryCellControl))]
        [XmlArrayItem(ElementName = "ImageCellControl", Type = typeof(ImageCellControl))]
        [XmlArrayItem(ElementName = "TextCellControl", Type = typeof(TextCellControl))]
        [XmlArrayItem(ElementName = "SwitchCellControl", Type = typeof(SwitchCellControl))]
        public List<BaseCell> Cells
        {
            get
            {
                return _Cells;
            }
            set
            {
                _Cells = value;
            }
        }
    }
}
