using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    public class DataCommandValidator : BaseValidator
    {
        bool _UseValueParameter = true;
        bool _UseErrorMessageField = true;

        [TypeConverter("CodeTorch.Core.Design.DataCommandTypeConverter,CodeTorch.Core.Design")]
        [Category("Data")]
        public string DataCommand { get; set; }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        [Description("This is the field returned from the datacommand that indicates if validation is valid or not - value should be boolean")]
        public string ValidationField { get; set; }

        [Category("Data")]
        [Description("When true will use value passed in ValueParameter instead of value passed in Screen Datacommand configuration")]
        public bool UseValueParameter
        {
            get { return _UseValueParameter; }
            set { _UseValueParameter = value; }
        }
        
        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        [Description("Use this to pass the value of the control being validated to sepcified parameter of datacommand")]
        public string ValueParameter { get; set; }

        [Category("Data")]
        [Description("When true will overwrite standard error message field with error message returned from data command")]
        public bool UseErrorMessageField
        {
            get { return _UseErrorMessageField; }
            set { _UseErrorMessageField = value; }
        }

        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        [Description("This is the field returned from the datacommand that contains custom error message")]
        public string ErrorMessageField { get; set; }

        

        
        

        
    }
}
