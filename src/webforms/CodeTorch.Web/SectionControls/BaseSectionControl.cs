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
using CodeTorch.Web.FieldTemplates;
using System.Web.Security;
using Telerik.Web.UI;
using CodeTorch.Core;
using CodeTorch.Web.Controls;
using CodeTorch.Web.Templates;

namespace CodeTorch.Web.SectionControls
{
    public class BaseSectionControl : Control, INamingContainer
    {
        App app;
        public Screen Screen { get; set; }
        public Section Section { get; set; }
        public string ResourceKeyPrefix = "";

        protected PlaceHolder SectionPlaceHolder;
        protected PlaceHolder ContentPlaceHolder;

        protected override void CreateChildControls()
        {
            SectionPlaceHolder = new PlaceHolder();
            SectionPlaceHolder.ID = "SectionPlaceHolder";
            Controls.Add(SectionPlaceHolder);
        }

        public virtual void RenderControl()
        {
            EnsureChildControls();

            ContentPlaceHolder = new PlaceHolder();

            HtmlGenericControl intro = null;
            if (!String.IsNullOrEmpty(Section.IntroText))
            {
                intro = new HtmlGenericControl("h5");
                intro.InnerHtml = Section.IntroText;
            }

            switch (Section.ContainerMode)
            {
                case SectionContainer.Plain:
                    //renders plain div
                    string divClass = String.Format("section", Section.ID);
                    if (!String.IsNullOrEmpty(Section.CssClass))
                    {
                        divClass += " " + Section.CssClass;
                    }
                    HtmlGenericControl plain_div = new HtmlGenericControl("div");
                    plain_div.Attributes.Add("class", divClass);

                    if (intro != null)
                    {
                        plain_div.Controls.Add(intro);
                    }

                    plain_div.Controls.Add(ContentPlaceHolder);

                    SectionPlaceHolder.Controls.Add(plain_div);
                    break;
                case SectionContainer.Panel:
                    //renders panel in boostrap format
                    if(this.Page == null)
                        this.Page =  HttpContext.Current.Handler as BasePage;

                    var sectionTemplatePath = String.IsNullOrEmpty(Section.ContainerTemplatePath) ? "~/templates/sections/sectionpaneltemplate.ascx" : Section.ContainerTemplatePath;
                    var panel = this.Page.LoadControl(sectionTemplatePath) as SectionPanelTemplate;
                    if (panel != null)
                    {
                        panel.IntroText = GetGlobalResourceString("IntroText", Section.IntroText);
                        panel.HeadingTitle = GetGlobalResourceString("Name", Section.Name);
                        panel.SectionCssClass = GetGlobalResourceString("Name", Section.CssClass);

                        panel.AddBody(this.ContentPlaceHolder);
                        panel.Update();
                    }
                    else
                    {
                        throw new Exception($"Template {sectionTemplatePath} probably does not inherit from SectionPanelTemplate");
                    }


                    SectionPlaceHolder.Controls.Add(panel);
                    
                    break;
                default:
                    if (intro != null)
                    {
                        SectionPlaceHolder.Controls.Add(intro);
                    }
                    SectionPlaceHolder.Controls.Add(ContentPlaceHolder);
                    break;
            }
        }

        public virtual void PopulateControl()
        {

        }

        public virtual void BindDataSource()
        {

        }

        public BaseSectionControl()
		{
            

            app = CodeTorch.Core.Configuration.GetInstance().App;
            
		}

        

    
       

        public string GetGlobalResourceString(string ResourceSet, string ResourceKey, string DefaultValue)
        {
            string retVal = DefaultValue;

            string ActualResourceKey = "";

            ActualResourceKey = GetResourceKeyName(ResourceKey);



            if (app.EnableLocalization)
            {
                object resourceValue = HttpContext.GetGlobalResourceObject(ResourceSet, ActualResourceKey);
                //object resourceValue = HttpContext.GetLocalResourceObject("", ResourceKey);

                if (resourceValue == null)
                {
                    retVal = DefaultValue;
                }
                else
                {
                    retVal = resourceValue.ToString();
                }
            }


            return retVal;
        }

