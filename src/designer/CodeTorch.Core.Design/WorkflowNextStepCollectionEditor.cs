using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;

namespace CodeTorch.Core.Design
{
    public class WorkflowNextStepCollectionEditor : CollectionEditor
    {
        public WorkflowNextStepCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        

        protected override string GetDisplayText(object value)
        {
            WorkflowNextStep step = (WorkflowNextStep)value;

            string retVal = step.Name;

   
            return base.GetDisplayText(retVal);
        }

        protected override object CreateInstance(Type itemType)
        {

            object retVal = base.CreateInstance(itemType);
            ((WorkflowNextStep)retVal).Parent = (WorkflowStep) Context.Instance;

            return retVal;
        }


    }
}
