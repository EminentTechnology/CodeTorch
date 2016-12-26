using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace CodeTorch.Core
{
    [Serializable]
    public class ScreenType
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TemplateFile { get; set; }

        public static ScreenType GetScreenType(Screen screen)
        {
            ScreenType screenType = Configuration.GetInstance().ScreenTypes
                            .Where(s =>
                                (
                                    (s.Name.ToLower() == screen.Type.ToLower())
                                )
                            )
                            .SingleOrDefault();

            return screenType;
        }

        public static ScreenType Load(System.Xml.Linq.XDocument doc)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ScreenType));
            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            ScreenType item = null;

            try
            {
                item = (ScreenType)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Error occurred while processing {0}", doc.Root.FirstNode.ToString()), ex);
            }

            return item;
        }

        public static void Save(ScreenType item)
        {
            string ConfigPath = ConfigurationLoader.GetFileConfigurationPath();

            
            string filePath = String.Format("{0}ScreenTypes\\{1}.xml", ConfigPath, item.Name);
            ConfigurationLoader.SerializeObjectToFile(item, filePath);

        }

        internal static int GetItemCount(string Name)
        {
            int retVal = 0;

            if (String.IsNullOrEmpty(Name))
            {
                retVal = Configuration.GetInstance().ScreenTypes.Count;
            }
            else
            {
                retVal = Configuration.GetInstance().ScreenTypes
                                .Where(i =>
                                    (
                                        (i.Name.ToLower() == Name.ToLower())
                                    )
                                ).Count();
            }

            return retVal;
        }

        public static ScreenType GetByName(string Name)
        {
            ScreenType item = Configuration.GetInstance().ScreenTypes
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
