using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeTorch.Core.Services;
using Telerik.Web.UI.Grid;
using Telerik.Web.UI;
using System.Web;
using System.Web.UI.WebControls;
using CodeTorch.Web.Code;
using System.Web.UI;
using CodeTorch.Core;
using CodeTorch.Web.Data;
using CodeTorch.Web.Templates;
using CodeTorch.Web.UserControls;

namespace CodeTorch.Web
{
    public class GridFunctions
    {
        public static void BuildGridColumn(BasePage page, RadGrid Grid, Grid GridObject, CodeTorch.Core.GridColumn Column, string ResourceKeyPrefix)
        {
            switch (Column.ColumnType)
            {
                case GridColumnType.BoundGridColumn:
                    BuildGridBoundColumn(page, Grid, GridObject, (BoundGridColumn)Column, ResourceKeyPrefix);
                    break;
                case GridColumnType.HyperLinkGridColumn:
                    BuildGridHyperLinkColumn(page, Grid, GridObject, (HyperLinkGridColumn)Column, ResourceKeyPrefix);
                    break;
                case GridColumnType.DeleteGridColumn:
                    BuildGridDeleteColumn(page, Grid, GridObject, (DeleteGridColumn)Column, ResourceKeyPrefix);
                    break;
                case GridColumnType.EditGridColumn:
                    BuildGridEditColumn(page, Grid, GridObject, (EditGridColumn)Column, ResourceKeyPrefix);
                    break;
                case GridColumnType.PickerLinkButtonGridColumn:
                    PickerLinkButtonColumn(page, Grid, GridObject, (PickerLinkButtonGridColumn)Column, ResourceKeyPrefix);
                    break;
                case GridColumnType.PickerHyperLinkGridColumn:
                    BuildGridPickerHyperLinkGridColumn(page, Grid, GridObject, (PickerHyperLinkGridColumn)Column, ResourceKeyPrefix);
                    break;
                case GridColumnType.BinaryImageGridColumn:
                    BuildBinaryImageGridColumn(page, Grid, GridObject, (BinaryImageGridColumn)Column, ResourceKeyPrefix);
                    break;

            }
        }

        public static void BuildBinaryImageGridColumn(BasePage page, RadGrid Grid, Grid GridObject, BinaryImageGridColumn Column, string ResourceKeyPrefix)
        {
            GridBinaryImageColumn col = new GridBinaryImageColumn();

            string HeaderText  = Common.CoalesceStr(col.HeaderText, Column.HeaderText);
            string DataAlternateTextFormatString = Common.CoalesceStr(col.DataAlternateTextFormatString, Column.DataAlternateTextFormatString);

            col.HeaderText = GetGlobalResourceString(page, Column, ResourceKeyPrefix, "HeaderText", HeaderText);
            col.SortExpression = Common.CoalesceStr(col.SortExpression, Column.SortExpression);
            col.DataField = Common.CoalesceStr(col.DataField, Column.DataField);
            col.UniqueName = Common.CoalesceStr(col.UniqueName, Column.UniqueName);
            col.DataAlternateTextField = Common.CoalesceStr(col.DataAlternateTextField, Column.DataAlternateTextField);
            col.DataAlternateTextFormatString = GetGlobalResourceString(page, Column, ResourceKeyPrefix, "DataAlternateTextFormatString", DataAlternateTextFormatString);

            //string defaultImageUrl = String.IsNullOrEmpty(col.DefaultImageUrlFormat) ? col.DefaultImageUrl;
            col.DefaultImageUrl = Common.CoalesceStr(col.DefaultImageUrl, Column.DefaultImageUrl);

            if (!String.IsNullOrEmpty(Column.ImageHeight))
            {
                col.ImageHeight = new Unit(Column.ImageHeight);
            }

            if (!String.IsNullOrEmpty(Column.ImageWidth))
            {
                col.ImageWidth = new Unit(Column.ImageWidth);
            }

            col.ImageAlign = Column.ImageAlign;
            col.ResizeMode = (Telerik.Web.UI.BinaryImageResizeMode)Enum.Parse(typeof(Telerik.Web.UI.BinaryImageResizeMode), Column.ResizeMode.ToString());
           

            FormatStyle(col, Column);

            Grid.MasterTableView.Columns.Add(col);
        }

        private static void BuildGridPickerHyperLinkGridColumn(BasePage page, RadGrid Grid, Grid GridObject, PickerHyperLinkGridColumn Column, string ResourceKeyPrefix)
        {
            GridHyperLinkColumn col = new GridHyperLinkColumn();

            string HeaderText = Common.CoalesceStr(col.HeaderText, Column.HeaderText);
            

            col.HeaderText = GetGlobalResourceString(page, Column, ResourceKeyPrefix, "HeaderText", HeaderText);

            col.SortExpression = Common.CoalesceStr(col.SortExpression, Column.SortExpression);
            col.Text = "test";


            FormatStyle(col, Column);

            Grid.MasterTableView.Columns.Add(col);
        }

        private static void BuildGridEditColumn(BasePage page, RadGrid Grid, Grid GridObject, EditGridColumn Column, string ResourceKeyPrefix)
        {
            GridEditCommandColumn col = new GridEditCommandColumn();

            string HeaderText = Common.CoalesceStr(col.HeaderText, Column.HeaderText);
            string Text = Common.CoalesceStr(col.EditText, Column.Text);

            col.HeaderText = GetGlobalResourceString(page, Column, ResourceKeyPrefix, "HeaderText", HeaderText);

            col.SortExpression = Common.CoalesceStr(col.SortExpression, Column.SortExpression);
            col.ButtonType = GridButtonColumnType.LinkButton;
            col.EditText = GetGlobalResourceString(page, Column, ResourceKeyPrefix, "Text", Text);

            FormatStyle(col, Column);

            Grid.MasterTableView.Columns.Add(col);
        }



