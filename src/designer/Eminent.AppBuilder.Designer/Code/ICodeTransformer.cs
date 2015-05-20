using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodeTorch.Designer.Code
{
    public interface ICodeTransformer
    {
        string EntityType { get; set; }
        XDocument Document { get; set; }
        List<string> GetSupportedEntityTypes();

        bool Execute();
    }
}
