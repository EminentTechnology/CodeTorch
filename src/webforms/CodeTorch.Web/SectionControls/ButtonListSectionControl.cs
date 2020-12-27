using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using CodeTorch.Core;
using CodeTorch.Web.Templates;

namespace CodeTorch.Web.SectionControls
{
    public class ButtonListSectionControl : BaseSectionControl
    {


        System.Web.UI.WebControls.PlaceHolder ListHolder;
        BasePage page;

        ButtonListSection _Me = null;
        public ButtonListSection Me
        {
            get
            {
                if ((_Me == null) && (this.Section != null) && (this.Section is ButtonListSection))
                {
                    _Me = (ButtonListSection)this.Section;
                }
                return _Me;
            }
        }



      


        public override void RenderControl()
        {
            page = HttpContext.Current.Handler as BasePage;
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
                
                if(!String.IsNullOrEmpty(Me.ContainerElement))
                {
                    container = new HtmlGenericControl(Me.ContainerElement); ;  
                } 

                foreach (var button in Me.Buttons)
                {

                    RenderButton(container, button);
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


        public void RenderButton(HtmlGenericControl container, ButtonControl button)
        {
            bool RenderItem = button.Visible;

            if ((button.VisiblePermission != null) && (button.VisiblePermission.CheckPermission))
            {
                RenderItem = Common.HasPermission(button.VisiblePermission.Name);
            }

            if (RenderItem)
            {

                string ResourcePrefix = String.Format("Screen.Sections.{0}.Buttons.{1}", this.Section.Name, button.Name);

                System.Web.UI.WebControls.Button b = new System.Web.UI.WebControls.Button();
                b.ID = button.Name;
                b.Text = GetGlobalResourceString(
                    String.Format("{0}.Text", ResourcePrefix),
                    button.Text);
                b.CausesValidation = button.CausesValidation;
                b.OnClientClick = button.OnClientClick;
                b.CommandName = button.CommandName;
                b.CommandArgument = button.CommandArgument;
                b.CssClass = button.CssClass;


                b.Click += button_Click;

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
                        ListHolder.Controls.Add(b);
                        ListHolder.Controls.Add(spacer);
                    }
                    else
                    {
                        //only item exists - eg div button div button div button
                        item.Controls.Add(b);
                        item.Controls.Add(spacer);
                        ListHolder.Controls.Add(item);
                    }
                }
                else
                {
                    if (item == null)
                    {
                        //container exists but not child - eg div button button
                        container.Controls.Add(b);
                        container.Controls.Add(spacer);
                    }
                    else
                    {
                        //bth exists - eg ul li button li button
                        item.Controls.Add(b);
                        item.Controls.Add(spacer);
                        container.Controls.Add(item);
                    }
                }





            }
        }

        void button_Click(object sender, EventArgs e)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


            try
            {
                //find specified button by name

                string buttonID = ((System.Web.UI.WebControls.Button)sender).ID;

                CodeTorch.Core.ButtonControl button = Me.Buttons.Where(x => x.Name?.ToLower() == buttonID?.ToLower()).SingleOrDefault();

                if (button != null)
                {
                    //button was found...so call any commands it may have tied to this action


                    ActionRunner runner = new ActionRunner();
                    Core.Action action = ObjectCopier.Clone<Core.Action>(button.OnClick);


                    if (action != null)
                    {
                        runner.Page = (BasePage)this.Page;
                        runner.Action = action;
                        runner.Execute();
                    }


                }
                else
                {
                    throw new Exception($"Could not find button '{buttonID}' in {Me.Name} Section - unable to invoke click actions. Please check button names are set in configuration.");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                page.DisplayErrorAlert(ex);

            }

        }



    }
}
