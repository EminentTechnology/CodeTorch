using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Xml;
using CodeTorch.Core;
using System.IO;
using System.Drawing.Design;

namespace CodeTorch.Core
{
    [Serializable]
    public class RestService
    {
        [ReadOnly(true)]
        public string Name { get; set; }

        public string EntityName { get; set; }
        public string EntityCollectionName { get; set; }

        public string Resource { get; set; }

        [Browsable(false)]
        public string Folder { get; set; } = "Services";

        public bool SupportJSON { get; set; } = true;
        public bool SupportXML { get; set; }

        List<BaseRestServiceMethod> methods = new List<BaseRestServiceMethod>();

        [XmlArray("Methods")]
        [Description("List all rest methods")]
        [XmlArrayItem(ElementName = "Get", Type = typeof(GetRestServiceMethod))]
        [XmlArrayItem(ElementName = "Post", Type = typeof(PostRestServiceMethod))]
        [XmlArrayItem(ElementName = "Put", Type = typeof(PutRestServiceMethod))]
        [XmlArrayItem(ElementName = "Delete", Type = typeof(DeleteRestServiceMethod))]
#if NETFRAMEWORK
        [Editor("CodeTorch.Core.Design.RestServiceMethodCollectionEditor,CodeTorch.Core.Design", typeof(UITypeEditor))]
#endif
        public virtual List<BaseRestServiceMethod> Methods
        {
            get
            {
                return methods;
            }
            set
            {
                methods = value;
            }

        }

        

        public static RestService Load(XDocument doc)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RestService));
            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            RestService item = null;

            try
            {
                item = (RestService)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Error occurred while processing RestService - {0}", doc.Root.FirstNode.ToString()), ex);
            }

            return item;

        }

        public static void Save(RestService item)
        {


            string ConfigPath = ConfigurationLoader.GetFileConfigurationPath();

            if (!Directory.Exists(String.Format("{0}RestServices\\{1}", ConfigPath, item.Folder)))
            {
                Directory.CreateDirectory(String.Format("{0}RestServices\\{1}", ConfigPath, item.Folder));
            }

            string filePath = String.Format("{0}RestServices\\{1}\\{2}.xml", ConfigPath, item.Folder, item.Name);

            ConfigurationLoader.SerializeObjectToFile(item, filePath);
            ConfigurationLoader.SerializeObjectToFile(item, filePath);

        }



        public static RestService GetByFolderAndName(string FolderName, string ScreenName)
        {
            RestService service = Configuration.GetInstance().RestServices
                            .Where(s =>
                                (
                                    (s.Folder.ToLower() == FolderName.ToLower()) &&
                                    (s.Name.ToLower() == ScreenName.ToLower())
                                )
                            )
                            .SingleOrDefault();

            return service;
        }

        public static List<RestService> GetByFolder(string FolderName)
        {
            var retVal = from item in Configuration.GetInstance().RestServices
                         where item.Folder.ToLower() == FolderName.ToLower()
                         select item;
            return retVal.ToList<RestService>();
        }

        public static List<string> GetDistinctFolders()
        {
            var retVal = (
                            from item in Configuration.GetInstance().RestServices
                            select item.Folder
                         )
                         .Distinct()
                         ;

            return retVal.ToList<string>();
        }

        internal static int GetItemCount(string Folder, string Name)
        {
            int retVal = 0;


            retVal = Configuration.GetInstance().RestServices
                            .Where(i =>
                                (
                                    (((!String.IsNullOrEmpty(Name)) && (i.Name.ToLower() == Name.ToLower())) || (String.IsNullOrEmpty(Name))) &&
                                    (((!String.IsNullOrEmpty(Folder)) && (i.Folder.ToLower() == Folder.ToLower())) || (String.IsNullOrEmpty(Folder)))
                                )
                            ).Count();


            return retVal;
        }
    }
}
