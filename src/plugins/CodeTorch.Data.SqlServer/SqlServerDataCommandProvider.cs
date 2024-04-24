using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Data;

using System.Text;
using CodeTorch.Core.Interfaces;
using CodeTorch.Core;
using System.Collections.Specialized;

namespace CodeTorch.Data.SqlServer
{
    public class SqlServerDataCommandProvider : DataLayerBase, IDataCommandProvider
    {
        public void Initialize(StringCollection settings)
        {
            //read any special config needed for specific implementation
        }

        public DataTable GetData(DataConnection connection, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, string commandText)
        {
            //?? what do i need connection type for again? maybe for multiple database support for same project
            CommandType sqlCommandType = GetCommandType(dataCommand.Type);
            return GetDataTable(connection, dataCommand, parameters, sqlCommandType, commandText);
        }

        public XmlDocument GetXmlData(DataConnection connection, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, string commandText)
        {


            
            CommandType sqlCommandType = GetCommandType(dataCommand.Type);
            return GetXmlDocument(connection, dataCommand, parameters, sqlCommandType, commandText);
        }

        public object ExecuteCommand(DataConnection connection, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, string commandText)
        {
            
            CommandType sqlCommandType = GetCommandType(dataCommand.Type);
            return Exec(connection, dataCommand, parameters, sqlCommandType, commandText);
        }

        public void RefreshSchema(DataConnection connection, CodeTorch.Core.DataCommand command)
        {
            RefreshSchemaDetails(connection, command);
        }

        #region common

        private CommandType GetCommandType(string commandType)
        {
            return (commandType.ToLower().Trim() == "storedprocedure") ? CommandType.StoredProcedure : CommandType.Text;
        }
        
        private void AddInParameter(Database db, DbCommand command, DataCommandParameter p, object value)
        {
            if (p.IsUserDefinedType)
            {
                if (p.IsTableType)
                {
                    //db.AddInParameter
                    AddTableValueParameter(db, command, p.Name, p.TypeName, GetValue(p, value), ',');
                }
            }
            else
            {
                db.AddInParameter(command, p.Name, (DbType)Enum.Parse(typeof(DbType), p.Type.ToString()), GetValue(p, value));
            }
        }
        
        private object GetValue(DataCommandParameter p, object Value)
        {
            object retVal = null;
            
            switch (p.Type)
            {
                case DataCommandParameterType.Guid:
                    if (Value != null)
                    {
                        if (Value.ToString() == String.Empty)
                        {
                            retVal = DBNull.Value;
                        }
                        else
                        {
                            retVal = new Guid(Value.ToString());
                        }
                    }
                    break;
                case DataCommandParameterType.Int32:
                    if (Value != null)
                    {
                        if (Value.ToString() == String.Empty)
                        {
                            retVal = DBNull.Value;
                        }
                        else
                        {
                            retVal = Value;
                        }
                    }
                    break;
                default:
                    if (Value != null)
                    {
                        if (Value.ToString() == String.Empty)
                        {
                            retVal = DBNull.Value;
                        }
                        else
                        {
                            retVal = Value;
                        }
                    }
                    break;
            }
            
            return retVal;
        }
        
        private void AddTableValueParameter(Database db, DbCommand command, string ParameterName, string TypeName, object value, char delimiter)
        {
            if ((value != null) && (!(value is DBNull)))
            {
                System.Data.SqlClient.SqlParameter p = new System.Data.SqlClient.SqlParameter();
                
                p.ParameterName = ParameterName;
                p.SqlDbType = SqlDbType.Structured;
                p.TypeName = TypeName;
                
                if (value is string)
                {
                    //assume csv
                    string[] items = value.ToString().Split(delimiter);
                    
                    DataTable dt = new DataTable();
                    
                    dt.Columns.Add("Value");
                    
                    for (int i = 0; i < items.Length; i++)
                    {
                        dt.Rows.Add(items[i]);
                    }
                    
                    p.Value = dt;
                }
                else
                {
                    p.Value = value;
                }
                
                command.Parameters.Add(p);
            }
        }
        
        #endregion
        
        #region datatable

