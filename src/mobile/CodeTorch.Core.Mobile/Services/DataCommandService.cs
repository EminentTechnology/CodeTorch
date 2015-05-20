using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Linq;
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
                    

                            retVal = Common.CreateInstance(connectionType.Assembly, connectionType.Class) as IDataCommandProvider;
                            retVal.Initialize(connectionType.Settings);
                            this.Connections.Add(connection.Name, retVal);
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
        

        public async Task<DataTable> GetDataForDataCommand( string dataCommandName, List<ScreenDataCommandParameter> parameters)
        {
            DataTable retVal = null;


            DataCommand command = DataCommand.GetDataCommand(dataCommandName);

            if (command != null)
            {
                retVal = await GetData( command, parameters, command.Text);
            }
            else
            {
                throw new Exception(String.Format("DataCommand {0} does not exist in configuration", dataCommandName));
            }

            return retVal;
        }

        public Task<DataTable> GetData(DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, string commandText)
        {
          TaskCompletionSource<DataTable> taskDT = new TaskCompletionSource<DataTable>();

            

            DataCommandPreProcessorArgs preArgs = ProcessDataCommandPreProcessor( dataCommand, parameters);
            bool SkipExecution = false;

            if (preArgs != null)
            {
                SkipExecution = preArgs.SkipExecution;

                
            }

            if (SkipExecution)
            { 
                if (preArgs.Data != null)
                {
                    if (preArgs.Data is DataTable)
                    {
                        taskDT.SetResult( (DataTable)preArgs.Data);
                    }
                }
            }
            else
            {
                DataConnection connection = dataCommand.GetDataConnection();
                if(connection == null)
                {
                    taskDT.SetException( new Exception(String.Format("Data Connection could not be found in configuration - {0}", dataCommand.DataConnection)));
                    
                }
                IDataCommandProvider dataSource = GetProvider(connection);

                var result = dataSource.GetData(
                    connection, 
                    dataCommand, 
                    parameters, 
                    commandText,
                    success => 
                    {
                        DataCommandPostProcessorArgs postArgs = ProcessDataCommandPostProcessor( dataCommand, parameters, success);
                        if (postArgs != null)
                        {
                            if (postArgs.Data is DataTable)
                            {
                                taskDT.SetResult((DataTable)postArgs.Data);
                            }
                            else
                            { 
                                taskDT.SetResult(success);
                            }
                        }
                        else 
                        { 
                            taskDT.SetResult(success);
                        }

                        
                   
                    },
                    error => {
                        taskDT.SetException( error);
                 
                    });
            }

            return taskDT.Task;

        }
        
        #endregion



        #region get xml data

        public async Task<XDocument> GetXmlData(DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, string commandText)
        {
            XDocument retVal = null;

            DataCommandPreProcessorArgs preArgs = ProcessDataCommandPreProcessor( dataCommand, parameters);
            bool SkipExecution = false;

            if (preArgs != null)
            {
                SkipExecution = preArgs.SkipExecution;

                if (preArgs.Data != null)
                {
                    if (preArgs.Data is XDocument)
                    {
                        retVal = (XDocument)preArgs.Data;
                    }
                }
            }

            if (!SkipExecution)
            {
                DataConnection connection = dataCommand.GetDataConnection();
                if(connection == null)
                {
                    throw new Exception(String.Format("Data Connection could not be found in configuration - {0}", dataCommand.DataConnection));
                }
                IDataCommandProvider DataSource = GetProvider(connection);
                retVal = await DataSource.GetXmlData(
                    connection, 
                    dataCommand, 
                    parameters, 
                    commandText,
                    success => 
                    {
                        DataCommandPostProcessorArgs postArgs = ProcessDataCommandPostProcessor( dataCommand, parameters, retVal);
                        if (postArgs != null)
                        {
                            if (postArgs.Data is XDocument)
                            {
                                retVal = (XDocument)postArgs.Data;
                            }
                        }
                   
                    },
                    error => {
                        throw error;
                 
                    });
            }

            

            return retVal;
        }

        public async Task<XDocument> GetXmlDataForDataCommand(string DataCommandName, List<ScreenDataCommandParameter> parameters)
        {
            XDocument retVal = null;

            DataCommand command = DataCommand.GetDataCommand(DataCommandName);

            if (command != null)
            {
                retVal = await GetXmlData(command, parameters, command.Text);
            }
            else
            {
                throw new Exception(String.Format("DataCommand {0} does not exist in configuration", DataCommandName));
            }

            return retVal;
        }

        #endregion

        #region execute command
        public async Task<object> ExecuteCommand( DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, string commandText)
        {

            object retVal = null;


            try
            {

                DataCommandPreProcessorArgs preArgs = ProcessDataCommandPreProcessor( dataCommand, parameters);
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
                    DataConnection connection = dataCommand.GetDataConnection();
                    if(connection == null)
                    {
                        throw new Exception(String.Format("Data Connection could not be found in configuration - {0}", dataCommand.DataConnection));
                    }
                    IDataCommandProvider dataSource = GetProvider(connection);
                    retVal = await dataSource.ExecuteCommand(
                        connection, 
                        dataCommand, 
                        parameters, 
                        commandText,
                        success => 
                        {
                            DataCommandPostProcessorArgs postArgs = ProcessDataCommandPostProcessor( dataCommand, parameters, retVal);
                            if (postArgs != null)
                            {
                                retVal = postArgs.Data;
                            }
                   
                        },
                        error => {
                            throw error;
                 
                        });
                }

                
            }
            catch (Exception ex)
            {
                Common.LogException(ex);
                throw ex;
            }

            return retVal;
        }









       

        public async Task<object> ExecuteDataCommand( string dataCommandName, List<ScreenDataCommandParameter> parameters)
        {
            object retVal = null;

            DataCommand command = DataCommand.GetDataCommand(dataCommandName);
            if (command != null)
            {
                retVal = await ExecuteCommand( command, parameters, command.Text);
            }
            else
            {
                throw new Exception(String.Format("Data Command could not be found in configuration - {0}", dataCommandName));
            }

            return retVal;
        }
        #endregion

        #region processors
        private DataCommandPreProcessorArgs ProcessDataCommandPreProcessor(DataCommand dataCommand, List<ScreenDataCommandParameter> parameters)
        {
            DataCommandPreProcessorArgs args = null;
            if (!String.IsNullOrEmpty(dataCommand.PreProcessingClass))
            {
                try
                {

                    IDataCommandPreProcessor instance = Common.CreateInstance(dataCommand.PreProcessingAssembly, dataCommand.PreProcessingClass) as IDataCommandPreProcessor;

                    

                    if (instance != null)
                    {
                       args = new DataCommandPreProcessorArgs();

                            args.DataCommand = dataCommand;
                            args.Parameters = parameters;
                            args.SkipExecution = false;

                        instance.Process(args);

                    }

                    

                    
                }
                catch (Exception ex)
                {
                    Common.LogException(ex);
                    //if (ex.InnerException != null)
                    //{
                    //    Common.LogException(ex, false);
                    //    throw ex.InnerException;
                    //}
                    //else
                    //{
                    //////    Common.LogException(ex);
                    //}
                }
            }
            return args;
        }

        private DataCommandPostProcessorArgs ProcessDataCommandPostProcessor( DataCommand dataCommand, List<ScreenDataCommandParameter> parameters, object data)
        {
            DataCommandPostProcessorArgs args = null;
            if (!String.IsNullOrEmpty(dataCommand.PostProcessingClass))
            {
                try
                {
                    IDataCommandPostProcessor instance = Common.CreateInstance(dataCommand.PostProcessingAssembly, dataCommand.PostProcessingClass) as IDataCommandPostProcessor;

                   
                    if (instance != null)
                    {
                        args = new DataCommandPostProcessorArgs();

                            args.DataCommand = dataCommand;
                            args.Parameters = parameters;
                            args.Data = data;

                        instance.Process(args);

                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        Common.LogException(ex, false);
                        throw ex.InnerException;
                    }
                    else
                    {
                        Common.LogException(ex);
                    }
                }
            }
            return args;
        }


        #endregion


        
      
       

        

    }
}
