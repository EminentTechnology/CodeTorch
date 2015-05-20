using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;
using System.Drawing.Design;

namespace CodeTorch.Core
{
    [
        XmlInclude(typeof(BoundGridColumn)),
        XmlInclude(typeof(DeleteGridColumn)),
        XmlInclude(typeof(EditGridColumn)),
        XmlInclude(typeof(HyperLinkGridColumn)),
        XmlInclude(typeof(PickerLinkButtonGridColumn)),
        XmlInclude(typeof(PickerHyperLinkGridColumn)),
        XmlInclude(typeof(BinaryImageGridColumn))
    ]
    [Serializable]
    public class GridColumn
    {


        GridColumnItemStyle _HeaderStyle = new GridColumnItemStyle();
        GridColumnItemStyle _ItemStyle = new GridColumnItemStyle();
        GridColumnItemStyle _FooterStyle = new GridColumnItemStyle();

        [Category("Appearance")]
        public GridColumnItemStyle HeaderStyle
        {
            get { return _HeaderStyle; }
            set { _HeaderStyle = value; }
        }

        [Category("Appearance")]
        public GridColumnItemStyle ItemStyle
        {
            get { return _ItemStyle; }
            set { _ItemStyle = value; }
        }

        [Category("Appearance")]
        public GridColumnItemStyle FooterStyle
        {
            get { return _FooterStyle; }
            set { _FooterStyle = value; }
        }

        [Category("Common")]
        public string HeaderText { get; set; }

        [Category("Common")]
        public string UniqueName { get; set; }

        [ReadOnly(true)]
        [Category("Common")]
        public virtual GridColumnType ColumnType { get; set; }

        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        [Category("Data")]
        public string SortExpression { get; set; }