        public static void BuildGridHyperLinkColumn(BasePage page, RadGrid Grid, Grid GridObject, HyperLinkGridColumn Column, string ResourceKeyPrefix)
        {
            GridHyperLinkColumn col = new GridHyperLinkColumn();

            string HeaderText = Common.CoalesceStr(col.HeaderText, Column.HeaderText);
            string DataTextFormatString = Common.CoalesceStr(col.DataTextFormatString, Column.DataTextFormatString);
            string Text = Common.CoalesceStr(col.Text, Column.Text);

            col.HeaderText = GetGlobalResourceString(page, Column, ResourceKeyPrefix, "HeaderText", HeaderText);

            col.SortExpression = Common.CoalesceStr(col.SortExpression, Column.SortExpression);
            col.DataNavigateUrlFields = Common.CoalesceDelimArr(col.DataNavigateUrlFields, Column.DataNavigateUrlFields, ',');

            string urlFormatString = Common.CreateUrlWithQueryStringContext(Column.DataNavigateUrlFormatString, Column.Context);
            col.DataNavigateUrlFormatString = Common.CoalesceStr(col.DataNavigateUrlFormatString, urlFormatString);
            col.DataTextField = Common.CoalesceStr(col.DataTextField, Column.DataTextField);
            col.DataTextFormatString = GetGlobalResourceString(page, Column, ResourceKeyPrefix, "DataTextFormatString", DataTextFormatString);
            col.Text = GetGlobalResourceString(page, Column, ResourceKeyPrefix, "Text", Text);

            if (!String.IsNullOrEmpty(Column.Target))
            {
                col.Target = Column.Target;
            }

            FormatStyle(col, Column);

            Grid.MasterTableView.Columns.Add(col);
        }

        public static void PickerLinkButtonColumn(BasePage page, RadGrid Grid, Grid GridObject, PickerLinkButtonGridColumn Column, string ResourceKeyPrefix)
        {

            GridTemplateColumn col = new GridTemplateColumn();

            string HeaderText = Common.CoalesceStr(col.HeaderText, Column.HeaderText);
            

            col.HeaderText = GetGlobalResourceString(page, Column, ResourceKeyPrefix, "HeaderText", HeaderText);

            col.ItemTemplate = new LinkButtonTemplate(Column.DataTextField, Column.DataField);

            FormatStyle(col, Column);

            Grid.MasterTableView.Columns.Add(col);
        }



        public static void BuildGridDeleteColumn(BasePage page, RadGrid Grid, Grid GridObject, DeleteGridColumn Column, string ResourceKeyPrefix)
        {
            GridButtonColumn col = new GridButtonColumn();

            col.CommandName = "Delete";

            string HeaderText = Common.CoalesceStr(col.HeaderText, Column.HeaderText);
            string ConfirmText = Common.CoalesceStr(col.ConfirmText, Column.ConfirmText);
            string Text = Common.CoalesceStr(col.Text, Column.Text);

            col.HeaderText = GetGlobalResourceString(page, Column, ResourceKeyPrefix, "HeaderText", HeaderText);

            col.SortExpression = Common.CoalesceStr(col.SortExpression, Column.SortExpression);
            col.ConfirmText = GetGlobalResourceString(page, Column, ResourceKeyPrefix, "ConfirmText", ConfirmText);
            col.Text = GetGlobalResourceString(page, Column, ResourceKeyPrefix, "Text", Text);

            switch (Column.ButtonType)
            { 
                case System.Web.UI.WebControls.ButtonType.Link:
                    col.ButtonType = GridButtonColumnType.LinkButton;
                    break;
                case System.Web.UI.WebControls.ButtonType.Button:
                    col.ButtonType = GridButtonColumnType.PushButton;
                    break;
                case System.Web.UI.WebControls.ButtonType.Image:
                    col.ButtonType = GridButtonColumnType.ImageButton;
                    col.ImageUrl = Common.CoalesceStr(col.ImageUrl, Column.ImageUrl);
                    break;
                default:
                    col.ButtonType = GridButtonColumnType.LinkButton;
                    break;
            }

            FormatStyle(col, Column);

            Grid.MasterTableView.Columns.Add(col);
        }

        public static void BuildGridBoundColumn(BasePage page, RadGrid Grid, Grid GridObject, BoundGridColumn Column, string ResourceKeyPrefix)
        {
            GridBoundColumn col = new GridBoundColumn();

            string HeaderText = Common.CoalesceStr(col.HeaderText, Column.HeaderText);
            string DataFormatString = Common.CoalesceStr(col.DataFormatString, Column.DataFormatString);

            col.HeaderText = GetGlobalResourceString(page, Column, ResourceKeyPrefix, "HeaderText", HeaderText);

            
            col.SortExpression = Common.CoalesceStr(col.SortExpression, Column.SortExpression);
            col.DataField = Common.CoalesceStr(col.DataField, Column.DataField);
            col.DataFormatString = GetGlobalResourceString(page, Column, ResourceKeyPrefix, "DataFormatString", DataFormatString);

            FormatStyle(col, Column);

            Grid.MasterTableView.Columns.Add(col);
        }

        private static void FormatStyle(Telerik.Web.UI.GridColumn column, CodeTorch.Core.GridColumn config)
        {
            TableItemStyle itemStyle = null;

            itemStyle = column.ItemStyle;
            FormatColumnItemStyle(config.ItemStyle, itemStyle);

            itemStyle = column.HeaderStyle;
            FormatColumnItemStyle(config.HeaderStyle, itemStyle);

            itemStyle = column.FooterStyle;
            FormatColumnItemStyle(config.FooterStyle, itemStyle);
        }

