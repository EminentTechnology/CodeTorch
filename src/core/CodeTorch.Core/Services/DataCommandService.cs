using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;

using System.Reflection;
using System.Xml;

using CodeTorch.Core.Interfaces;

namespace CodeTorch.Core.Services
{
    public class DataCommandService 
    {
       

        static readonly DataCommandService instance = new DataCommandService();

        

        Dictionary<string, IDataCommandProvider> _Connections = new Dictionary<string, IDataCommandProvider>();

        public Dictionary<string, IDataCommandProvider> Connections
        {
            get
            {
                return _Connections;
            }
            set
            {
                _Connections = value;
            }
        }

        public static DataCommandService GetInstance()
        {
            return instance;
        }

        private DataCommandService()
        {
        }

        public IDataCommandProvider GetProvider(DataConnection connection)
        {
            IDataCommandProvider retVal = null;

            if (this.Connections.ContainsKey(connection.Name))
            {
                //get cached data command provider
                retVal = this.Connections[connection.Name];
            }
            else
            { 
                //add data command provider to cache

                DataConnectionType connectionType = connection.GetConnectionType();

                if (connectionType != null)
                {
                    if (!String.IsNullOrEmpty(connectionType.Class) && !String.IsNullOrEmpty(connectionType.Assembly))
                    {

                        try
                        {
                            Assembly providerAssembly = Assembly.Load(connectionType.Assembly);
                            if (providerAssembly != null)
                            {
                                Type type = providerAssembly.GetType(connectionType.Class, true, true);

                                if (type != null)
                                {

                                    ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                                    retVal = constructor.Invoke(null) as IDataCommandProvider;

                                    retVal.Initialize(connectionType.Settings);

                                    this.Connections.Add(connection.Name, retVal);
                                }
                            }
                        }
                        catch {
                            //silent error
                        }
                    }
                }
            }

            if (retVal == null)
            {
                throw new Exception(String.Format("No valid data command provider found for connection {0}", connection.Name));
            }

            return retVal;
        }

      

        #region get data
        public DataTable GetDataForDataCommand(string dataCommandName, List<ScreenDataCommandParameter> parameters)
        {
            return GetDataForDataCommand(null, dataCommandName, parameters);
        }

        public DataTable GetDataForDataCommand(DbTransaction tran, string dataCommandName, List<ScreenDataCommandParameter> parameters)
        {
            DataTable retVal = null;


            DataCommand command = DataCommand.GetDataCommand(dataCommandName);

            if (command != null)
            {
                retVal = GetData(tran, command, parameters, command.Text);
            }
            else
            {
                throw new ApplicationException(String.Format("DataCommand {0} does not exist in configuration", dataCommandName));
            }

            return retVal;
        }

        public DataTable GetData(DbTransaction tran, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, string commandText)
        {
            DataConnection connection = dataCommand.GetDataConnection();
            return GetData(tran, connection, dataCommand, parameters, commandText);
        }
        public DataTable GetData(DbTransaction tran, DataConnection connection, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters,  string commandText)
        {
            DataTable retVal = null;

            DataCommandPreProcessorArgs preArgs = ProcessDataCommandPreProcessor(tran, dataCommand, parameters);
            bool SkipExecution = false;

            if (preArgs != null)
            {
                SkipExecution = preArgs.SkipExecution;

                if (preArgs.Data != null)
                {
                    if (preArgs.Data is DataTable)
                    {
                        retVal = (DataTable)preArgs.Data;
                    }
                }
            }

            if (!SkipExecution)
            {
                
                if(connection == null)
                {
                    throw new Exception(String.Format("Data Connection could not be found in configuration - {0}", dataCommand.DataConnection));
                }
                IDataCommandProvider dataSource = GetProvider(connection);

                retVal = dataSource.GetData(connection, dataCommand, parameters, commandText);
            }

            DataCommandPostProcessorArgs postArgs = ProcessDataCommandPostProcessor(tran, dataCommand, parameters, retVal);
            if (postArgs != null)
            {
                if (postArgs.Data is DataTable)
                {
                    retVal = (DataTable)postArgs.Data;
                }
            }

            return retVal;
        }

        
        #endregion

        #region get xml data

