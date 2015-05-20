using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;

namespace CodeTorch.Core.Design
{
    public class SecurityGroupCollectionEditor : CollectionEditor
    {
        public SecurityGroupCollectionEditor(Type type)
            : base(type)
        {
        }

        protected override bool CanSelectMultipleInstances()
        {
            return false;
        }

        protected override Type[] CreateNewItemTypes()
        {
            List<Type> types = new List<Type>();

            types.Add(typeof(RoleSecurityGroup));
            types.Add(typeof(UserSecurityGroup));
            types.Add(typeof(EveryoneSecurityGroup));

            if (Context.Instance is WorkflowNextStep)
            {
                types.Add(typeof(WorkflowDynamicSecurityGroup));
            }

            return types.ToArray<Type>();
        }

        protected override string GetDisplayText(object value)
        {
            BaseSecurityGroup group = (BaseSecurityGroup)value;

            string retVal = String.Empty;

            switch (group.Type.ToLower())
            { 
                case "role":
                    retVal = ((RoleSecurityGroup)group).Role;
                    break;
                
                case "user":
                    retVal = ((UserSecurityGroup)group).User;
                    break;
                case "dynamic":
                    retVal = String.Format("Dynamic - {0}", ((WorkflowDynamicSecurityGroup)group).DataCommand);
                    break;
                case "everyone":
                    retVal = "Everyone";
                    break;
            }

            return base.GetDisplayText(retVal);
        }

        protected override object CreateInstance(Type itemType)
        {

            object retVal = base.CreateInstance(itemType);
            ((BaseSecurityGroup)retVal).Parent = Context.Instance;

            return retVal;
        }


    }
}