        private static void FormatColumnItemStyle(CodeTorch.Core.GridColumnItemStyle config, TableItemStyle itemStyle)
        {
            if (itemStyle != null)
            {
                itemStyle.Wrap = config.Wrap;

                if (!String.IsNullOrEmpty(config.CssClass))
                    itemStyle.CssClass = config.CssClass;

                if (config.HorizonalAlign != HorizontalAlignment.NotSet)
                    itemStyle.HorizontalAlign = (HorizontalAlign)Enum.Parse(typeof(HorizontalAlign), config.HorizonalAlign.ToString()); 

                if (config.VerticalAlign != VerticalAlignment.NotSet)
                    itemStyle.VerticalAlign = (VerticalAlign)Enum.Parse(typeof(VerticalAlign), config.VerticalAlign.ToString());
            }
        }

        


        public static void HideAddCommandItem(RadGrid Grid)
        {
            GridItem[] items = Grid.MasterTableView.GetItems(GridItemType.CommandItem);
            
            if ((items != null) && (items.Length > 0))
            {
                foreach (GridCommandItem cmdItem in items)
                {
                    if (cmdItem != null)
                    {
                        Control AddText = (cmdItem.FindControl("InitInsertButton"));
                        if (AddText != null) AddText.Visible = false;

                        Control AddButton = (cmdItem.FindControl("AddNewRecordButton"));
                        if (AddButton != null) AddButton.Visible = false;

                    }
                }
            }
        }

        public static void BuildSimpleGrid(BasePage page, RadGrid Grid, CodeTorch.Core.Grid GridConfig,  string ResourceKeyPrefix)
        {
            if (GridConfig != null)
            {
                if (!String.IsNullOrEmpty(GridConfig.CssClass))
                {
                    Grid.CssClass = GridConfig.CssClass;
                }

                if (!String.IsNullOrEmpty(GridConfig.SkinID))
                {
                    Grid.SkinID = GridConfig.SkinID;
                }

                if (!String.IsNullOrEmpty(GridConfig.Skin))
                {
                    Grid.Skin = GridConfig.Skin;
                }

                //Grid.MasterTableView.EnableColumnsViewState = false;

                
                Grid.MasterTableView.CommandItemDisplay = (Telerik.Web.UI.GridCommandItemDisplay)Enum.Parse(typeof(Telerik.Web.UI.GridCommandItemDisplay), GridConfig.CommandItemDisplay.ToString());
                Grid.MasterTableView.ShowHeader = GridConfig.ShowHeader;

                SetupCsvExportSettings(Grid, GridConfig);
                Grid.MasterTableView.CommandItemSettings.ShowRefreshButton = GridConfig.ShowRefreshButton;

                Grid.MasterTableView.CommandItemSettings.ShowExportToExcelButton = GridConfig.ShowExportToExcelButton;
                Grid.MasterTableView.CommandItemSettings.ShowExportToPdfButton = GridConfig.ShowExportToPdfButton;
                Grid.MasterTableView.CommandItemSettings.ShowExportToWordButton = GridConfig.ShowExportToWordButton;

                //Grid.ExportSettings.ExportOnlyData = GridConfig.ExportOnlyData;
                Grid.ExportSettings.OpenInNewWindow = GridConfig.ExportOpenInNewWindow;
                Grid.ExportSettings.IgnorePaging = GridConfig.ExportIgnorePaging;
                Grid.ExportSettings.HideStructureColumns = GridConfig.ExportHideStructureColumns;
                if (!String.IsNullOrEmpty(GridConfig.ExportFileName))
                {
                    Grid.ExportSettings.FileName = GridConfig.ExportFileName;
                }
                else
                {
                    if (!String.IsNullOrEmpty(GridConfig.Name))
                    {
                        Grid.ExportSettings.FileName = GridConfig.Name;
                    }
                }
                
                Grid.MasterTableView.EnableColumnsViewState = false;
               // SectionHeaderLabel.Text = GetGlobalResourceString(page, GridConfig, ResourceKeyPrefix, "Name", GridConfig.Name);
                //SectionHelpTextLabel.Text = GetGlobalResourceString(page, GridConfig, ResourceKeyPrefix, "HelpText", GridConfig.HelpText); 
                Grid.AllowSorting = GridConfig.AllowSorting;
                Grid.AllowPaging = GridConfig.AllowPaging;

                if (!String.IsNullOrEmpty(GridConfig.DataKeyNames))
                    Grid.MasterTableView.DataKeyNames = GridConfig.DataKeyNames.Split(',');

                if (Grid.AllowPaging)
                {
                    Grid.PageSize = GridConfig.PageSize;
                }

                SetupGridScrolling(Grid, GridConfig);



                foreach (CodeTorch.Core.GridColumn column in GridConfig.Columns)
                {
                    bool isColumnVisible = true;

                    if (column.VisiblePermission.CheckPermission)
                    {
                        isColumnVisible = Common.HasPermission(column.VisiblePermission.Name);
                    }

                    if(isColumnVisible)
                    {
                        GridFunctions.BuildGridColumn(page,Grid, GridConfig, column, ResourceKeyPrefix + ".Columns");
                    }
                }

                SetupGridGrouping(Grid, GridConfig);
                SetupGridClientSettings(Grid, GridConfig);

            }

        }


        private static void SetupGridScrolling(RadGrid Grid, CodeTorch.Core.Grid GridConfig)
        {
            //scrolling
            Grid.ClientSettings.Scrolling.AllowScroll = GridConfig.AllowScroll;
            Grid.ClientSettings.Scrolling.FrozenColumnsCount = GridConfig.FrozenColumnsCount;
            Grid.ClientSettings.Scrolling.SaveScrollPosition = GridConfig.SaveScrollPosition;
            Grid.ClientSettings.Scrolling.UseStaticHeaders = GridConfig.UseStaticHeaders;

            if (!String.IsNullOrEmpty(GridConfig.ScrollBarWidth))
                Grid.ClientSettings.Scrolling.ScrollBarWidth = new Unit(GridConfig.ScrollBarWidth);

            if (!String.IsNullOrEmpty(GridConfig.ScrollHeight))
                Grid.ClientSettings.Scrolling.ScrollHeight = new Unit(GridConfig.ScrollHeight);

        }