        PermissionCheck _VisiblePermission = new PermissionCheck();
        [Category("Security")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public PermissionCheck VisiblePermission
        {
            get {return _VisiblePermission; }
            set { _VisiblePermission = value; }
        }

        PermissionCheck _ExportPermission = new PermissionCheck();
        [Category("Security")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public PermissionCheck ExportPermission
        {
            get { return _ExportPermission; }
            set { _ExportPermission = value; }
        }

        PermissionCheck _LinkPermission = new PermissionCheck();
        [Category("Security")]
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public PermissionCheck LinkPermission 
        {
            get {return _LinkPermission; }
            set { _LinkPermission = value; }
        }

        [Browsable(false)]
        [XmlIgnore()]
        public Grid Parent { get; set; }

        private bool _IncludeInExport = true;
        [Category("Export")]
        public bool IncludeInExport 
        { 
            get
            { 
                return _IncludeInExport;
            } 
            set
            {
                _IncludeInExport = value;
            } 
        }

        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        [Category("Export")]
        public string ExportDataField { get; set; }

        

        public static IEnumerable<ResourceItem> GetResourceKeys(Screen Screen, GridColumn Column, String Prefix)
        {
            List<ResourceItem> retVal = new List<ResourceItem>();

            //add
            AddResourceKey(retVal, Screen, Column, Prefix, "HeaderText", Column.HeaderText);


            switch (Column.ColumnType)
            {
                case GridColumnType.BoundGridColumn:
                    
                    //AddResourceKey(retVal, Screen, Column, Prefix, "DataFormatString", ((BoundGridColumn)Column).DataFormatString);
                    break;
                case GridColumnType.HyperLinkGridColumn:
                    //AddResourceKey(retVal, Screen, Column, Prefix, "DataTextFormatString", ((HyperLinkGridColumn)Column).DataTextFormatString);
                    AddResourceKey(retVal, Screen, Column, Prefix, "Text", ((HyperLinkGridColumn)Column).Text);
                    break;
                case GridColumnType.DeleteGridColumn:
                     AddResourceKey(retVal, Screen, Column, Prefix, "ConfirmText", ((DeleteGridColumn)Column).ConfirmText);
                     AddResourceKey(retVal, Screen, Column, Prefix, "Text", ((DeleteGridColumn)Column).Text);
                    break;
                case GridColumnType.EditGridColumn:
                    AddResourceKey(retVal, Screen, Column, Prefix, "Text", ((EditGridColumn)Column).Text);
                    break;
                case GridColumnType.PickerLinkButtonGridColumn:
                    break;
                case GridColumnType.PickerHyperLinkGridColumn:
                    break;
                case GridColumnType.BinaryImageGridColumn:
                    //AddResourceKey(retVal, Screen, Column, Prefix, "DataAlternateTextFormatString", ((BinaryImageGridColumn)Column).DataAlternateTextFormatString);
                    break;

            }

            return retVal;
        }

        protected static void AddResourceKey(List<ResourceItem> keys, Screen screen, GridColumn Column, string Prefix, string ResourceKey, string DefaultValue)
        {
            ResourceItem key = new Core.ResourceItem();

            key.ResourceSet = String.Format("App/{0}/{1}", screen.Folder, screen.Name);
            string UniqueName = Column.UniqueName;

            if (String.IsNullOrEmpty(UniqueName))
            {
                UniqueName = Column.HeaderText;
            }

            if (String.IsNullOrEmpty(Prefix))
            {
                key.Key = String.Format("{0}.{1}", UniqueName, ResourceKey);
            }
            else
            {
                key.Key = String.Format("{0}.{1}.{2}", Prefix, UniqueName, ResourceKey);
            }
            key.Value = DefaultValue;

            if (!String.IsNullOrEmpty(DefaultValue))
            {
                keys.Add(key);
            }
        }


        public static GridColumn GetNewColumn(string type)
        {
            GridColumn retval = null;

            switch (type.ToLower())
            {
                case "binaryimagegridcolumn":
                    retval = new BinaryImageGridColumn();
                    break;
                case "boundgridcolumn":
                    retval = new BoundGridColumn();
                    break;
                case "deletegridcolumn":
                    retval = new DeleteGridColumn();
                    break;
                case "editgridcolumn":
                    retval = new EditGridColumn();
                    break;
                case "hyperlinkgridcolumn":
                    retval = new HyperLinkGridColumn();
                    break;
                case "pickerhyperlinkgridcolumn":
                    retval = new PickerHyperLinkGridColumn();
                    break;
                case "pickerlinkbuttongridcolumn":
                    retval = new PickerLinkButtonGridColumn();
                    break;
            }

            return retval;
        }

        public static void Convert(GridColumn current, GridColumn newColumn)
        {
            newColumn.ExportDataField = current.ExportDataField;
            newColumn.HeaderText = current.HeaderText;
            newColumn.IncludeInExport = current.IncludeInExport;
            newColumn.SortExpression = current.SortExpression;
            newColumn.UniqueName = current.UniqueName;

            newColumn.Parent = current.Parent;

            switch (current.ColumnType)
            {
                case GridColumnType.BinaryImageGridColumn:
                    ConvertFromBinaryImageGridColumn((BinaryImageGridColumn)current, newColumn);
                    break;
                case GridColumnType.BoundGridColumn:
                    ConvertFromBoundGridColumn((BoundGridColumn)current, newColumn);
                    break;
                case GridColumnType.DeleteGridColumn:
                    ConvertFromDeleteGridColumn((DeleteGridColumn)current, newColumn);
                    break;
                case GridColumnType.EditGridColumn:
                    ConvertFromEditGridColumn((EditGridColumn)current, newColumn);
                    break;
                case GridColumnType.HyperLinkGridColumn:
                    ConvertFromHyperLinkGridColumn((HyperLinkGridColumn)current, newColumn);
                    break;
                case GridColumnType.PickerHyperLinkGridColumn:
                    ConvertFromPickerHyperLinkGridColumn((PickerHyperLinkGridColumn)current, newColumn);
                    break;
                case GridColumnType.PickerLinkButtonGridColumn:
                    ConvertFromPickerLinkButtonGridColumn((PickerLinkButtonGridColumn)current, newColumn);
                    break;
            }
        }

        private static void ConvertToDeleteColumnDefaults(GridColumn newColumn)
        {
            ((DeleteGridColumn)newColumn).Text = "Delete";
            ((DeleteGridColumn)newColumn).ConfirmText = "WARNING: You are about to delete this item\r\rPress OK to DELETE this item\rPress CANCEL to LEAVE this item alone";
        }

        private static void ConvertFromBinaryImageGridColumn(BinaryImageGridColumn current, GridColumn newColumn)
        {
            switch (newColumn.ColumnType)
            {
                case GridColumnType.BoundGridColumn:
                    ((BoundGridColumn)newColumn).DataField = current.DataField;
                    break;
                case GridColumnType.DeleteGridColumn:
                    ConvertToDeleteColumnDefaults(newColumn);
                    break;
                case GridColumnType.EditGridColumn:
                    ((EditGridColumn)newColumn).DataTextField = current.DataField;
                    break;
                case GridColumnType.HyperLinkGridColumn:
                    ((HyperLinkGridColumn)newColumn).DataTextField = current.DataField;
                    ((HyperLinkGridColumn)newColumn).DataNavigateUrlFields = current.DataField;
                    break;
                case GridColumnType.PickerHyperLinkGridColumn:
                    break;
                case GridColumnType.PickerLinkButtonGridColumn:
                    ((PickerLinkButtonGridColumn)newColumn).DataField = current.DataField;
                    ((PickerLinkButtonGridColumn)newColumn).DataTextField = current.DataField;
                    break;
            }
        }

       

        private static void ConvertFromBoundGridColumn(BoundGridColumn current, GridColumn newColumn)
        {
            switch (newColumn.ColumnType)
            {
                case GridColumnType.BinaryImageGridColumn:
                    ((BinaryImageGridColumn)newColumn).DataField = current.DataField;
                    break;
               
                case GridColumnType.DeleteGridColumn:
                    ConvertToDeleteColumnDefaults(newColumn);
                    break;
                case GridColumnType.EditGridColumn:
                    ((EditGridColumn)newColumn).DataTextField = current.DataField;
                    break;
                case GridColumnType.HyperLinkGridColumn:
                    ((HyperLinkGridColumn)newColumn).DataTextField = current.DataField;
                    ((HyperLinkGridColumn)newColumn).DataTextFormatString = current.DataFormatString;
                    ((HyperLinkGridColumn)newColumn).DataNavigateUrlFields = current.DataField;
                    break;
                case GridColumnType.PickerHyperLinkGridColumn:
                    break;
                case GridColumnType.PickerLinkButtonGridColumn:
                    ((PickerLinkButtonGridColumn)newColumn).DataField = current.DataField;
                    ((PickerLinkButtonGridColumn)newColumn).DataTextField = current.DataFormatString;
                    break;
            }
        }

        private static void ConvertFromDeleteGridColumn(DeleteGridColumn current, GridColumn newColumn)
        {
            switch (newColumn.ColumnType)
            {
                case GridColumnType.BinaryImageGridColumn:

                    break;
                case GridColumnType.BoundGridColumn:
                    break;
               
                case GridColumnType.EditGridColumn:
                    ((EditGridColumn)newColumn).Text = current.Text;
                    break;
                case GridColumnType.HyperLinkGridColumn:
                    break;
                case GridColumnType.PickerHyperLinkGridColumn:
                    break;
                case GridColumnType.PickerLinkButtonGridColumn:
                    break;
            }
        }

        private static void ConvertFromEditGridColumn(EditGridColumn current, GridColumn newColumn)
        {
            switch (newColumn.ColumnType)
            {
                case GridColumnType.BinaryImageGridColumn:

                    break;
                case GridColumnType.BoundGridColumn:
                    break;
                case GridColumnType.DeleteGridColumn:
                    ConvertToDeleteColumnDefaults(newColumn);
                    break;
               
                case GridColumnType.HyperLinkGridColumn:
                    break;
                case GridColumnType.PickerHyperLinkGridColumn:
                    break;
                case GridColumnType.PickerLinkButtonGridColumn:
                    break;
            }
        }

        private static void ConvertFromHyperLinkGridColumn(HyperLinkGridColumn current, GridColumn newColumn)
        {
            switch (newColumn.ColumnType)
            {
                case GridColumnType.BinaryImageGridColumn:
                    ((BinaryImageGridColumn)newColumn).DataField = current.DataTextField;
                    break;
                case GridColumnType.BoundGridColumn:
                    ((BoundGridColumn)newColumn).DataField = current.DataTextField;
                    ((BoundGridColumn)newColumn).DataFormatString = current.DataTextFormatString;
                    break;
                case GridColumnType.DeleteGridColumn:
                    ConvertToDeleteColumnDefaults(newColumn);
                    break;
                case GridColumnType.EditGridColumn:
                    ((EditGridColumn)newColumn).Text = current.Text;
                    break;
                
                case GridColumnType.PickerHyperLinkGridColumn:
                    break;
                case GridColumnType.PickerLinkButtonGridColumn:
                    ((PickerLinkButtonGridColumn)newColumn).DataField = current.DataTextField;
                    ((PickerLinkButtonGridColumn)newColumn).DataTextField = current.DataTextField;
                    
                    break;
            }
        }

        private static void ConvertFromPickerHyperLinkGridColumn(PickerHyperLinkGridColumn current, GridColumn newColumn)
        {
            switch (newColumn.ColumnType)
            {
                case GridColumnType.BinaryImageGridColumn:

                    break;
                case GridColumnType.BoundGridColumn:
                    break;
                case GridColumnType.DeleteGridColumn:
                    ConvertToDeleteColumnDefaults(newColumn);
                    break;
                case GridColumnType.EditGridColumn:
                    break;
                case GridColumnType.HyperLinkGridColumn:
                    break;
               
                case GridColumnType.PickerLinkButtonGridColumn:
                    break;
            }
        }

        private static void ConvertFromPickerLinkButtonGridColumn(PickerLinkButtonGridColumn current, GridColumn newColumn)
        {
            switch (newColumn.ColumnType)
            {
                case GridColumnType.BinaryImageGridColumn:
                    ((BinaryImageGridColumn)newColumn).DataField = current.DataTextField;
                    break;
                case GridColumnType.BoundGridColumn:
                    ((BoundGridColumn)newColumn).DataField = current.DataField;
                    break;
                case GridColumnType.DeleteGridColumn:
                    ConvertToDeleteColumnDefaults(newColumn);
                    break;
                case GridColumnType.EditGridColumn:
                    ((BoundGridColumn)newColumn).DataField = current.DataField;
                    break;
                case GridColumnType.HyperLinkGridColumn:
                    ((HyperLinkGridColumn)newColumn).DataTextField = current.DataTextField;
                    ((HyperLinkGridColumn)newColumn).DataNavigateUrlFields = current.DataField;
                    break;
                case GridColumnType.PickerHyperLinkGridColumn:
                    break;
                
            }
        }
       
    }
}
