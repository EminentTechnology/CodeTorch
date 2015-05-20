using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace CodeTorch.Core
{
    public class BaseSection
    {

        public string Name { get; set; }
        public virtual string Type { get; set; }
    }
}
