﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;
namespace CodeTorch.Core
{
    [Serializable]
    public class SectionType
    {
        public string Name { get; set; }
        public string Assembly { get; set; }
        public string Class { get; set; }

        public string AbstractionAssembly { get; set; }
        public string AbstractionClass { get; set; }


        public static SectionType Load(XDocument doc)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SectionType));
            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            SectionType item = null;

            try
            {
                item = (SectionType)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Error occurred while processing SectionType", doc.Root.FirstNode.ToString()), ex);
            }

            return item;

        }

        public static void Save(SectionType item)
        {
            string ConfigPath = ConfigurationLoader.GetFileConfigurationPath();

            
            string filePath = String.Format("{0}SectionTypes\\{1}.xml", ConfigPath, item.Name);
            ConfigurationLoader.SerializeObjectToFile(item, filePath);

        }

        public static SectionType GetSectionType(Section item)
        {
            SectionType sectionType = Configuration.GetInstance().SectionTypes
                            .Where(s =>
                                (
                                    (s.Name.ToLower() == item.Type.ToLower())
                                )
                            )
                            .SingleOrDefault();

            return sectionType;
        }

        internal static int GetItemCount(string Name)
        {
            int retVal = 0;

            if (String.IsNullOrEmpty(Name))
            {
                retVal = Configuration.GetInstance().SectionTypes.Count;
            }
            else
            {
                retVal = Configuration.GetInstance().SectionTypes
                                .Where(i =>
                                    (
                                        (i.Name.ToLower() == Name.ToLower())
                                    )
                                ).Count();
            }

            return retVal;
        }

        public static SectionType GetByName(string Name)
        {
            SectionType item = Configuration.GetInstance().SectionTypes
                            .Where(i =>
                                (
                                    (i.Name.ToLower() == Name.ToLower())
                                )
                            )
                            .SingleOrDefault();

            return item;
        }

        private static Type[] loadedTypes = null;
        public static Type[] GetTypeArray()
        {

            if (loadedTypes == null)
            {
                List<Type> types = new List<Type>();

                foreach (SectionType section in Configuration.GetInstance().SectionTypes)
                {
                    string assemblyQualifiedName = String.Format("{0}, {1}", section.AbstractionClass, section.AbstractionAssembly);
                    Type t = Type.GetType(assemblyQualifiedName, false, true);

                    if (t != null)
                    {
                        types.Add(t);
                    }
                }

                loadedTypes = types.ToArray();
            }
            
            return loadedTypes;
        }
    }
}