        public static void BuildEditableGrid(BasePage page, RadGrid Grid, CodeTorch.Core.Grid GridConfig, ScreenActionLink AddLink,  string ResourceKeyPrefix)
        {
            if (GridConfig != null)
            {
                if (!String.IsNullOrEmpty(GridConfig.CssClass))
                {
                    Grid.CssClass = GridConfig.CssClass;
                }

                if (!String.IsNullOrEmpty(GridConfig.SkinID))
                {
                    Grid.SkinID = GridConfig.SkinID;
                }

                if (!String.IsNullOrEmpty(GridConfig.Skin))
                {
                    Grid.Skin = GridConfig.Skin;
                }

                Grid.MasterTableView.CommandItemDisplay = (Telerik.Web.UI.GridCommandItemDisplay)Enum.Parse(typeof(Telerik.Web.UI.GridCommandItemDisplay), GridConfig.CommandItemDisplay.ToString());
                Grid.MasterTableView.CommandItemSettings.AddNewRecordText = AddLink.Text;
                Grid.MasterTableView.ShowHeader = GridConfig.ShowHeader;


                Grid.MasterTableView.CommandItemSettings.ShowRefreshButton = GridConfig.ShowRefreshButton;
                SetupCsvExportSettings(Grid, GridConfig);
                Grid.MasterTableView.CommandItemSettings.ShowExportToExcelButton = GridConfig.ShowExportToExcelButton;
                Grid.MasterTableView.CommandItemSettings.ShowExportToPdfButton = GridConfig.ShowExportToPdfButton;
                Grid.MasterTableView.CommandItemSettings.ShowExportToWordButton = GridConfig.ShowExportToWordButton;

                

                Grid.ExportSettings.OpenInNewWindow = GridConfig.ExportOpenInNewWindow;
                Grid.ExportSettings.IgnorePaging = GridConfig.ExportIgnorePaging;
                Grid.ExportSettings.HideStructureColumns = GridConfig.ExportHideStructureColumns;
                if (!String.IsNullOrEmpty(GridConfig.ExportFileName))
                {
                    Grid.ExportSettings.FileName = GridConfig.ExportFileName;
                }
                else
                {
                    if (!String.IsNullOrEmpty(GridConfig.Name))
                    {
                        Grid.ExportSettings.FileName = GridConfig.Name;
                    }
                }

                Grid.MasterTableView.EnableColumnsViewState = false;

                Grid.MasterTableView.EditMode = GridEditMode.EditForms;
                Grid.MasterTableView.EditFormSettings.EditFormType = GridEditFormType.WebUserControl;
                Grid.MasterTableView.EditFormSettings.UserControlName = "~/templates/sections/griddetail.ascx";

                //SectionHeaderLabel.Text = GetGlobalResourceString(page, GridConfig, ResourceKeyPrefix, "Name", GridConfig.Name);
                //SectionHelpTextLabel.Text = GetGlobalResourceString(page, GridConfig, ResourceKeyPrefix, "HelpText", GridConfig.HelpText); 
                Grid.AllowSorting = GridConfig.AllowSorting;
                Grid.AllowPaging = GridConfig.AllowPaging;

                if (!String.IsNullOrEmpty(GridConfig.DataKeyNames))
                    Grid.MasterTableView.DataKeyNames = GridConfig.DataKeyNames.Split(',');

                if (Grid.AllowPaging)
                {
                    Grid.PageSize = GridConfig.PageSize;
                }

                SetupGridScrolling(Grid, GridConfig);

                foreach (CodeTorch.Core.GridColumn column in GridConfig.Columns)
                {
                    bool isColumnVisible = true;

                    if (column.VisiblePermission.CheckPermission)
                    {
                        isColumnVisible = Common.HasPermission(column.VisiblePermission.Name);
                    }

                    if(isColumnVisible)
                    {
                        GridFunctions.BuildGridColumn(page,Grid, GridConfig, column, ResourceKeyPrefix + ".Columns");
                    }
                  
                }

                SetupGridGrouping(Grid, GridConfig);
                SetupGridClientSettings(Grid, GridConfig);
            }

        }

        public static void SetupGridClientSettings(RadGrid Grid, CodeTorch.Core.Grid GridConfig)
        {
            if (!String.IsNullOrEmpty(GridConfig.DataKeyNames))
                Grid.MasterTableView.ClientDataKeyNames = GridConfig.DataKeyNames.Split(',');
           // Grid.MasterTableView.ClientDataKeyNames

            Grid.ClientSettings.AllowRowsDragDrop = GridConfig.AllowRowsDragDrop;
            Grid.ClientSettings.Selecting.AllowRowSelect = GridConfig.AllowRowSelect;
            Grid.ClientSettings.Selecting.EnableDragToSelectRows = GridConfig.EnableDragToSelectRows;

            Grid.ClientSettings.ClientEvents.OnRowDropping = GridConfig.OnRowDropping;
            Grid.ClientSettings.ClientEvents.OnRowDropped = GridConfig.OnRowDropped;
            


        }

