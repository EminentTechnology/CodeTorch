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

            return new Type[]
            {
                typeof(AutoCompleteBoxControl),
                typeof(ButtonControl),
                typeof(CheckBoxControl),
                typeof(DatePickerControl),
                typeof(DropDownListControl),
                typeof(EditorControl),
                typeof(EmailAddressControl),
                typeof(FileUploadControl),
                typeof(GenericControl),
                typeof(HyperLinkControl),
                typeof(LabelControl),
                typeof(ListBoxControl),
                typeof(LookupDropDownListControl),
                typeof(LookupListBoxControl),
                typeof(MultiComboDropDownListControl),
                typeof(NumericTextBoxControl),
                typeof(PasswordControl),
                typeof(PhotoPickerControl),
                typeof(PickerControl),
                typeof(SocialShareControl),
                typeof(TextAreaControl),
                typeof(TextBoxControl),
                typeof(TreeViewControl),
                typeof(WorkflowStatusControl)
            
            };
        }

        protected override string GetDisplayText(object value)
        {
            BaseControl control = (BaseControl)value;

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
                    if (section.Controls.Count > 0)
                    {
                        BaseControl copy = section.Controls[section.Controls.Count - 1];

                        ((BaseControl)retVal).ControlContainerCssClass = copy.ControlContainerCssClass;
                        ((BaseControl)retVal).ControlContainerElement = copy.ControlContainerElement;
                        ((BaseControl)retVal).ControlGroupCssClass = copy.ControlGroupCssClass;
                        ((BaseControl)retVal).ControlGroupElement = copy.ControlGroupElement;
                        ((BaseControl)retVal).HelpTextElement = copy.HelpTextElement;
                        ((BaseControl)retVal).HelpTextCssClass = copy.HelpTextCssClass;
                        ((BaseControl)retVal).LabelContainerCssClass = copy.LabelContainerCssClass;
                        ((BaseControl)retVal).LabelContainerElement = copy.LabelContainerElement;
                        ((BaseControl)retVal).LabelRendersBeforeControl = copy.LabelRendersBeforeControl;
                        ((BaseControl)retVal).LabelCssClass = copy.LabelCssClass;

                    }
                    else
                    {
                        ((BaseControl)retVal).LabelCssClass = "col-md-4";
                        ((BaseControl)retVal).ControlContainerCssClass = "col-md-8";
                        ((BaseControl)retVal).ControlContainerElement = "div";
                        ((BaseControl)retVal).ControlGroupCssClass = "form-group";
                        ((BaseControl)retVal).ControlGroupElement = "div";
                        ((BaseControl)retVal).HelpTextElement = "span";
                        ((BaseControl)retVal).HelpTextCssClass = "help-block";
                        ((BaseControl)retVal).LabelContainerCssClass = "";
                        ((BaseControl)retVal).LabelContainerElement = "";
                        ((BaseControl)retVal).LabelRendersBeforeControl = true;
                        
                    }
                }

                ((BaseControl)retVal).Parent = Context.Instance;
            }

            return retVal;
        }

        
    }
}
