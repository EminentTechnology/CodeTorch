using CodeTorch.Core.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Core.Design
{
    public class DataCommandColumnTypeConverter : StringConverter
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

            if (context.Instance is Screen)
            {
                
                Screen screen = (Screen)context.Instance;

                if (context.PropertyDescriptor.Name == "SectionZoneLayoutDataField")
                {

                    var retVal = from item in Configuration.GetInstance().DataCommands
                                 from column in item.Columns
                                 where item.Name == screen.SectionZoneLayoutDataCommand
                                 select column.Name;

                    var tempList = retVal.ToList<String>();
                    tempList.Insert(0, String.Empty);

                    string[] items = tempList.ToArray<string>();

                    list = new StandardValuesCollection(items);
                }

            }

            if (context.Instance is Picker)
            {
                Picker picker = (Picker)context.Instance;

                var retVal = from item in Configuration.GetInstance().DataCommands
                             from column in item.Columns
                             where item.Name == picker.DataCommand
                             select column.Name;

                var tempList = retVal.ToList<String>();
                tempList.Insert(0, String.Empty);

                string[] items = tempList.ToArray<string>();

                list = new StandardValuesCollection(items);

            }

            if (context.Instance is ScreenPageTemplate)
            {
                ScreenPageTemplate pageTemplate = (ScreenPageTemplate)context.Instance;

                if (!String.IsNullOrEmpty(pageTemplate.DataCommand))
                {
                    var retVal = from item in Configuration.GetInstance().DataCommands
                                 from column in item.Columns
                                 where item.Name == pageTemplate.DataCommand
                                 select column.Name;

                    var tempList = retVal.ToList<String>();
                    tempList.Insert(0, String.Empty);

                    string[] items = tempList.ToArray<string>();

                    list = new StandardValuesCollection(items);
                }

            }








            if (context.Instance is Grid)
            {
                Grid grid = (Grid)context.Instance;

                if (!String.IsNullOrEmpty(grid.SelectDataCommand))
                {
                    var retVal = from item in Configuration.GetInstance().DataCommands
                                 from column in item.Columns
                                 where item.Name == grid.SelectDataCommand
                                 select column.Name;

                    var tempList = retVal.ToList<String>();
                    tempList.Insert(0, String.Empty);

                    string[] items = tempList.ToArray<string>();

                    list = new StandardValuesCollection(items);
                }

            }

            if (context.Instance is GridColumn)
            {
                GridColumn col = (GridColumn)context.Instance;

                if (!String.IsNullOrEmpty(col.Parent.SelectDataCommand))
                {
                    var retVal = from item in Configuration.GetInstance().DataCommands
                                 from column in item.Columns
                                 where item.Name == col.Parent.SelectDataCommand
                                 select column.Name;

                    var tempList = retVal.ToList<String>();
                    tempList.Insert(0, String.Empty);

                    string[] items = tempList.ToArray<string>();

                    list = new StandardValuesCollection(items);
                }
            }

            if (context.Instance is GridGroupByField)
            {
                GridGroupByField groupByField = (GridGroupByField)context.Instance;

                if ((groupByField.Expression != null) && (groupByField.Expression.Grid != null))
                {
                    if (!String.IsNullOrEmpty(groupByField.Expression.Grid.SelectDataCommand))
                    {
                        var retVal = from item in Configuration.GetInstance().DataCommands
                                     from column in item.Columns
                                     where item.Name == groupByField.Expression.Grid.SelectDataCommand
                                     select column.Name;

                        var tempList = retVal.ToList<String>();
                        tempList.Insert(0, String.Empty);

                        string[] items = tempList.ToArray<string>();

                        list = new StandardValuesCollection(items);
                    }
                }
            }

            if (context.Instance is Widget)
            {
                //if propery is DataField
                if (context.PropertyDescriptor.Name.ToLower() == "datafield")
                {
                    list = GetDataFieldColumns(context, list);
                }
                else
                {

                    if (context.Instance is DropDownListControl)
                    {
                        DropDownListControl control = (DropDownListControl)context.Instance;

                        if (!String.IsNullOrEmpty(control.SelectDataCommand))
                        {
                            var retVal = from item in Configuration.GetInstance().DataCommands
                                         from column in item.Columns
                                         where item.Name.ToLower() == control.SelectDataCommand.ToLower()
                                         select column.Name;

                            var tempList = retVal.ToList<String>();
                            tempList.Insert(0, String.Empty);

                            string[] items = tempList.ToArray<string>();
                            list = new StandardValuesCollection(items);
                        }
                    }

                    if (context.Instance is ListBoxControl)
                    {
                        ListBoxControl control = (ListBoxControl)context.Instance;

                        if (!String.IsNullOrEmpty(control.SelectDataCommand))
                        {
                            var retVal = from item in Configuration.GetInstance().DataCommands
                                         from column in item.Columns
                                         where item.Name.ToLower() == control.SelectDataCommand.ToLower()
                                         select column.Name;

                            var tempList = retVal.ToList<String>();
                            tempList.Insert(0, String.Empty);

                            string[] items = tempList.ToArray<string>();
                            list = new StandardValuesCollection(items);
                        }
                    }

                    if (context.Instance is TreeViewControl)
                    {
                        TreeViewControl control = (TreeViewControl)context.Instance;

                        if (!String.IsNullOrEmpty(control.SelectDataCommand))
                        {
                            var retVal = from item in Configuration.GetInstance().DataCommands
                                         from column in item.Columns
                                         where item.Name.ToLower() == control.SelectDataCommand.ToLower()
                                         select column.Name;

                            var tempList = retVal.ToList<String>();
                            tempList.Insert(0, String.Empty);

                            string[] items = tempList.ToArray<string>();
                            list = new StandardValuesCollection(items);
                        }
                    }
                }
            }

            if (context.Instance is DataCommandValidator)
            {
                DataCommandValidator validator = (DataCommandValidator)context.Instance;

                if (!String.IsNullOrEmpty(validator.DataCommand))
                {
                    var retVal = from item in Configuration.GetInstance().DataCommands
                                 from column in item.Columns
                                 where item.Name.ToLower() == validator.DataCommand.ToLower()
                                 select column.Name;

                    var tempList = retVal.ToList<String>();
                    tempList.Insert(0, String.Empty);

                    string[] items = tempList.ToArray<string>();
                    list = new StandardValuesCollection(items);
                }
            }



            if (context.Instance is WorkflowDynamicSecurityGroup)
            {
                WorkflowDynamicSecurityGroup dynamicGroup = (WorkflowDynamicSecurityGroup)context.Instance;

                if (!String.IsNullOrEmpty(dynamicGroup.DataCommand))
                {
                    var retVal = from item in Configuration.GetInstance().DataCommands
                                 from column in item.Columns
                                 where item.Name.ToLower() == dynamicGroup.DataCommand.ToLower()
                                 select column.Name;

                    var tempList = retVal.ToList<String>();
                    tempList.Insert(0, String.Empty);

                    string[] items = tempList.ToArray<string>();
                    list = new StandardValuesCollection(items);
                }
            }

            if (context.Instance is EmailWorkflowAction)
            {
                EmailWorkflowAction emailAction = (EmailWorkflowAction)context.Instance;

                string dataCommandName = null;

                switch (context.PropertyDescriptor.Name.ToLower())
                {
                    case "attachmentdocumentidfield":
                    case "attachmenttoaddressfield":
                        dataCommandName = emailAction.AttachmentDataCommand;
                        break;
                    case "bccaddressdisplaynamefield":
                    case "bccaddressfield":
                    case "bcctoaddressfield":
                        dataCommandName = emailAction.BCCAddressDataCommand;
                        break;
                    case "ccaddressdisplaynamefield":
                    case "ccaddressfield":
                    case "cctoaddressfield":
                        dataCommandName = emailAction.CCAddressDataCommand;
                        break;
                    case "contenttemplatefield":
                        dataCommandName = emailAction.ContentTemplateDataCommand;
                        break;
                    case "fromaddressdisplaynamefield":
                    case "fromaddressfield":
                    case "fromtoaddressfield":
                        dataCommandName = emailAction.FromAddressDataCommand;
                        break;
                    case "subjectfield":
                    case "subjecttoaddressfield":
                        dataCommandName = emailAction.SubjectDataCommand;
                        break;
                    case "toaddressdisplaynamefield":
                    case "toaddressfield":
                        dataCommandName = emailAction.ToAddressDataCommand;
                        break;
                }

                if (!String.IsNullOrEmpty(dataCommandName))
                {
                    var retVal = from item in Configuration.GetInstance().DataCommands
                                 from column in item.Columns
                                 where item.Name.ToLower() == dataCommandName.ToLower()
                                 select column.Name;

                    var tempList = retVal.ToList<String>();
                    tempList.Insert(0, String.Empty);

                    string[] items = tempList.ToArray<string>();
                    list = new StandardValuesCollection(items);
                }
            }

            if (context.Instance is WorkflowContentDataItem)
            {
                WorkflowContentDataItem emailDataItem = (WorkflowContentDataItem)context.Instance;

                if (!String.IsNullOrEmpty(emailDataItem.DataCommand))
                {
                    var retVal = from item in Configuration.GetInstance().DataCommands
                                 from column in item.Columns
                                 where item.Name.ToLower() == emailDataItem.DataCommand.ToLower()
                                 select column.Name;

                    var tempList = retVal.ToList<String>();
                    tempList.Insert(0, String.Empty);

                    string[] items = tempList.ToArray<string>();
                    list = new StandardValuesCollection(items);
                }
            }

            if (context.Instance is ContentDataItem)
            {
                ContentDataItem contentDataItem = (ContentDataItem)context.Instance;

                if (!String.IsNullOrEmpty(contentDataItem.DataCommand))
                {
                    var retVal = from item in Configuration.GetInstance().DataCommands
                                 from column in item.Columns
                                 where item.Name.ToLower() == contentDataItem.DataCommand.ToLower()
                                 select column.Name;

                    var tempList = retVal.ToList<String>();
                    tempList.Insert(0, String.Empty);

                    string[] items = tempList.ToArray<string>();
                    list = new StandardValuesCollection(items);
                }
            }







            if (context.Instance is SMSWorkflowAction)
            {
                SMSWorkflowAction smsAction = (SMSWorkflowAction)context.Instance;

                string dataCommandName = null;

                switch (context.PropertyDescriptor.Name.ToLower().Substring(0, 2))
                {
                    case "co"://content
                        dataCommandName = smsAction.ContentTemplateDataCommand;
                        break;
                    case "ph"://phone
                        dataCommandName = smsAction.PhoneDataCommand;
                        break;
                }

                if (!String.IsNullOrEmpty(dataCommandName))
                {
                    var retVal = from item in Configuration.GetInstance().DataCommands
                                 from column in item.Columns
                                 where item.Name.ToLower() == dataCommandName.ToLower()
                                 select column.Name;

                    var tempList = retVal.ToList<String>();
                    tempList.Insert(0, String.Empty);

                    string[] items = tempList.ToArray<string>();

                    list = new StandardValuesCollection(items);
                }
            }



            return list;


        }

        private static StandardValuesCollection GetDataFieldColumns(ITypeDescriptorContext context, StandardValuesCollection list)
        {
            Widget control = (Widget)context.Instance;
            if ((control.Parent != null) && (control.Parent is Section))
            {
                Section section = (Section)control.Parent;

                if ((section.Parent != null) && (section.Parent is Screen))
                {
                    Screen screen = (Screen)section.Parent;

                    ActionCommand actionComand = screen.OnPageLoad.Commands.Where(c => c.Type.ToLower() == "defaultorpopulatescreencommand").SingleOrDefault();
                    if ((actionComand != null) && (actionComand is DefaultOrPopulateScreenCommand))
                    {
                        DefaultOrPopulateScreenCommand defaultCommand = (DefaultOrPopulateScreenCommand)actionComand;

                        string DataCommandName = null;
                        if (!String.IsNullOrEmpty(defaultCommand.RetrieveCommand))
                        {
                            DataCommandName = defaultCommand.RetrieveCommand;
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(defaultCommand.DefaultCommand))
                            {
                                DataCommandName = defaultCommand.DefaultCommand;
                            }
                        }

                        if (!String.IsNullOrEmpty(DataCommandName))
                        {
                            var retVal = from item in Configuration.GetInstance().DataCommands
                                         from column in item.Columns
                                         where item.Name.ToLower() == DataCommandName.ToLower()
                                         select column.Name;

                            string[] items = retVal.ToArray<string>();
                            list = new StandardValuesCollection(items);
                        }
                    }

                }
            }
            return list;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }
    }
}
