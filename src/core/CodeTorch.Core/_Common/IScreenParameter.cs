using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace CodeTorch.Core
{

    public interface IScreenParameter
    {

        string Name { get; set; }
        ScreenInputType InputType { get; set; }
        string InputKey { get; set; }
        string Default { get; set; }
        object Value { get; set; }

        
    }
}