        private static void SetupCsvExportSettings(RadGrid Grid, CodeTorch.Core.Grid GridConfig)
        {
            Grid.MasterTableView.CommandItemSettings.ShowExportToCsvButton = GridConfig.ShowExportToCsvButton;
            Grid.ExportSettings.Csv.ColumnDelimiter = (Telerik.Web.UI.GridCsvDelimiter)Enum.Parse(typeof(Telerik.Web.UI.GridCsvDelimiter), GridConfig.CsvColumnDelimiter.ToString());
            Grid.ExportSettings.Csv.RowDelimiter = (Telerik.Web.UI.GridCsvDelimiter)Enum.Parse(typeof(Telerik.Web.UI.GridCsvDelimiter), GridConfig.CsvRowDelimiter.ToString());
            Grid.ExportSettings.Csv.EncloseDataWithQuotes = GridConfig.CsvEncloseDataWithQuotes;

            if (!String.IsNullOrEmpty(GridConfig.CsvFileExtension))
                Grid.ExportSettings.Csv.FileExtension = GridConfig.CsvFileExtension;

        }

        private static void SetupGridGrouping(RadGrid Grid, CodeTorch.Core.Grid GridConfig)
        {
            if (GridConfig != null)
            {
                Grid.GroupingEnabled = GridConfig.GroupingEnabled;

                if (Grid.GroupingEnabled)
                {
                    Grid.MasterTableView.GroupLoadMode = (Telerik.Web.UI.GridGroupLoadMode)Enum.Parse(typeof(Telerik.Web.UI.GridGroupLoadMode), GridConfig.GroupLoadMode.ToString());
                    if (Grid.MasterTableView.GroupLoadMode == Telerik.Web.UI.GridGroupLoadMode.Client)
                    {
                        Grid.ClientSettings.AllowGroupExpandCollapse = true;
                    }
                    else
                    {
                        Grid.ClientSettings.AllowGroupExpandCollapse = GridConfig.AllowGroupExpandCollapse;
                    }

                    Grid.MasterTableView.GroupsDefaultExpanded = GridConfig.GroupsDefaultExpanded;
                    Grid.ShowGroupPanel = GridConfig.ShowGroupPanel;
                    Grid.ClientSettings.AllowGroupExpandCollapse = GridConfig.AllowGroupExpandCollapse;



                    foreach (CodeTorch.Core.GridGroupByExpression expression in GridConfig.GroupByExpressions)
                    {
                        Telerik.Web.UI.GridGroupByExpression e = new Telerik.Web.UI.GridGroupByExpression();

                        foreach (CodeTorch.Core.GridGroupByField field in expression.Fields)
                        {
                            Telerik.Web.UI.GridGroupByField gridGroupByField = new Telerik.Web.UI.GridGroupByField();

                            if (field is GridGroupBySelectField)
                            {
                                GridGroupBySelectField selectfield = (GridGroupBySelectField)field;
                                gridGroupByField.FieldName = selectfield.FieldName;

                                if (!String.IsNullOrEmpty(selectfield.FieldAlias))
                                {
                                    gridGroupByField.FieldAlias = selectfield.FieldAlias;
                                }
                                gridGroupByField.Aggregate = (Telerik.Web.UI.GridAggregateFunction)Enum.Parse(typeof(Telerik.Web.UI.GridAggregateFunction), selectfield.Aggregate.ToString());
                                gridGroupByField.HeaderText = selectfield.HeaderText;
                                gridGroupByField.HeaderValueSeparator = selectfield.HeaderValueSeparator;
                                gridGroupByField.FormatString = selectfield.FormatString;
                                

                                e.SelectFields.Add(gridGroupByField);
                            }

                            if (field is GridGroupByGroupField)
                            {
                                GridGroupByGroupField groupfield = (GridGroupByGroupField)field;
                                gridGroupByField.FieldName = groupfield.FieldName;
                                gridGroupByField.SortOrder = (Telerik.Web.UI.GridSortOrder) Enum.Parse(typeof(Telerik.Web.UI.GridSortOrder), groupfield.SortOrder.ToString()); 

                                e.GroupByFields.Add(gridGroupByField);
                            }
                        }

                        Grid.MasterTableView.GroupByExpressions.Add(e);
                    }
                }

            }

        }

        public static void HandleItemCommand(RadGrid Grid, CodeTorch.Core.Grid GridConfig, object source, GridCommandEventArgs e)
        {
            //HANDLE EDIT GRIDS
            if (e.CommandName == RadGrid.InitInsertCommandName) //"Add new" button clicked
            {
                GridEditCommandColumn editColumn = GetEditColumn(Grid, GridConfig);
                if (editColumn != null)
                {
                    editColumn.Visible = false;
                }
            }
            else if (e.CommandName == RadGrid.RebindGridCommandName && e.Item.OwnerTableView.IsItemInserted)
            {
                //dont't allow refreshing of grid while in add mode
                e.Canceled = true;
            }
            else
            {
                GridEditCommandColumn editColumn = GetEditColumn(Grid, GridConfig);
                if (editColumn != null)
                {
                    if (!editColumn.Visible)
                        editColumn.Visible = true;
                }
            }

            //HANDLE EXPORTS
            if(
                 (e.CommandName == RadGrid.ExportToCsvCommandName) || 
                 (e.CommandName == RadGrid.ExportToExcelCommandName) || 
                 (e.CommandName == RadGrid.ExportToPdfCommandName) || 
                 (e.CommandName == RadGrid.ExportToWordCommandName) 
              )
            {
                int gridColumnIndex = 0;
                foreach (CodeTorch.Core.GridColumn column in GridConfig.Columns)
                {
                    bool isColumnVisible = true;

                    if (column.VisiblePermission.CheckPermission)
                    {
                        isColumnVisible = Common.HasPermission(column.VisiblePermission.Name);
                    }

                    if (isColumnVisible)
                    {
                        bool includeInExport = column.IncludeInExport;

                        if(includeInExport)
                        {
                            if (column.ExportPermission.CheckPermission)
                            {
                                includeInExport = Common.HasPermission(column.ExportPermission.Name);
                            } 
                        }

                        if (!includeInExport)
                        {
                            Grid.Columns[gridColumnIndex].Visible = false;
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(column.ExportDataField))
                            {
                                switch (column.ColumnType)
                                {
                                    case GridColumnType.BoundGridColumn:
                                        GridBoundColumn boundcol = ((GridBoundColumn)Grid.Columns[gridColumnIndex]);
                                        boundcol.DataField = column.ExportDataField;
                                        break;
                                    case GridColumnType.HyperLinkGridColumn:
                                        GridHyperLinkColumn hypercol = ((GridHyperLinkColumn)Grid.Columns[gridColumnIndex]);
                                        hypercol.DataTextField = column.ExportDataField;
                                        break;

                                }
                            }
                        }

                        gridColumnIndex++;
                    }
                }
            }
        }

