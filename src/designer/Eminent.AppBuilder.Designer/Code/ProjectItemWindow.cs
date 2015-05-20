using CodeTorch.Core.Attributes;
using CodeTorch.Designer.Designers.PropertyValueChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;


namespace CodeTorch.Designer.Code
{
    public class ProjectItemWindow : DocumentWindow
    {
        static readonly Dictionary<string, ProjectItemWindow> instance = new Dictionary<string, ProjectItemWindow>();
        public string Key { get; set; }

        public static void Remove(ProjectItemWindow window)
        {
            if (instance.ContainsKey(window.Key))
            {
                instance.Remove(window.Key);
            }
        }

        RadPropertyGrid PropertyGrid;
        Dictionary<string, string> editors;
        Dictionary<string, string> onValueChangedCommands;

        public static ProjectItemWindow GetInstance(SolutionTreeNode node)
        {
            return GetInstance(node.Text, node.Key, node.Object, node);
        }

        public static ProjectItemWindow GetInstance(string title, string objectKey, object configObject, object Tag)
        {
            ProjectItemWindow retVal = null;

            if (instance.ContainsKey(objectKey))
            {
                retVal = instance[objectKey];
            }
            else
            { 

                RadPropertyGrid propertyGrid = new RadPropertyGrid();
                propertyGrid = new RadPropertyGrid();
                propertyGrid.Dock = DockStyle.Fill;
                propertyGrid.SelectedObject = configObject;
                propertyGrid.ToolbarVisible = true;
                propertyGrid.PropertySort = PropertySort.CategorizedAlphabetical;
                

                ProjectItemWindow window = new ProjectItemWindow(propertyGrid);
                window.Key = objectKey;
                window.Tag = Tag;
                window.Text = title;
                window.editors = GetCustomEditors(configObject);
                window.onValueChangedCommands = GetValueChangedCommands(configObject);
                

                window.Controls.Add(window.PropertyGrid);



                instance.Add(objectKey, window);

                retVal = window;
            }
            return retVal;
        }

        private static Dictionary<string, string> GetCustomEditors(object o)
        {
            Dictionary<string, string> retVal = new Dictionary<string, string>();

            Type t = o.GetType(); ;
            PropertyInfo[] properties = t.GetProperties();

            for (int i = 0; i < properties.Length; i++)
            {
                CustomEditorAttribute editor = (CustomEditorAttribute)Attribute.GetCustomAttribute(properties[i], typeof(CustomEditorAttribute));

                if (editor != null)
                {
                    retVal.Add(properties[i].Name, editor.Editor);   
                }
            }

            return retVal;

        }

        private static Dictionary<string, string> GetValueChangedCommands(object o)
        {
            Dictionary<string, string> retVal = new Dictionary<string, string>();

            Type t = o.GetType();
            PropertyInfo[] properties = t.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < properties.Length; i++)
            {
                PropertyValueChangedCommandAttribute command = (PropertyValueChangedCommandAttribute)Attribute.GetCustomAttribute(properties[i], typeof(PropertyValueChangedCommandAttribute));

                if (command != null)
                {
                    retVal.Add(command.PropertyName, command.Command);
                }
            }

            return retVal;

        }

        private ProjectItemWindow(RadPropertyGrid propertyGrid)
        {
            this.PropertyGrid = propertyGrid;

            PropertyGrid.CreateItemElement += PropertyGrid_CreateItemElement;
            PropertyGrid.Editing += PropertyGrid_Editing;
            PropertyGrid.PropertyValueChanged += PropertyGrid_PropertyValueChanged;
           
            
        }

        

       

       

        void PropertyGrid_PropertyValueChanged(object sender, PropertyGridItemValueChangedEventArgs e)
        {
            
            //if this property has an on value changed command
            string propertyName = GetLongPropertyName(e.Item);
            if (onValueChangedCommands.ContainsKey(propertyName))
            {
                string commandClassName = onValueChangedCommands[propertyName];

                if (!String.IsNullOrEmpty(commandClassName))
                {
                    //create the command dynamically and execute it
                    Assembly a = Assembly.GetExecutingAssembly();
                    string fullClassName = String.Format("CodeTorch.Designer.Designers.PropertyValueChanged.{0}", commandClassName);
                    IPropertyValueChangedCommand command = (IPropertyValueChangedCommand) a.CreateInstance(fullClassName,true);
                    command.Execute(this.PropertyGrid);
                }
            }

            
        }

        private string GetLongPropertyName(PropertyGridItemBase item)
        {
            string retVal = item.Name;

            if (item.Parent != null)
            {
                retVal = String.Format("{0}.{1}", GetLongPropertyName(item.Parent), item.Name);
            }

            return retVal;
        }

        

        void PropertyGrid_CreateItemElement(object sender, CreatePropertyGridItemElementEventArgs e)
        {
            if (editors.ContainsKey(e.Item.Name))
            {
                string editor = editors[e.Item.Name];

                if (!String.IsNullOrEmpty(editor))
                {
                    e.ItemElementType = Type.GetType(String.Format("CodeTorch.Designer.Editors.{0}", editor));
                }
            }
        }

        void PropertyGrid_Editing(object sender, PropertyGridItemEditingEventArgs e)
        {
            if (editors.ContainsKey(e.Item.Name))
            {
                string editor = editors[e.Item.Name];

                if (!String.IsNullOrEmpty(editor))
                {
                    e.Cancel = true;
                }
            }

        }


        
    }
}
