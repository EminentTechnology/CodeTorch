using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Drawing.Design;

namespace CodeTorch.Core
{
    [Serializable]
    public class WorkflowStep
    {

        private List<BaseWorkflowAction> _Actions = new List<BaseWorkflowAction>();
        private List<WorkflowNextStep> _PossibleNextSteps = new List<WorkflowNextStep>();
        bool _UpdateEntityWithStatusCode = true;

        
        [Category("Common")]
        public string Code { get; set; }

        [Category("Common")]
        public string Name { get; set; }


        [Category("Common")]
        public bool UpdateEntityWithStatusCode
        {
            get { return _UpdateEntityWithStatusCode; }
            set { _UpdateEntityWithStatusCode = value; }
        }

        [Category("Workflow Actions")]
        [XmlArray("Actions")]
        [XmlArrayItem(ElementName = "DataCommandWorkflowAction", Type = typeof(DataCommandWorkflowAction))]
        [XmlArrayItem(ElementName = "EmailWorkflowAction", Type = typeof(EmailWorkflowAction))]
        [XmlArrayItem(ElementName = "SMSWorkflowAction", Type = typeof(SMSWorkflowAction))]
#if NETFRAMEWORK
        [Editor("CodeTorch.Core.Design.WorkflowActionCollectionEditor,CodeTorch.Core.Design", typeof(UITypeEditor))]
#endif
        public virtual List<BaseWorkflowAction> Actions
        {
            get
            {
                return _Actions;
            }
            set
            {
                _Actions = value;
            }

        }

        [Category("Possible Steps")]
        [XmlArray("PossibleNextSteps")]
        [XmlArrayItem("Step")]
#if NETFRAMEWORK
        [Editor("CodeTorch.Core.Design.WorkflowNextStepCollectionEditor,CodeTorch.Core.Design", typeof(UITypeEditor))]  
#endif
        public List<WorkflowNextStep> PossibleNextSteps
        {
            get
            {
                return _PossibleNextSteps;
            }
            set
            {
                _PossibleNextSteps = value;
            }

        }

        [Browsable(false)]
        [XmlIgnore()]
        public Workflow Workflow { get; set; }
    }
}
