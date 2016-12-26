using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;


namespace CodeTorch.Core
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Menu
    {
        bool _RequiresAuthentication = true;
        private List<MenuItem> _Items = new List<MenuItem>();

        [ReadOnly(true)]
        public string Name { get; set; }

        [Category("Appearance")]
        public string CssClass { get; set; }

        
        [Category("Security")]
        public bool RequiresAuthentication { get { return _RequiresAuthentication; } set { _RequiresAuthentication = value; } }

        [XmlArray("Items")]
        [XmlArrayItem("Item")]
        [Description("List of root menu items")]
        public List<MenuItem> Items
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

        public static Menu Load(XDocument doc)
        {

            XmlSerializer serializer = new XmlSerializer(typeof(Menu));
            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            Menu menu = null;

            try
            {
                menu = (Menu)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Error occurred while processing Menu", doc.Root.FirstNode.ToString()), ex);
            }

            return menu;

        }

        public static int GetTotalMenuItemCount(Menu menu)
        {
            int retVal = 0;

            foreach(MenuItem item in menu.Items)
            {
                retVal += GetMenuItemCount(item);
            }

            return retVal;
        }

        public static int GetMenuItemCount(MenuItem item)
        {
            int retVal = 0;

            retVal += item.Items.Count;

            foreach (MenuItem i in item.Items)
            {
                if (i.Items.Count > 0)
                {
                    retVal += GetMenuItemCount(i);
                }
            }

            return retVal;
        }

        public static void Save(Menu item)
        {

            string ConfigPath = ConfigurationLoader.GetFileConfigurationPath();
            XmlSerializer x = new XmlSerializer(item.GetType());

            if (!Directory.Exists(String.Format("{0}Menus", ConfigPath)))
            {
                Directory.CreateDirectory(String.Format("{0}Menus", ConfigPath));
            }

            string filePath = String.Format("{0}Menus\\{1}.xml", ConfigPath, item.Name);
            ConfigurationLoader.SerializeObjectToFile(item, filePath);

        }

        public static Menu GetMenu(ScreenMenu screenMenu)
        {
            Menu menu = Configuration.GetInstance().Menus
                            .Where(m =>
                                (
                                    (m.Name.ToLower() == screenMenu.Name.ToLower())
                                )
                            )
                            .SingleOrDefault();

            return menu;
        }

        public static Menu GetMenu(string screenMenu)
        {
            Menu menu = Configuration.GetInstance().Menus
                            .Where(m =>
                                (
                                    (m.Name.ToLower() == screenMenu.ToLower())
                                )
                            )
                            .SingleOrDefault();

            return menu;
        }

        internal static int GetItemCount(string Name)
        {
            int retVal = 0;

            if (String.IsNullOrEmpty(Name))
            {
                retVal = Configuration.GetInstance().Menus.Count;
            }
            else
            {
                retVal = Configuration.GetInstance().Menus
                                .Where(i =>
                                    (
                                        (i.Name.ToLower() == Name.ToLower())
                                    )
                                ).Count();
            }

            return retVal;
        }

        public static List<ResourceItem> GetResourceKeys(Menu menu)
        {
            List<ResourceItem> retVal = new List<ResourceItem>();

            
            foreach (MenuItem item in menu.Items)
            {

                List<ResourceItem> childKeys = GetResourceKeys(item, menu.Name, String.Empty);
                retVal.AddRange(childKeys);
            }

            return retVal;
        }

        public static List<ResourceItem> GetResourceKeys(MenuItem item, string MenuName, string Prefix)
        {
            List<ResourceItem> retVal = new List<ResourceItem>();
            string newPrefix = null;

            if (String.IsNullOrEmpty(Prefix))
            {
                ResourceItem key = new Core.ResourceItem();

                key.ResourceSet = String.Format("Menu.{0}", MenuName);
                key.Key = String.Format("{0}.MenuItem.Name", item.Code);
                key.Value = item.Name;

                retVal.Add(key);
                newPrefix = String.Format("{0}", item.Code);
            }
            else
            {
                ResourceItem key = new Core.ResourceItem();

                key.ResourceSet = String.Format("Menu.{0}", MenuName);
                key.Key = String.Format("{0}.{1}.MenuItem.Name",  Prefix, item.Code);
                key.Value = item.Name;

                retVal.Add(key);
                
                newPrefix = String.Format("{0}.{1}", Prefix, item.Code);
            }

            
            
            foreach (MenuItem i in item.Items)
            {
                List<ResourceItem> childKeys = GetResourceKeys(i, MenuName, newPrefix);
                retVal.AddRange(childKeys);
            }

            return retVal;
        }
    }
}
