using Eminent.AppBuilder.Core;
using Eminent.AppBuilder.Core.Attributes;
using Eminent.AppBuilder.Core.Designers;
using Eminent.AppBuilder.Designer.Designers.PropertyValueChanged;
using Eminent.AppBuilder.Designer.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;

namespace Eminent.AppBuilder.Designer.Code
{
    public class EditScreenWindow : DocumentWindow
    {
        static readonly Dictionary<string, EditScreenWindow> instance = new Dictionary<string, EditScreenWindow>();
        public string Key { get; set; }

        public static void Remove(EditScreenWindow window)
        {
            if (instance.ContainsKey(window.Key))
            {
                instance.Remove(window.Key);
            }
        }

        RadPropertyGrid PropertyGrid;
        Dictionary<string, string> editors;
        Dictionary<string, string> onValueChangedCommands;

        public static EditScreenWindow GetInstance(SolutionTreeNode node)
        {
            EditScreenWindow retVal = null;

            if (instance.ContainsKey(node.Key))
            {
                retVal = instance[node.Key];
            }
            else
            {

                retVal = RenderScreen(node, retVal);
            }
            return retVal;
        }

        private static EditScreenWindow RenderScreen(SolutionTreeNode node, EditScreenWindow retVal)
        {
            EditScreen Me = (EditScreen)node.Object;

            RadPropertyGrid propertyGrid = new RadPropertyGrid();
            propertyGrid = new RadPropertyGrid();
            propertyGrid.Dock = DockStyle.Fill;
            propertyGrid.SelectedObject = node.Object;
            propertyGrid.ToolbarVisible = true;
            propertyGrid.PropertySort = PropertySort.Categorized;


            EditScreenWindow window = new EditScreenWindow(propertyGrid);
            window.Key = node.Key;
            window.Tag = node;
            window.Text = node.Text;
            window.editors = GetCustomEditors(node.Object);
            window.onValueChangedCommands = GetValueChangedCommands(node.Object);


            //window.Controls.Add(window.PropertyGrid);

            TableLayoutPanel screen = new TableLayoutPanel();

            screen.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            screen.ColumnCount = 1;
            screen.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));

            screen.RowCount = 3;
            screen.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            screen.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            screen.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));

            screen.Dock = DockStyle.Fill;

            //add textbox for title
            TextBox title = new TextBox();
            title.Font = new System.Drawing.Font("Verdana", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            screen.Controls.Add(title, 0, 0);
            title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            title.Location = new System.Drawing.Point(0, 0);
            title.DataBindings.Add("Text", Me, "Title.Name");


            //add textbox for subtitle
            TextBox subtitle = new TextBox();
            screen.Controls.Add(subtitle, 0, 1);
            subtitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            subtitle.Location = new System.Drawing.Point(0, 0);
            subtitle.DataBindings.Add("Text", Me, "SubTitle.Name");

            TableLayoutPanel zoneContainer = SetupSectionZoneLayout(Me);
            screen.Controls.Add(zoneContainer, 0, 2);
            

            window.Controls.Add(screen);


            instance.Add(node.Key, window);

            retVal = window;
            return retVal;
        }

        private static TableLayoutPanel SetupSectionZoneLayout(EditScreen Me)
        {
            TableLayoutPanel zoneContainer = new TableLayoutPanel();

            SectionZoneLayout layout = null;
            int RowCount = 1;
            int ColumnCount = 1;

            int ActiveRowCount = 1;
            int ActiveColumnCount = 1;

            int InactiveColumnCount = 0;
            int InactiveRowCount = 0;

            if(!String.IsNullOrEmpty(Me.SectionZoneLayout))
            {
                layout = SectionZoneLayout.GetByName(Me.SectionZoneLayout);
            }
            else
            {
                string defaultZoneLayout = Eminent.AppBuilder.Core.Configuration.GetInstance().App.DefaultZoneLayout;
                if(!String.IsNullOrEmpty(defaultZoneLayout))
                {
                    layout = SectionZoneLayout.GetByName(defaultZoneLayout);
                }
            }

            if(layout != null)
            {
                //add zone container
                
                //todo - need to remove it from here
                //zoneContainer.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
                zoneContainer.ColumnCount = layout.ColumnCount;
                //zoneContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));

                if (!String.IsNullOrEmpty(layout.DesignerColumnWidths))
                {
                    string[] colWidths = layout.DesignerColumnWidths.Split(',');
                    for (int i = 0; i < colWidths.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(colWidths[i]))
                        {
                            if (colWidths[i].Trim().EndsWith("%"))
                            {
                                float width = (float)Convert.ToDouble(colWidths[i].Replace("%", ""));
                                zoneContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, width));


                            }
                            else
                            {
                                float width = (float)Convert.ToDouble(colWidths[i].Replace("%", ""));
                                zoneContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, width));
                            }
                        }
                        else
                        {
                            zoneContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.AutoSize));
                        }
                    }
                }


                zoneContainer.RowCount = layout.RowCount;
                if (!String.IsNullOrEmpty(layout.DesignerRowWidths))
                {
                    string[] rowWidths = layout.DesignerRowWidths.Split(',');
                    for (int i = 0; i < rowWidths.Length; i++)
                    {
                        if (!String.IsNullOrEmpty(rowWidths[i]))
                        {
                            if (rowWidths[i].Trim().EndsWith("%"))
                            {
                                float width = (float)Convert.ToDouble(rowWidths[i].Replace("%", ""));
                                zoneContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, width));
                            }
                            else
                            {
                                float width = (float)Convert.ToDouble(rowWidths[i].Replace("%", ""));
                                zoneContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, width));
                            }
                        }
                        else
                        {
                            zoneContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
                        }
                    }
                }
            }

            
            zoneContainer.Dock = DockStyle.Fill;

            foreach(SectionDivider z in layout.Zones)
            {
                SectionZoneDesigner zone = new SectionZoneDesigner();
                zone.Dock = DockStyle.Fill;
                zone.Screen = Me;
                zone.Zone = z;
                zone.InitControl();
                //zone.s
                zoneContainer.Controls.Add(zone, z.ColumnIndex, z.RowIndex);
                zoneContainer.SetColumnSpan(zone, z.ColumnSpan);
                zoneContainer.SetRowSpan(zone, z.RowSpan);


            }
            return zoneContainer;
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

        private EditScreenWindow(RadPropertyGrid propertyGrid)
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
                    string fullClassName = String.Format("Eminent.AppBuilder.Designer.Designers.PropertyValueChanged.{0}", commandClassName);
                    IPropertyValueChangedCommand command = (IPropertyValueChangedCommand)a.CreateInstance(fullClassName, true);
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
                    e.ItemElementType = Type.GetType(String.Format("Eminent.AppBuilder.Designer.Editors.{0}", editor));
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
