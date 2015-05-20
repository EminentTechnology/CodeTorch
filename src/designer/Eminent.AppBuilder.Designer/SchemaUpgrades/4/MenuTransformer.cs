using CodeTorch.Designer.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodeTorch.Designer.SchemaUpgrades._4
{
    class MenuTransformer: ICodeTransformer
    {
        

        public bool Execute()
        {
            return ProcessMenu(Document);
        }

        public string EntityType {get;set;}
        public XDocument Document { get; set; }

        public List<string> GetSupportedEntityTypes()
        {
            List<String> types = new List<string>();

            types.Add("Menu");

            return types;
        }
        
  

        public  bool ProcessMenu(XDocument Document)
        {
            bool retVal = false;

            XElement Name = Document.Root.Element("Name");
            if (Name != null)
            {
                if (!String.IsNullOrEmpty(Name.Value))
                {
                    if (Name.Value.ToLower() == "mainmenu")
                    { 
                        XElement CssClass = Document.Root.Element("CssClass");
                        if (CssClass == null)
                        {
                            Document.Root.Add(new XElement("CssClass", "menu"));
                            retVal = true;
                        }
                        

                    }
                }
            }


            return retVal;

        }

        


        
    }
}
