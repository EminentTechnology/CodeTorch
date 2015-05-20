using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Serialization;

namespace CodeTorch.Core
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    [Serializable]
    public class Document
    {
        //provided by config
        public string EntityID { get; set; }//specific customer 123

        public string EntityType { get; set; }//customer

        public string DocumentType { get; set; }

        //provided by file upload client/provider
        [Browsable(false)]
        [XmlIgnore()]
        public string FileName { get; set; }

        [Browsable(false)]
        [XmlIgnore()]
        public string ID { get; set; }

        [Browsable(false)]
        [XmlIgnore()]
        public string ContentType { get; set; }

        [Browsable(false)]
        [XmlIgnore()]
        public byte File { get; set; }

        [Browsable(false)]
        [XmlIgnore()]
        public string Url { get; set; }

        [Browsable(false)]
        [XmlIgnore()]
        public int Size { get; set; }

        [Browsable(false)]
        [XmlIgnore()]
        public Stream Stream { get; set; }

        List<Setting> _settings = new List<Setting>();

        [XmlArray("Settings")]
        [XmlArrayItem("Setting")]
        public List<Setting> Settings
        {
            get
            {
                return _settings;
            }
            set
            {
                _settings = value;
            }
        }
    }
}