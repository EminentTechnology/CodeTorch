using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;

namespace CodeTorch.Core.Design
{
    public class GridGroupByFieldCollectionEditor: CollectionEditor
    {
        public GridGroupByFieldCollectionEditor(Type type)
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
                typeof(GridGroupByGroupField),
                typeof(GridGroupBySelectField)
            
            };
        }

       

        protected override string GetDisplayText(object value)
        {
            GridGroupByField field = (GridGroupByField)value;

            string retVal = field.FieldName;

            if (String.IsNullOrEmpty(retVal))
                retVal = field.FieldName;

            return base.GetDisplayText(retVal);
        }

        protected override object CreateInstance(Type itemType)
        {
            GridGroupByExpression expression = (GridGroupByExpression)Context.Instance;

            object retVal = base.CreateInstance(itemType);
            ((GridGroupByField)retVal).Expression = expression;

            return retVal;
        }
    }
}
