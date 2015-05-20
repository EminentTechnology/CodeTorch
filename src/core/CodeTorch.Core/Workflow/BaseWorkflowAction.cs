using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Data.Common;

namespace CodeTorch.Core
{
    [Serializable]
    public class BaseWorkflowAction
    {
        [Category("Common")]
        public string Name { get; set; }

        [Category("Common")]
        [ReadOnly(true)]
        public virtual string Type { get; set; }



        [Browsable(false)]
        [XmlIgnore()]
        public WorkflowStep WorkflowStep { get; set; }

        public virtual void Execute(DbTransaction tran, string WorkflowCode, string FromWorkflowStepCode, string ToWorkflowStepCode, string EntityID, string Comment, string UserName)
        { 
            
        }
    }
}
