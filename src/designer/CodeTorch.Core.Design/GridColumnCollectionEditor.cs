using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;

namespace CodeTorch.Core.Design
{
    public class GridColumnCollectionEditor: CollectionEditor
    {
        public GridColumnCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        protected override Type[] CreateNewItemTypes()
        {
            return new Type[]
            {
                typeof(BoundGridColumn),
                typeof(DeleteGridColumn),
                typeof(EditGridColumn),
                typeof(HyperLinkGridColumn),
                typeof(PickerHyperLinkGridColumn),
                typeof(PickerLinkButtonGridColumn),
                typeof(BinaryImageGridColumn),
                typeof(ClientSelectGridColumn),

            };
        }

        protected override string GetDisplayText(object value)
        {
            GridColumn column = (GridColumn)value;

            string retVal = column.HeaderText;

            return base.GetDisplayText(retVal);
        }

        protected override object CreateInstance(Type itemType)
        {
            Grid grid = (Grid) Context.Instance;

            object retVal = base.CreateInstance(itemType);
            ((GridColumn)retVal).Parent = grid;

            return retVal;
        }
    }
}
