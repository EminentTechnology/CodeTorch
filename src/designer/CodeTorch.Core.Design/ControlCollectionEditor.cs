using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;

namespace CodeTorch.Core.Design
{
    public class ControlCollectionEditor : CollectionEditor
    {
        public ControlCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        protected override Type[] CreateNewItemTypes()
        {

            Type[] types = ControlType.GetTypeArray();

            return types;
        }

        protected override string GetDisplayText(object value)
        {
            Widget control = (Widget)value;

            string retVal = control.Label;

            if (String.IsNullOrEmpty(retVal))
                retVal = control.Name;

            return base.GetDisplayText(retVal);
        }

        protected override object CreateInstance(Type itemType)
        {
            object retVal = base.CreateInstance(itemType);

            if (Context.Instance != null)
            {
                if (Context.Instance is Section)
                {
                    Section section = (Section)Context.Instance;
                    if (section.Widgets.Count > 0)
                    {
                        Widget copy = section.Widgets[section.Widgets.Count - 1];

                        ((Widget)retVal).ControlContainerCssClass = copy.ControlContainerCssClass;
                        ((Widget)retVal).ControlContainerElement = copy.ControlContainerElement;
                        ((Widget)retVal).ControlGroupCssClass = copy.ControlGroupCssClass;
                        ((Widget)retVal).ControlGroupElement = copy.ControlGroupElement;
                        ((Widget)retVal).HelpTextElement = copy.HelpTextElement;
                        ((Widget)retVal).HelpTextCssClass = copy.HelpTextCssClass;
                        ((Widget)retVal).LabelContainerCssClass = copy.LabelContainerCssClass;
                        ((Widget)retVal).LabelContainerElement = copy.LabelContainerElement;
                        ((Widget)retVal).LabelRendersBeforeControl = copy.LabelRendersBeforeControl;
                        ((Widget)retVal).LabelCssClass = copy.LabelCssClass;

                    }
                    else
                    {
                        ((Widget)retVal).LabelCssClass = "col-md-4";
                        ((Widget)retVal).ControlContainerCssClass = "col-md-8";
                        ((Widget)retVal).ControlContainerElement = "div";
                        ((Widget)retVal).ControlGroupCssClass = "form-group";
                        ((Widget)retVal).ControlGroupElement = "div";
                        ((Widget)retVal).HelpTextElement = "span";
                        ((Widget)retVal).HelpTextCssClass = "help-block";
                        ((Widget)retVal).LabelContainerCssClass = "";
                        ((Widget)retVal).LabelContainerElement = "";
                        ((Widget)retVal).LabelRendersBeforeControl = true;
                        
                    }
                }

                ((Widget)retVal).Parent = Context.Instance;
            }

            return retVal;
        }

        
    }
}
