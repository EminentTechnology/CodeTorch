﻿using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

using log4net;

namespace Eminent.AppBuilder.BuildTasks
{
    public abstract class DataLayerBase
    {


        

       

        public static DbCommand GetSqlStringCommand(string CommandText)
        {
            DbCommand command = new SqlCommand();
            command.CommandText = CommandText;
            command.CommandType = CommandType.Text;

            return command;
        }

        public static DbCommand GetStoredProcCommand(string CommandText)
        {
            DbCommand command = new SqlCommand();
            command.CommandText = CommandText;
            command.CommandType = CommandType.StoredProcedure;

            return command;
        }

        public static void AddInParameter(DbCommand command, string ParameterName, DbType DBType, object value)
        {
            DbParameter p;

            p = command.CreateParameter();
            p.ParameterName = ParameterName;
            p.DbType = DBType;

            if (value == null)
            {
                p.Value = DBNull.Value;
            }
            else
            {
                p.Value = value;
            }

            command.Parameters.Add(p);
        }

        public static object GetParameterValue(DbCommand command, string ParameterName)
        {
            object retVal;

            retVal = command.Parameters[ParameterName].Value;

            return retVal;


        }

        public static void AddOutParameter(DbCommand command, string ParameterName, DbType DBType, object value)
        {
            DbParameter p;

            p = command.CreateParameter();
            p.ParameterName = ParameterName;
            p.DbType = DBType;
            if (value == null)
            {
                p.Value = DBNull.Value;
            }
            else
            {
                p.Value = value;
            }
            p.Direction = ParameterDirection.InputOutput;

            command.Parameters.Add(p);
        }

        public static void AddTableValueParameter(Database db, DbCommand command, string ParameterName, string TypeName, DataTable value)
        {
            if (value != null)
            {

                System.Data.SqlClient.SqlParameter p = new System.Data.SqlClient.SqlParameter();

                p.ParameterName = ParameterName;
                p.SqlDbType = SqlDbType.Structured;
                p.TypeName = TypeName;
                p.Value = value;



                command.Parameters.Add(p);
            }
        }

        public static DbConnection GetConnection(Database db, string connectionString)
        {

            DbConnection retVal = null;

            if (String.IsNullOrEmpty(connectionString))
            {
                retVal = db.CreateConnection();
            }
            else
            {
                retVal = new SqlConnection(connectionString);
            }
            return retVal;
        }

        #region ExecuteDataSet
        public static DataSet ExecuteDataSet(Database db, DbTransaction tran, DbCommand cmd)
        {
            //declare variables
            DataSet ret = null;

            ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            LogSQL(cmd, log);


            if (db != null)
            {
                if (tran == null)
                {
                    ret = db.ExecuteDataSet(cmd);
                }
                else
                {
                    ret = db.ExecuteDataSet(cmd, tran);
                }
            }
            else
            {
                SqlDataAdapter da = new SqlDataAdapter();

                if (tran == null)
                {
                    ret = new DataSet();
                    da.SelectCommand = (SqlCommand)cmd;
                    da.Fill(ret);
                }
                else
                {
                    cmd.Transaction = tran;
                    cmd.Connection = cmd.Transaction.Connection;
                    ret = new DataSet();
                    da.SelectCommand = (SqlCommand)cmd;
                    da.Fill(ret);
                }

            }

            return ret;

        }
        #endregion

        #region ExecuteDataTable
        public static DataTable ExecuteDataTable(Database db, DbTransaction tran, DbCommand cmd)
        {
            //declare variables
            DataTable ret = null;



            ret = ExecuteDataSet(db, tran, cmd).Tables[0];

            return ret;
        }

        private static void LogSQL(DbCommand cmd, ILog log)
        {
            if (log.IsDebugEnabled)
            {
                StringBuilder msg = new StringBuilder();

                msg.AppendFormat("\r\n\r\nExecuting CommandText:{0}; CommandType: {1}.", cmd.CommandText, cmd.CommandType);
                foreach (DbParameter p in cmd.Parameters)
                {
                    msg.AppendFormat("\r\nParameterName:{0};\tValue:{1};\tDbType:{2};\tDirection:{3}", p.ParameterName, p.Value, p.DbType, p.Direction);
                }

                log.Debug(msg.ToString());
            }
        }
        #endregion

        #region ExecuteNonQuery
        public static int ExecuteNonQuery(Database db, DbTransaction tran, DbCommand cmd)
        {
            //declare variable
            int retVal = 0;

            ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            LogSQL(cmd, log);


            if (db != null)
            {
                if (tran == null)
                {
                    retVal = db.ExecuteNonQuery(cmd);

                }
                else
                {

                    retVal = db.ExecuteNonQuery(cmd, tran);
                }
            }
            else
            {
                if (tran == null)
                {

                    retVal = cmd.ExecuteNonQuery();


                }
                else
                {
                    cmd.Transaction = tran;
                    cmd.Connection = cmd.Transaction.Connection;
                    retVal = cmd.ExecuteNonQuery();
                }
            }

            return retVal;
        }
        #endregion

        #region ExecuteXml
        public static XmlDocument ExecuteXml(Database db, DbTransaction tran, DbCommand cmd)
        {
            //declare variables
            XmlDocument ret = new XmlDocument();
            SqlDatabase sqlDb = (SqlDatabase)db;

            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            LogSQL(cmd, log);



            if (tran == null)
            {
                using (XmlReader reader = sqlDb.ExecuteXmlReader(cmd))
                {
                    ret.Load(reader);
                }
            }
            else
            {
                using (XmlReader reader = sqlDb.ExecuteXmlReader(cmd, tran))
                {
                    ret.Load(reader);
                }
            }


            return ret;

        }
        #endregion

        #region "LoadTable"
        public static int LoadTable(DbConnection connection, DbTransaction tran, string TableName, DataTable dtTableData)
        {
            int retVal = -1;

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection as SqlConnection, SqlBulkCopyOptions.Default, tran as SqlTransaction))
            {

                bulkCopy.DestinationTableName = TableName;
                bulkCopy.WriteToServer(dtTableData);
                retVal = 1;
            }

            return retVal;
        }
        #endregion
    }
}
