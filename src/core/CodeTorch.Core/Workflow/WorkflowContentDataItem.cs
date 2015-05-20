using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CodeTorch.Core
{
    [Serializable]
    public class WorkflowContentDataItem
    {
        [Category("Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string ContentWorkflowCodeParameter { get; set; }
        [Category("Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string ContentEntityIDParameter { get; set; }
        [Category("Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string ContentFromWorkflowStepCodeParameter { get; set; }
        [Category("Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string ContentToWorkflowStepCodeParameter { get; set; }
        [Category("Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string ContentCommentParameter { get; set; }
        [Category("Parameters")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandParameterTypeConverter,CodeTorch.Core.Design")]
        public string ContentUsernameParameter { get; set; }
        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandColumnTypeConverter,CodeTorch.Core.Design")]
        public string ContentToAddressField { get; set; }
        [Category("Data")]
        public string DataItem { get; set; }
        [Category("Data")]
        [TypeConverter("CodeTorch.Core.Design.DataCommandTypeConverter,CodeTorch.Core.Design")]
        public string DataCommand { get; set; }

        public override string ToString()
        {
            return DataItem;
        }
    }
}
