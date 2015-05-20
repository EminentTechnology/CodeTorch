using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;

namespace CodeTorch.Core.Design
{
    public class GridGroupByExpressionCollectionEditor : CollectionEditor
    {
        public GridGroupByExpressionCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        

        protected override string GetDisplayText(object value)
        {
            GridGroupByExpression expression = (GridGroupByExpression)value;

            StringBuilder retVal = new StringBuilder();
            retVal.AppendFormat("{0} fields", expression.Fields.Count);
            if (expression.Fields.Count > 0)
            {
                string sep = "";
                retVal.Append(":");
                foreach (GridGroupByField f in expression.Fields)
                {
                    retVal.AppendFormat("{0}{1}",sep,f.FieldName);
                    sep = ",";
                }
            }

            return base.GetDisplayText(retVal.ToString());
        }

        protected override object CreateInstance(Type itemType)
        {
            Grid grid = (Grid)Context.Instance;

            object retVal = base.CreateInstance(itemType);
            ((GridGroupByExpression)retVal).Grid = grid;

            return retVal;
        }
    }
}
