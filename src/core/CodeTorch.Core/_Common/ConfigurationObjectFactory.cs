using CodeTorch.Core.ConfigurationObjects;
using CodeTorch.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Core
{
    public static class ConfigurationObjectFactory
    {
        public const string ENTITY_TYPE_APP = "App";
        public const string ENTITY_TYPE_CONTROL_TYPE = "ControlType";

        

        public const string ENTITY_TYPE_DATA_CONNECTION = "DataConnection";
        public const string ENTITY_TYPE_DATA_CONNECTION_TYPE = "DataConnectionType";

        public const string ENTITY_TYPE_EMAIL_CONNECTION = "EmailConnection";
        public const string ENTITY_TYPE_EMAIL_CONNECTION_TYPE = "EmailConnectionType";

        public const string ENTITY_TYPE_DOCUMENT_REPOSITORY = "DocumentRepository";
        public const string ENTITY_TYPE_DOCUMENT_REPOSITORY_TYPE = "DocumentRepositoryType";

        
        public const string ENTITY_TYPE_DATA_COMMAND = "DataCommand";
        
        public const string ENTITY_TYPE_LOOKUP = "Lookup";
        public const string ENTITY_TYPE_MENU = "Menu";
        public const string ENTITY_TYPE_PAGE_TEMPLATE = "PageTemplate";
        public const string ENTITY_TYPE_PERMISSION = "Permission";
        public const string ENTITY_TYPE_PICKER = "Picker";
        public const string ENTITY_TYPE_SEQUENCE = "Sequence";
        public const string ENTITY_TYPE_TEMPLATE = "Template";
        public const string ENTITY_TYPE_WORKFLOW = "Workflow";
        public const string ENTITY_TYPE_WORKFLOW_TYPE = "WorkflowType";
        public const string ENTITY_TYPE_SCREEN_TYPE = "ScreenType";
        public const string ENTITY_TYPE_SCREEN = "Screen";
        public const string ENTITY_TYPE_SECTION_TYPE = "SectionType";
        public const string ENTITY_TYPE_SECTION_ZONE_LAYOUT = "SectionZoneLayout";
        public const string ENTITY_TYPE_REST_SERVICE = "RestService";

       

        private static Dictionary<string, Func<IConfigurationObject2>> configurationObjectMap = new Dictionary<string, Func<IConfigurationObject2>>()
        {
            {ENTITY_TYPE_APP, () => { return new AppConfigurationObject(); }},
            {ENTITY_TYPE_CONTROL_TYPE, () => { return new ControlTypeConfigurationObject(); }},

            {ENTITY_TYPE_DATA_COMMAND, () => { return new DataCommandConfigurationObject(); }},
            {ENTITY_TYPE_DATA_CONNECTION, () => { return new DataConnectionConfigurationObject(); }},
            {ENTITY_TYPE_DATA_CONNECTION_TYPE, () => { return new DataConnectionTypeConfigurationObject(); }},
            {ENTITY_TYPE_DOCUMENT_REPOSITORY, () => { return new DocumentRepositoryConfigurationObject(); }},
            {ENTITY_TYPE_DOCUMENT_REPOSITORY_TYPE, () => { return new DocumentRepositoryTypeConfigurationObject(); }},
            {ENTITY_TYPE_EMAIL_CONNECTION, () => { return new EmailConnectionConfigurationObject(); }},
            {ENTITY_TYPE_EMAIL_CONNECTION_TYPE, () => { return new EmailConnectionTypeConfigurationObject(); }},
            {ENTITY_TYPE_LOOKUP, () => { return new LookupConfigurationObject(); }},
            {ENTITY_TYPE_MENU, () => { return new MenuConfigurationObject(); }},
            {ENTITY_TYPE_PAGE_TEMPLATE, () => { return new PageTemplateConfigurationObject(); }},
            {ENTITY_TYPE_PERMISSION, () => { return new PermissionConfigurationObject(); }},
            {ENTITY_TYPE_PICKER, () => { return new PickerConfigurationObject(); }},
            {ENTITY_TYPE_REST_SERVICE, () => { return new RestServiceConfigurationObject(); }},
            {ENTITY_TYPE_SCREEN, () => { return new ScreenConfigurationObject(); }},
            {ENTITY_TYPE_SCREEN_TYPE, () => { return new ScreenTypeConfigurationObject(); }},
            {ENTITY_TYPE_SECTION_TYPE, () => { return new SectionTypeConfigurationObject(); }},
            {ENTITY_TYPE_SECTION_ZONE_LAYOUT, () => { return new SectionZoneLayoutConfigurationObject(); }},
            {ENTITY_TYPE_SEQUENCE, () => { return new SequenceConfigurationObject(); }},
           // {ENTITY_TYPE_TEMPLATE, () => { return new TemplateConfigurationObject(); }},
            {ENTITY_TYPE_WORKFLOW, () => { return new WorkflowConfigurationObject(); }},
            {ENTITY_TYPE_WORKFLOW_TYPE, () => { return new WorkflowTypeConfigurationObject(); }}
        };

        public static IConfigurationObject2 CreateConfigurationObject(string entityName)
        {
            return configurationObjectMap[entityName]();
        }

    }
}
