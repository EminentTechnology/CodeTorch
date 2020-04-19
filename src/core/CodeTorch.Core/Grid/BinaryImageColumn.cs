using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace CodeTorch.Core
{
    public enum BinaryImageResizeMode
    { 
        None = 0,
        Fit=1,
        Crop=2,
        Fill=3
    }

    [Serializable]
    public class BinaryImageGridColumn : GridColumn
    {
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        [Category("Data")]
        public string DataField { get; set; }

        [Category("Appearance")]
        public string ImageHeight { get; set; }

        [Category("Appearance")]
        public string ImageWidth { get; set; }

        [Category("Appearance")]
        public ImageAlign ImageAlign { get; set; }

        [Category("Appearance")]
        public BinaryImageResizeMode ResizeMode { get; set; }

        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        [Category("Data")]
        public string DataAlternateTextField { get; set; }

        [Category("Data")]
        public string DataAlternateTextFormatString { get; set; }

        [Category("Data")]
        public string DefaultImageUrl { get; set; }

        [Category("Data")]
        public string DefaultImageUrlFieldFormat { get; set; }

        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        [Category("Data")]
        public string DefaultImageUrlField { get; set; }

        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        [Category("Data")]
        public string DataNavigateUrlFields { get; set; }

        [Category("Data")]
        public string DataNavigateUrlFormatString { get; set; }

        [Category("Common")]
        public string Context { get; set; }

        [Category("Data")]
        public string Target { get; set; }

        public override GridColumnType ColumnType
        {
            get
            {
                return GridColumnType.BinaryImageGridColumn;
            }
            set
            {
                base.ColumnType = value;
            }
        }
    }
}