        public string GetResourceKeyName(string ResourceKey)
        {
            string retVal = null;
            if (String.IsNullOrEmpty(this.ResourceKeyPrefix))
            {
                retVal = String.Format("{0}.{1}", Section.Name, ResourceKey);
            }
            else
            {
                retVal = String.Format("{0}.{1}.{2}", this.ResourceKeyPrefix, Section.Name, ResourceKey);
            }
            return retVal;
        }

        public string GetGlobalResourceString(string ResourceKey, string DefaultValue)
        {
            string retVal = null;
            string ResourceSet = Common.StripVirtualPath(HttpContext.Current.Request.Url.AbsolutePath);


            retVal = GetGlobalResourceString(ResourceSet, ResourceKey, DefaultValue);

            return retVal;
        }

        public string GetLocalResourceString(string ResourceKey, string DefaultValue)
        {
            string retVal = DefaultValue;

            if (app.EnableLocalization)
            {
                object resourceValue = HttpContext.GetLocalResourceObject(HttpContext.Current.Request.Path,ResourceKey);
                //object resourceValue = HttpContext.GetLocalResourceObject("", ResourceKey);

                if (resourceValue == null)
                {
                    retVal = DefaultValue;
                }
                else
                {
                    retVal = resourceValue.ToString();
                }
            }

            return retVal;
        }

        public virtual void Update(string SectionID, string MemberType, string Property, string Value)
        {
            if ((MemberType.ToLower() == "property") && (Property.ToLower() == "visible"))
            {
                this.Visible = Convert.ToBoolean(Value);
            }

            if ((MemberType.ToLower() == "property") && (Property.ToLower() == "name"))
            {
                SetTitle(SectionID, Value);
            }


        }

        public virtual void SetTitle(string SectionID, string Value)
        {
            

        }


