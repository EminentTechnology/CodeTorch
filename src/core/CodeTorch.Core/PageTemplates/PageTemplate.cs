using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    public class PageTemplate
    {
        private List<PageTemplateItem> _Items = new List<PageTemplateItem>();

        [ReadOnly(true)]
        public string Name { get; set; }
        public string Path { get; set; }

        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        public List<PageTemplateItem> Items
        {
            get
            {
                return _Items;
            }
            set
            {
                _Items = value;
            }

        }

        public static PageTemplate Load(XDocument doc)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PageTemplate));
            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            PageTemplate item = null;

            try
            {
                item = (PageTemplate)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Error occurred while processing PageTemplate - {0}", doc.Root.FirstNode.ToString()), ex);
            }

            return item;

        }

        public static void Save(PageTemplate item)
        {
            string ConfigPath = ConfigurationLoader.GetFileConfigurationPath();

            if (!Directory.Exists(String.Format("{0}PageTemplates", ConfigPath)))
            {
                Directory.CreateDirectory(String.Format("{0}PageTemplates", ConfigPath));
            }

            string filePath = String.Format("{0}PageTemplates\\{1}.xml", ConfigPath, item.Name);
            ConfigurationLoader.SerializeObjectToFile(item, filePath);

        }

        public static int GetItemCount(string Name)
        {
            int retVal = 0;

            if (String.IsNullOrEmpty(Name))
            {
                retVal = Configuration.GetInstance().PageTemplates.Count;
            }
            else
            {
                retVal = Configuration.GetInstance().PageTemplates
                                .Where(i =>
                                    (
                                        (i.Name.ToLower() == Name.ToLower())
                                    )
                                ).Count();
            }

            return retVal;
        }

        public static PageTemplate GetByName(string Name)
        {
            PageTemplate item = Configuration.GetInstance().PageTemplates
                            .Where(i =>
                                (
                                    (i.Name.ToLower() == Name.ToLower())
                                )
                            )
                            .SingleOrDefault();

            return item;
        }

        



    }
}
