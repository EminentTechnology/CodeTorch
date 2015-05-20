using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;
using System.ComponentModel;
using CodeTorch.Core.Interfaces;
using System.Reflection;

namespace CodeTorch.Core
{
    [Serializable]
    public class Lookup
    {
        [ReadOnly(true)]
        [Description("The name of this lookup - same as Lookup Type")]
        public string Name { get; set; }

        private List<LookupItem> _Items = new List<LookupItem>();


        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        public List<LookupItem> Items
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
        


        public static void Load(XDocument doc)
        {
            
            XmlSerializer serializer = new XmlSerializer(typeof(Lookup));
            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            Lookup item = null;

            try
            {
                item = (Lookup)serializer.Deserialize(reader);

                


            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Error occurred while processing Lookup - {0}", doc.Root.FirstNode.ToString()), ex);
            }

            Configuration.GetInstance().Lookups.Add(item);

        }

        public static void Save(Lookup item)
        {
            string ConfigPath = ConfigurationLoader.GetFileConfigurationPath();

            if (!Directory.Exists(String.Format("{0}Lookups", ConfigPath)))
            {
                Directory.CreateDirectory(String.Format("{0}Lookups", ConfigPath));
            }
            
            string filePath = String.Format("{0}Lookups\\{1}.xml", ConfigPath, item.Name);
            ConfigurationLoader.SerializeObjectToFile(item, filePath);

        }

        internal static int GetItemCount(string Name)
        {
            int retVal = 0;

            if (String.IsNullOrEmpty(Name))
            {
                retVal = Configuration.GetInstance().Lookups.Count;
            }
            else
            {
                retVal = Configuration.GetInstance().Lookups
                                .Where(i =>
                                    (
                                        (i.Name.ToLower() == Name.ToLower())
                                    )
                                ).Count();
            }

            return retVal;
        }

        public static Lookup GetByName(string Name)
        {
            Lookup item = Configuration.GetInstance().Lookups
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
