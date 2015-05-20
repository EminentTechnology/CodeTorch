using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using CodeTorch.Core.Platform;


namespace CodeTorch.Core
{
    public class MobileScreen
    {
        public string Name { get; set; }
        
        public string Type { get; set; }

        
        
        OnPlatformString _BackgroundImage = new OnPlatformString();
        public OnPlatformString BackgroundImage
        {
            get
            {
                return this._BackgroundImage;
            }
            set
            {
                this._BackgroundImage = value;
            }
        }
        
        OnPlatformString _BackgroundIcon = new OnPlatformString();
        public OnPlatformString BackgroundIcon
        {
            get
            {
                return this._BackgroundIcon;
            }
            set
            {
                this._BackgroundIcon = value;
            }
        }

        OnPlatformString _Title = new OnPlatformString();
        public OnPlatformString Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                this._Title = value;
            }
        }

        OnPlatformBool _DisplayNavigationBar = new OnPlatformBool(true);
        public OnPlatformBool DisplayNavigationBar
        {
            get
            {
                return this._DisplayNavigationBar;
            }
            set
            {
                this._DisplayNavigationBar = value;
            }
        }

        OnPlatformString _BackgroundColor = new OnPlatformString();
        public OnPlatformString BackgroundColor
        {
            get
            {
                return this._BackgroundColor;
            }
            set
            {
                this._BackgroundColor = value;
            }
        }

        public virtual bool RequireAuthentication { get; set; }

        private List<BaseControl> _Controls = new List<BaseControl>();


        [XmlArray("Controls")]
        [XmlArrayItem(ElementName = "AbsoluteLayoutControl", Type = typeof(AbsoluteLayoutControl))]
        [XmlArrayItem(ElementName = "ActivityIndicatorControl", Type = typeof(ActivityIndicatorControl))]
        [XmlArrayItem(ElementName = "BoxViewControl", Type = typeof(BoxViewControl))]
        [XmlArrayItem(ElementName = "ButtonControl", Type = typeof(ButtonControl))]
        [XmlArrayItem(ElementName = "DatePickerControl", Type = typeof(DatePickerControl))]
        [XmlArrayItem(ElementName = "EditorControl", Type = typeof(EditorControl))]
        [XmlArrayItem(ElementName = "EntryControl", Type = typeof(EntryControl))]
        [XmlArrayItem(ElementName = "FrameControl", Type = typeof(FrameControl))]
        [XmlArrayItem(ElementName = "GridControl", Type = typeof(GridControl))]
        [XmlArrayItem(ElementName = "ImageControl", Type = typeof(ImageControl))]
        [XmlArrayItem(ElementName = "LabelControl", Type = typeof(LabelControl))]
        [XmlArrayItem(ElementName = "ListViewControl", Type = typeof(ListViewControl))]
        [XmlArrayItem(ElementName = "PickerControl", Type = typeof(PickerControl))]
        [XmlArrayItem(ElementName = "ProgressBarControl", Type = typeof(ProgressBarControl))]
        [XmlArrayItem(ElementName = "RelativeLayoutControl", Type = typeof(RelativeLayoutControl))]
        [XmlArrayItem(ElementName = "ScrollViewControl", Type = typeof(ScrollViewControl))]
        [XmlArrayItem(ElementName = "SearchBarControl", Type = typeof(SearchBarControl))]
        [XmlArrayItem(ElementName = "SliderControl", Type = typeof(SliderControl))]
        [XmlArrayItem(ElementName = "StackLayoutControl", Type = typeof(StackLayoutControl))]
        [XmlArrayItem(ElementName = "TableViewControl", Type = typeof(TableViewControl))]
        [XmlArrayItem(ElementName = "TimePickerControl", Type = typeof(TimePickerControl))]
        [XmlArrayItem(ElementName = "WebViewControl", Type = typeof(WebViewControl))]
        public virtual List<BaseControl> Controls
        {
            get
            {
                return _Controls;
            }
            set
            {
                _Controls = value;
            }

        }

        List<ScreenDataCommand> _commands = new List<ScreenDataCommand>();

        [XmlArray("DataCommands")]
        [XmlArrayItem("DataCommand")]
        public virtual List<ScreenDataCommand> DataCommands
        {
            get
            {
                return _commands;
            }
            set
            {
                _commands = value;
            }

        }


        public static MobileScreen Load(string Name)
        {

            MobileScreen retVal = null;
            string item = String.Format("{0}.{1}.{2}.xml", ConfigurationLoader.ConfigAssemblyName, "MobileScreens", Name);
            using (Stream fileStream = ConfigurationLoader.ConfigAssembly.GetManifestResourceStream(item))
            {
                using (XmlReader xreader = XmlReader.Create(fileStream))
                {
                    XDocument doc = XDocument.Load(xreader);
                    string pageType = doc.Root.Element("Type").Value.ToLower();

                    switch (pageType)
                    {
                        case "mobilecontent":
                            retVal = MobileContentScreen.Load(doc);
                            break;
                        case "mobilenavigation":
                            retVal = MobileNavigationScreen.Load(doc);
                            break;
                        case "mobiletabbed":
                            retVal = MobileTabbedScreen.Load(doc);
                            break;
                        default:
                            retVal = MobileContentScreen.Load(doc);
                            break;
                    }

                }
            }

            return retVal;

        }


        public override string ToString()
        {
            return this.Name.ToString();
        }

        public static MobileScreen GetByName(string ScreenName)
        {
            MobileScreen screen = Configuration.GetInstance().MobileScreens
                            .Where(s => 
                                (
                                          (s.Name.ToLower() == ScreenName.ToLower())
                                )
                            )
                            .SingleOrDefault();

            if (screen == null)
            { 
                //attempt to load screen from config
                screen = Load(ScreenName);
                
            }

            return screen;
        }

        
    }
}
