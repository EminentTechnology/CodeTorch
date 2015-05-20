using System;
using System.Data;
using System.Data.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CodeTorch.Core.Services;
using CodeTorch.Web.Data;
using CodeTorch.Web.UserControls;
using CodeTorch.Web.Templates;
using System.Web.Security;
using Telerik.Web.UI;
using CodeTorch.Core;
using CodeTorch.Core.Messages;

namespace CodeTorch.Web.SectionControls
{
    public class EditableGridSectionControl : BaseSectionControl
    {

        RadGrid Grid;

        protected System.Web.UI.WebControls.HyperLink SectionEditLink;

        bool DisplayAddButton = false;

        EditableGridSection _Me = null;
        public EditableGridSection Me
        {
            get
            {
                if ((_Me == null) && (this.Section != null))
                {
                    _Me = (EditableGridSection)this.Section;
                }
                return _Me;
            }
        }


        BasePage page;

        protected void Page_Init(object sender, EventArgs e)
        {


        }

        public override void RenderControl()
        {
            page = HttpContext.Current.Handler as BasePage;
            base.RenderControl();

            Grid = new RadGrid();
            Grid.AutoGenerateColumns = false;

            this.ContentPlaceHolder.Controls.Add(Grid);

            BuildGrid();
            CheckActionLinkPermission(Me.ActionLink);

            page.MessageBus.Subscribe<PerformSearchMessage>(FillGrid);

            if (!Me.LoadDataOnPageLoad)
            {
                this.Visible = false;
            }

        }

        public void CheckActionLinkPermission(ScreenActionLink ActionLink)
        {
            DisplayAddButton = GridFunctions.CheckActionLinkPermission(ActionLink);

        }

        private void BuildGrid()
        {
            string ResourceKeyPrefix = String.Format("Screen.Sections.{0}.Control", Section.Name);
            GridFunctions.BuildEditableGrid(page, Grid, Me.Grid, Me.ActionLink, ResourceKeyPrefix);

            Grid.ItemCreated += new GridItemEventHandler(Grid_ItemCreated);
            Grid.ItemCommand += new GridCommandEventHandler(Grid_ItemCommand);

            Grid.InsertCommand += new GridCommandEventHandler(Grid_InsertCommand);
            Grid.UpdateCommand += new GridCommandEventHandler(Grid_UpdateCommand);
            Grid.DeleteCommand += new GridCommandEventHandler(Grid_DeleteCommand);
            
            Grid.GridExporting += new OnGridExportingEventHandler(Grid_GridExporting);
            
            Grid.ItemDataBound += new GridItemEventHandler(Grid_ItemDataBound);
            Grid.NeedDataSource += new GridNeedDataSourceEventHandler(Grid_NeedDataSource);


        }

        #region Grid Events
        void Grid_ItemCreated(object source, GridItemEventArgs e)
        {

            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                GridFunctions.HandleEditableGridItemCreated(page, Grid, Me.Grid, Me.Sections, Me.SectionZoneLayout, DisplayAddButton, source, e);
            }
            catch (Exception ex)
            {
                page.DisplayErrorAlert(ex);
                log.Error(ex);
                
            }
        }

        void Grid_InsertCommand(object source, GridCommandEventArgs e)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                GridFunctions.HandleInsertCommand(page, Grid, Me.Grid, Me.InsertCommand, source, e);
            }
            catch (Exception ex)
            {
                page.DisplayErrorAlert(ex);

                log.Error(ex);
            }
        }

        void Grid_UpdateCommand(object source, GridCommandEventArgs e)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                GridFunctions.HandleUpdateCommand(page, Grid, Me.Grid, Me.UpdateCommand, source, e);
            }
            catch (Exception ex)
            {
                page.DisplayErrorAlert(ex);

                log.Error(ex);
            }

        }

        void Grid_DeleteCommand(object source, GridCommandEventArgs e)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                GridFunctions.HandleDeleteCommand(page, Grid, Me.Grid, source, e);
            }
            catch (Exception ex)
            {
                page.DisplayErrorAlert(ex);
                log.Error(ex);
                
            }
        }

        void Grid_NeedDataSource(object source, GridNeedDataSourceEventArgs e)
        {

            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                FillGrid();
            }
            catch (Exception ex)
            {
                page.DisplayErrorAlert(ex);
                log.Error(ex);

            }
        }


        void Grid_ItemCommand(object source, GridCommandEventArgs e)
        {


            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                GridFunctions.HandleItemCommand(Grid, Me.Grid, source, e);
            }
            catch (Exception ex)
            {
                page.DisplayErrorAlert(ex);
                log.Error(ex);
                
            }
        }

        void Grid_ItemDataBound(object source, GridItemEventArgs e)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                bool UseDefaultCommand = !String.IsNullOrEmpty(Me.DefaultCommand);
                GridFunctions.HandleEditableGridItemDataBound(page, Grid, Me.Grid, UseDefaultCommand, Me.DefaultCommand, source, e);
            }
            catch (Exception ex)
            {
                page.DisplayErrorAlert(ex);
                log.Error(ex);
                
            }
        }

        void Grid_GridExporting(object source, GridExportingArgs e)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                GridFunctions.HandleGridExporting(Grid, Me.Grid, source, e, this.Page.Response);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw ex;
            }

        }

        #endregion //end grid events




        public override void PopulateControl()
        {
            base.PopulateControl();
            FillGrid();
        }


        public override void BindDataSource()
        {
            base.BindDataSource();
            Grid.DataBind();
        }

        private void FillGrid(PerformSearchMessage message)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                Grid.MasterTableView.CurrentPageIndex = 0;
                FillGrid();
                Grid.DataBind();
                this.Visible = true;
            }
            catch (Exception ex)
            {
                page.DisplayErrorAlert(ex);
                log.Error(ex);
            }
        }

        private void FillGrid()
        {
            if (String.IsNullOrEmpty(Me.Grid.SelectDataCommand))
            {
                throw new ApplicationException("Invalid SelectDataCommand");

            }

            DataCommandService dataCommand = DataCommandService.GetInstance();
            List<ScreenDataCommandParameter> parameters = GetParameters();

            Grid.DataSource = dataCommand.GetDataForDataCommand(Me.Grid.SelectDataCommand, parameters);



        }

        private List<ScreenDataCommandParameter> GetParameters()
        {
            PageDB pageDB = new PageDB();
            List<ScreenDataCommandParameter> parameters;
            BasePage page = ((BasePage)this.Page);
            parameters = pageDB.GetPopulatedCommandParameters(Me.Grid.SelectDataCommand, (BasePage)this.Page);
            return parameters;
        }





    }
}
