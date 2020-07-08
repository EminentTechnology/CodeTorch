using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CodeTorch.Core
{
    public class Configuration
    {
        static readonly Configuration instance = new Configuration();

        public static Configuration GetInstance()
        {
            return instance;
        }

        private Configuration()
        {
        }

        public App App = new App();

        public List<ControlType> ControlTypes = new List<ControlType>();
        public List<DataCommand> DataCommands = new List<DataCommand>();
        public List<DataConnection> DataConnections = new List<DataConnection>();
        public List<DataConnectionType> DataConnectionTypes = new List<DataConnectionType>();
        public List<DocumentRepository> DocumentRepositories = new List<DocumentRepository>();
        
        public List<DocumentRepositoryType> DocumentRepositoryTypes = new List<DocumentRepositoryType>();
        
        public List<Lookup> Lookups = new List<Lookup>();
        public List<Menu> Menus = new List<Menu>();
        public List<Permission> Permissions = new List<Permission>();
        public List<Picker> Pickers = new List<Picker>();
        public List<RestService> RestServices = new List<RestService>();
        public List<ScreenType> ScreenTypes = new List<ScreenType>();
        public List<Screen> Screens = new List<Screen>();
        public List<SectionType> SectionTypes = new List<SectionType>();
        public List<Sequence> Sequences = new List<Sequence>();
        public List<Workflow> Workflows = new List<Workflow>();
        //public List<Template> Templates = new List<Template>();
        public List<PageTemplate> PageTemplates = new List<PageTemplate>();
        public List<SectionZoneLayout> SectionZoneLayouts = new List<SectionZoneLayout>();

        public List<WorkflowType> WorkflowTypes = new List<WorkflowType>();
        public List<EmailConnectionType> EmailConnectionTypes = new List<EmailConnectionType>();
        public List<EmailConnection> EmailConnections = new List<EmailConnection>();
        
        

        
        
        
        

        public string ConfigurationPath { get; set; }

        //TODO - removed from standard version but may need to be replaced
        //public CompilerResults GenerateConfigurationAssembly( string rootNamespace, List<string> outputLocations)
        //{
        //    return Compiler.GenerateConfigurationAssembly(ConfigurationPath, rootNamespace, outputLocations);
        //}

        

    }
}

