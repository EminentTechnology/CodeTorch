using CodeTorch.Designer.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodeTorch.Designer.SchemaUpgrades._5
{
    public class SectionTypeTransformer : ICodeTransformer
    {
        public string EntityType { get; set; }
        public XDocument Document { get; set; }

        public bool Execute()
        {

            ProcessSectionType(Document);
            return true;

            
        }

        private void ProcessSectionType(XDocument document)
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
                    Document.Root.Add(new XElement("AbstractionClass", String.Format("CodeTorch.Core.{0}Section", SectionName)));
                }
            }
        }

        public List<string> GetSupportedEntityTypes()
        {
            List<String> types = new List<string>();

            types.Add("SectionType");

            return types;
        }
    }
}
