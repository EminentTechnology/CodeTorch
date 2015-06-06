using CodeTorch.Designer.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodeTorch.Designer.SchemaUpgrades._6
{
    class AppTransformer : ICodeTransformer
    {
        public string EntityType { get; set; }
        public XDocument Document { get; set; }

        public bool Execute()
        {

            ProcessApp(Document);
            return true;


        }

        private void ProcessApp(XDocument document)
        {
            XElement DefaultErrorMessageFormatString = Document.Root.Element("DefaultErrorMessageFormatString");
            if (DefaultErrorMessageFormatString == null)
            {
                Document.Root.Add(new XElement("DefaultErrorMessageFormatString", "<strong>The following error(s) occurred</strong>:<ul><li>{0}</li></ul>"));
            }
        }

        public List<string> GetSupportedEntityTypes()
        {
            List<String> types = new List<string>();

            types.Add("App");

            return types;
        }
    }
}