        private DataTable GetDataTable(DataConnection connection, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, CommandType commandType, string commandText)
        {
            DataTable retVal = null;
            Database db = CreateDatabase(connection);
            
            //create command and specify stored procedure name
            DbCommand command = null;
            
            switch (commandType)
            {
                case CommandType.Text:
                    command = db.GetSqlStringCommand(commandText);
                    break;
                case CommandType.StoredProcedure:
                    command = db.GetStoredProcCommand(commandText);
                    break;
            }
            
            // specify stored procedure parameters
            
            if (parameters != null)
            {
                foreach (DataCommandParameter p in dataCommand.Parameters)
                {
                    object value = null;
                    ScreenDataCommandParameter screenParam;
                    
                    try
                    {
                        screenParam = parameters.Where(sp => sp.Name.ToLower() == p.Name.ToLower()).SingleOrDefault();
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("Screen Parameter is null when value is expected", ex);
                    }
                    
                    if (screenParam != null)
                    {
                        value = screenParam.Value;
                    }
                    
                    switch (p.Direction)
                    {
                        case DataCommandParameterDirection.In:
                            AddInParameter(db, command, p, value);
                            break;
                    }
                }
            }
            
            //execute command
            retVal = ExecuteDataTable(db, null, command);
            return retVal;
        }
        
        #endregion
        
        #region xml

        private XmlDocument GetXmlDocument(DataConnection connection, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, CommandType commandType, string commandText)
        {
            XmlDocument retVal = null;
            Database db = CreateDatabase(connection);
            
            //create command and specify stored procedure name
            DbCommand command = null;
            
            switch (commandType)
            {
                case CommandType.Text:
                    command = db.GetSqlStringCommand(commandText);
                    break;
                case CommandType.StoredProcedure:
                    command = db.GetStoredProcCommand(commandText);
                    break;
            }

            // specify stored procedure parameters
            
            if (parameters != null)
            {
                foreach (DataCommandParameter p in dataCommand.Parameters)
                {
                    object value = null;
                    ScreenDataCommandParameter screenParam;
                    
                    try
                    {
                        screenParam = parameters.Where(sp => sp.Name.ToLower() == p.Name.ToLower()).SingleOrDefault();
                    }
                    catch (Exception ex)
                    {
                        throw new ApplicationException("Screen Parameter is null when value is expected", ex);
                    }
                    
                    if (screenParam != null)
                    {
                        value = screenParam.Value;
                    }
                    
                    switch (p.Direction)
                    {
                        case DataCommandParameterDirection.In:
                            AddInParameter(db, command, p, value);
                            break;
                    }
                }
            }
            
            //execute command
            retVal = ExecuteXml(db, null, command);
            return retVal;
        }

        #endregion

        #region execute

        private object Exec(DataConnection connection, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, CommandType commandType, string commandText)
        {
            object retVal = null; 
            bool hasOutputParameters = false;
            string firstOutputParameterName = String.Empty;

            Database db = CreateDatabase(connection);
            
            //create command and specify stored procedure name
            DbCommand command = null;
            
            switch (commandType)
            {
                case CommandType.Text:
                    command = db.GetSqlStringCommand(commandText);
                    break;
                case CommandType.StoredProcedure:
                    command = db.GetStoredProcCommand(commandText);
                    break;
            }

            // specify stored procedure parameters
            
            if (parameters != null)
            {
                foreach (DataCommandParameter p in dataCommand.Parameters)
                {
                    object value = null;
                    ScreenDataCommandParameter screenParam = parameters.Where(sp => sp.Name.ToLower() == p.Name.ToLower()).SingleOrDefault();
                    
                    if (screenParam != null)
                    {
                        value = screenParam.Value;
                    }
                    
                    switch (p.Direction)
                    {
                        case DataCommandParameterDirection.In:
                            AddInParameter(db, command, p, value);
                            break;
                        case DataCommandParameterDirection.Out:
                            if (!hasOutputParameters)
                            {
                                hasOutputParameters = true;
                                firstOutputParameterName = p.Name;
                            }
                            
                            //db.
                            db.AddParameter(command, p.Name, (DbType)Enum.Parse(typeof(DbType), p.Type.ToString()), ParameterDirection.InputOutput, p.Name, DataRowVersion.Current, value);
                            DbParameter param = command.Parameters[p.Name];
                            param.Size = p.Size;
                            //db.AddOutParameter(command, p.Name, (DbType)Enum.Parse(typeof(DbType), p.Type.ToString()), p.Size);
                            
                            break;
                    }
                }
            }
            
            //execute command
            ExecuteNonQuery(db, null, command);
            
            if (hasOutputParameters)
            {
                retVal = db.GetParameterValue(command, firstOutputParameterName);
            }
            
