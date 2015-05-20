using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core.Design
{
    public class WorkflowStepTypeConverter : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            //this means a standard list of values are supported
            return true;
        }

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            //the actual list of standard items to return
            StandardValuesCollection list = null;

            if (context.Instance is WorkflowNextStep)
            {
                Workflow workflow = ((WorkflowNextStep)context.Instance).Parent.Workflow;


                var retVal = from step in workflow.Steps
                             select step.Name;

                var tempList = retVal.ToList<String>();
                tempList.Insert(0, String.Empty);

                string[] items = tempList.ToArray<string>();
                list = new StandardValuesCollection(items);
               
            }



            return list;


        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}