        public XmlDocument GetXmlData(DbTransaction tran, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, string commandText)
        {
            DataConnection connection = dataCommand.GetDataConnection();
            return GetXmlData(tran, connection, dataCommand, parameters, commandText);
        }
        public XmlDocument GetXmlData(DbTransaction tran, DataConnection connection, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters,  string commandText)
        {
            XmlDocument retVal = null;

            DataCommandPreProcessorArgs preArgs = ProcessDataCommandPreProcessor(tran, dataCommand, parameters);
            bool SkipExecution = false;

            if (preArgs != null)
            {
                SkipExecution = preArgs.SkipExecution;

                if (preArgs.Data != null)
                {
                    if (preArgs.Data is XmlDocument)
                    {
                        retVal = (XmlDocument)preArgs.Data;
                    }
                }
            }

            if (!SkipExecution)
            {
                
                if(connection == null)
                {
                    throw new Exception(String.Format("Data Connection could not be found in configuration - {0}", dataCommand.DataConnection));
                }
                IDataCommandProvider DataSource = GetProvider(connection);
                retVal = DataSource.GetXmlData(connection, dataCommand, parameters, commandText);
            }

            DataCommandPostProcessorArgs postArgs = ProcessDataCommandPostProcessor(tran, dataCommand, parameters, retVal);
            if (postArgs != null)
            {
                if (postArgs.Data is XmlDocument)
                {
                    retVal = (XmlDocument)postArgs.Data;
                }
            }

            return retVal;
        }

        public XmlDocument GetXmlDataForDataCommand(string DataCommandName, List<ScreenDataCommandParameter> parameters)
        {
            return GetXmlDataForDataCommand(null, DataCommandName, parameters);
        }

        public XmlDocument GetXmlDataForDataCommand(DbTransaction tran, string DataCommandName, List<ScreenDataCommandParameter> parameters)
        {
            XmlDocument retVal = null;


            DataCommand command = DataCommand.GetDataCommand(DataCommandName);

            if (command != null)
            {
                retVal = GetXmlData(tran, command, parameters, command.Text);
            }
            else
            {
                throw new ApplicationException(String.Format("DataCommand {0} does not exist in configuration", DataCommandName));
            }

            return retVal;
        }

        #endregion

        #region execute command
        public object ExecuteCommand(DbTransaction tran, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, string commandText)
        {
            DataConnection connection = dataCommand.GetDataConnection();
            return ExecuteCommand(tran, connection, dataCommand, parameters, commandText);
        }
        public object ExecuteCommand(DbTransaction tran, DataConnection connection, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, string commandText)
        {

            object retVal = null;


            try
            {

                DataCommandPreProcessorArgs preArgs = ProcessDataCommandPreProcessor(tran, dataCommand, parameters);
                bool SkipExecution = false;

                if (preArgs != null)
                {
                    SkipExecution = preArgs.SkipExecution;

                    if (preArgs.Data != null)
                    {
                        retVal = preArgs.Data;
                    }
                }

                if (!SkipExecution)
                {
                    
                    if(connection == null)
                    {
                        throw new Exception(String.Format("Data Connection could not be found in configuration - {0}", dataCommand.DataConnection));
                    }
                    IDataCommandProvider dataSource = GetProvider(connection);
                    retVal = dataSource.ExecuteCommand(connection, dataCommand, parameters, commandText);
                }

                DataCommandPostProcessorArgs postArgs = ProcessDataCommandPostProcessor(tran, dataCommand, parameters, retVal);
                if (postArgs != null)
                {
                    retVal = postArgs.Data;
                }
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                throw ex;
            }

            return retVal;
        }









        public object ExecuteDataCommand(string DataCommandName, List<ScreenDataCommandParameter> parameters)
        {
            return ExecuteDataCommand(null, DataCommandName, parameters);
        }

        public object ExecuteDataCommand(DbTransaction tran, string dataCommandName, List<ScreenDataCommandParameter> parameters)
        {
            object retVal = null;

            DataCommand command = DataCommand.GetDataCommand(dataCommandName);
            if (command != null)
            {
                retVal = ExecuteCommand(tran, command, parameters, command.Text);
            }
            else
            {
                throw new ApplicationException(String.Format("Data Command could not be found in configuration - {0}", dataCommandName));
            }

            return retVal;
        }
        #endregion

