using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using CodeTorch.Core.Platform;


namespace CodeTorch.Core
{
   
    public class BaseControl
    {
        private bool _Visible = true;

        public bool Visible
        {
            get
            {
                return _Visible;
            }
            set
            {
                _Visible = value;
            }
        }

        public string Name { get; set; }
        public virtual string Type { get; set; }
        //public bool IsRequired { get; set; }
        public bool IsClippedToBounds { get; set; }
        //public virtual string Width { get; set; }

        //TODO Thickness


        OnPlatformLayoutOptions _HorizontalOptions = new OnPlatformLayoutOptions();
        public OnPlatformLayoutOptions HorizontalOptions
        {
            get
            {
                return this._HorizontalOptions;
            }
            set
            {
                this._HorizontalOptions = value;
            }
        }

        OnPlatformLayoutOptions _VerticalOptions = new OnPlatformLayoutOptions();
        public OnPlatformLayoutOptions VerticalOptions
        {
            get
            {
                return this._VerticalOptions;
            }
            set
            {
                this._VerticalOptions = value;
            }
        }

        OnPlatformDouble _HeightRequest = new OnPlatformDouble();
        public OnPlatformDouble HeightRequest
        {
            get
            {
                return this._HeightRequest;
            }
            set
            {
                this._HeightRequest = value;
            }
        }

        OnPlatformDouble _WidthRequest = new OnPlatformDouble();
        public OnPlatformDouble WidthRequest
        {
            get
            {
                return this._WidthRequest;
            }
            set
            {
                this._WidthRequest = value;
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
        public List<BaseControl> Controls
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

    }
}