        public void AddControl(Screen Screen, Section Section, Control parent, Widget control)
        {
            try
            {
                bool IsControlVisible = true;
                bool IsControlEditable = true;
                ControlType controlType = null;
                ControlType labelControlType = null;
                BaseFieldTemplate ctrl = null;
                BaseFieldTemplate readOnlyCtrl = null;
                System.Web.UI.WebControls.BaseValidator val = null;


                HtmlGenericControl group = null;

                HtmlGenericControl labelContainer = null;
                System.Web.UI.WebControls.Label label = null;

                HtmlGenericControl controlContainer = null;
                PlaceHolder groupContainer = null;

                Control help = null;

                IsControlVisible = DetermineControlVisibility(control, IsControlVisible);
                IsControlEditable = DetermineIfControlIsEditable(control, IsControlVisible, IsControlEditable);

                //setup overall container
                groupContainer = new PlaceHolder();
                groupContainer.ID = String.Format("{0}_GroupContainer", control.Name);
                groupContainer.Visible = IsControlVisible;

                //setup control group if needed
                if (!String.IsNullOrEmpty(control.ControlGroupElement))
                {
                    group = new HtmlGenericControl(control.ControlGroupElement);
                    group.ID = String.Format("{0}_FormGroup", control.Name);

                    if (!String.IsNullOrEmpty(control.ControlGroupCssClass))
                    {
                        group.Attributes.Add("class", control.ControlGroupCssClass);
                    }
                }

                //setup label container if needed
                if (!String.IsNullOrEmpty(control.LabelContainerElement))
                {
                    labelContainer = new HtmlGenericControl(control.LabelContainerElement);
                    labelContainer.ID = String.Format("{0}_LabelContainer", control.Name);

                    if (!String.IsNullOrEmpty(control.LabelContainerCssClass))
                    {
                        labelContainer.Attributes.Add("class", control.LabelContainerCssClass);
                    }
                }

                //setup control container if needed
                if (!String.IsNullOrEmpty(control.ControlContainerElement))
                {
                    controlContainer = new HtmlGenericControl(control.ControlContainerElement);
                    controlContainer.ID = String.Format("{0}_ControlContainer", control.Name);

                    if (!String.IsNullOrEmpty(control.ControlContainerCssClass))
                    {
                        controlContainer.Attributes.Add("class", control.ControlContainerCssClass);
                    }
                }

                //setup actual control
                controlType = ControlType.GetControlType(control);
                if (controlType == null)
                {
                    throw new ApplicationException(String.Format("Control of type {0} could not be found in configuration", control.Type));
                }

                if (!IsControlEditable)
                {
                    labelControlType = ControlType.GetControlType("Label");
                }

                if (controlType != null)
                {
                    //main edit control
                    ctrl = LoadBaseFieldTemplateFromControlType(controlType);
                    ctrl.ID = control.Name;
                    ctrl.ResourceKeyPrefix = String.Format("Screen.Sections.{0}.Control", Section.Name);
                    BaseFieldTemplate renderControl = null;

                    //alternate edit control in read only mode
                    if (!IsControlEditable)
                    {

                        readOnlyCtrl = LoadBaseFieldTemplateFromControlType(labelControlType);
                        readOnlyCtrl.ID = String.Format("{0}_ReadOnly_Label", control.Name);
                    }

                    if (IsControlEditable)
                    {
                        renderControl = ctrl;
                    }
                    else
                    {
                        renderControl = readOnlyCtrl;
                    }

                    //label
                    label = new System.Web.UI.WebControls.Label();
                    label.ID = String.Format("{0}_Label", control.Name);
                    if (!String.IsNullOrEmpty(control.LabelCssClass))
                    {
                        string labelCss = String.Format("{0}", control.LabelCssClass);

                        if (control.IsRequired)
                        {
                            labelCss += " required required-label";
                        }

                        label.CssClass = labelCss;
                    }
                    if (!String.IsNullOrEmpty(control.Label))
                    {
                        string labelText = GetGlobalResourceString(String.Format("Control.{0}.Label", control.Name), control.Label);
                        labelText += ":";

                        label.Controls.Add(new LiteralControl(labelText));
                    }

                    //help text
                    if (!String.IsNullOrEmpty(control.HelpText))
                    {
                        string helpText = GetGlobalResourceString(String.Format("Control.{0}.HelpText", control.Name), control.HelpText);

                        if (String.IsNullOrEmpty(control.HelpTextElement))
                        {
                            help = new LiteralControl(control.HelpText);
                        }
                        else
                        {
                            HtmlGenericControl helpControl = new HtmlGenericControl(control.HelpTextElement);
                            helpControl.InnerHtml = control.HelpText;

                            if (!String.IsNullOrEmpty(control.HelpTextCssClass))
                            {
                                helpControl.Attributes.Add("class", control.HelpTextCssClass);
                            }

                            help = helpControl;
                        }


                    }


                    //validation controls
                    List<System.Web.UI.WebControls.BaseValidator> validators = new List<System.Web.UI.WebControls.BaseValidator>();
                    if (IsControlEditable)
                    {
                        //custom validators
                        foreach (CodeTorch.Core.BaseValidator validator in control.Validators)
                        {
                            System.Web.UI.WebControls.BaseValidator validatorControl = GetValidator(control, validator);
                            validators.Add(validatorControl);
                        }
                    }

                    //assign base control, section and screen references
                    ctrl.Widget = control;
                    if (!IsControlEditable)
                    {

                        readOnlyCtrl.Widget = CreateLabelControlFromWidget(control);
                        readOnlyCtrl.Value = ctrl.Value;
                        readOnlyCtrl.SetDisplayText(ctrl.DisplayText);
                    }
                    ctrl.Section = Section;
                    ctrl.Screen = Screen;

                    //now that all controls have been defined we now need to render  html based on rendering mode
                    if (group != null)
                    {
                        if (control.RenderLabel)
                        {
                            if (control.LabelWrapsControl)
                            {
                                AssignControl(group, labelContainer, label);
                                AssignControl(label, controlContainer, renderControl, help);
                            }
                            else
                            {
                                label.AssociatedControlID = renderControl.ClientID;

                                if (control.LabelRendersBeforeControl)
                                {
                                    AssignControl(group, labelContainer, label);
                                    AssignControl(group, controlContainer, renderControl, help);
                                }
                                else
                                {
                                    AssignControl(group, controlContainer, renderControl, help);
                                    AssignControl(group, labelContainer, label);
                                }
                            }
                        }
                        else
                        {
                            AssignControl(group, controlContainer, renderControl, help);
                        }

                        

                    }
                    else
                    {
                        Control g = groupContainer;// (parent != null) ? (Control)parent : (Control)this.ContentPlaceHolder;

                        if (control.RenderLabel)
                        {
                            if (control.LabelWrapsControl)
                            {
                                AssignControl(g, labelContainer, label);
                                AssignControl(label, controlContainer, renderControl, help);
                            }
                            else
                            {
                                label.AssociatedControlID = renderControl.ClientID;

                                if (control.LabelRendersBeforeControl)
                                {
                                    AssignControl(g, labelContainer, label);
                                    AssignControl(g, controlContainer, renderControl, help);
                                }
                                else
                                {
                                    AssignControl(g, controlContainer, renderControl, help);
                                    AssignControl(g, labelContainer, label);
                                }
                            }
                        }
                        else
                        {
                            AssignControl(g, controlContainer, renderControl, help);
                        }

                        
                    }


                    if (parent != null)
                    {
                        if (group != null)
                        {
                            groupContainer.Controls.Add(group);
                        }
                        parent.Controls.Add(groupContainer);
                    }
                    else
                    {
                        if (group != null)
                        {
                            groupContainer.Controls.Add(group);
                        }
                        this.ContentPlaceHolder.Controls.Add(groupContainer);
                    }


                    string requiredErrorMessage = String.Format("{0} is required.", control.Label);
                    string requiredErrorMessageResourceValue = GetGlobalResourceString(String.Format("Control.{0}.IsRequired.ErrorMessage", control.Name), requiredErrorMessage);

                    //get container for validation
                    Control valContainer = ((Control)controlContainer ??
                                    (
                                        (Control)group ??
                                        (
                                            ((Control)parent ?? (Control)this.ContentPlaceHolder)
                                        )
                                    )
                                );




                    if (!String.IsNullOrEmpty(control.Name))
                    {
                        if (ctrl.SupportsValidation())
                        {
                            val = ctrl.GetRequiredValidator(control, IsControlEditable, requiredErrorMessageResourceValue);
                            valContainer.Controls.Add(val);

                            if (IsControlEditable)
                            {
                                //custom validators
                                foreach (System.Web.UI.WebControls.BaseValidator validator in validators)
                                {
                                    valContainer.Controls.Add(validator);
                                }
                            }

                        }
                    }


                }
                else
                {
                    string ErrorMessageFormat = "<span style='color:red'>ERROR - Could not load control {0} - {1} - control type returned null object</span>";
                    string ErrorMessages = String.Format(ErrorMessageFormat, control.Name, control.Type);


                    this.ContentPlaceHolder.Controls.Add(new LiteralControl(ErrorMessages));


                }
            }
            catch (Exception ex)
            {
                this.ContentPlaceHolder.Controls.Add(new LiteralControl(ex.Message));
            }


        }