        #region processors
        private DataCommandPreProcessorArgs ProcessDataCommandPreProcessor(DbTransaction tran, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters)
        {
            DataCommandPreProcessorArgs args = null;
            if (!String.IsNullOrEmpty(dataCommand.PreProcessingClass))
            {
                try
                {
                    Assembly processorAssembly = Assembly.Load(dataCommand.PreProcessingAssembly);
                    if (processorAssembly != null)
                    {
                        Type type = processorAssembly.GetType(dataCommand.PreProcessingClass, true, true);

                        if (type != null)
                        {
                            MethodInfo method = type.GetMethod("Process");
                            ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                            object instance = constructor.Invoke(null);

                            args = new DataCommandPreProcessorArgs();

                            args.Transaction = tran;
                            args.DataCommand = dataCommand;
                            args.Parameters = parameters;
                            args.SkipExecution = false;

                            object[] argParameter = new object[1];
                            argParameter[0] = args;

                            try
                            {
                                method.Invoke(instance, argParameter);
                            }
                            catch (CodeTorchException cex)
                            {
                                string errorFormat = "Error in PreProcessor - {0}";

                                if (cex.MoreInfo == null)
                                {
                                    cex.MoreInfo = String.Format(errorFormat, dataCommand.PreProcessingClass); ;
                                }

                                Common.LogException(cex, false);
                                throw cex;
                            }
                            catch (Exception e)
                            {
                                string errorFormat = "Error in PreProcessor - {0}";

                                CodeTorchException preProcessorException;
                                if (e.InnerException != null)
                                {
                                    Common.LogException(e, false);

                                    if(e.InnerException is CodeTorchException)
                                    {
                                        preProcessorException = e.InnerException as CodeTorchException;
                                        if (preProcessorException.MoreInfo == null)
                                        {
                                            preProcessorException.MoreInfo = String.Format(errorFormat, dataCommand.PreProcessingClass);
                                        }
                                    }
                                    else
                                    {
                                        preProcessorException = new CodeTorchException(e.InnerException.Message, e.InnerException);
                                        preProcessorException.MoreInfo = String.Format(errorFormat, dataCommand.PreProcessingClass);
                                    }
                                    
                                    throw preProcessorException;
                                }
                                else
                                {
                                    Common.LogException(e, false);
                                    preProcessorException = new CodeTorchException(e.Message, e.InnerException);
                                    preProcessorException.MoreInfo = String.Format(errorFormat, dataCommand.PreProcessingClass);
                                    throw preProcessorException;
                                }

                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    Common.LogException(ex);

                }
            }
            return args;
        }

        private DataCommandPostProcessorArgs ProcessDataCommandPostProcessor(DbTransaction tran, DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, object data)
        {
            DataCommandPostProcessorArgs args = null;
            if (!String.IsNullOrEmpty(dataCommand.PostProcessingClass))
            {
                try
                {
                    Assembly processorAssembly = Assembly.Load(dataCommand.PostProcessingAssembly);
                    if (processorAssembly != null)
                    {
                        Type type = processorAssembly.GetType(dataCommand.PostProcessingClass, true, true);

                        if (type != null)
                        {
                            MethodInfo method = type.GetMethod("Process");
                            ConstructorInfo constructor = type.GetConstructor(Type.EmptyTypes);
                            object instance = constructor.Invoke(null);

                            args = new DataCommandPostProcessorArgs();

                            args.Transaction = tran;
                            args.DataCommand = dataCommand;
                            args.Parameters = parameters;
                            args.Data = data;


                            object[] argParameter = new object[1];
                            argParameter[0] = args;

                            
                            try
                            {
                                method.Invoke(instance, argParameter);
                            }
                            catch (CodeTorchException cex)
                            {
                                string errorFormat = "Error in PostProcessor - {0}";

                                if (cex.MoreInfo == null)
                                {
                                    cex.MoreInfo = String.Format(errorFormat, dataCommand.PreProcessingClass); ;
                                }

                                Common.LogException(cex, false);
                                throw cex;
                            }
                            catch (Exception e)
                            {
                                string errorFormat = "Error in PostProcessor - {0}";

                                CodeTorchException postProcessorException;
                                if (e.InnerException != null)
                                {
                                    Common.LogException(e, false);

                                    if (e.InnerException is CodeTorchException)
                                    {
                                        postProcessorException = e.InnerException as CodeTorchException;
                                        if (postProcessorException.MoreInfo == null)
                                        {
                                            postProcessorException.MoreInfo = String.Format(errorFormat, dataCommand.PostProcessingClass);
                                        }
                                    }
                                    else
                                    {
                                        postProcessorException = new CodeTorchException(e.InnerException.Message, e.InnerException);
                                        postProcessorException.MoreInfo = String.Format(errorFormat, dataCommand.PostProcessingClass);
                                    }
                                    
                                    throw postProcessorException;
                                }
                                else
                                {
                                    Common.LogException(e, false);
                                    postProcessorException = new CodeTorchException(e.Message, e.InnerException);
                                    postProcessorException.MoreInfo = String.Format(errorFormat, dataCommand.PreProcessingClass);
                                    throw postProcessorException;
                                }

                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    Common.LogException(ex);
                }
            }
            return args;
        }


        #endregion


        
      
       

        

    }
}
