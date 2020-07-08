using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel;


namespace CodeTorch.Core
{
    [Serializable]
    public class WorkflowNextStep
    {
        string _Code = null;
        List<BaseSecurityGroup> _SecurityGroups = new List<BaseSecurityGroup>();

        [TypeConverter("CodeTorch.Core.Design.WorkflowStepTypeConverter,CodeTorch.Core.Design")]
        public string Name { get; set; }

        public bool RequireComment { get; set; }

        [XmlArray("SecurityGroups")]
        [XmlArrayItem(ElementName = "EveryoneSecurityGroup", Type = typeof(EveryoneSecurityGroup))]
        [XmlArrayItem(ElementName = "RoleSecurityGroup", Type = typeof(RoleSecurityGroup))]
        [XmlArrayItem(ElementName = "UserSecurityGroup", Type = typeof(UserSecurityGroup))]
        [XmlArrayItem(ElementName = "DynamicSecurityGroup", Type = typeof(WorkflowDynamicSecurityGroup))]
#if NETFRAMEWORK
        [Editor("CodeTorch.Core.Design.SecurityGroupCollectionEditor,CodeTorch.Core.Design", typeof(UITypeEditor))]
#endif
        public List<BaseSecurityGroup> SecurityGroups
        {
            get { return _SecurityGroups; }
            set { _SecurityGroups = value; }
        }

        [Browsable(false)]
        [XmlIgnore()]
        public WorkflowStep Parent { get; set; }

        [Browsable(false)]
        [XmlIgnore()]
        public string Code
        {
            get 
            {
                if (_Code == null)
                {
                    if (Parent != null)
                    {
                        WorkflowStep step = Parent.Workflow.Steps
                                .Where(s =>
                                    (
                                        (s.Name.ToLower() == Name.ToLower())
                                    )
                                )
                                .SingleOrDefault();

                        if (step != null)
                        {
                            _Code = step.Code;
                        }
                    }
                }

                return _Code;
            }
        }
    }
}
