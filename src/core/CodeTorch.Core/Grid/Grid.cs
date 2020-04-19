using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Drawing.Design;

namespace CodeTorch.Core
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Grid
    {
        int _PageSize = 20;
        bool _AllowSorting = true;
        bool _AllowPaging = true;
        bool _AllowDelete = false;
        bool _ShowHeader = true;
        string _HelpText = "";
        GridCommandItemDisplay _GridCommandItemDisplay = GridCommandItemDisplay.Top;

        [Category("Appearance")]
        [Description("CSS Class attached to this control")]
        public string CssClass { get; set; }

        [Category("Appearance")]
        public string Skin { get; set; }

        [Category("Appearance")]
        public string SkinID { get; set; }

        public string Name { get; set; }

        public string HelpText { get { return _HelpText; } set { _HelpText = value; } }

        [Category("Appearance")]
        public bool ShowHeader { get { return _ShowHeader; } set { _ShowHeader = value; } }

        [Category("Appearance")]
        public GridCommandItemDisplay CommandItemDisplay { get { return _GridCommandItemDisplay; } set { _GridCommandItemDisplay = value; } }
        


        public string DataKeyNames { get; set; }
        public string DataKeyParameterNames { get; set; }

        [TypeConverter("CodeTorch.Core.Design.ScreenDataCommandTypeConverter,CodeTorch.Core.Design")]
        public string SelectDataCommand { get; set; }

        [TypeConverter("CodeTorch.Core.Design.ScreenDataCommandTypeConverter,CodeTorch.Core.Design")]
        public string DeleteDataCommand { get; set; }


        public bool AllowDelete { get { return _AllowDelete; } set { _AllowDelete = value; } }

        public bool AllowPaging { get { return _AllowPaging; } set { _AllowPaging = value; } }

        public int PageSize { get { return _PageSize; } set { _PageSize = value; } }

        public bool AllowSorting { get { return _AllowSorting; } set { _AllowSorting = value; } }

        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string SortField { get; set; }

        public GridSortOrder SortOrder { get; set; }

        private List<GridColumn> _Columns = new List<GridColumn>();

        private List<GridGroupByExpression> _Expressions = new List<GridGroupByExpression>();


        [XmlArray("Columns")]
        [XmlArrayItem(ElementName = "BoundGridColumn", Type = typeof(BoundGridColumn))]
        [XmlArrayItem(ElementName = "EditGridColumn", Type = typeof(EditGridColumn))]
        [XmlArrayItem(ElementName = "DeleteGridColumn", Type = typeof(DeleteGridColumn))]
        [XmlArrayItem(ElementName = "HyperLinkGridColumn", Type = typeof(HyperLinkGridColumn))]
        [XmlArrayItem(ElementName = "PickerLinkButtonGridColumn", Type = typeof(PickerLinkButtonGridColumn))]
        [XmlArrayItem(ElementName = "BinaryImageGridColumn", Type = typeof(BinaryImageGridColumn))]
        [XmlArrayItem(ElementName = "ClientSelectGridColumn", Type = typeof(ClientSelectGridColumn))]
        //[XmlArrayItem(ElementName = "PickerHyperLinkGridColumn", Type = typeof(PickerHyperLinkGridColumn))]
#if NETFRAMEWORK
        [Editor("CodeTorch.Core.Design.GridColumnCollectionEditor,CodeTorch.Core.Design", typeof(UITypeEditor))]  
#endif
        [Description("List of grid columns")]
        public List<GridColumn> Columns
        {
            get
            {
                return _Columns;
            }
            set
            {
                _Columns = value;
            }

        }




        [XmlArray("GroupByExpressions")]
        [XmlArrayItem(ElementName = "Expression", Type = typeof(GridGroupByExpression))]
#if NETFRAMEWORK
        [Editor("CodeTorch.Core.Design.GridGroupByExpressionCollectionEditor,CodeTorch.Core.Design", typeof(UITypeEditor))]  
#endif
        [Description("List of group by expressions")]
        public List<GridGroupByExpression> GroupByExpressions
        {
            get
            {
                return _Expressions;
            }
            set
            {
                _Expressions = value;
                
            }

        }

        public GridGroupLoadMode GroupLoadMode { get; set; }
        //TODO - need to set allow Group Expan Collapse to true
        bool _GroupsDefaultExpanded = true;

        public bool GroupsDefaultExpanded
        {
            get { return _GroupsDefaultExpanded; }
            set { _GroupsDefaultExpanded = value; }
        }


        public bool AllowMultiRowSelection { get; set; }

        public bool GroupingEnabled { get; set; }
        public bool ShowGroupPanel { get; set; }
        public bool AllowGroupExpandCollapse { get; set; }

        

        bool _ShowRefreshButton = true;
        public bool ShowRefreshButton
        {
            get
            {
                return _ShowRefreshButton;
            }
            set
            {
                _ShowRefreshButton = value;
            }
        }

        bool _ShowExportToCsvButton = true;
        [Category("Export")]
        public bool ShowExportToCsvButton
        {
            get
            {
                return _ShowExportToCsvButton;
            }
            set
            {
                _ShowExportToCsvButton = value;
            }
        }

        [Category("Export")]
        public bool ShowExportToExcelButton { get; set; }
        [Category("Export")]
        public bool ShowExportToPdfButton { get; set; }
        [Category("Export")]
        public bool ShowExportToWordButton { get; set; }

        //bool _ExportOnlyData = false;
        //[Category("Export")]
        //public bool ExportOnlyData
        //{
        //    get
        //    {
        //        return _ExportOnlyData;
        //    }
        //    set
        //    {
        //        _ExportOnlyData = value;
        //    }
        //}

        bool _ExportIgnorePaging = true;
        [Category("Export")]
        public bool ExportIgnorePaging
        {
            get
            {
                return _ExportIgnorePaging;
            }
            set
            {
                _ExportIgnorePaging = value;
            }
        }

        bool _ExportOpenInNewWindow = true;
        [Category("Export")]
        public bool ExportOpenInNewWindow
        {
            get
            {
                return _ExportOpenInNewWindow;
            }
            set
            {
                _ExportOpenInNewWindow = value;
            }
        }

        bool _ExportHideStructureColumns = true;
        [Category("Export")]
        public bool ExportHideStructureColumns
        {
            get
            {
                return _ExportHideStructureColumns;
            }
            set
            {
                _ExportHideStructureColumns = value;
            }
        }

        [Category("Export")]
        public string ExportFileName { get; set; }


        [Category("Scrolling")]
        public bool AllowScroll { get; set; }

        [Category("Scrolling")]
        public bool UseStaticHeaders { get; set; }

        [Category("Scrolling")]
        public int FrozenColumnsCount { get; set; }

        bool _SaveScrollPosition = true;
        [Category("Scrolling")]
        public bool SaveScrollPosition
        {
            get
            {
                return _SaveScrollPosition;
            }
            set
            {
                _SaveScrollPosition = value;
            }
        }

        [Category("Scrolling")]
        public string ScrollBarWidth { get; set; }

        [Category("Scrolling")]
        public string ScrollHeight { get; set; }
        

        [Category("Csv")]
        public bool CsvEncloseDataWithQuotes  { get; set; }

        [Category("Csv")]
        public string CsvFileExtension { get; set; }

        GridCsvDelimiter _CsvColumnDelimiter = GridCsvDelimiter.Comma;
        [Category("Csv")]
        public GridCsvDelimiter CsvColumnDelimiter
        {
            get
            {
                return _CsvColumnDelimiter;
            }
            set
            {
                _CsvColumnDelimiter = value;
            }
        }

        GridCsvDelimiter _CsvRowDelimiter = GridCsvDelimiter.NewLine;
        [Category("Csv")]
        public GridCsvDelimiter CsvRowDelimiter
        {
            get
            {
                return _CsvRowDelimiter;
            }
            set
            {
                _CsvRowDelimiter = value;
            }
        }
        

        [Category("Client Settings")]
        public bool AllowRowsDragDrop { get; set; }
        [Category("Client Settings")]
        public bool AllowRowSelect { get; set; }
        [Category("Client Settings")]
        public bool EnableDragToSelectRows { get; set; }

        [Category("Client Events")]
        public string OnRowDropping { get; set; }
        [Category("Client Events")]
        public string OnRowDropped { get; set; }



        [Browsable(false)]
        [XmlIgnore()]
        public object Parent { get; set; }

        public override string ToString()
        {
            string retVal = Name;

            return retVal;
        }
    }
}
