using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    // Summary:
    //     This enum specifies whether the RadAutoCompleteInputType is a token or text.
    public enum AutoCompleteInputType
    {
        // Summary:
        //     RadAutoCompleteInputType is set to a Token.
        Token = 0,
        //
        // Summary:
        //     RadAutoCompleteInputType is set to Text.
        Text = 1,
    }

    // Summary:
    //     This enum specifies if the RadAutoCompleteFilter uses Contains or StartsWith
    //     functionality.
    public enum AutoCompleteFilter
    {
        // Summary:
        //     RadAutoCompleteFilter is set to Contains filtering mode.
        Contains = 0,
        //
        // Summary:
        //     RadAutoCompleteFilter is set to StartsWith filtering mode.
        StartsWith = 1,
    }

    // Summary:
    //     This enum specifies if the RadAutoCompleteFilter uses Contains or StartsWith
    //     functionality.
    public enum AutoCompleteDropDownPosition
    {
        // Summary:
        //     The DropDown is horizontally aligned to the currently typed text.
        Automatic = 0,
        //
        // Summary:
        //     The DropDown is horizontally aligned to the RadAutoCompleteBox.
        Static = 1,
    }

    // Summary:
    //     This enum specifies if the RadAutoCompleteFilter uses Contains or StartsWith
    //     functionality.
    public enum AutoCompleteSelectionMode
    {
        // Summary:
        //     The control can have only one entry at a time.
        Single = 0,
        //
        // Summary:
        //     The default behaviour - the control can have multiple entries.
        Multiple = 1,
    }

    [Serializable]
    public class AutoCompleteBoxControl : Widget
    {
        bool _AllowTokenEditing = true;
        bool _EnableClientFiltering = true;
        AutoCompleteSelectionMode _SelectionMode = AutoCompleteSelectionMode.Multiple;

       

        public override string Type
        {
            get
            {
                return "AutoCompleteBox";
            }
            set
            {
                base.Type = value;
            }
        }

        public bool AutoPostBack { get; set; }

        string _Width = "100%";
        public override string Width
        {
            get
            {
                return _Width;
            }
            set
            {
                _Width = value;
            }
        }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.ScreenDataCommandTypeConverter,CodeTorch.Core.Design")]
        public string SelectDataCommand { get; set; }

        public string RelatedControl { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string DataTextField { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string DataValueField { get; set; }

        public string EmptyMessage { get; set; }

        public AutoCompleteInputType InputType { get; set; }
        public AutoCompleteFilter Filter { get; set; }
        public AutoCompleteDropDownPosition DropDownPosition { get; set; }

        public int MinFilterLength { get; set; }
        public int MaxResultCount { get; set; }

        public bool AllowCustomEntry { get; set; }

        public string DropDownWidth { get; set; }
        public string DropDownHeight { get; set; }

        public bool AllowTokenEditing
        {
            get { return _AllowTokenEditing; }
            set { _AllowTokenEditing = value; }
        }

        public bool EnableClientFiltering
        {
            get { return _EnableClientFiltering; }
            set { _EnableClientFiltering = value; }
        }

        public string Delimiter { get; set; }
        public string ClientDropDownItemTemplate { get; set; }

        public AutoCompleteSelectionMode SelectionMode
        {
            get { return _SelectionMode; }
            set { _SelectionMode = value; }
        }

        private Action _OnClick = new Action();

        [Category("Actions")]
        public Action OnClick
        {
            get
            {
                return _OnClick;
            }
            set
            {
                _OnClick = value;
            }

        }
        


        public static IEnumerable<ResourceItem> GetResourceKeys(Screen Screen, AutoCompleteBoxControl Control, String Prefix)
        {
            List<ResourceItem> retVal = new List<ResourceItem>();

            AddResourceKey(retVal, Screen, Control, Prefix, "EmptyMessage", Control.EmptyMessage);
            AddResourceKey(retVal, Screen, Control, Prefix, "ClientDropDownItemTemplate", Control.ClientDropDownItemTemplate);



            return retVal;
        }
    }
}