        private BaseFieldTemplate LoadBaseFieldTemplateFromControlType(ControlType controlType)
        {
            BaseFieldTemplate ctrl = null;

            if (!String.IsNullOrEmpty(controlType.Assembly) && !String.IsNullOrEmpty(controlType.Class))
            {
                ctrl = (BaseFieldTemplate)Activator.CreateInstance(controlType.Assembly, controlType.Class).Unwrap();
            }
            
            return ctrl;
        }

        private static bool DetermineIfControlIsEditable(Widget control, bool IsControlVisible, bool IsControlEditable)
        {
            if ((IsControlVisible) && (control.EditPermission.CheckPermission))
            {
                IsControlEditable = Common.HasPermission(control.EditPermission.Name);
            }
            return IsControlEditable;
        }

        private static bool DetermineControlVisibility(Widget control, bool IsControlVisible)
        {
            //Set initial control visibility
            IsControlVisible = control.Visible;

            //check to see if control being rendered requires a permission to be seen
            if (control.VisiblePermission.CheckPermission)
            {
                IsControlVisible = Common.HasPermission(control.VisiblePermission.Name);
            }
            return IsControlVisible;
        }



        private LabelControl CreateLabelControlFromWidget(Widget control)
        {
            LabelControl retVal = new LabelControl();

            retVal.Name = String.Format("{0}_Label", control.Name);
            retVal.DataField = control.DataField;
            retVal.HelpText = control.HelpText;
            retVal.Width = control.Width;

            return retVal;
        }

