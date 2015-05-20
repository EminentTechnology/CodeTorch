using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core.Design
{
    public class DataCommandParameterTypeConverter: StringConverter
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

            if (context.Instance is Picker)
            {
                Picker picker = (Picker)context.Instance;

                var retVal = from item in Configuration.GetInstance().DataCommands
                             from param in item.Parameters
                             where item.Name == picker.DataCommand
                             select param.Name;

                var tempList = retVal.ToList<String>();
                tempList.Insert(0, String.Empty);

                string[] items = tempList.ToArray<string>();

                list = new StandardValuesCollection(items);

            }

            if (context.Instance is DataCommandValidator)
            {
                DataCommandValidator validator = (DataCommandValidator)context.Instance;

                if (!String.IsNullOrEmpty(validator.DataCommand))
                {
                    var retVal = from item in Configuration.GetInstance().DataCommands
                                 from param in item.Parameters
                                 where item.Name == validator.DataCommand
                                 select param.Name;

                    var tempList = retVal.ToList<String>();
                    tempList.Insert(0, String.Empty);

                    string[] items = tempList.ToArray<string>();

                    list = new StandardValuesCollection(items);
                }
            }

            

            

            if (context.Instance is DataCommandWorkflowAction)
            {
                DataCommandWorkflowAction commandAction = (DataCommandWorkflowAction)context.Instance;

                var retVal = from item in Configuration.GetInstance().DataCommands
                             from param in item.Parameters
                             where item.Name == commandAction.ExecuteCommand
                             select param.Name;

                var tempList = retVal.ToList<String>();
                tempList.Insert(0, String.Empty);

                string[] items = tempList.ToArray<string>();

                list = new StandardValuesCollection(items);

            }

            if (context.Instance is WorkflowDynamicSecurityGroup)
            {
                WorkflowDynamicSecurityGroup dynamicGroup = (WorkflowDynamicSecurityGroup)context.Instance;

                var retVal = from item in Configuration.GetInstance().DataCommands
                             from param in item.Parameters
                             where item.Name == dynamicGroup.DataCommand
                             select param.Name;

                var tempList = retVal.ToList<String>();
                tempList.Insert(0, String.Empty);

                string[] items = tempList.ToArray<string>();

                list = new StandardValuesCollection(items);

            }

            if (context.Instance is EmailWorkflowAction)
            {
                EmailWorkflowAction emailAction = (EmailWorkflowAction)context.Instance;

                string dataCommandName = null;

                switch (context.PropertyDescriptor.Name.ToLower().Substring(0,2))
                {
                    case "at"://attachment
                        dataCommandName = emailAction.AttachmentDataCommand;
                        break;
                    case "bc"://bccaddress
                        dataCommandName = emailAction.BCCAddressDataCommand;
                        break;
                    case "cc"://ccaddress
                        dataCommandName = emailAction.CCAddressDataCommand;
                        break;
                    case "co"://content
                        dataCommandName = emailAction.ContentTemplateDataCommand;
                        break;
                    case "fr"://fromaddress
                        dataCommandName = emailAction.FromAddressDataCommand;
                        break;
                    case "su"://subject
                        dataCommandName = emailAction.SubjectDataCommand;
                        break;
                    case "to"://toaddress
                        dataCommandName = emailAction.ToAddressDataCommand;
                        break;
                }

                if (!String.IsNullOrEmpty(dataCommandName))
                {
                    var retVal = from item in Configuration.GetInstance().DataCommands
                                 from param in item.Parameters
                                 where item.Name.ToLower() == dataCommandName.ToLower()
                                 select param.Name;

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
                                 from param in item.Parameters
                                 where item.Name.ToLower() == dataCommandName.ToLower()
                                 select param.Name;

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
                                 from param in item.Parameters
                                 where item.Name.ToLower() == emailDataItem.DataCommand.ToLower()
                                 select param.Name;

                    var tempList = retVal.ToList<String>();
                    tempList.Insert(0, String.Empty);

                    string[] items = tempList.ToArray<string>();

                    list = new StandardValuesCollection(items);
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
