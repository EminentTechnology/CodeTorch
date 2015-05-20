using CodeTorch.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.WinControls.UI;

namespace CodeTorch.Designer.Designers.PropertyValueChanged
{
    public class ScreenPageTemplateChangedCommand : IPropertyValueChangedCommand
    {

        void IPropertyValueChangedCommand.Execute(RadPropertyGrid grid)
        {
            object  o = grid.SelectedObject;

            if (o is Screen)
            {
                ScreenPageTemplate t = ((Screen)o).PageTemplate;

                PropertyGridItem item =  grid.Items["PageTemplate"];
                if (t.Mode == ScreenPageTemplateMode.Static)
                {

                    item.GridItems["Name"].Visible = true;

                    item.GridItems["DataCommand"].Visible = false;
                    item.GridItems["DataField"].Visible = false;
                }
                else
                {
                    item.GridItems["Name"].Visible = false;

                    item.GridItems["DataCommand"].Visible = true;
                    item.GridItems["DataField"].Visible = true;
                }
            }
        }
    }
}
