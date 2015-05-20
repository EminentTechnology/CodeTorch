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
    class WorkflowTransformer: ICodeTransformer
    {
        

        public bool Execute()
        {
            ProcessWorkflow(Document);
            return true;
        }

        public string EntityType {get;set;}
        public XDocument Document { get; set; }

        public List<string> GetSupportedEntityTypes()
        {
            List<String> types = new List<string>();

            types.Add("Workflow");

            return types;
        }



        public void ProcessWorkflow(XDocument Document)
        {

            XElement WorkflowType = Document.Root.Element("WorkflowType");

            if (WorkflowType == null)
            {
                Document.Root.Add(new XElement("WorkflowType", "Default"));
            }
            
  

        }

        


        
    }
}
