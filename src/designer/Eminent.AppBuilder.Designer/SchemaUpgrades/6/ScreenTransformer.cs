using CodeTorch.Designer.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodeTorch.Designer.SchemaUpgrades._6
{
    class ScreenTransformer: ICodeTransformer
    {
        bool IsDirty = false;

        public bool Execute()
        {
            return ProcessScreen(Document);
        }

        public string EntityType {get;set;}
        public XDocument Document { get; set; }

        public List<string> GetSupportedEntityTypes()
        {
            List<String> types = new List<string>();

            types.Add("Screen");

            return types;
        }



        public bool ProcessScreen(XDocument Document)
        {
            IsDirty = false;


            IsDirty = ProcessSections(Document);

            return IsDirty;

        }

        private  bool ProcessSections(XDocument Document)
        {
            bool retVal = false;
            XElement Sections = Document.Root.Element("Sections");
            if (Sections != null)
            {
                foreach (XElement section in Sections.Elements())
                {


                    retVal = ProcessSection(Document, section);
                }
            }

            return retVal;

        }

        private  bool ProcessSection(XDocument Document, XElement section)
        {
            bool retVal = true;

            XNamespace xsi = "http://www.w3.org/2001/XMLSchema-instance";

            string localName = section.Name.LocalName;

            if (localName == "EditableGridSection")
            {
                XElement gridSections = section.Element("Sections");
                if (gridSections != null)
                {
                    foreach (XElement gridSection in gridSections.Elements())
                    {
                        ProcessSection(Document, gridSection);

                    }
                }
            }

            section.Name = "Section";
            section.SetAttributeValue(xsi + "type", localName);

            return retVal;
        }



        
    }
}
