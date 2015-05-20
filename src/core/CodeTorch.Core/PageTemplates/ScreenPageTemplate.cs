using CodeTorch.Core.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace CodeTorch.Core
{
    public enum ScreenPageTemplateMode 
    { 
        Static,
        Dynamic
    }

     [Serializable]
     [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ScreenPageTemplate
    {
         
         public ScreenPageTemplateMode Mode { get; set; }

         [TypeConverter("CodeTorch.Core.Design.PageTemplateTypeConverter,CodeTorch.Core.Design")]
         public string Name{get;set;}

         [TypeConverter("CodeTorch.Core.Design.DataCommandTypeConverter,CodeTorch.Core.Design")]
         public string DataCommand { get; set; }

         [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
         public string DataField { get; set; }

         public override string ToString()
         {
             string retVal = base.ToString();

             if (Mode == ScreenPageTemplateMode.Static)
             {
                 if (!String.IsNullOrEmpty(Name))
                 {
                     retVal = String.Format("Static - {0}", Name);
                 }
                 else
                 {
                     retVal = "Static - No template selected";
                 }
             }
             else
             {
                 if (!String.IsNullOrEmpty(DataCommand))
                 {
                     retVal = String.Format("Dynamic - {0}", DataCommand);
                 }
                 else
                 {
                     retVal = "Dynamic - No template selected";
                 }
             }

             return retVal;
         }
    }
}
