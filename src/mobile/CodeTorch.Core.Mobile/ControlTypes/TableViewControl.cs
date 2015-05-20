using CodeTorch.Core.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Xamarin.Forms;

namespace CodeTorch.Core
{

    public class TableViewControl : BaseControl
    {

        public override string Type
        {
            get
            {
                return "TableView";
            }
            set
            {
                base.Type = value;
            }
        }

       

        public TableIntent Intent { get; set; }

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

        OnPlatformBool _HasUnevenRows = new OnPlatformBool();
        public OnPlatformBool HasUnevenRows
        {
            get
            {
                return this._HasUnevenRows;
            }
            set
            {
                this._HasUnevenRows = value;
            }
        }

        OnPlatformInt _RowHeight = new OnPlatformInt();
        public OnPlatformInt RowHeight
        {
            get
            {
                return this._RowHeight;
            }
            set
            {
                this._RowHeight = value;
            }
        }

        List<BaseSection> _sections = new List<BaseSection>();
        [XmlArray("Sections")]
        [XmlArrayItem(ElementName = "TableSectionControl", Type = typeof(TableSectionControl))]
        public List<BaseSection> Sections
        {
            get
            {
                return this._sections;
            }
            set
            {
                this._sections = value;
            }
        }
       

    }
}
