using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;

namespace CodeTorch.Core.Design
{
    public class SectionCollectionEditor : CollectionEditor
    {
        public SectionCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        protected override Type[] CreateNewItemTypes()
        {
            Type[] types = SectionType.GetTypeArray();
            
            return types;
        }

        protected override string GetDisplayText(object value)
        {
            Section section = (Section)value;

            string retVal = section.Name;

            return base.GetDisplayText(retVal);
        }

        protected override object CreateInstance(Type itemType)
        {
            object retVal = base.CreateInstance(itemType);
            ((Section)retVal).Parent = Context.Instance;

           

            return retVal;
        }
    }
}
