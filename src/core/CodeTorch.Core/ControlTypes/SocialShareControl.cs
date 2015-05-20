using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CodeTorch.Core
{
    [Serializable]
    public class SocialShareControl : BaseControl
    {
        public override string Type
        {
            get
            {
                return "SocialShare";
            }
            set
            {
                base.Type = value;
            }
        }

        [Category("Appearance")]
        public string Skin { get; set; }
        [Category("Appearance")]
        public string Height { get; set; }

        [Category("Dialog")]
        public string DialogHeight { get; set; }
        [Category("Dialog")]
        public string DialogLeft { get; set; }
        [Category("Dialog")]
        public string DialogTop { get; set; }
        [Category("Dialog")]
        public string DialogWidth { get; set; }
        
        bool _HideIframesOnDialogMove = true;

        [Category("Dialog")]
        public bool HideIframesOnDialogMove
        {
            get { return _HideIframesOnDialogMove; }
            set { _HideIframesOnDialogMove = value; }
        }


        [Category("External IDs")]
        public string FacebookAppId { get; set; }
        [Category("External IDs")]
        public string YammerAppId { get; set; }
        [Category("External IDs")]
        public string GoogleAnalyticsUA { get; set; }

        
        public string ToolTip { get; set; }

        [Category("Shared Items")]
        public string TitleToShare { get; set; }

        [Category("Data")]
        public string DataTitleToShareField { get; set; }
        
        

        [Category("Shared Items")]
        public string UrlToShare { get; set; }

        [Category("Shared Items")]
        public string UrlToShareFormatString { get; set; }

        List<SocialNetwork> _DisplayedNetworks = new List<SocialNetwork>();

        [Category("Shared Items")]
        [XmlArray("DisplayedNetworks")]
        [XmlArrayItem("SocialNetwork")]
        [Description("List of social networks displayed in main UI")]
        public List<SocialNetwork> DisplayedNetworks
        {
            get
            {
                return _DisplayedNetworks;
            }
            set
            {
                _DisplayedNetworks = value;
            }

        }


        List<SocialNetwork> _CompactNetworks = new List<SocialNetwork>();

        [Category("Shared Items")]
        [XmlArray("CompactNetworks")]
        [XmlArrayItem("SocialNetwork")]
        [Description("List of available social networks displayed in compact mode")]
        public List<SocialNetwork> CompactNetworks
        {
            get
            {
                return _CompactNetworks;
            }
            set
            {
                _CompactNetworks = value;
            }

        }
        
    }
}