        public static void HandleGridExporting(RadGrid Grid, Grid grid, object source, GridExportingArgs e, HttpResponse Response)
        {
            if (e.ExportType == ExportType.Csv)
            {
                Response.ContentType = "application/csv";
                Response.AddHeader("Content-Type", "text/csv");
                Response.BinaryWrite(new ASCIIEncoding().GetBytes(e.ExportOutput));
                Response.End();
            }
        }

        public static void HandleInsertCommand(BasePage page, RadGrid Grid, CodeTorch.Core.Grid GridConfig,  string InsertCommandName, object source, GridCommandEventArgs e)
        {
            DataCommandService dataCommandDB = DataCommandService.GetInstance();
            PageDB pageDB = new PageDB();

            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            if (page.IsValid)
            {
                try
                {
                    List<ScreenDataCommandParameter> parameters = null;

                    log.Info("\r\n\r\nIn Insert Mode");
                    if (String.IsNullOrEmpty(InsertCommandName))
                    {
                        throw new ApplicationException("InsertCommand is invalid");
                    }
                    log.DebugFormat("InsertCommand:{0}", InsertCommandName);

                    parameters = pageDB.GetPopulatedCommandParameters(InsertCommandName, page);
                    dataCommandDB.ExecuteDataCommand(InsertCommandName, parameters);
                }
                catch (Exception ex)
                {
                    e.Canceled = true;
                    page.DisplayErrorAlert(ex);
                }
            }
        }

        public static void HandleUpdateCommand(BasePage page, RadGrid Grid, CodeTorch.Core.Grid GridConfig, string UpdateCommandName, object source, GridCommandEventArgs e)
        {
            DataCommandService dataCommandDB = DataCommandService.GetInstance();
            PageDB pageDB = new PageDB();

            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            if (page.IsValid)
            {
                try
                {
                    
                    List<ScreenDataCommandParameter> parameters = null;

                    log.Info("In Edit Mode");
                    if (String.IsNullOrEmpty(UpdateCommandName))
                    {
                        throw new ApplicationException("UpdateCommand is invalid");
                    }
                    log.DebugFormat("UpdateCommand:{0}", UpdateCommandName);

                    if (String.IsNullOrEmpty(GridConfig.DataKeyNames) || String.IsNullOrEmpty(GridConfig.DataKeyParameterNames))
                    {
                        throw new ApplicationException("Grid DataKeyNames or DataKeyParameterNames is not set in configuration");
                    }


                    parameters = pageDB.GetPopulatedCommandParameters(UpdateCommandName, page);

                    string[] dataKeyArray = GridConfig.DataKeyNames.Split(',');
                    string[] dataKeyParamArray = GridConfig.DataKeyParameterNames.Split(',');

                    if ((dataKeyArray.Length > 0))
                    {
                        //map grid data keys to the screen data command parameters 
                        for (int i = 0; i < dataKeyArray.Length; i++)
                        {
                            if (i < dataKeyParamArray.Length)
                            {
                                SetGridDataKeysScreenDataCommandParameter(parameters, dataKeyParamArray[i], Grid.MasterTableView.DataKeyValues[e.Item.ItemIndex][dataKeyParamArray[i].Replace("@", "")]);
                            }
                        }
                    }
                    else
                    {
                        throw new ApplicationException("Grid DataKeyNames is not set in configuration");
                    }

                    dataCommandDB.ExecuteDataCommand(UpdateCommandName, parameters);

                }
                catch (Exception ex)
                {

                    e.Canceled = true;
                    page.DisplayErrorAlert(ex);
                }
            }


        }

        public static void HandleEditableGridItemCreated(BasePage page, RadGrid Grid, CodeTorch.Core.Grid GridConfig, List<Section> Sections, string SectionZoneLayout, bool DisplayAddButton, object source, GridItemEventArgs e)
        {


            if (e.Item is GridCommandItem)
            {
                if (!DisplayAddButton)
                {
                    GridFunctions.HideAddCommandItem(Grid);
                }
            }

            if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
            {
                Control c = page.FindControlRecursive(e.Item, GridEditFormItem.EditFormUserControlID);
                if (c != null)
                {
                    GridDetail detail = ((GridDetail)c);

                    detail.Sections = Sections;
                    detail.SectionZoneLayout = SectionZoneLayout;

                    if (e.Item.OwnerTableView.IsItemInserted)
                    {
                        detail.Mode = SectionMode.Insert;
                        detail.Render();

                        detail.SetSaveButtonToInsertMode();
                        detail.SettingSectionTitle("Add");

                    }
                    else
                    {
                        detail.Mode = SectionMode.Edit;
                        
                        detail.Render();


                        detail.SetSaveButtonToUpdateMode();
                        detail.SettingSectionTitle("Edit");


                    }


                }
            }
        }

        public static void HandleItemDataBound(BasePage page, RadGrid Grid, CodeTorch.Core.Grid GridConfig,  object sender, GridItemEventArgs e)
        {
            HandleBinaryImageColumnsOnDataBound(Grid, GridConfig, e);
        }

