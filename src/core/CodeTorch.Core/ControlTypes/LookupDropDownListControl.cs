﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    public class LookupDropDownListControl: Widget
    {
        public bool AutoPostBack { get; set; }
        public bool IncludeAdditionalListItem { get; set; }
        public bool CausesValidation { get; set; }

        public string AdditionalListItemText { get; set; }
        public string AdditionalListItemValue { get; set; }

        bool _ShowToggleImage = true;
        public bool ShowToggleImage
        {
            get
            {
                return _ShowToggleImage;
            }
            set
            {
                _ShowToggleImage = value;
            }
        }

        [Category("Appearance")]
        public string Skin { get; set; }

         [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.LookupTypeConverter,CodeTorch.Core.Design")]
        public string LookupType { get; set; }

        [Category("Load On Demand")]
        public bool EnableLoadOnDemand { get; set; }

        public string RelatedControl { get; set; }


        [Category("Load On Demand")]
        public bool EnableItemCaching { get; set; }
        [Category("Load On Demand")]
        public bool EnableVirtualScrolling { get; set; }

        public bool MarkFirstMatch { get; set; }
        public bool IsCaseSensitive { get; set; }
        public DropDownListFilterMode FilterMode { get; set; }
        public DropDownListRenderingMode RenderingMode { get; set; }
        public string DropDownWidth { get; set; }

        private Action _SelectedIndexChanged = new Action();


        [Category("Actions")]
        public Action SelectedIndexChanged
        {
            get
            {
                return _SelectedIndexChanged;
            }
            set
            {
                _SelectedIndexChanged = value;
            }

        }


        public override string Type
        {
            get
            {
                return "LookupDropDownList";
            }
            set
            {
                base.Type = value;
            }
        }

        public static IEnumerable<ResourceItem> GetResourceKeys(Screen Screen, LookupDropDownListControl Control, String Prefix)
        {
            List<ResourceItem> retVal = new List<ResourceItem>();

            //add title
            AddResourceKey(retVal, Screen, Control, Prefix, "AdditionalListItemText", Control.AdditionalListItemText);

           


            return retVal;
        }
    }
}
