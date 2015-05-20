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
    public class MobileContentScreen:   MobileScreen
    {
        public static MobileContentScreen Load(string Name)
        {



            MobileContentScreen retVal = null;
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

        public static MobileContentScreen Load(XDocument doc)
        {


            XmlSerializer serializer = new XmlSerializer(typeof(MobileContentScreen));

            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            MobileContentScreen screen = null;

            try
            {

                screen = (MobileContentScreen)serializer.Deserialize(reader);



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
