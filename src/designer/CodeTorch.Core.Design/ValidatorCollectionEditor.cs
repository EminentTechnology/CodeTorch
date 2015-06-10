using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;

namespace CodeTorch.Core.Design
{
    public class ValidatorCollectionEditor : CollectionEditor
    {
        public ValidatorCollectionEditor(Type type)
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
                typeof(CompareValidator),
                typeof(DataCommandValidator),
                typeof(RangeValidator),
                typeof(RegularExpressionValidator)
            };
        }

        protected override string GetDisplayText(object value)
        {
            BaseValidator validator = (BaseValidator)value;

            string retVal = validator.Name;

            return base.GetDisplayText(retVal);
        }

        protected override object CreateInstance(Type itemType)
        {

            object retVal = base.CreateInstance(itemType);
            ((BaseValidator)retVal).Parent = Context.Instance;

            try
            {
                ((BaseValidator)retVal).Name = String.Format("{0}{1}", ((Widget)Context.Instance).Name, retVal.GetType().Name);
            }
            catch { }

            return retVal;
        }


    }
}
