using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CodeTorch.Core;

namespace CodeTorch.Designer.Forms
{
    public partial class NewItemDialog : Form
    {
        public string ItemType;
        public string ItemDisplayName;

        public NewItemDialog()
        {
            InitializeComponent();
        }

        private bool ValidateSaveRequest()
        {
            bool IsErrorPresent = false;
            StringBuilder errors = new StringBuilder();

            ValidateRequiredField(ItemName, ItemDisplayName, ref IsErrorPresent, errors);

            if (ItemName.Text.Trim() != String.Empty)
            {
                ValidateExistingControl(ItemName, ItemName.Text, ref IsErrorPresent, errors);
            }

            if (IsErrorPresent)
            {
                MessageBox.Show("The following error(s) occurred:\n\n" + errors.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            return (!IsErrorPresent);

        }

        private void ValidateExistingControl(Control ctrl, string Caption, ref bool IsErrorPresent, StringBuilder errors)
        {
            int count = 0;

            switch (ItemType.ToLower())
            {
                case "dataconnection":
                    count = GetDataConnectionCountByName();
                    break;
                case "dataconnectiontype":
                    count = GetDataConnectionTypeCountByName();
                    break;
                case "documentrepository":
                    count = GetDocumentRepositoryCountByName();
                    break;
                case "documentrepositorytype":
                    count = GetDocumentRepositoryTypeCountByName();
                    break;
                case "emailconnection":
                    count = GetEmailConnectionCountByName();
                    break;
                case "emailconnectiontype":
                    count = GetEmailConnectionTypeCountByName();
                    break;
                case "lookup":
                    count = GetLookupCountByName();
                    break;
                case "pagetemplate":
                    count = PageTemplate.GetItemCount(ItemName.Text);
                    break;
                case "picker":
                    count = GetPickerCountByName();
                    break;
                case "restservice":
                    count = GetRestServiceCountByName();
                    break;
                case "screentype":
                    count = GetScreenTypeCountByName();
                    break;
                case "sectiontype":
                    count = GetSectionTypeCountByName();
                    break;
                case "sectionzonelayout":
                    count = GetSectionZoneLayoutCountByName();
                    break;
                case "sequence":
                    count = GetSequenceCountByName();
                    break;
                case "template":
                    count = Template.GetItemCount(ItemName.Text);
                    break;
                case "workflow":
                    count = GetWorkflowCountByName();
                    break;
                case "workflowtype":
                    count = GetWorkflowTypeCountByName();
                    break;
            }

            if (count > 0)
            {
                IsErrorPresent = true;
                string errorMessage = String.Format("{0} {1} already exists.", ItemDisplayName, Caption);
                errorProvider.SetError(ctrl, errorMessage);
                errors.AppendLine(errorMessage);
            }
            else
            {
                errorProvider.SetError(ctrl, String.Empty);
            }
        }

        

        private void ValidateRequiredField(Control ctrl, string Caption, ref bool IsErrorPresent, StringBuilder errors)
        {
            if (ctrl.Text.Trim() == String.Empty)
            {
                IsErrorPresent = true;
                string errorMessage = String.Format("{0} is required", Caption);
                errorProvider.SetError(ctrl, errorMessage);
                errors.AppendLine(errorMessage);
            }
            else
            {
                errorProvider.SetError(ctrl, String.Empty);
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateSaveRequest())
                {
                    switch (ItemType.ToLower())
                    {
                        case "dataconnection":
                            SaveDataConnection();
                            break;
                        case "dataconnectiontype":
                            SaveDataConnectionType();
                            break;
                        case "documentrepository":
                            SaveDocumentRepository();
                            break;
                        case "documentrepositorytype":
                            SaveDocumentRepositoryType();
                            break;
                        case "emailconnection":
                            SaveEmailConnection();
                            break;
                        case "emailconnectiontype":
                            SaveEmailConnectionType();
                            break;
                        case "lookup":
                            SaveLookup();
                            break;
                        case "pagetemplate":
                            SavePageTemplate();
                            break;
                        case "picker":
                            SavePicker();
                            break;
                        case "restservice":
                            SaveRestService();
                            break;
                        case "screentype":
                            SaveScreenType();
                            break;
                        case "sectiontype":
                            SaveSectionType();
                            break;
                        case "sectionzonelayout":
                            SaveSectionZoneLayout();
                            break;
                        case "sequence":
                            SaveSequence();
                            break;
                        case "template":
                            SaveTemplate();
                            break;
                        case "workflow":
                            SaveWorkflow();
                            break;
                        case "workflowtype":
                            SaveWorkflowType();
                            break;
                    } 
                    
                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
        }

        

        private void NewItemDialog_Load(object sender, EventArgs e)
        {
            NameLabel.Text = ItemDisplayName + ":";
        }



        private int GetDataConnectionCountByName()
        {
            var retVal = from item in Configuration.GetInstance().DataConnections
                         where item.Name.ToLower() == ItemName.Text.ToLower()
                         select item;
            return retVal.Count<DataConnection>();
        }

        private int GetDataConnectionTypeCountByName()
        {
            var retVal = from item in Configuration.GetInstance().DataConnectionTypes
                         where item.Name.ToLower() == ItemName.Text.ToLower()
                         select item;
            return retVal.Count<DataConnectionType>();
        }

        private int GetDocumentRepositoryCountByName()
        {
            var retVal = from item in Configuration.GetInstance().DocumentRepositories
                         where item.Name.ToLower() == ItemName.Text.ToLower()
                         select item;
            return retVal.Count<DocumentRepository>();
        }

        //
        private int GetDocumentRepositoryTypeCountByName()
        {
            var retVal = from item in Configuration.GetInstance().DocumentRepositoryTypes
                         where item.Name.ToLower() == ItemName.Text.ToLower()
                         select item;
            return retVal.Count<DocumentRepositoryType>();
        }

        private int GetEmailConnectionCountByName()
        {
            var retVal = from item in Configuration.GetInstance().EmailConnections
                         where item.Name.ToLower() == ItemName.Text.ToLower()
                         select item;
            return retVal.Count<EmailConnection>();
        }

        private int GetEmailConnectionTypeCountByName()
        {
            var retVal = from item in Configuration.GetInstance().EmailConnectionTypes
                         where item.Name.ToLower() == ItemName.Text.ToLower()
                         select item;
            return retVal.Count<EmailConnectionType>();
        }
       

        private void SaveDataConnection()
        {
            DataConnection item = new DataConnection();

            item.Name = ItemName.Text;

            Configuration.GetInstance().DataConnections.Add(item);
            DataConnection.Save(item);

        }

        private void SaveDataConnectionType()
        {
            DataConnectionType item = new DataConnectionType();

            item.Name = ItemName.Text;

            Configuration.GetInstance().DataConnectionTypes.Add(item);
            DataConnectionType.Save(item);

        }

        private void SaveEmailConnection()
        {
            EmailConnection item = new EmailConnection();

            item.Name = ItemName.Text;

            Configuration.GetInstance().EmailConnections.Add(item);
            EmailConnection.Save(item);

        }

        private void SaveEmailConnectionType()
        {
            EmailConnectionType item = new EmailConnectionType();

            item.Name = ItemName.Text;

            Configuration.GetInstance().EmailConnectionTypes.Add(item);
            EmailConnectionType.Save(item);

        }

        private void SaveDocumentRepository()
        {
            DocumentRepository item = new DocumentRepository();

            item.Name = ItemName.Text;

            Configuration.GetInstance().DocumentRepositories.Add(item);
            DocumentRepository.Save(item);

        }

        private void SaveDocumentRepositoryType()
        {
            DocumentRepositoryType item = new DocumentRepositoryType();

            item.Name = ItemName.Text;

            Configuration.GetInstance().DocumentRepositoryTypes.Add(item);
            DocumentRepositoryType.Save(item);

        }

      

        private int GetLookupCountByName()
        {
            var retVal = from item in Configuration.GetInstance().Lookups
                         where item.Name.ToLower() == ItemName.Text.ToLower()
                         select item;
            return retVal.Count<Lookup>();
        }

        private void SaveLookup()
        {
            Lookup item = new Lookup();

            item.Name = ItemName.Text;

            Configuration.GetInstance().Lookups.Add(item);
            Lookup.Save(item);

        }

        private int GetPickerCountByName()
        {
            var retVal = from item in Configuration.GetInstance().Pickers
                         where item.Name.ToLower() == ItemName.Text.ToLower()
                         select item;
            return retVal.Count<Picker>();
        }

        private int GetRestServiceCountByName()
        {
            var retVal = from item in Configuration.GetInstance().RestServices
                         where item.Name.ToLower() == ItemName.Text.ToLower()
                         select item;
            return retVal.Count<RestService>();
        }

        private void SavePicker()
        {
            Picker item = new Picker();

            item.Name = ItemName.Text;

            Configuration.GetInstance().Pickers.Add(item);
            Picker.Save(item);

        }

        private void SaveRestService()
        {
            RestService item = new RestService();

            item.Name = ItemName.Text;

            Configuration.GetInstance().RestServices.Add(item);
            RestService.Save(item);

        }

        private int GetScreenTypeCountByName()
        {
            var retVal = from item in Configuration.GetInstance().ScreenTypes
                         where item.Name.ToLower() == ItemName.Text.ToLower()
                         select item;
            return retVal.Count<ScreenType>();
        }

        private void SaveScreenType()
        {
            ScreenType item = new ScreenType();

            item.Name = ItemName.Text;

            Configuration.GetInstance().ScreenTypes.Add(item);
            ScreenType.Save(item);

        }

        private int GetSectionTypeCountByName()
        {
            var retVal = from item in Configuration.GetInstance().SectionTypes
                         where item.Name.ToLower() == ItemName.Text.ToLower()
                         select item;
            return retVal.Count<SectionType>();
        }

        private int GetSectionZoneLayoutCountByName()
        {
            var retVal = from item in Configuration.GetInstance().SectionZoneLayouts
                         where item.Name.ToLower() == ItemName.Text.ToLower()
                         select item;
            return retVal.Count<SectionZoneLayout>();
        }


        private void SaveSectionType()
        {
            SectionType item = new SectionType();

            item.Name = ItemName.Text;

            Configuration.GetInstance().SectionTypes.Add(item);
            SectionType.Save(item);

        }

        private void SaveSectionZoneLayout()
        {
            SectionZoneLayout item = new SectionZoneLayout();

            item.Name = ItemName.Text;

            Configuration.GetInstance().SectionZoneLayouts.Add(item);
            SectionZoneLayout.Save(item);

        }

        private int GetSequenceCountByName()
        {
            var retVal = from item in Configuration.GetInstance().Sequences
                         where item.Name.ToLower() == ItemName.Text.ToLower()
                         select item;
            return retVal.Count<Sequence>();
        }

        private void SaveSequence()
        {
            Sequence item = new Sequence();

            item.Name = ItemName.Text;

            Configuration.GetInstance().Sequences.Add(item);
            Sequence.Save(item);

        }

        private void SavePageTemplate()
        {
            PageTemplate item = new PageTemplate();

            item.Name = ItemName.Text;

            Configuration.GetInstance().PageTemplates.Add(item);
            PageTemplate.Save(item);

        }

        private int GetWorkflowCountByName()
        {
            var retVal = from item in Configuration.GetInstance().Workflows
                         where item.Name.ToLower() == ItemName.Text.ToLower()
                         select item;
            return retVal.Count<Workflow>();
        }

        private int GetWorkflowTypeCountByName()
        {
            var retVal = from item in Configuration.GetInstance().WorkflowTypes
                         where item.Name.ToLower() == ItemName.Text.ToLower()
                         select item;
            return retVal.Count<WorkflowType>();
        }

        private void SaveWorkflow()
        {
            Workflow item = new Workflow();

            item.Code = ItemName.Text;
            item.Name = ItemName.Text;
            item.EntityName =  ItemName.Text;

            Configuration.GetInstance().Workflows.Add(item);
            Workflow.Save(item);

        }

        private void SaveWorkflowType()
        {
            WorkflowType item = new WorkflowType();


            item.Name = ItemName.Text;


            Configuration.GetInstance().WorkflowTypes.Add(item);
            WorkflowType.Save(item);

        }

        private void SaveTemplate()
        {
            Template item = new Template();

            item.Name = ItemName.Text;

            Configuration.GetInstance().Templates.Add(item);
            Template.Save(item);

        }

        


    }
}
