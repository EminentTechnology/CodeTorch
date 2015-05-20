using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    public class PageTemplateItem
    {
        public string Name { get; set; }
        
        public string Assembly { get; set; }
        public string Class { get; set; }
        public string Path { get; set; }

        public string Property { get; set; }
        public string Value { get; set; }


        public override string ToString()
        {
            return Name;
        }
    }
}
