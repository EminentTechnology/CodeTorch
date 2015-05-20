using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core.Design
{
    public class ScreenDataCommandTypeConverter : StringConverter
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

                var retVal = from item in screen.DataCommands
                             select item.Name;

                var tempList = retVal.ToList<String>();
                tempList.Insert(0, String.Empty);

                string[] items = tempList.ToArray<string>();
                list = new StandardValuesCollection(items);
            }

            

            if (context.Instance is BaseSection)
            {
                BaseSection section = (BaseSection)context.Instance;

                if (section.Parent != null)
                {
                    if (section.Parent is Screen)
                    {
                        Screen parentScreen = (Screen)section.Parent;
                        var retVal = from item in parentScreen.DataCommands
                                     select item.Name;

                        var tempList = retVal.ToList<String>();
                        tempList.Insert(0, String.Empty);

                        string[] items = tempList.ToArray<string>();
                        list = new StandardValuesCollection(items);
                    }

                    
                }
                else
                {

                    var retVal = from item in Configuration.GetInstance().DataCommands
                                 select item.Name;

                    var tempList = retVal.ToList<String>();
                    tempList.Insert(0, String.Empty);

                    string[] items = tempList.ToArray<string>();
                    list = new StandardValuesCollection(items);
                }

                
            }

            if (context.Instance is Grid)
            {
                Grid grid = (Grid)context.Instance;
                

                if (grid.Parent == null)
                {
                    var retVal = from item in Configuration.GetInstance().DataCommands
                                 select item.Name;

                    var tempList = retVal.ToList<String>();
                    tempList.Insert(0, String.Empty);

                    string[] items = tempList.ToArray<string>();
                    list = new StandardValuesCollection(items);
                }
                else
                {
                    if (grid.Parent is Screen)
                    {
                        Screen parentScreen = (Screen)grid.Parent;
                        var retVal = from item in parentScreen.DataCommands
                                     select item.Name;

                        var tempList = retVal.ToList<String>();
                        tempList.Insert(0, String.Empty);

                        string[] items = tempList.ToArray<string>();
                        list = new StandardValuesCollection(items);
                    }

                    
                }

                
            }

            if (context.Instance is DropDownListControl)
            {
                DropDownListControl control = (DropDownListControl)context.Instance;

                
                if (control.Parent is BaseSection)
                {
                    BaseSection section = (BaseSection)control.Parent;

                    
                    if (section.Parent != null)
                    {
                        if (section.Parent is Screen)
                        {
                            Screen parentScreen = (Screen)section.Parent;
                            var retVal = from item in parentScreen.DataCommands
                                         select item.Name;

                            var tempList = retVal.ToList<String>();
                            tempList.Insert(0, String.Empty);

                            string[] items = tempList.ToArray<string>();
                            list = new StandardValuesCollection(items);
                        }

                       

                    }
                    else
                    {
                        //handle bug during initial creation where we donot have a parent
                        var retVal = from item in Configuration.GetInstance().DataCommands
                                     select item.Name;

                        var tempList = retVal.ToList<String>();
                        tempList.Insert(0, String.Empty);

                        string[] items = tempList.ToArray<string>();
                        list = new StandardValuesCollection(items);
                    }

                    

                }



               

               

                
            }

            if (context.Instance is ListBoxControl)
            {
                ListBoxControl control = (ListBoxControl)context.Instance;


                if (control.Parent is BaseSection)
                {
                    BaseSection section = (BaseSection)control.Parent;

                    
                    if (section.Parent != null)
                    {
                        if (section.Parent is Screen)
                        {
                            Screen parentScreen = (Screen)section.Parent;
                            var retVal = from item in parentScreen.DataCommands
                                         select item.Name;

                            var tempList = retVal.ToList<String>();
                            tempList.Insert(0, String.Empty);

                            string[] items = tempList.ToArray<string>();
                            list = new StandardValuesCollection(items);
                        }

                       

                    }
                    else
                    {
                        //handle bug during initial creation where we donot have a parent
                        var retVal = from item in Configuration.GetInstance().DataCommands
                                     select item.Name;

                        var tempList = retVal.ToList<String>();
                        tempList.Insert(0, String.Empty);

                        string[] items = tempList.ToArray<string>();
                        list = new StandardValuesCollection(items);
                    }

                    

                }




                




            }

            if (context.Instance is TreeViewControl)
            {
                TreeViewControl control = (TreeViewControl)context.Instance;


                if (control.Parent is BaseSection)
                {
                    BaseSection section = (BaseSection)control.Parent;

                    
                    if (section.Parent != null)
                    {
                        if (section.Parent is Screen)
                        {
                            Screen parentScreen = (Screen)section.Parent;
                            var retVal = from item in parentScreen.DataCommands
                                         select item.Name;

                            var tempList = retVal.ToList<String>();
                            tempList.Insert(0, String.Empty);

                            string[] items = tempList.ToArray<string>();
                            list = new StandardValuesCollection(items);
                        }

                        

                    }
                    else
                    {
                        //handle bug during initial creation where we donot have a parent
                        var retVal = from item in Configuration.GetInstance().DataCommands
                                     select item.Name;

                        var tempList = retVal.ToList<String>();
                        tempList.Insert(0, String.Empty);

                        string[] items = tempList.ToArray<string>();
                        list = new StandardValuesCollection(items);
                    }

                    

                }




               




            }

            if (context.Instance is WorkflowStatusControl)
            {
                WorkflowStatusControl workflowStatus = (WorkflowStatusControl)context.Instance;

                if (workflowStatus.Parent is BaseSection)
                {
                    BaseSection section = (BaseSection)workflowStatus.Parent;

                    
                    if (section.Parent != null)
                    {
                        if (section.Parent is Screen)
                        {
                            Screen parentScreen = (Screen)section.Parent;
                            var retVal = from item in parentScreen.DataCommands
                                         select item.Name;

                            var tempList = retVal.ToList<String>();
                            tempList.Insert(0, String.Empty);

                            string[] items = tempList.ToArray<string>();
                            list = new StandardValuesCollection(items);
                        }

                        

                    }
                    else
                    {
                        //handle bug during initial creation where we do not have a parent
                        var retVal = from item in Configuration.GetInstance().DataCommands
                                     select item.Name;

                        var tempList = retVal.ToList<String>();
                        tempList.Insert(0, String.Empty);

                        string[] items = tempList.ToArray<string>();
                        list = new StandardValuesCollection(items);
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