        public static void HandleEditableGridItemDataBound(BasePage page, RadGrid Grid, CodeTorch.Core.Grid GridConfig,  bool UseDefaultCommand, string DefaultCommandName, object sender, GridItemEventArgs e)
        {
            HandleBinaryImageColumnsOnDataBound(Grid, GridConfig, e);


            HandleEditableGridDetailPopulation(page, GridConfig, UseDefaultCommand, DefaultCommandName, e);

           
        }

        private static void HandleBinaryImageColumnsOnDataBound(RadGrid Grid, CodeTorch.Core.Grid GridConfig, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;

                int gridColumnIndex = 0;
                foreach (CodeTorch.Core.GridColumn column in GridConfig.Columns)
                {
                    if (column is BinaryImageGridColumn)
                    {
                        TableCell cell = null;
                        RadBinaryImage img = null;
                        BinaryImageGridColumn c = ((BinaryImageGridColumn)column);

                        if (!String.IsNullOrEmpty(column.UniqueName))
                        {
                            cell = dataItem[column.UniqueName];
                            img = (RadBinaryImage)cell.Controls[0];
                        }
                        else
                        {
                            cell = dataItem[Grid.Columns[gridColumnIndex]];
                            img = (RadBinaryImage)cell.Controls[0];
                        }

                        if (img != null)
                        {
                            if (!String.IsNullOrEmpty(c.DataNavigateUrlFields) || !String.IsNullOrEmpty(c.DataNavigateUrlFormatString)) 
                            {
                                HyperLink link = new HyperLink();
                                string linkUrl = String.Empty;

                                string urlFormatString = Common.CreateUrlWithQueryStringContext(c.DataNavigateUrlFormatString, c.Context);
                                if (!String.IsNullOrEmpty(c.DataNavigateUrlFields))
                                {
                                    object[] fields = c.DataNavigateUrlFields.Split(',');
                                    object[] fieldValues = new object[fields.Length];

                                    for(int i=0; i<fields.Length; i++)
                                    {
                                        string fieldName = fields[i].ToString();
                                        object value = ((DataRowView)e.Item.DataItem)[fieldName];
                                        fieldValues[i] = value ;
                                    }

                                    linkUrl = String.Format(urlFormatString, fieldValues);
                                }

                                link.NavigateUrl = linkUrl;

                                link.Target = Common.CoalesceStr(link.Target, c.Target);

                                cell.Controls.Remove(img);
                                link.Controls.Add(img);
                                cell.Controls.AddAt(0, link);
                            }

                            if (((DataRowView)e.Item.DataItem)[c.DataField] is DBNull)
                            {
                                string imageUrl = c.DefaultImageUrl;
                                if (!String.IsNullOrEmpty(c.DefaultImageUrlField))
                                {
                                    if (String.IsNullOrEmpty(c.DefaultImageUrlFieldFormat))
                                    {
                                        imageUrl = ((DataRowView)e.Item.DataItem)[c.DefaultImageUrlField].ToString();
                                        if (!imageUrl.ToLower().Contains("//"))
                                        {
                                            imageUrl = "//" + imageUrl;
                                        }
                                    }
                                    else
                                    {
                                        imageUrl = String.Format(c.DefaultImageUrlFieldFormat, ((DataRowView)e.Item.DataItem)[c.DataAlternateTextField]);
                                    }
                                }

                                if (!String.IsNullOrEmpty(imageUrl))
                                    img.ImageUrl = imageUrl;
                            }

                            
                        }
                    }

                    gridColumnIndex++;
                }

            }
        }

        private static void HandleEditableGridDetailPopulation(BasePage page, CodeTorch.Core.Grid GridConfig, bool UseDefaultCommand, string DefaultCommandName, GridItemEventArgs e)
        {
            if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
            {
                Control c = page.FindControlRecursive(e.Item, GridEditFormItem.EditFormUserControlID);
                if (c != null)
                {
                    GridDetail detail = ((GridDetail)c);

                   

                    if (e.Item.OwnerTableView.IsItemInserted)
                    {
                        if (UseDefaultCommand)
                        {
                            if (!String.IsNullOrEmpty(DefaultCommandName))
                            {
                                DataCommandService dataCommandDB = DataCommandService.GetInstance();
                                PageDB pageDB = new PageDB();

                                List<ScreenDataCommandParameter> parameters = pageDB.GetPopulatedCommandParameters(DefaultCommandName, page);
                                DataTable data = dataCommandDB.GetDataForDataCommand(GridConfig.SelectDataCommand, parameters);
                                detail.Default(e.Item, data);
                            }
                        }
                    }
                    else
                    {
                        DataRowView data = ((DataRowView)detail.DataItem);
                        detail.Populate(e.Item);
                    }

                    detail.ExecuteAfterPopulateSections();
                }
            }
        }

       

        public static void HandleDeleteCommand(BasePage page, RadGrid Grid, CodeTorch.Core.Grid GridConfig, object source, GridCommandEventArgs e)
        {
            DataCommandService dataCommandDB = DataCommandService.GetInstance();
            PageDB pageDB = new PageDB();

            try
            {


                if (GridConfig.DeleteDataCommand != String.Empty)
                {
                    if (String.IsNullOrEmpty(GridConfig.DataKeyNames) || String.IsNullOrEmpty(GridConfig.DataKeyParameterNames))
                    {
                        throw new ApplicationException("Grid DataKeyNames or DataKeyParameterNames is not set in configuration");
                    }
                    else
                    {
                        List<ScreenDataCommandParameter> parameters = pageDB.GetPopulatedCommandParameters(GridConfig.DeleteDataCommand, page);
                        string[] dataKeyArray = GridConfig.DataKeyNames.Split(',');
                        string[] dataKeyParamArray = GridConfig.DataKeyParameterNames.Split(',');

                        if ((dataKeyArray.Length > 0))
                        {
                            //map grid data keys to the screen data command parameters 
                            for (int i = 0; i < dataKeyArray.Length; i++)
                            {
                                if (i < dataKeyParamArray.Length)
                                {
                                    SetGridDataKeysScreenDataCommandParameter(parameters, dataKeyParamArray[i],
                                            e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex][dataKeyArray[i]]
                                            );
                                }
                            }

                            dataCommandDB.ExecuteDataCommand(GridConfig.DeleteDataCommand, parameters);


                        }
                        else
                        {
                            throw new ApplicationException("Grid DataKeyNames is not set in configuration");
                        }

                    }

                }
                else
                {
                    throw new ApplicationException("DeleteCommand is invalid");
                }

            }
            catch (Exception ex)
            {
                page.DisplayErrorAlert(ex);

                Common.LogException(ex, false);
            }

        }

        public static GridEditCommandColumn GetEditColumn(RadGrid Grid,  CodeTorch.Core.Grid GridConfig)
        {
            GridEditCommandColumn col = null;
            CodeTorch.Core.GridColumn editColumn = GridConfig.Columns.Where(c =>
                            (
                                (c is EditGridColumn)
                            )
                        )
                        .SingleOrDefault();

            if (editColumn != null)
            {
                int columnIndex = Enumerable.Range(0, GridConfig.Columns.Count).First(i => GridConfig.Columns[i] is EditGridColumn);
                col = (GridEditCommandColumn)Grid.MasterTableView.Columns[columnIndex]; ;
            }

            return col;
        }

        public static void SetGridDataKeysScreenDataCommandParameter(List<ScreenDataCommandParameter> parameters, string dataKeyParam, object value)
        {

            if (!String.IsNullOrEmpty(dataKeyParam))
            {
                ScreenDataCommandParameter mappedParam = parameters.Where(p =>
                        (
                            (p.Name.ToLower() == dataKeyParam.ToLower())
                        )
                    ).SingleOrDefault();

                if (mappedParam != null)
                {
                    mappedParam.Value = value;
                }
            }

        }

        public static void FillGrid(BasePage page, RadGrid Grid, CodeTorch.Core.Grid GridConfig)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            try
            {
                DataCommandService dataCommandDB = DataCommandService.GetInstance();
                PageDB pageDB = new PageDB();

                if (String.IsNullOrEmpty(GridConfig.SelectDataCommand))
                {
                    throw new ApplicationException("SelectDataCommand is not configured");
                }

                List<ScreenDataCommandParameter> parameters = pageDB.GetPopulatedCommandParameters(GridConfig.SelectDataCommand, page);

                Grid.DataSource = dataCommandDB.GetDataForDataCommand(GridConfig.SelectDataCommand, parameters);

                
            }
            catch (Exception ex)
            {
                page.DisplayErrorAlert(ex);

                log.Error(ex);
            }
        }

        public static bool CheckActionLinkPermission(ScreenActionLink ActionLink)
        {
            bool DisplayAddButton = false;
            if (ActionLink != null)
            {
                if (ActionLink.ShowLink)
                {
                    if (ActionLink.Permission.CheckPermission)
                    {

                        if (Common.HasPermission(ActionLink.Permission.Name))
                        {
                            DisplayAddButton = true;
                        }

                    }
                    else
                    {
                        DisplayAddButton = true;

                    }
                }

            }

            return DisplayAddButton;

        }

        public static string GetGlobalResourceString(BasePage page, Core.GridColumn column,  string ResourceKeyPrefix, string ResourceKey, string DefaultValue)
        {
            string retVal = null;
            string ResourceSet = Common.StripVirtualPath(page.Request.Url.AbsolutePath);


            retVal = GetGlobalResourceString(page, column, ResourceSet, ResourceKeyPrefix, ResourceKey, DefaultValue);

            return retVal;
            
        }

        public static string GetGlobalResourceString(BasePage page, Core.GridColumn column, string ResourceSet, string ResourceKeyPrefix, string ResourceKey, string DefaultValue)
        {
            string retVal = DefaultValue;

            string ActualResourceKey = "";

            if (String.IsNullOrEmpty(ResourceKeyPrefix))
            {
                ActualResourceKey = String.Format("{0}.{1}", column.UniqueName, ResourceKey);
            }
            else
            {
                ActualResourceKey = String.Format("{0}.{1}.{2}", ResourceKeyPrefix, column.UniqueName, ResourceKey);
            }


            
            retVal  = page.GetGlobalResourceString(ResourceSet, ActualResourceKey, DefaultValue);
             
            


            return retVal;
        }

        public static string GetGlobalResourceString(BasePage page, Core.Grid grid, string ResourceKeyPrefix, string ResourceKey, string DefaultValue)
        {
            string retVal = null;
            string ResourceSet = Common.StripVirtualPath(page.Request.Url.AbsolutePath);


            retVal = GetGlobalResourceString(page, grid, ResourceSet, ResourceKeyPrefix, ResourceKey, DefaultValue);

            return retVal;

        }

        public static string GetGlobalResourceString(BasePage page, Core.Grid grid, string ResourceSet, string ResourceKeyPrefix, string ResourceKey, string DefaultValue)
        {
            string retVal = DefaultValue;

            string ActualResourceKey = "";

            if (String.IsNullOrEmpty(ResourceKeyPrefix))
            {
                ActualResourceKey = String.Format("{0}",  ResourceKey);
            }
            else
            {
                ActualResourceKey = String.Format("{0}.{1}", ResourceKeyPrefix, ResourceKey);
            }



            retVal = page.GetGlobalResourceString(ResourceSet, ActualResourceKey, DefaultValue);




            return retVal;
        }
        
    }
}
