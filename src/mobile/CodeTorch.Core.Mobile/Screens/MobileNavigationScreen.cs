using CodeTorch.Core.Platform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xamarin.Forms;

namespace CodeTorch.Core
{
    public class MobileNavigationScreen:   MobileScreen
    {
        OnPlatformString _BarBackgroundColor = new OnPlatformString();
        public OnPlatformString BarBackgroundColor
        {
            get
            {
                return this._BarBackgroundColor;
            }
            set
            {
                this._BarBackgroundColor = value;
            }
        }

        OnPlatformString _BarTextColor = new OnPlatformString();
        public OnPlatformString BarTextColor
        {
            get
            {
                return this._BarTextColor;
            }
            set
            {
                this._BarTextColor = value;
            }
        }

       

        OnPlatformString _RootScreen = new OnPlatformString();
        public OnPlatformString RootScreen
        {
            get
            {
                return this._RootScreen;
            }
            set
            {
                this._RootScreen = value;
            }
        }

        public static MobileNavigationScreen Load(string Name)
        {



            MobileNavigationScreen retVal = null;
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

        public static MobileNavigationScreen Load(XDocument doc)
        {


            XmlSerializer serializer = new XmlSerializer(typeof(MobileNavigationScreen));

            XmlReader reader = doc.CreateReader();
            reader.MoveToContent();

            MobileNavigationScreen screen = null;

            try
            {

                screen = (MobileNavigationScreen)serializer.Deserialize(reader);



                Configuration.GetInstance().MobileScreens.Add(screen);
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Error occurred while processing Screen - {0}", doc.Root.FirstNode.ToString()), ex);
            }

            return screen;

        }

        public string GetRootScreen()
        {
           
            string rootScreen = null;

            Device.OnPlatform(
                Android: () =>
                {
                    if (!String.IsNullOrEmpty(OnPlatformString.GetAndroidValue(this.RootScreen)))
                        rootScreen = OnPlatformString.GetAndroidValue(this.RootScreen);


                },
                iOS: () =>
                {
                    if (!String.IsNullOrEmpty(OnPlatformString.GetiOSValue(this.RootScreen)))
                        rootScreen = OnPlatformString.GetiOSValue(this.RootScreen);

                  
                },
                WinPhone: () =>
                {
                    if (!String.IsNullOrEmpty(OnPlatformString.GetWinPhoneValue(this.RootScreen)))
                        rootScreen = OnPlatformString.GetWinPhoneValue(this.RootScreen);

                  
                }
            );



            return rootScreen;
            
        }
    }
}
