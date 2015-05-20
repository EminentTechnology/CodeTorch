using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    public class Sequence
    {
        Int64 _SeedValue = 1;
        Int64 _Increment = 1;

        [ReadOnly(true)]
        public string Name { get; set; }

        public string Prefix { get; set; }

        public Int64 SeedValue { get { return _SeedValue; } set { _SeedValue = value; } }
        public Int64 Increment { get { return _Increment; } set { _Increment = value; } }

        public static void Load(XDocument doc)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Sequence));
            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            Sequence item = null;

            try
            {
                item = (Sequence)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Error occurred while processing Sequence - {0}", doc.Root.FirstNode.ToString()), ex);
            }

            Configuration.GetInstance().Sequences.Add(item);

        }

        public static void Save(Sequence item)
        {
            string ConfigPath = ConfigurationLoader.GetFileConfigurationPath();

            if (!Directory.Exists(String.Format("{0}Sequences", ConfigPath)))
            {
                Directory.CreateDirectory(String.Format("{0}Sequences", ConfigPath));
            }
            
            string filePath = String.Format("{0}Sequences\\{1}.xml", ConfigPath, item.Name);
            ConfigurationLoader.SerializeObjectToFile(item, filePath);

        }

        internal static int GetItemCount(string Name)
        {
            int retVal = 0;

            if (String.IsNullOrEmpty(Name))
            {
                retVal = Configuration.GetInstance().Sequences.Count;
            }
            else
            {
                retVal = Configuration.GetInstance().Sequences
                                .Where(i =>
                                    (
                                        (i.Name.ToLower() == Name.ToLower())
                                    )
                                ).Count();
            }

            return retVal;
        }

        public static Sequence GetByName(string Name)
        {
            Sequence item = Configuration.GetInstance().Sequences
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
