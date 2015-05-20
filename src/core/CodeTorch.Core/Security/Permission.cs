using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    public class Permission
    {
        [ReadOnly(true)]
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }



        public static void Load(XDocument doc)
        {
            var retVal = from permission in doc.Elements("Permission")
                         select new Permission
                         {
                             Name = permission.Element("Name").Value,
                             Category = permission.Element("Category").Value,
                             Description = permission.Element("Description").Value
                         };

            Permission val = retVal.First<Permission>();
            Configuration.GetInstance().Permissions.Add(val);

        }

        public static void Save(Permission item)
        {
            string ConfigPath = ConfigurationLoader.GetFileConfigurationPath();

            if (!Directory.Exists(String.Format("{0}Permissions", ConfigPath)))
            {
                Directory.CreateDirectory(String.Format("{0}Permissions", ConfigPath));
            }
            
            string filePath = String.Format("{0}Permissions\\{1}.xml", ConfigPath, item.Name);
            ConfigurationLoader.SerializeObjectToFile(item, filePath);

        }


        internal static int GetItemCount(string Name)
        {
            int retVal = 0;

            if (String.IsNullOrEmpty(Name))
            {
                retVal = Configuration.GetInstance().Permissions.Count;
            }
            else
            {
                retVal = Configuration.GetInstance().Permissions
                                .Where(i =>
                                    (
                                        (i.Name.ToLower() == Name.ToLower())
                                    )
                                ).Count();
            }

            return retVal;
        }

        public static Permission GetByName(string Name)
        {
            Permission item = Configuration.GetInstance().Permissions
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
