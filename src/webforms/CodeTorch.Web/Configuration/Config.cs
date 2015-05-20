using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CodeTorch.Web.Configuration
{
    public class Config
    {
        static readonly Config instance = new Config();

        public static Config GetInstance()
        {
            return instance;
        }

        private Config()
        {
               
        }

        public void Load(string ConfigurationPath)
        {
            ConfigurationPath = @"C:\sandbox\lasaa-sms\Code\Source\LasaaSMS\Configuration";

            this.ControlTypes.Clear();
            
            ControlType c;
            
            c = new ControlType();
            c.Name = "Label";
            c.FilePath = "~/standard/fieldtemplates/Label.ascx";
            this.ControlTypes.Add(c);

            c = new ControlType();
            c.Name = "TextBox";
            c.FilePath = "~/standard/fieldtemplates/TextBox.ascx";
            this.ControlTypes.Add(c);

            System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(c.GetType());

            TextWriter textWriter = null;
            
            textWriter = new StreamWriter(@"C:\control.xml");
            x.Serialize(textWriter, c);
            textWriter.Close();

            


        }

        List<ControlType> _ControlTypes = new List<ControlType>();
        public List<ControlType> ControlTypes
        {
            get
            {
                return _ControlTypes;
            }
        }

    }
}
