using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    public class Picker
    {
        int _Height = 600;
        int _Width = 600;

        [ReadOnly(true)]
        public string Name { get; set; }
        public string Url { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string DisplayField { get; set; }
        
        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string ValueField { get; set; }

        [TypeConverter("CodeTorch.Core.Design.DataCommandTypeConverter,CodeTorch.Core.Design")]
        [Category("Data")]
        public string DataCommand { get; set; }
        
        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string DataCommandParameter { get; set; }

        [Category("Appearance")]
        public int Height { get { return _Height; } set { _Height = value; } }

        [Category("Appearance")]
        public int Width { get { return _Width; } set { _Width = value; } }

        public static Picker Load(XDocument doc)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Picker));
            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            Picker item = null;

            try
            {
                item = (Picker)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Error occurred while processing Picker - {0}", doc.Root.FirstNode.ToString()), ex);
            }

            return item;

        }

        public static void Save(Picker item)
        {

            string ConfigPath = ConfigurationLoader.GetFileConfigurationPath();
            XmlSerializer x = new XmlSerializer(item.GetType());

            if (!Directory.Exists(String.Format("{0}Pickers", ConfigPath)))
            {
                Directory.CreateDirectory(String.Format("{0}Pickers", ConfigPath));
            }

            string filePath = String.Format("{0}Pickers\\{1}.xml", ConfigPath, item.Name);
            ConfigurationLoader.SerializeObjectToFile(item, filePath);

        }

        public static Picker GetPicker(string PickerName)
        {
            Picker picker = Configuration.GetInstance().Pickers
                            .Where(p =>
                                (
                                    (p.Name.ToLower() == PickerName.ToLower())
                                )
                            )
                            .SingleOrDefault();

            return picker;
        }

        internal static int GetItemCount(string Name)
        {
            int retVal = 0;

            if (String.IsNullOrEmpty(Name))
            {
                retVal = Configuration.GetInstance().Pickers.Count;
            }
            else
            {
                retVal = Configuration.GetInstance().Pickers
                                .Where(i =>
                                    (
                                        (i.Name.ToLower() == Name.ToLower())
                                    )
                                ).Count();
            }

            return retVal;
        }
    }
}
