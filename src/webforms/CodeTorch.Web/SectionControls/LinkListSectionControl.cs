using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CodeTorch.Core;

namespace CodeTorch.Web.SectionControls
{
    public class LinkListSectionControl : BaseSectionControl
    {
       
        System.Web.UI.WebControls.PlaceHolder ListHolder;

        LinkListSection _Me = null;
        public LinkListSection Me
        {
            get
            {
                if ((_Me == null) && (this.Section != null) && (this.Section is LinkListSection))
                {
                    _Me = (LinkListSection)this.Section;
                }
                return _Me;
            }
        }

        public override void RenderControl()
        {
            base.RenderControl();


            ListHolder = new PlaceHolder();
            this.ContentPlaceHolder.Controls.Add(ListHolder);

            RenderList();

        }

        public override void PopulateControl()
        {
            base.PopulateControl();
        }

        private void RenderList()
        {
            ListHolder.Controls.Clear();
           
            if (String.IsNullOrEmpty(Me.SelectDataCommand))
            {
                HtmlGenericControl container = null;

                if (!String.IsNullOrEmpty(Me.ContainerElement))
                {
                    container = new HtmlGenericControl(Me.ContainerElement); ;
                } 

                foreach (var link in Me.Items)
                {

                    bool RenderItem = link.Visible;

                    if ((link.Permission != null) && (link.Permission.CheckPermission))
                    {
                        RenderItem = Common.HasPermission(link.Permission.Name);
                    }

                    if (RenderItem)
                    {

                        string ResourcePrefix = String.Format("Screen.Sections.{0}.Items.{1}", this.Section.Name, link.Text);
                        string url = Common.CreateUrlWithQueryStringContext(link.Url, link.Context);

                        

                        System.Web.UI.WebControls.HyperLink a = new System.Web.UI.WebControls.HyperLink();
                        a.AppRelativeTemplateSourceDirectory = GetAppRelativeTemplateSourceDirectory();
                        a.NavigateUrl = url;
                        a.Text = GetGlobalResourceString(
                            String.Format("{0}.Text", ResourcePrefix),
                            link.Text);

                        LiteralControl spacer = new LiteralControl();
                        spacer.Text = " ";

                        HtmlGenericControl item = null;
                        if (!String.IsNullOrEmpty(Me.ItemElement))
                        {
                            item = new HtmlGenericControl(Me.ItemElement);

                        }

                        if (container == null)
                        {
                            if (item == null)
                            {
                                //neither - eg button button button
                                ListHolder.Controls.Add(a);
                                ListHolder.Controls.Add(spacer);
                            }
                            else
                            {
                                //only item exists - eg div button div button div button
                                item.Controls.Add(a);
                                item.Controls.Add(spacer);
                                ListHolder.Controls.Add(item);
                            }
                        }
                        else
                        {
                            if (item == null)
                            {
                                //container exists but not child - eg div button button
                                container.Controls.Add(a);
                                container.Controls.Add(spacer);
                            }
                            else
                            {
                                //bth exists - eg ul li button li button
                                item.Controls.Add(a);
                                item.Controls.Add(spacer);
                                container.Controls.Add(item);
                            }
                        }
                    }
                }

                if (container != null)
                {
                    ListHolder.Controls.Add(container);
                }

            }
            else
            { 
                //render list from db
            }

           
        }

        private static string GetAppRelativeTemplateSourceDirectory()
        {
            string rootPath = HttpContext.Current.Request.ApplicationPath;
            if (!rootPath.EndsWith("/"))
            {
                rootPath += "/";
            }

            Uri requestUri = HttpContext.Current.Request.Url;
            string folderPath = requestUri.AbsolutePath.Remove(0, rootPath.Length);
            string lastSegment = requestUri.Segments[requestUri.Segments.Length - 1];
            folderPath = folderPath.Remove(folderPath.LastIndexOf(lastSegment));
            return "~/" + folderPath;
        }



    }
}
