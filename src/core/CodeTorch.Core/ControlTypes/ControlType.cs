using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Design;

namespace CodeTorch.Core
{
    [Serializable]
    public class ControlType
    {
        public string Name { get; set; }
        public string Assembly { get; set; }
        public string Class { get; set; }

        public string AbstractionAssembly { get; set; }
        public string AbstractionClass { get; set; }


        private StringCollection _Actions = new StringCollection();
        private StringCollection _Routines = new StringCollection();

        [XmlArray("Actions")]
        [XmlArrayItem("Action")]
        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a",typeof(System.Drawing.Design.UITypeEditor))] 
        public StringCollection Actions
        {
            get
            {
                return _Actions;
            }
            set
            {
                _Actions = value;
            }

        }

        [XmlArray("Routines")]
        [XmlArrayItem("Routine")]
        [Editor(@"System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(System.Drawing.Design.UITypeEditor))]
        public StringCollection Routines
        {
            get
            {
                return _Routines;
            }
            set
            {
                _Routines = value;
            }

        }

       
        public static void Load(XDocument doc)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ControlType));
            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            ControlType item = null;

            try
            {
                item = (ControlType)serializer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(String.Format("Error occurred while processing ControlType - {0}", doc.Root.FirstNode.ToString()), ex);
            }

            Configuration.GetInstance().ControlTypes.Add(item);

        }

        public static void Save(ControlType item)
        {
            string ConfigPath = ConfigurationLoader.GetFileConfigurationPath();

            if (!Directory.Exists(String.Format("{0}ControlTypes", ConfigPath)))
            {
                Directory.CreateDirectory(String.Format("{0}ControlTypes", ConfigPath));
            }
            
            string filePath = String.Format("{0}ControlTypes\\{1}.xml", ConfigPath, item.Name);
            ConfigurationLoader.SerializeObjectToFile(item, filePath);
        
        }

        public static ControlType GetControlType(Widget control)
        {
            ControlType controlType = Configuration.GetInstance().ControlTypes
                            .Where(c =>
                                (
                                    (c.Name.ToLower() == control.Type.ToLower())
                                )
                            )
                            .SingleOrDefault();

            return controlType;
        }

        public static ControlType GetControlType(string controlTypeName)
        {
            ControlType controlType = Configuration.GetInstance().ControlTypes
                            .Where(c =>
                                (
                                    (c.Name.ToLower() == controlTypeName.ToLower())
                                )
                            )
                            .SingleOrDefault();

            return controlType;
        }






        internal static int GetItemCount(string Name)
        {
            int retVal=0;

            if (String.IsNullOrEmpty(Name))
            {
                retVal = Configuration.GetInstance().ControlTypes.Count;
            }
            else
            {
                retVal = Configuration.GetInstance().ControlTypes
                                .Where(c =>
                                    (
                                        (c.Name.ToLower() == Name.ToLower())
                                    )
                                ).Count();
            }

            return retVal;
        }

        public static Type[] GetTypeArray()
        {
            List<Type> types = new List<Type>();

            foreach (ControlType widget in Configuration.GetInstance().ControlTypes)
            {
                string assemblyQualifiedName = String.Format("{0}, {1}", widget.AbstractionClass, widget.AbstractionAssembly);
                Type t = Type.GetType(assemblyQualifiedName, false, true);

                if (t != null)
                {
                    types.Add(t);
                }
            }

            return types.ToArray();
        }
    }


}
