using CodeTorch.Core;
using CodeTorch.Core.Interfaces;
using CodeTorch.Core.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Security.Authorization
{
    public class DataCommandUserNameAuthorizationProvider : IAuthorizationProvider
    {
        DataCommandService sql = DataCommandService.GetInstance();

        private const string DataCommandUserGetPermissions = "User_GetPermissionsByUserName";
        private const string DataCommandRoleGetByUserAndRoleName = "Role_GetByUserNameAndRoleName";
        private const string DataCommandPermissionSave = "Permission_Save";
        private const string DataCommandRoleAddAllPermissions = "Role_AddAllPermissions";

        private const string ParameterUserName = "@UserName";
        private const string ParameterPermissionName = "@PermissionName";
        private const string ParameterRoleName = "@RoleName";

        
        private const string ParameterCategory = "@PermissionCategory";
        private const string ParameterDescription = "@PermissionDescription";

        private const string ColumnHasPermission = "HasPermission";

        public void Initialize(string config)
        {
            //read any special config needed for specific implementation
        }

        public bool HasPermission(string permissionName)
        {
            bool retVal = false;

            UserIdentityService userIdentity = UserIdentityService.GetInstance();
            string userName = userIdentity.IdentityProvider.GetUserName();

            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter p = null;

            p = new ScreenDataCommandParameter(ParameterUserName, userName);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterPermissionName, permissionName);
            parameters.Add(p);

            //get data from data command
            DataTable dt = sql.GetDataForDataCommand(DataCommandUserGetPermissions, parameters);

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (Convert.ToBoolean(row[ColumnHasPermission]))
                    {
                        retVal = true;
                        break;
                    }
                }
            }

            
            return retVal;
        }

        public bool IsInRole(string roleName)
        {
            bool retVal = false;

            UserIdentityService userIdentity = UserIdentityService.GetInstance();
            string userName = userIdentity.IdentityProvider.GetUserName();

            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter p = null;

            p = new ScreenDataCommandParameter(ParameterUserName, userName);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterRoleName, roleName);
            parameters.Add(p);

            //get data from data command
            DataTable dt = sql.GetDataForDataCommand(DataCommandRoleGetByUserAndRoleName, parameters);

            if (dt.Rows.Count > 0)
            {
                retVal = true;
            }


            return retVal;
        }

        public void SavePermission(DataConnection connection, Permission permission)
        {
            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter p = null;


            p = new ScreenDataCommandParameter(ParameterPermissionName, permission.Name);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterCategory, permission.Category);
            parameters.Add(p);

            p = new ScreenDataCommandParameter(ParameterDescription, permission.Description);
            parameters.Add(p);

            DataCommand command = DataCommand.GetDataCommand(DataCommandPermissionSave);




            if (command == null)
                throw new Exception(String.Format("DataCommand {0} could not be found in configuration", DataCommandPermissionSave));
            //execute command
            sql.ExecuteCommand(null, connection, command, parameters, command.Text);
        }

        public void AddAllPermissionsToRole(DataConnection connection, string roleName)
        {
            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();
            ScreenDataCommandParameter p = null;


            p = new ScreenDataCommandParameter(ParameterRoleName, roleName);
            parameters.Add(p);

            DataCommand command = DataCommand.GetDataCommand(DataCommandRoleAddAllPermissions);




            if (command == null)
                throw new Exception(String.Format("DataCommand {0} could not be found in configuration", DataCommandRoleAddAllPermissions));
            //execute command
            sql.ExecuteCommand(null,  connection, command, parameters, command.Text);

            
        }
    }
}
