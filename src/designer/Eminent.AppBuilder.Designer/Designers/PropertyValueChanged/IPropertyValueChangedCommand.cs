using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.WinControls.UI;

namespace CodeTorch.Designer.Designers.PropertyValueChanged
{
    public interface IPropertyValueChangedCommand
    {
        void Execute(RadPropertyGrid grid);
    }
}
