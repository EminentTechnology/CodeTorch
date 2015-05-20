using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;

namespace CodeTorch.Core.Design
{
    public class WorkflowStepCollectionEditor : CollectionEditor
    {
        public WorkflowStepCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }



        protected override string GetDisplayText(object value)
        {
            WorkflowStep step = (WorkflowStep)value;

            string retVal = step.Name;


            return base.GetDisplayText(retVal);
        }

        protected override object CreateInstance(Type itemType)
        {

            object retVal = base.CreateInstance(itemType);
            ((WorkflowStep)retVal).Workflow = (Workflow)Context.Instance;

            return retVal;
        }


    }
}
