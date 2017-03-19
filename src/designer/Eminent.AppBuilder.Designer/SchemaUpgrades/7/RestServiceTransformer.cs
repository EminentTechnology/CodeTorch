using CodeTorch.Designer.Code;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CodeTorch.Designer.SchemaUpgrades._7
{
    class RestServiceTransformer: ICodeTransformer
    {
        

        public bool Execute()
        {

            

            ProcessRestService(Document);
            return true;
        }

        public string EntityType {get;set;}
        public XDocument Document { get; set; }

        public List<string> GetSupportedEntityTypes()
        {
            List<String> types = new List<string>();

            types.Add("RestService");

            return types;
        }

        public void GenerateServicesFolder(string configPath)
        {
            //create services folder
            string dirpath = String.Format(@"{0}RestServices\{1}\", configPath, "Services");
            if (!Directory.Exists(dirpath))
            {
                Directory.CreateDirectory(dirpath);
            }

            //move all files in 

            string rootFolderPath = String.Format(@"{0}RestServices\", configPath);
            string destinationPath = dirpath;
            string filesToMove = @"*.xml";
            string[] fileList = System.IO.Directory.GetFiles(rootFolderPath, filesToMove);
            foreach (string path in fileList)
            {
                string file = Path.GetFileName(path);

                string moveTo = destinationPath + file;
                //moving file
                File.Move(path, moveTo);

            }
        }

        private void ProcessRestService(XDocument document)
        {
            XElement Folder = Document.Root.Element("Folder");
            if (Folder == null)
            {
                Document.Root.Add(new XElement("Folder", "Services"));
            }

            XElement SupportJSON = Document.Root.Element("SupportJSON");
            if (SupportJSON == null)
            {
                Document.Root.Add(new XElement("SupportJSON", "true"));
            }

            XElement SupportXML = Document.Root.Element("SupportXML");
            if (SupportXML == null)
            {
                Document.Root.Add(new XElement("SupportXML", "false"));
            }
        }






    }
}
