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
    public class SectionZoneLayout
    {
        [ReadOnly(true)]
        [Description("The name of this section zone layout")]
        public string Name { get; set; }

        List<SectionDivider> _dividers = new List<SectionDivider>();

        [XmlArray("Dividers")]
        [XmlArrayItem("Divider")]
        [Description("List of root divs visble in layout")]
        //[Category("Data")]
        //[Editor(typeof(ScreenDataCommandCollectionEditor), typeof(UITypeEditor))]
        public virtual List<SectionDivider> Dividers
        {
            get
            {
                return _dividers;
            }
            set
            {
                _dividers = value;
            }

        }

        public static void Load(XDocument doc)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SectionZoneLayout));
            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            SectionZoneLayout item = null;

            try
            {
                item = (SectionZoneLayout)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Error occurred while processing SectionZoneLayout - {0}", doc.Root.FirstNode.ToString()), ex);
            }

            Configuration.GetInstance().SectionZoneLayouts.Add(item);

        }

        public static void Save(SectionZoneLayout item)
        {
            string ConfigPath = ConfigurationLoader.GetFileConfigurationPath();

            if (!Directory.Exists(String.Format("{0}SectionZoneLayouts", ConfigPath)))
            {
                Directory.CreateDirectory(String.Format("{0}SectionZoneLayouts", ConfigPath));
            }

            string filePath = String.Format("{0}SectionZoneLayouts\\{1}.xml", ConfigPath, item.Name);
            ConfigurationLoader.SerializeObjectToFile(item, filePath);

        }

        internal static int GetItemCount(string Name)
        {
            int retVal = 0;

            if (String.IsNullOrEmpty(Name))
            {
                retVal = Configuration.GetInstance().SectionZoneLayouts.Count;
            }
            else
            {
                retVal = Configuration.GetInstance().SectionZoneLayouts
                                .Where(c =>
                                    (
                                        (c.Name.ToLower() == Name.ToLower())
                                    )
                                ).Count();
            }

            return retVal;
        }

        public static SectionZoneLayout GetByName(string Name)
        {
            SectionZoneLayout item = null;

            if (!String.IsNullOrEmpty(Name))
            {
                item = Configuration.GetInstance().SectionZoneLayouts
                                 .Where(i =>
                                     (
                                         (i.Name.ToLower() == Name.ToLower())
                                     )
                                 )
                                 .SingleOrDefault();
            }

            return item;
        }

        public List<String> GetDividerNames()
        {
            

            List<String> panes = new List<string>();

            foreach (SectionDivider d in this.Dividers)
            {
                FindZones(d, panes);
            }

            panes.Sort();

            return panes;
        }

        private void FindZones(SectionDivider div, List<String> panes)
        {
            if (!String.IsNullOrEmpty(div.Name))
            {
                panes.Add(div.Name);

                
            }

            if (div.Dividers.Count > 0)
            {
                foreach (SectionDivider d in div.Dividers)
                {
                    FindZones(d, panes);
                }
            }
        }
    }
}
