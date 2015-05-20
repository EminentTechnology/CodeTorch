using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CodeTorch.Core
{
    public class MobileTabbedScreen:   MobileScreen
    {
        List<String> _Tabs = new List<string>();
        public List<String> Tabs
        {
            get { return _Tabs; }
            set { _Tabs = value; }
        }

        public static MobileTabbedScreen Load(string Name)
        {



            MobileTabbedScreen retVal = null;
            string item = String.Format("{0}.{1}.{2}.xml", ConfigurationLoader.ConfigAssemblyName, "MobileScreens", Name);
            using (Stream fileStream = ConfigurationLoader.ConfigAssembly.GetManifestResourceStream(item))
            {
                using (XmlReader xreader = XmlReader.Create(fileStream))
                {
                    XDocument doc = XDocument.Load(xreader);

                    retVal = Load(doc);
                }
            }

            return retVal;

        }

        public static MobileTabbedScreen Load(XDocument doc)
        {


            XmlSerializer serializer = new XmlSerializer(typeof(MobileTabbedScreen));

            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            MobileTabbedScreen screen = null;

            try
            {

                screen = (MobileTabbedScreen)serializer.Deserialize(reader);



                Configuration.GetInstance().MobileScreens.Add(screen);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Error occurred while processing Screen - {0}", doc.Root.FirstNode.ToString()), ex);
            }

            return screen;

        }
    }
}
