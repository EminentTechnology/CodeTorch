using CodeTorch.Core;
using CodeTorch.Web.Templates;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace CodeTorch.Web.SectionControls
{
    public class CriteriaSectionControl : BaseSectionControl
    {
        System.Web.UI.WebControls.PlaceHolder Criteria;

        BasePage page;

        CriteriaSection _Me = null;
        public CriteriaSection Me
        {
            get
            {
                if ((_Me == null) && (this.Section != null) && (this.Section is CriteriaSection))
                {
                    _Me = (CriteriaSection)this.Section;
                }
                return _Me;
            }
        }

        public override void RenderControl()
        {
            page = HttpContext.Current.Handler as BasePage;
            base.RenderControl();


            Criteria = new PlaceHolder();
            this.ContentPlaceHolder.Controls.Add(Criteria);

            //CodeTorch.Web.CriteriaFunctions.CriteriaButtonClickDelegate clickFunction = this.CriteriaButton_Click;
            RenderCriteria(page,  Me, this.Criteria);
        }


        public override void PopulateControl()
        {
            base.PopulateControl();
        }

        public void RenderCriteria(CodeTorch.Web.Templates.BasePage page,  CodeTorch.Core.CriteriaSection criteria, PlaceHolder CriteriaPlaceholder)
        {
            int NoOfCriteriaFields = criteria.Widgets.Count;
            int RowCount = (NoOfCriteriaFields / criteria.ControlsPerRow);
     
            if ((NoOfCriteriaFields % criteria.ControlsPerRow) > 0)
            {
                RowCount++;
            }

            Control row = null;
            for (int RowIndex = 0; RowIndex < RowCount; RowIndex++)
            {
                //create row
                if (!String.IsNullOrEmpty(Me.RowElement))
                {
                    HtmlGenericControl grow = new HtmlGenericControl(Me.RowElement);

                    if (!String.IsNullOrEmpty(Me.RowCssClass))
                        grow.Attributes.Add("class", Me.RowCssClass);

                    row = grow;
                }
                else
                {
                    row = new PlaceHolder();
                }
                CriteriaPlaceholder.Controls.Add(row);

                int ColumnIndex = RowIndex * criteria.ControlsPerRow;
                int rowEnd = ColumnIndex + (criteria.ControlsPerRow-1);
                if(rowEnd > NoOfCriteriaFields-1)
                {
                    rowEnd = NoOfCriteriaFields-1;
                }

                Control column = null;
                while (ColumnIndex <= rowEnd)
                {

                    Widget control = Me.Widgets[ColumnIndex];

                    column = null;

                    if (!String.IsNullOrEmpty(Me.ColumnElement))
                    {
                        HtmlGenericControl gcolumn = new HtmlGenericControl(Me.ColumnElement);

                        if (!String.IsNullOrEmpty(Me.ColumnCssClass))
                            gcolumn.Attributes.Add("class", Me.ColumnCssClass);

                        column = gcolumn;

                        
                    }
                    else
                    {
                        column = new PlaceHolder();
                    }

                    row.Controls.Add(column);
                    AddControl(page.Screen, Section, column, control);

                    ColumnIndex++;
                }
            }
        }
    }
}
