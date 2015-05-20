using System;
using System.Linq;
using System.ComponentModel;
using System.Xml.Serialization;

namespace CodeTorch.Core
{
    [Serializable]
    public class BaseSecurityGroup
    {
        [ReadOnly(true)]
        public virtual string Type { get; set; }

        [Browsable(false)]
        [XmlIgnore()]
        public object Parent { get; set; }

        public static bool IsUserInSecurityGroup(BaseSecurityGroup group, string UserName)
        {
            bool retVal = false;
            App app = Configuration.GetInstance().App;

            switch (group.Type.ToLower())
            {
                case "everyone":
                    retVal = true;
                    break;

                case "user":
                    UserSecurityGroup userGroup = (UserSecurityGroup)group;
                    if (!String.IsNullOrEmpty(userGroup.User))
                    {
                        if (userGroup.User.ToLower() == UserName)
                        {
                            retVal = true;
                        }
                    }
                    break;

                

                case "role":
                    throw new NotSupportedException();
                    //RoleSecurityGroup roleGroup = (RoleSecurityGroup)group;
                    //RoleDB roleDB = new RoleDB();
                    //DataTable role = roleDB.GetRoleByUserNameAndRoleName(null, app.AuthorizationMode.SupportsMultipleRoles, UserName, roleGroup.Role);
                    //if (role.Rows.Count > 1)
                    //{
                    //    retVal = true;
                    //}
                    break;

            }

            return retVal;
        }
    }
}
