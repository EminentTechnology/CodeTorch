using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;

namespace CodeTorch.Core.Design
{
    public class WorkflowActionCollectionEditor : CollectionEditor
    {
        public WorkflowActionCollectionEditor(Type type)
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
                typeof(DataCommandWorkflowAction),
                typeof(EmailWorkflowAction),
                typeof(SMSWorkflowAction)

            
            };
        }

        protected override string GetDisplayText(object value)
        {
            BaseWorkflowAction action = (BaseWorkflowAction)value;

            string retVal = action.Name;

            return base.GetDisplayText(retVal);
        }

        protected override object CreateInstance(Type itemType)
        {

            object retVal = base.CreateInstance(itemType);
            ((BaseWorkflowAction)retVal).WorkflowStep = (WorkflowStep) Context.Instance;

            return retVal;
        }


    }
}
