using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodeTorch.Core;
using CodeTorch.Web.Templates;

namespace CodeTorch.Web.FieldTemplates
{
    [ValidationPropertyAttribute("ValidationValue")]
    public class BaseFieldTemplate: Control, INamingContainer
    {

        protected App app;
        public string ControlID = "";
        public string _ValidationGroup = "";

        protected DataTable _Settings = null;


        public BaseControl BaseControl { get; set; }
        public Screen Screen { get; set; }
   
        public Section Section { get; set; }
        public object ValueObject { get; set; }
        public object RecordObject { get; set; }

        public string ResourceKeyPrefix = "";
        

        public BaseFieldTemplate()
		{
            this.Init +=new EventHandler(BaseFieldTemplate_Init);
            this.Load += new EventHandler(BaseFieldTemplate_Load);

            app = CodeTorch.Core.Configuration.GetInstance().App;
		}

        void BaseFieldTemplate_Init(object sender, EventArgs e)
        {
            EnsureChildControls();
            InitControl(sender, e);
        }


        

        void BaseFieldTemplate_Load(object sender, EventArgs e)
        {
            EnsureChildControls();
            LoadControl(sender, e);
        }

        public virtual string Value
        {
            get
            {
                return null;
            }
            set
            {
                
            }
        }

        public virtual string ValidationValue
        {
            get
            {
                return Value;
            }
        }

  

        

        public virtual void LoadControl(object sender, EventArgs e)
        {
           
        }

        public virtual void Refresh()
        {

        }

        public virtual void Update(string ControlName, string MemberType, string Property, string Value)
        {
  

            if (MemberType.ToLower() == "property")
            {
                switch (Property.ToLower())
                {
                    case "label":
                        SetLabel(ControlName, Value);
                        break;

                    case "visible":
                        SetVisibility( ControlName, Value);
                        break;
                    case "isrequired":
                        SetIsRequired(ControlName, Value);
                        break;
                }
                
            }

            

            
        }

        public virtual string GetValidationControlIDSuffix()
        {
            return String.Empty;
        }

        

        public virtual void InitControl(object sender, EventArgs e)
        {
            
        }

        public virtual string DisplayText
        {
            get
            {
                return Value;
            }
            set
            {

            }

        }

        public virtual void SetDisplayText(string val)
        {

        }

        public string ValidationGroup
        {
            get
            {
                return _ValidationGroup;
            }
            set
            {
                _ValidationGroup = value;
            }
        }

        public string GetGlobalResourceString(string ResourceSet, string ResourceKey, string DefaultValue)
        {
            string retVal = DefaultValue;

            string ActualResourceKey = "";
            
            if(String.IsNullOrEmpty(this.ResourceKeyPrefix))
            {
                if (BaseControl != null)
                {
                    ActualResourceKey = String.Format("{0}.{1}", BaseControl.Name, ResourceKey);
                }
                else
                {
                    ActualResourceKey = String.Format("{0}",  ResourceKey);
                }
            }
            else
            {
                ActualResourceKey = String.Format("{0}.{1}.{2}", this.ResourceKeyPrefix, BaseControl.Name, ResourceKey);
            }

            

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

        public virtual void SetVisibility(string ControlName, string Value)
        {
            EnsureChildControls();
            //get the controls row and make it visisble
            BasePage p = (BasePage)this.Page;
            Control row = p.FindControlRecursive(String.Format("{0}_GroupContainer", ControlName));
            if (row != null)
            {
                row.Visible = Convert.ToBoolean(Value);
            }

            
            
        }

        public virtual void SetLabel(string ControlName, string Value)
        {
            EnsureChildControls();
            //get the controls row and make it visisble
            BasePage p = (BasePage)this.Page;
            Control label = p.FindControlRecursive(String.Format("{0}_Label", ControlName));
            if (label != null)
            {
                System.Web.UI.WebControls.Label labelCtrl = (System.Web.UI.WebControls.Label)label;
                labelCtrl.Text = Value;
            }

        }

        public virtual void SetIsRequired(string ControlName, string Value)
        {
            EnsureChildControls();
            //get the controls row and make it visisble
            BasePage p = (BasePage)this.Page;
            Control reqIndicator = p.FindControlRecursive(String.Format("{0}_Required_Indicator", ControlName));
            if (reqIndicator != null)
            {
                LiteralControl indicator = (LiteralControl)reqIndicator;
                indicator.Visible = Convert.ToBoolean(Value);
                
            }

            Control reqValidator = p.FindControlRecursive(String.Format("RequiredFieldValidator{0}", ControlName));
            if (reqValidator != null)
            {
                System.Web.UI.WebControls.BaseValidator val = (System.Web.UI.WebControls.BaseValidator)reqValidator;
                if (Convert.ToBoolean(Value))
                {
                    val.Enabled = true;
                    val.ControlToValidate = ControlName;
                }
                else
                {
                    val.Enabled = false;
                }
            }


        }

        public virtual bool SupportsValidation()
        {
            return true;
        }

        public virtual System.Web.UI.WebControls.BaseValidator GetRequiredValidator(BaseControl control, bool IsControlEditable, string requiredErrorMessage)
        {
            EnsureChildControls();
            System.Web.UI.WebControls.BaseValidator val = new RequiredFieldValidator();

            val.ID = "RequiredFieldValidator" + control.Name;
            val.Display = ValidatorDisplay.None;

            val.EnableClientScript = true;
            

            val.ErrorMessage = requiredErrorMessage;
            val.Text = "*";
            val.ControlToValidate = control.Name + this.GetValidationControlIDSuffix();
            val.Enabled = (IsControlEditable && control.IsRequired);
            return val;
        }

       
    }
}
