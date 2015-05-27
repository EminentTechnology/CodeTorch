using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using CodeTorch.Core;
using System.Web.UI.HtmlControls;
using CodeTorch.Web.Templates;
using System.Web.UI.WebControls;
using System.Data;
using CodeTorch.Web.SectionControls;

namespace CodeTorch.Web.UserControls
{
    public class GridDetail : UserControl
    {
        //used in edit and overview
        protected PlaceHolder SectionLayout;
        

    

        public Screen Screen { get; set; }
        public List<Section> Sections { get; set; }

        public List<BaseSectionControl> SectionControls = new List<BaseSectionControl>();

        private object _dataItem = null;
        public object DataItem
        {
            get
            {
                return this._dataItem;
            }
            set
            {
                this._dataItem = value;
            }
        }

        public SectionMode Mode { get; set; }
        public string SectionZoneLayout { get; set; }

        BasePage page;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            page = (BasePage)this.Page;

            Screen = page.Screen;

            

            
        }

        private void PopulateSections()
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            try
            {
                foreach (BaseSectionControl section in this.SectionControls)
                {
                    try
                    {
                        section.PopulateControl();


                    }
                    catch (Exception ex)
                    {
                        string ErrorMessageFormat = "<span class='ErrorMessages'>ERROR - {0} - {2} Section - {1})</span>";
                        string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, section.Section.Name, section.Section.Type);

                        section.Controls.Add(new LiteralControl(ErrorMessages));
                    }
                }


            }
            catch (Exception ex)
            {
                page.DisplayErrorAlert(ex);

                log.Error(ex);
            }
        }

       

        public void Render()
        {

            

            List<Section> baseSections = Sections.ConvertAll(x => (Section)x);
            page.RenderPageSections(this.SectionLayout, SectionZoneLayout, Screen, baseSections, false, Mode, "Screen.Sections.");
           

            
        }

        public void SetSaveButtonToInsertMode()
        {
            Control Save = page.FindControlRecursive(this, "Save");
            if (Save != null)
            {
                if (Save is System.Web.UI.WebControls.Button)
                {
                    ((System.Web.UI.WebControls.Button)Save).CommandName = "PerformInsert";
                }

                if (Save is System.Web.UI.WebControls.LinkButton)
                {
                    ((System.Web.UI.WebControls.LinkButton)Save).CommandName = "PerformInsert";
                }

                
            }
        }

        public void SetSaveButtonToUpdateMode()
        {
            Control Save = page.FindControlRecursive(this, "Save");
            if (Save != null)
            {
                if (Save is System.Web.UI.WebControls.Button)
                {
                    ((System.Web.UI.WebControls.Button)Save).CommandName = "Update";
                }

                if (Save is System.Web.UI.WebControls.LinkButton)
                {
                    ((System.Web.UI.WebControls.LinkButton)Save).CommandName = "Update";
                }


            }
        }

      
        public void SettingSectionTitle(string mode)
        {

            //foreach (Control control in LeftPane.Controls)
            //{
            //    if (control != null)
            //    {
            //        PageSectionControl pageSectionControl = (PageSectionControl)control;

            //        string title = pageSectionControl.GetSectionHeaderLabel;
            //        title = Common.Replace(title, "{MODE}", mode, StringComparison.CurrentCultureIgnoreCase);

            //        pageSectionControl.SetSectionTitle(title);
            //        break;
            //    }
            //}
        }

        internal void Populate(Control item)
        {
            BasePage page = (BasePage)this.Page;

            PopulateSections();

            foreach (Section section in Sections)
            {
                page.PopulateFormByDataRowView(item, section.Controls, ((DataRowView)DataItem), true);

            }

            
        }

        internal void Default(Control item, DataTable data)
        {
            BasePage page = (BasePage)this.Page;

            PopulateSections();

            foreach (Section section in Sections)
            {
                page.PopulateFormByDataTable(item, section.Controls, data, true);

            }
        }

        internal void ExecuteAfterPopulateSections()
        {
            foreach (Section section in Sections)
            {
                ExecuteAfterPopulateSection(section);
            }
        }

        private void ExecuteAfterPopulateSection(Section Section)
        {
            try
            {
                ActionRunner runner = new ActionRunner();
                Core.Action action = null;

                if (Section.AfterPopulateSection != null)
                {
                    action = ObjectCopier.Clone<Core.Action>(Section.AfterPopulateSection);
                }


                if (action != null)
                {
                    runner.Page = (BasePage)this.Page;
                    runner.Action = action;
                    runner.Execute();
                }

            }
            catch (Exception ex)
            {
                throw ex;

            }
        }
    }
}
