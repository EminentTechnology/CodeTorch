using CodeTorch.Core;
using CodeTorch.Core.Services;
using CodeTorch.Web.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace CodeTorch.Web.FieldTemplates
{
    public class TreeView : BaseFieldTemplate
    {
        protected Telerik.Web.UI.RadTreeView ctrl;

        protected override void CreateChildControls()
        {
            ctrl = new Telerik.Web.UI.RadTreeView();
            ctrl.ID = "ctrl";
            Controls.Add(ctrl);
        }

        DataCommandService dataCommand = DataCommandService.GetInstance();
        PageDB pageDB = new PageDB();

        TreeViewControl _Me = null;
        public TreeViewControl Me
        {
            get
            {
                if (_Me == null)
                {
                    _Me = (TreeViewControl)this.BaseControl;
                }
                return _Me;
            }
        }

        

        public override void InitControl(object sender, EventArgs e)
        {
            base.InitControl(sender, e);

            try
            {
                ctrl.CheckChildNodes = Me.CheckChildNodes;


                ctrl.CheckBoxes = Me.DisplayCheckBoxes;
                

                ctrl.EnableDragAndDrop = false;
                ctrl.EnableDragAndDropBetweenNodes = false;
                ctrl.RenderMode = Telerik.Web.UI.RenderMode.Auto;
                //ctrl.SingleExpandPath
                //ctrl.ExpandAllNodes
                

                ctrl.CausesValidation = Me.CausesValidation;


                if (!String.IsNullOrEmpty(Me.Width))
                {
                    ctrl.Width = new Unit(Me.Width);
                }

                if (!String.IsNullOrEmpty(Me.Height))
                {
                    ctrl.Height = new Unit(Me.Height);
                }

                ctrl.CssClass = "form-control";
                if (!String.IsNullOrEmpty(Me.CssClass))
                {
                    ctrl.CssClass += " " + Me.CssClass;
                }

                if (!String.IsNullOrEmpty(Me.SkinID))
                {
                    ctrl.SkinID = Me.SkinID;
                }

                if (!String.IsNullOrEmpty(Me.Skin))
                {
                    ctrl.Skin = Me.Skin;
                }

                ctrl.EnableDragAndDrop = Me.EnableDragAndDrop;
                ctrl.EnableDragAndDropBetweenNodes = Me.EnableDragAndDropBetweenNodes;

                ctrl.OnClientMouseOver = Me.OnClientMouseOver;
                ctrl.OnClientMouseOut = Me.OnClientMouseOut;
                

                ctrl.NodeDataBound += ctrl_NodeDataBound;

            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID, Me.Type, this.ID);

                //this.ctrl.Items.Insert(0, new RadListBoxItem(ErrorMessages, String.Empty));
                this.ctrl.BackColor = Color.Red;

            }


        }

        void ctrl_NodeDataBound(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {
            DataRowView row = (DataRowView)e.Node.DataItem;

            if (!String.IsNullOrEmpty(Me.DataCheckedField))
            {
                e.Node.Checked = Convert.ToBoolean(row[Me.DataCheckedField]);
            }

            if (!String.IsNullOrEmpty(Me.DataCheckableField))
            {
                e.Node.Checkable = Convert.ToBoolean(row[Me.DataCheckableField]);
            }

            if (!String.IsNullOrEmpty(Me.DataSelectedField))
            {
                e.Node.Selected = Convert.ToBoolean(row[Me.DataSelectedField]);
            }

            if (!String.IsNullOrEmpty(Me.DataImageUrlField))
            {
                e.Node.ImageUrl = row[Me.DataImageUrlField].ToString();
            }

            if (!String.IsNullOrEmpty(Me.DataCssClassField))
            {
                e.Node.CssClass = row[Me.DataCssClassField].ToString();
            }

            if (!String.IsNullOrEmpty(Me.DataEnabledField))
            {
                e.Node.Enabled = Convert.ToBoolean(row[Me.DataEnabledField]);
            }
        }

        public override void LoadControl(object sender, EventArgs e)
        {

            try
            {
                if (!this.Page.IsPostBack)
                {
                    FillList();

                
                }

            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID, Me.Type, this.ID);

                //this.ctrl.Items.Insert(0, new RadListBoxItem(ErrorMessages, String.Empty));
                this.ctrl.BackColor = Color.Red;

            }
        }

        public override void Refresh()
        {

            try
            {
                FillList();

            }
            catch (Exception ex)
            {
                string ErrorMessageFormat = "ERROR - {0} - Control {1} ({2} - {3})";
                string ErrorMessages = String.Format(ErrorMessageFormat, ex.Message, this.ControlID, Me.Type, this.ID);

                //this.ctrl.Items.Insert(0, new RadListBoxItem(ErrorMessages, String.Empty));
                this.ctrl.BackColor = Color.Red;

            }
        }

        private void FillList()
        {
            ctrl.DataFieldID = Me.DataFieldID;
            ctrl.DataFieldParentID = Me.DataFieldParentID;
            //ctrl.DataModelID = "";
            ctrl.DataNavigateUrlField = Me.DataNavigateUrlField;
            ctrl.DataTextField = Me.DataTextField;
            //ctrl.DataTextFormatString = ""; ;
            ctrl.DataValueField = Me.DataValueField;

            

            this.ctrl.DataSource = GetData();
            this.ctrl.DataBind();



            //to support criteria list edit - add mode
            //ctrl.DataSource = null;

        }

        private DataTable GetData()
        {


            List<ScreenDataCommandParameter> parameters = pageDB.GetPopulatedCommandParameters(Me.SelectDataCommand, ((CodeTorch.Web.Templates.BasePage)this.Page));

            PopulateScreenParameterWithControlValue(parameters);
            

            return dataCommand.GetDataForDataCommand(Me.SelectDataCommand, parameters);



        }

        private void PopulateScreenParameterWithControlValue(List<ScreenDataCommandParameter> parameters)
        {
            List<ScreenDataCommandParameter> ctrlValueParameters = parameters.Where(p =>
                       (
                           (p.InputType == ScreenInputType.Special) &&
                           (p.InputKey.ToLower().Trim() == "controlvalue")
                       )
                   ).ToList<ScreenDataCommandParameter>();

            foreach (ScreenDataCommandParameter p in ctrlValueParameters)
            {
                p.Value = this.Value;
            }
        }
    }
}
