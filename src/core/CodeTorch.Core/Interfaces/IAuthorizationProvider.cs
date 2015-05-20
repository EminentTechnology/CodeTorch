using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Core.Interfaces
{
    public interface IAuthorizationProvider
    {
        void Initialize(string config);
        bool HasPermission(string permissionName);
        bool IsInRole(string roleName);

        //ideally not here - but needed to support appbuilder permission saving
        void SavePermission (DataConnection connection, Permission permission);
        void AddAllPermissionsToRole(DataConnection connection, string roleName);
    }
}
