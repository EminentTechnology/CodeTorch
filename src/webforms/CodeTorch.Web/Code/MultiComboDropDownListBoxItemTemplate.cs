using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeTorch.Web.FieldTemplates;
using CodeTorch.Core;
using Telerik.Web.UI;

namespace CodeTorch.Web
{
    public class MultiComboDropDownListBoxItemTemplate:ITemplate
    {
        ListItemType templateType;
        MultiComboDropDownListControl Me;
        public string ResourceKeyPrefix = "";
        MultiComboDropDownListBox ctrl;

        public MultiComboDropDownListBoxItemTemplate(MultiComboDropDownListBox ctrl, ListItemType type, MultiComboDropDownListControl control, string ResourceKeyPrefix)
        {
            this.ctrl = ctrl;
            templateType = type;
            Me = control;
            this.ResourceKeyPrefix = ResourceKeyPrefix;
        }


        public void InstantiateIn(Control container)
        {
            PlaceHolder ph = new PlaceHolder();
            StringBuilder builder = null;
            switch (templateType)
            { 
                case ListItemType.Header:
                    builder = new StringBuilder();
                    builder.Append("<ul class='MultiComboDropDown'>");
                    foreach (MultiComboDropDownListColumn col in Me.Columns)
                    {
                        string resourceKey = String.Format("Columns.{0}.HeaderText", col.DataField);
                        string headerText = ctrl.GetGlobalResourceString(resourceKey, col.HeaderText);
                        builder.AppendFormat("<li class='header' style='width:{1}'>{0}</li>", headerText, col.Width);
                    }
                    builder.Append("</ul>");

                    ph.Controls.Add(
                        new LiteralControl(
                                builder.ToString()
                                )
                        );

                    break;

                case ListItemType.Item:
                    
                    ph.Controls.Add(new LiteralControl("<ul class='MultiComboDropDown'>"));
                    
                    foreach (MultiComboDropDownListColumn col in Me.Columns)
                    {
                        System.Web.UI.WebControls.Label item = new System.Web.UI.WebControls.Label();
                        item.ID = col.DataField;

                        ph.Controls.Add(new LiteralControl(String.Format("<li class='item' style='width:{0}'>", col.Width)));
                        ph.Controls.Add(item);
                        ph.Controls.Add(new LiteralControl("</li>"));

                        
                    }
                    ph.Controls.Add(new LiteralControl("</ul>"));

                    ph.DataBinding += new EventHandler(ItemTemplate_DataBinding);
                    break;
            }

            container.Controls.Add(ph);


        }

        void ItemTemplate_DataBinding(object sender, EventArgs e)
        {
             PlaceHolder ph = (PlaceHolder)sender;
               RadComboBoxItem ri = (RadComboBoxItem)ph.NamingContainer;

               foreach (MultiComboDropDownListColumn col in Me.Columns)
               {
                   string data = (string)DataBinder.Eval(ri.DataItem, col.DataField);

                   System.Web.UI.WebControls.Label item = (System.Web.UI.WebControls.Label)ph.FindControl(col.DataField);
                   if (item != null)
                   {
                       if (String.IsNullOrEmpty(data))
                       {
                           item.Text = "&nbsp;";
                       }
                       else
                       {
                           item.Text = data;
                       }
                   }
                  
               }

        }

        
    }
}
