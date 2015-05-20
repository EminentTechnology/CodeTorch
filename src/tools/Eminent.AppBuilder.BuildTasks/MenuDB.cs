using System;
using System.Data;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Eminent.AppBuilder.BuildTasks
{
    public class MenuDB: DataLayerBase
    {
        
        public DataTable GetMenus(Database db, DbTransaction tran)
        {
            
            DataTable retVal;

            string SQL = "SELECT * FROM Menu ORDER BY MenuName";

            //create command and specify stored procedure name
            DbCommand command = GetSqlStringCommand(SQL);

           // execute command
            retVal = ExecuteDataTable(db, tran, command);

            return retVal;
        }


        public int DeleteMenu(Database db, DbTransaction tran, string MenuName)
        {


            int retVal;

            string SQL = "DELETE menuitem FROM Menu m JOIN MenuItem i ON m.MenuID = i.MenuID WHERE m.MenuName = @MenuName";

            //create command and execute sql
            DbCommand command = GetSqlStringCommand(SQL);
            AddInParameter(command, "@MenuName", DbType.String, MenuName);
            retVal = ExecuteNonQuery(db, tran, command);

            //create command and execute sql
            SQL = "DELETE menu WHERE MenuName = @MenuName";
            command = GetSqlStringCommand(SQL);
            AddInParameter(command, "@MenuName", DbType.String, MenuName);
            retVal = ExecuteNonQuery(db, tran, command);

            return retVal;
        }

        public int InsertMenu(Database db, DbTransaction tran, out int MenuID, string MenuName, int Version, bool IsRootMenu)
        {
           
            int retVal = 0;
            MenuID = 0;

            string SQL = @"INSERT INTO Menu 
                        (
	                        MenuName,
	                        IsRootMenu,
	                        Version,
	                        CreatedOn,
	                        CreatedBy,
	                        ModifiedOn,
	                        ModifiedBy,
	                        Status
                        )
                        VALUES
                        (
	                        @MenuName,
	                        @IsRootMenu,
	                        @Version,
	                        GetDate(),
	                        'SYSTEM',
	                        GetDate(),
	                        'SYSTEM',
	                        1
                        )
                    
                    SELECT @MenuID=@@IDENTITY
                ";

            //create command and execute sql
            DbCommand command = GetSqlStringCommand(SQL);

            AddOutParameter(command, "@MenuID", DbType.Int32, MenuID);
            AddInParameter(command, "@MenuName", DbType.String, MenuName);
            AddInParameter(command, "@Version", DbType.String, Version);
            AddInParameter(command, "@IsRootMenu", DbType.String, IsRootMenu);

            retVal = ExecuteNonQuery(db, tran, command);

            MenuID = Convert.ToInt32(GetParameterValue(command, "@MenuID"));

            return retVal;

        }

        public int InsertMenuItem(Database db, DbTransaction tran, out int MenuItemID,
              int? ParentID, int MenuID, int Sequence, string MenuItemName
            , string MenuItemCode, string PermissionName, string Location
            , string Context, string CSSClass, string Target
            )
        {

            int retVal = 0;
            MenuItemID = 0;

            string SQL = @"INSERT INTO MenuItem 
                            (
	                            MenuID,
	                            ParentID,
	                            Sequence,
	                            MenuItemName,
	                            MenuItemCode,
	                            PermissionName,
	                            Location,
	                            Context,
	                            CSSClass,
	                            Target,
	                            Status,
	                            CreatedOn,
	                            CreatedBy,
	                            ModifiedOn,
	                            ModifiedBy
                            )
                            VALUES
                            (
	                            @MenuID,
	                            @ParentID,
	                            @Sequence,
	                            @MenuItemName,
	                            @MenuItemCode,
	                            @PermissionName,
	                            @Location,
	                            @Context,
	                            @CSSClass,
	                            @Target,
	                            1,
	                            GetDate(),
	                            'SYSTEM',
	                            GetDate(),
	                            'SYSTEM'
                            )
                    
                    SELECT @MenuItemID=@@IDENTITY
                ";

            //create command and execute sql
            DbCommand command = GetSqlStringCommand(SQL);

            AddOutParameter(command, "@MenuItemID", DbType.Int32, MenuItemID);
            AddInParameter(command, "@ParentID", DbType.Int32, ParentID);
            AddInParameter(command, "@MenuID", DbType.Int32, MenuID);
            AddInParameter(command, "@Sequence", DbType.Int32, Sequence);
            AddInParameter(command, "@MenuItemName", DbType.String, MenuItemName);
            AddInParameter(command, "@MenuItemCode", DbType.String, MenuItemCode);
            AddInParameter(command, "@PermissionName", DbType.String, PermissionName);
            AddInParameter(command, "@Location", DbType.String, Location);
            AddInParameter(command, "@Context", DbType.String, Context);
            AddInParameter(command, "@CSSClass", DbType.String, CSSClass);
            AddInParameter(command, "@Target", DbType.String, Target);


            retVal = ExecuteNonQuery(db, tran, command);

            MenuItemID = Convert.ToInt32(GetParameterValue(command, "@MenuItemID"));

            return retVal;

        }
    }
}
