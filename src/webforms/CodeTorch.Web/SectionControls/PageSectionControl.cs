using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.HtmlControls;
using CodeTorch.Core.Services;
using CodeTorch.Web.Data;

using CodeTorch.Core;
using CodeTorch.Web.Templates;

namespace CodeTorch.Web.SectionControls
{
    public class PageSectionControl: BaseSectionControl
    {

        HtmlGenericControl groupContainer;

        DetailsSection _DetailsSection = null;
        public DetailsSection DetailsSection
        {
            get
            {
                if ((_DetailsSection == null) && (this.Section != null) && (this.Section is DetailsSection))
                {
                    _DetailsSection = (DetailsSection)this.Section;
                }
                return _DetailsSection;
            }
        }

        EditSection _EditSection = null;
        public EditSection EditSection
        {
            get
            {
                if ((_EditSection == null) && (this.Section != null) && (this.Section is EditSection))
                {
                    _EditSection = (EditSection)this.Section;
                }
                return _EditSection;
            }
        }

        CriteriaSection _CriteriaSection = null;
        public CriteriaSection CriteriaSection
        {
            get
            {
                if ((_CriteriaSection == null) && (this.Section != null) && (this.Section is CriteriaSection))
                {
                    _CriteriaSection = (CriteriaSection)this.Section;
                }
                return _CriteriaSection;
            }
        }

        

        public override void RenderControl()
        {
            base.RenderControl();

            string ContainerElement = null;
            string ContainerCssClass = null;

            if (DetailsSection != null)
            { 
                ContainerElement = DetailsSection.ContainerElement;
                ContainerCssClass = DetailsSection.ContainerCssClass;
            }

            if (EditSection != null)
            {
                ContainerElement = EditSection.ContainerElement;
                ContainerCssClass = EditSection.ContainerCssClass;
            }

            if (CriteriaSection != null)
            {
                ContainerElement = CriteriaSection.ContainerElement;
                ContainerCssClass = CriteriaSection.ContainerCssClass;
            }

            if (!String.IsNullOrEmpty(ContainerElement))
            {
                groupContainer = new HtmlGenericControl(ContainerElement);

                if (!String.IsNullOrEmpty(ContainerCssClass))
                    groupContainer.Attributes.Add("class", ContainerCssClass);

                this.ContentPlaceHolder.Controls.Add(groupContainer);
            }
            


            foreach (Widget control in Section.Widgets)
            {
                AddControl(Screen, Section, groupContainer, control);
            }


        }

        public override void PopulateControl()
        {
            base.PopulateControl();

            string DataCommandName = null;

            if (DetailsSection != null)
            {
                DataCommandName = DetailsSection.SelectDataCommand;
            }


            if (!String.IsNullOrEmpty(DataCommandName))
            {
                DataCommandService dataCommandDB = DataCommandService.GetInstance();
                PageDB pageDB = new PageDB();

                List<ScreenDataCommandParameter> parameters = null;
                BasePage page = (BasePage)this.Page;

                parameters = GetParameters(DataCommandName);
                DataTable data = dataCommandDB.GetDataForDataCommand(DataCommandName, parameters);

                page.PopulateFormByDataTable(Section.Widgets, data);

            }

            ExecuteAfterPopulateSection();
        }

        private void ExecuteAfterPopulateSection()
        {
            try
            {
                ActionRunner runner = new ActionRunner();
                Core.Action action = null;

                if (Section is DetailsSection)
                {
                    if (DetailsSection.AfterPopulateSection != null)
                    {
                        action = ObjectCopier.Clone<Core.Action>(DetailsSection.AfterPopulateSection);
                    }
                }
                
                if(Section is EditSection)
                {
                    action = ObjectCopier.Clone<Core.Action>(EditSection.AfterPopulateSection);
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

        private List<ScreenDataCommandParameter> GetParameters(string DataCommandName)
        {
            PageDB pageDB = new PageDB();
            List<ScreenDataCommandParameter> parameters;
            BasePage page = ((BasePage)this.Page);
            parameters = pageDB.GetPopulatedCommandParameters(DataCommandName, (BasePage)this.Page);
            return parameters;
        }

        



        

       

        

       
        

      

    }
}