            return retVal;
        }
        
        #endregion

        #region metadata for designer
        private void RefreshSchemaDetails(DataConnection connection, CodeTorch.Core.DataCommand command)
        {

            switch (command.Type.ToString().ToLower())
            {
                case "storedprocedure":
                    //get parameters
                    DataTable paramDT = GetStoredProcedureParameters(connection, null, command.Text);

                    command.Parameters.Clear();
                    foreach (DataRow p in paramDT.Rows)
                    {
                        CodeTorch.Core.DataCommandParameter param = new CodeTorch.Core.DataCommandParameter();
                        param.Name = p["ParameterName"].ToString();
                        param.Size = Convert.ToInt32(p["length"]);
                        param.Type = GetParameterTypeFromDBType(p["ParameterDBType"].ToString(), Convert.ToBoolean(p["is_user_defined"]), Convert.ToBoolean(p["is_table_type"]));
                        param.Direction = GetParameterDirectionFromDB(Convert.ToBoolean(p["IsOutParam"]));
                        param.TypeName = p["ParameterDBType"].ToString();
                        param.IsUserDefinedType = Convert.ToBoolean(p["is_user_defined"]);
                        param.IsTableType = Convert.ToBoolean(p["is_table_type"]);

                        command.Parameters.Add(param);
                    }

                    break;

                case "text":
                    string[] paramTokens = command.Text.Split('@');
                    if (paramTokens.Length > 0)
                    {
                        List<string> paramlist = new List<string>();

                        for (int paramIndex = 1; paramIndex < paramTokens.Length; paramIndex++)
                        {
                            string temp = paramTokens[paramIndex];
                            int paramEndIndex = temp.Length - 1;

                            for (int i = 0; i < temp.Length; i++)
                            {
                                bool IsValidChar = false;

                                //if character is letter or digit
                                if( Char.IsLetterOrDigit(temp[i]))
                                {
                                    IsValidChar = true;
                                }

                                //if character is @
                                if( temp[i] == '@')
                                {
                                    IsValidChar = true;
                                }

                                if( temp[i] == '$')
                                {
                                    IsValidChar = true;
                                }

                                if( temp[i] == '#')
                                {
                                    IsValidChar = true;
                                }

                                if( temp[i] == '_')
                                {
                                    IsValidChar = true;
                                }

                                if (!IsValidChar)
                                {
                                    paramEndIndex = (i - 1);
                                    break;
                                }
                            }

                            string paramitem = string.Empty;
                            paramitem = paramTokens[paramIndex].Substring(0, paramEndIndex+1);

                            if (!paramlist.Contains("@" + paramitem))
                            {
                                paramlist.Add("@" + paramitem);
                            }
                        }

                        command.Parameters.Clear();

                        foreach (string paramItem in paramlist)
                        {
                            CodeTorch.Core.DataCommandParameter param = new CodeTorch.Core.DataCommandParameter();

                            param.Name = paramItem;
                            param.Size = 200;
                            param.Type = DataCommandParameterType.String;
                            param.Direction = DataCommandParameterDirection.In;

                            command.Parameters.Add(param);
                        }
                    }
                    break;
            }

            if (command.ReturnType.ToString().ToLower() == "datatable")
            {
                DataColumnCollection columns = this.InferDataCommandResultsetColumns(connection, command);


                command.Columns.Clear();
                foreach (DataColumn col in columns)
                {

                    CodeTorch.Core.DataCommandColumn column = new CodeTorch.Core.DataCommandColumn();

                    column.Name = col.ColumnName;
                    column.Type = col.DataType.Name;

                    command.Columns.Add(column);

                }
            }


        }

        public DataColumnCollection InferDataCommandResultsetColumns(DataConnection connection, CodeTorch.Core.DataCommand cmd)
        {
            DataTable data;
            Database db = CreateDatabase(connection);
            StringBuilder SQL = new StringBuilder();

            SQL.AppendLine("SET FMTONLY ON ");
            string sep = "";

            if (cmd.Type.ToString().ToLower() == "storedprocedure")
            {
                SQL.Append("EXEC ");
            }
            SQL.Append(cmd.Text);
            SQL.Append(" ");

            if (cmd.Type.ToString().ToLower() == "storedprocedure")
            {
                foreach (CodeTorch.Core.DataCommandParameter p in cmd.Parameters)
                {
                    if (!p.IsUserDefinedType)
                    {
                        SQL.AppendFormat("{0} {1}=null", sep, p.Name);
                        sep = ",";
                    }
                }
            }
            SQL.AppendLine();
            SQL.AppendLine("SET FMTONLY OFF ");

            //create command and specify stored procedure name
            DbCommand command = db.GetSqlStringCommand(SQL.ToString());

            // specify stored procedure parameters
            if (cmd.Type.ToString().ToLower() == "text")
            {
                foreach (CodeTorch.Core.DataCommandParameter p in cmd.Parameters)
                {
                    db.AddInParameter(command, p.Name, DbType.String, DBNull.Value);
                }
            }


            //execute command
            data = ExecuteDataTable(db, null, command);

            return data.Columns;



        }

