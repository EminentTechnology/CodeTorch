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


            ProcessSections(Document);

            return IsDirty;

        }

        private  void ProcessSections(XDocument Document)
        {
            XElement Sections = Document.Root.Element("Sections");
            if (Sections != null)
            {
                foreach (XElement section in Sections.Elements())
                {


                    ProcessSection(Document, section);
                }
            }

        }

        private  void ProcessSection(XDocument Document, XElement section)
        {
            if (section.Name.LocalName == "EditableGridSection")
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



            //process section controls
            if (section.Element("Controls") != null)
            {
                foreach (XElement control in section.Element("Controls").Elements())
                {
                    ProcessControl(Document, section, control);
                }
            }
        }

        private  void ProcessControl(XDocument Document, XElement section, XElement control)
        {
            ProcessFileUploadControl(control);

            
          
        }

        private void ProcessFileUploadControl(XElement control)
        {
            if (control.Name.ToString().ToLower() == "fileuploadcontrol")
            {
                XElement DocumentRepository = control.Element("DocumentRepository");
                if (DocumentRepository == null)
                {
                    //DocumentRepository should not exist

                    IsDirty = true;

                    string EntityID = string.Empty;
                    string EntityType = string.Empty;
                    string DocumentType = "TEMP";
                    string StorageMode = string.Empty;
                    string StorageProviderFolder = string.Empty;

                    XElement EntityIDElement = control.Element("EntityID");
                    if (EntityIDElement != null)
                    {
                        EntityID = EntityIDElement.Value;
                        EntityIDElement.Remove();
                    }

                    XElement EntityTypeElement = control.Element("EntityType");
                    if (EntityTypeElement != null)
                    {
                        EntityType = EntityTypeElement.Value;
                        EntityTypeElement.Remove();
                    }

                    XElement StorageModeElement = control.Element("StorageMode");
                    if (StorageModeElement != null)
                    {
                        StorageMode = StorageModeElement.Value;
                        StorageModeElement.Remove();
                    }

                    XElement StorageProviderFolderElement = control.Element("StorageProviderFolder");
                    if (StorageProviderFolderElement != null)
                    {
                        StorageProviderFolder = StorageProviderFolderElement.Value;
                        StorageProviderFolderElement.Remove();
                    }

                    XElement DocumentRepositoryElement = control.Element("DocumentRepository");
                    if (DocumentRepositoryElement == null)
                    {
                        if (StorageMode.ToLower() == "amazons3")
                        {
                            control.Add(new XElement("DocumentRepository", "S3"));
                        }
                        else
                        {
                            control.Add(new XElement("DocumentRepository", "DB"));
                        }

                    }

                    XElement DocumentElement = control.Element("Document");
                    if (DocumentElement == null)
                    {
                        DocumentElement = new XElement("Document");

                        DocumentElement.Add(new XElement("EntityID", EntityID));
                        DocumentElement.Add(new XElement("EntityType", EntityType));
                        DocumentElement.Add(new XElement("DocumentType", DocumentType));

                        if (StorageMode.ToLower() == "amazons3")
                        {
                            DocumentElement.Add(new XElement("Settings",
                                    new XElement("Setting",
                                        new XElement("Name", StorageProviderFolder),
                                        new XElement("Value", "")
                                        )
                                    )
                                 );
                        }

                        control.Add(DocumentElement);


                    }

                    //remove unused elements if they exist
                    RemoveElement(control, "StorageMode");
                    RemoveElement(control, "StorageProviderUserNameSource");
                    RemoveElement(control, "StorageProviderUserNameKey");
                    RemoveElement(control, "StorageProviderPasswordSource");
                    RemoveElement(control, "StorageProviderPasswordKey");
                    RemoveElement(control, "StorageProviderContainerKey");
                    RemoveElement(control, "StorageProviderContainerSource");
                    RemoveElement(control, "StorageProviderRegionKey");
                    RemoveElement(control, "StorageProviderRegionSource");
                }
            }
        }

        private static void RemoveElement(XElement control, string element)
        {
            
            if (control.Element(element) != null)
            {
                control.Element(element).Remove();
            }
        }


        
    }
}
