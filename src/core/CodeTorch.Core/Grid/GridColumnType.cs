using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    [Serializable]
    public enum GridColumnType
    {
        BoundGridColumn,
        DeleteGridColumn,
        EditGridColumn,
        HyperLinkGridColumn,
        PickerLinkButtonGridColumn,
        PickerHyperLinkGridColumn,
        BinaryImageGridColumn
    }
}
