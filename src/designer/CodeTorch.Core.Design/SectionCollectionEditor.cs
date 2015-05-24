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
            return new Type[]
            {
                typeof(AlertSection),
                typeof(ButtonListSection),
                typeof(ContentSection),
                typeof(CriteriaSection),
                typeof(CustomSection),
                typeof(DetailsSection),
                typeof(EditableGridSection),
                typeof(EditSection),
                typeof(GridSection),
                typeof(ImageSection),
                typeof(LinkListSection),
                typeof(RDLCViewerSection),
                typeof(TemplateSection)
            
            };
        }

        protected override string GetDisplayText(object value)
        {
            BaseSection section = (BaseSection)value;

            string retVal = section.Name;

            return base.GetDisplayText(retVal);
        }

        protected override object CreateInstance(Type itemType)
        {
            object retVal = base.CreateInstance(itemType);
            ((BaseSection)retVal).Parent = Context.Instance;

           

            return retVal;
        }
    }
}
