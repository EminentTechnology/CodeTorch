using CodeTorch.Designer.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodeTorch.Designer.SchemaUpgrades._6
{
    public class ControlTypeTransformer : ICodeTransformer
    {
        public string EntityType { get; set; }
        public XDocument Document { get; set; }

        public bool Execute()
        {

            ProcessControlType(Document);
            return true;


        }

        private void ProcessControlType(XDocument document)
        {
            XElement Name = Document.Root.Element("Name");
            if (Name != null)
            {
                string SectionName = Name.Value;

                XElement AbstractionAssembly = Document.Root.Element("AbstractionAssembly");
                if (AbstractionAssembly == null)
                {
                    Document.Root.Add(new XElement("AbstractionAssembly", "CodeTorch.Core"));
                }

                XElement AbstractionClass = Document.Root.Element("AbstractionClass");
                if (AbstractionClass == null)
                {
                    Document.Root.Add(new XElement("AbstractionClass", String.Format("CodeTorch.Core.{0}Control", SectionName)));
                }
            }
        }

        public List<string> GetSupportedEntityTypes()
        {
            List<String> types = new List<string>();

            types.Add("ControlType");

            return types;
        }
    }
}