        private System.Web.UI.WebControls.BaseValidator GetValidator(Widget control, Core.BaseValidator validator)
        {
            System.Web.UI.WebControls.BaseValidator retVal = null;
            //string validationErrorMessage = null;
            //string validationResourceKeyPrefix = String.Format("", this.ResourceKeyPrefix, validator.Name)
            //validationErrorMessage = GetGlobalResourceString("", compareConfig.ErrorMessage);

            switch (validator.GetType().Name.ToLower())
            {
                case "comparevalidator":
                    Core.CompareValidator compareConfig = (Core.CompareValidator)validator;
                    System.Web.UI.WebControls.CompareValidator compareActual = new System.Web.UI.WebControls.CompareValidator();

                    compareActual.ID = compareConfig.Name;
                    compareActual.Display = ValidatorDisplay.None;


                    compareActual.ErrorMessage = compareConfig.ErrorMessage;
                    compareActual.Text = "*";
                    compareActual.ControlToValidate = control.Name;

                    //validator specific
                    compareActual.Type = (System.Web.UI.WebControls.ValidationDataType)Enum.Parse(typeof(System.Web.UI.WebControls.ValidationDataType), compareConfig.Type.ToString()); 
                    compareActual.ControlToCompare = compareConfig.ControlToCompare;
                    compareActual.Operator = (System.Web.UI.WebControls.ValidationCompareOperator)Enum.Parse(typeof(System.Web.UI.WebControls.ValidationCompareOperator), compareConfig.Operator.ToString());
                    compareActual.ValueToCompare = compareConfig.ValueToCompare;

                    retVal = (System.Web.UI.WebControls.BaseValidator)compareActual;

                    break;
                case "datacommandvalidator":
                    Core.DataCommandValidator dataConfig = (Core.DataCommandValidator)validator;
                    CustomDataCommandValidator dataActual = new CustomDataCommandValidator();

                    dataActual.ID = dataConfig.Name;
                    dataActual.Display = ValidatorDisplay.None;

                    dataActual.ErrorMessage = dataConfig.ErrorMessage;
                    dataActual.Text = "*";
                    dataActual.ControlToValidate = control.Name;

                    //validator specific
                    dataActual.DataCommand = dataConfig.DataCommand;
                    dataActual.ValidationField = dataConfig.ValidationField;
                    dataActual.UseValueParameter = dataConfig.UseValueParameter;
                    dataActual.ValueParameter = dataConfig.ValueParameter;
                    dataActual.UseErrorMessageField = dataConfig.UseErrorMessageField;
                    dataActual.ErrorMessageField = dataConfig.ErrorMessageField;

                    dataActual.ServerValidate += new ServerValidateEventHandler(DataCommandValidator_ServerValidate);

                    retVal = (System.Web.UI.WebControls.BaseValidator)dataActual;
                    break;
                case "regularexpressionvalidator":
                    Core.RegularExpressionValidator regexpConfig = (Core.RegularExpressionValidator)validator;
                    System.Web.UI.WebControls.RegularExpressionValidator regexpActual = new System.Web.UI.WebControls.RegularExpressionValidator();

                    regexpActual.ID = regexpConfig.Name;
                    regexpActual.Display = ValidatorDisplay.None;

                    regexpActual.ErrorMessage = regexpConfig.ErrorMessage;
                    regexpActual.Text = "*";
                    regexpActual.ControlToValidate = control.Name;

                    //validator specific
                    regexpActual.ValidationExpression = regexpConfig.ValidationExpression;

                    retVal = (System.Web.UI.WebControls.BaseValidator)regexpActual;
                    break;
                case "rangevalidator":
                    Core.RangeValidator rangeConfig = (Core.RangeValidator)validator;
                    System.Web.UI.WebControls.RangeValidator rangeActual = new System.Web.UI.WebControls.RangeValidator();

                    rangeActual.ID = rangeConfig.Name;
                    rangeActual.Display = ValidatorDisplay.None;

                    rangeActual.ErrorMessage = rangeConfig.ErrorMessage;
                    rangeActual.Text = "*";
                    rangeActual.ControlToValidate = control.Name;

                    //validator specific
                    rangeActual.Type = (System.Web.UI.WebControls.ValidationDataType)Enum.Parse(typeof(System.Web.UI.WebControls.ValidationDataType), rangeConfig.Type.ToString());
                    rangeActual.MinimumValue = rangeConfig.MinimumValue;
                    rangeActual.MaximumValue = rangeConfig.MaximumValue;

                    retVal = (System.Web.UI.WebControls.BaseValidator)rangeActual;
                    break;
            }

            return retVal;
        }

