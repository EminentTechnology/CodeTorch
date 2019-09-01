using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using CodeTorch.Core.Services;
using CodeTorch.Web.Data;
using CodeTorch.Web.Templates;
using Telerik.Web.UI;
using CodeTorch.Core;
using CodeTorch.Core.Messages;
using System.Web.UI.HtmlControls;

namespace CodeTorch.Web.SectionControls
{
    public class GridSectionControl: BaseSectionControl
    {

        HtmlGenericControl groupContainer;

        protected HyperLink ActionLink;
        protected Image ActionLinkImage;
        protected Label ActionLinkLabel;
        protected RadGrid Grid;
        
        protected System.Web.UI.WebControls.HyperLink SectionEditLink;
      

        GridSection _Me = null;
        public GridSection Me
        {
            get
            {
                if ((_Me == null) && (this.Section != null))
                {
                    _Me = (GridSection)this.Section;
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

            ActionLink = new HyperLink();
            ActionLinkImage = new Image();
            ActionLinkLabel = new Label();

            Grid = new RadGrid();

            ActionLink.Controls.Add(ActionLinkImage);
            ActionLink.Controls.Add(ActionLinkLabel);

            

            string ContainerElement = Me.ContainerElement;
            string ContainerCssClass = Me.ContainerCssClass;

            if (!String.IsNullOrEmpty(ContainerElement))
            {
                groupContainer = new HtmlGenericControl(ContainerElement);

                if (!String.IsNullOrEmpty(ContainerCssClass))
                    groupContainer.Attributes.Add("class", ContainerCssClass);

                groupContainer.Controls.Add(ActionLink);
                groupContainer.Controls.Add(Grid);

                this.ContentPlaceHolder.Controls.Add(groupContainer);
            }
            else
            {
                this.ContentPlaceHolder.Controls.Add(ActionLink);
                this.ContentPlaceHolder.Controls.Add(Grid);
            }

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
            if (ActionLink != null)
            {
                if (ActionLink.ShowLink)
                {
                    this.ActionLink.NavigateUrl = ActionLink.Url;
                    this.ActionLinkLabel.Text = ActionLink.Text;

                    if (ActionLink.Permission.CheckPermission)
                    {
                        this.ActionLink.Visible = Common.HasPermission(ActionLink.Permission.Name);
                        this.ActionLinkLabel.Visible = this.ActionLink.Visible;
                    }
                    else
                    {
                        if (this.ActionLinkLabel.Text.Trim() == String.Empty)
                        {
                            this.ActionLink.Visible = false;
                            this.ActionLinkLabel.Visible = false;
                        }
                        else
                        {
                            this.ActionLink.Visible = true;
                            this.ActionLinkLabel.Visible = true;
                        }
                    }
                }
                else
                {
                    this.ActionLink.Visible = false;
                }

            }
            else
            {
                this.ActionLink.Visible = false;
            }

        }

        private void BuildGrid()
        {
            string ResourceKeyPrefix = String.Format("Screen.Sections.{0}.Control", Section.Name) ;
            GridFunctions.BuildSimpleGrid(page, Grid, Me.Grid,   ResourceKeyPrefix);

            Grid.DeleteCommand += new GridCommandEventHandler(Grid_DeleteCommand);

            Grid.GridExporting += new OnGridExportingEventHandler(Grid_GridExporting);

            Grid.ItemCommand += new GridCommandEventHandler(Grid_ItemCommand);
            Grid.ItemCreated += new GridItemEventHandler(Grid_ItemCreated);
            Grid.ItemDataBound += new GridItemEventHandler(Grid_ItemDataBound);

            Grid.NeedDataSource += new GridNeedDataSourceEventHandler(Grid_NeedDataSource);


        }

        #region Grid Events
        void Grid_ItemCreated(object sender, GridItemEventArgs e)
        {

            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                if (e.Item is GridCommandItem)
                {
                    GridFunctions.HideAddCommandItem(this.Grid);
                }
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
                GridFunctions.HandleDeleteCommand(page, this.Grid, Me.Grid, source, e);
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
                GridFunctions.HandleItemDataBound(page, this.Grid, Me.Grid,  source, e);
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
                page.DisplayErrorAlert(ex);
                log.Error(ex);
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