        private DataCommandParameterDirection GetParameterDirectionFromDB(bool IsOutParam)
        {

            DataCommandParameterDirection retVal = (IsOutParam) ? DataCommandParameterDirection.Out : DataCommandParameterDirection.In;
            return retVal;
        }

        private DataCommandParameterType GetParameterTypeFromDBType(string DBType, bool IsUserDefinedType, bool IsTable)
        {
            DataCommandParameterType retVal = DataCommandParameterType.String;

            string newType = "String";


            if (IsUserDefinedType)
            {
                if (IsTable)
                {
                    newType = "Structured";
                }
                else
                {
                    newType = "Object";
                }
            }
            else
            {
                switch (DBType.ToLower())
                {
                    case "image":
                        newType = "Object";
                        break;
                    case "text":
                        newType = "String";
                        break;
                    case "uniqueidentifier":
                        newType = "Guid";
                        break;
                    case "tinyint":
                        newType = "Int32";
                        break;
                    case "smallint":
                        newType = "Int32";
                        break;
                    case "int":
                        newType = "Int32";
                        break;
                    case "smalldatetime":
                        newType = "DateTime";
                        break;
                    case "real":
                        newType = "Decimal";
                        break;
                    case "money":
                        newType = "Decimal";
                        break;
                    case "datetime":
                        newType = "DateTime";
                        break;
                    case "float":
                        newType = "Single";
                        break;
                    case "sql_variant":
                        newType = "Object";
                        break;
                    case "ntext":
                        newType = "String";
                        break;
                    case "bit":
                        newType = "Boolean";
                        break;
                    case "decimal":
                        newType = "Decimal";
                        break;
                    case "numeric":
                        newType = "Decimal";
                        break;
                    case "smallmoney":
                        newType = "Decimal";
                        break;
                    case "bigint":
                        newType = "Int64";
                        break;
                    case "varbinary":
                        newType = "Binary";
                        break;
                    case "varchar":
                        newType = "String";
                        break;
                    case "binary":
                        newType = "Binary";
                        break;
                    case "char":
                        newType = "String";
                        break;
                    case "timestamp":
                        newType = "Int64";
                        break;
                    case "nvarchar":
                        newType = "String";
                        break;
                    case "nchar":
                        newType = "String";
                        break;
                    default:
                        newType = "String";
                        break;
                }
            }

            retVal = (DataCommandParameterType)Enum.Parse(typeof(DataCommandParameterType), newType);
            return retVal;
        }

        public DataTable GetStoredProcedureParameters(DataConnection connection, DbTransaction tran, string SPName)
        {
            DataTable retVal;
            Database db = CreateDatabase(connection);
            string commandText = @"select 
	                                        p.Name ParameterName,  
	                                        p.parameter_id colID, 
	                                        p.parameter_id colorder, 
	                                        t.Name ParameterDBType, 
	                                        p.max_length Length, 
	                                        p.precision xprec, 
	                                        p.scale, 
	                                        p.is_output IsOutParam, 
	                                        t.is_nullable IsNullable,
	                                        is_readonly, 
	                                        t.is_user_defined, 
	                                        t.is_table_type
                                        from 
	                                        sys.parameters p 
	                                        join sys.types t 
		                                        on p.user_type_id = t.user_type_id 
                                        where 
	                                        p.object_id = object_id(@SPName)  
                                        order by 
	                                        p.parameter_id";

            //create command and specify stored procedure name
            DbCommand command = db.GetSqlStringCommand(commandText);

            // specify stored procedure parameters
            db.AddInParameter(command, "@SPName", DbType.String, SPName);

            //execute command
            retVal = ExecuteDataTable(db, tran, command);

            return retVal;
        }

        #endregion
    }
}