        private static void AssignControl(Control parent, Control controlContainer, Control control)
        { 
            AssignControl( parent,  controlContainer,  control,  null);
        }

        private static void AssignControl(Control parent, Control controlContainer, Control control, Control control2)
        {
            //assign control
            if (control != null)
            {
                if (controlContainer != null)
                {
                    controlContainer.Controls.Add(control);

                    if (control2 != null)
                    {
                        controlContainer.Controls.Add(control2);
                    }

                    parent.Controls.Add(controlContainer);
                }
                else
                {

                    parent.Controls.Add(control);

                    if (control2 != null)
                    {
                        parent.Controls.Add(control2);
                    }
                }
            }
        }

        void DataCommandValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            bool IsValid = false;
            DataCommandService dataCommand = DataCommandService.GetInstance();
            PageDB pageDB = new PageDB();

            try
            {
                CustomDataCommandValidator validator = (CustomDataCommandValidator)source;
                List<ScreenDataCommandParameter> parameters = pageDB.GetPopulatedCommandParameters(validator.DataCommand, ((CodeTorch.Web.Templates.BasePage)this.Page));

                if (validator.UseValueParameter)
                {
                    ScreenDataCommandParameter valueParameter = parameters.Where(p =>
                                        (
                                            (p.Name.ToLower() == validator.ValueParameter.ToLower())
                                        )
                                    )
                                    .SingleOrDefault();

                    if (valueParameter != null)
                    {
                        valueParameter.Value = args.Value;
                    }
                }

                DataTable data = dataCommand.GetDataForDataCommand(validator.DataCommand, parameters);

                if (data.Rows.Count == 1)
                {
                    if (validator.UseErrorMessageField)
                    {
                        if (String.IsNullOrEmpty(validator.ErrorMessageField))
                        {
                            throw new ApplicationException(String.Format("ErrorMessageField for validator {0} is not configured", validator.ID));
                        }
                        else
                        {
                            if (data.Columns.Contains(validator.ErrorMessageField))
                            {
                                validator.ErrorMessage = data.Rows[0][validator.ErrorMessageField].ToString();
                            }
                        }
                    }

                    if (data.Columns.Contains(validator.ValidationField))
                    {
                        IsValid = Convert.ToBoolean(data.Rows[0][validator.ValidationField]);
                    }
                }
            }
            catch (Exception ex)
            {
                args.IsValid = false;
                ((System.Web.UI.WebControls.BaseValidator)source).ErrorMessage = "Validator: " + ex.Message;
            }

            args.IsValid = IsValid;
        }
    }
}
