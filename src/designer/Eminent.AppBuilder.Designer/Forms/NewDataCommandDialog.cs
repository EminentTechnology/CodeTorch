using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CodeTorch.Core.Services;
using CodeTorch.Core;
using CodeTorch.Core.Interfaces;
using CodeTorch.Designer.Code;

namespace CodeTorch.Designer.Forms
{
    public partial class NewDataCommandDialog : Form
    {
        public Project Project = new Project();
        
        public DataCommandService dataCommandDB;
      

        public string DataCommandID = Guid.Empty.ToString();

        public NewDataCommandDialog()
        {
            InitializeComponent();

            dataCommandDB = DataCommandService.GetInstance();
        }

        private void DataCommandDialog_Load(object sender, EventArgs e)
        {
            try
            {
                //dataCommandDB.ConnectionInfo = Connection;

                PopulateDataConnection();

            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
        }

        private void PopulateDataConnection()
        {
            DataConnectionList.DataSource = null;
            DataConnectionList.DataSource = Core.Configuration.GetInstance().DataConnections;
            DataConnectionList.DisplayMember = "Name";
            DataConnectionList.ValueMember = "Name";
            DataConnectionList.Text = "";
        }

        private void PopulateCommandTypes()
        {
            string connection = null;
            CommandTypeList.DataSource = null;

            if (DataConnectionList.SelectedItem != null)
            {
                connection = ((DataConnection)DataConnectionList.SelectedItem).Name;
            }
            

            if(!String.IsNullOrEmpty(connection))
            {
                DataConnection c = DataConnection.GetByName(connection);
                if (c != null)
                {
                    DataConnectionType ct = c.GetConnectionType();

                    if (ct != null)
                    {
                        CommandTypeList.DataSource = ct.CommandTypes;
                    }
                }
            }

            CommandTypeList.Text = "";
        }

        private void PopulateScreen(DataCommand Data)
        {
            this.CommandName.Text = Data.Name;
            this.CommandTypeList.Text = Data.Type.ToString();
            this.CommandText.Text = Data.Text;
            this.CommandReturnTypeList.Text = Data.ReturnType.ToString();
        }

        private void Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateSaveRequest())
                {

                    DataCommand cmd = new DataCommand();

                    cmd.Name = CommandName.Text;
                    cmd.DataConnection = DataConnectionList.Text;
                    cmd.Type = CommandTypeList.Text.ToString();
                    cmd.Text = CommandText.Text;
                    cmd.ReturnType = (DataCommandReturnType)Enum.Parse(typeof(DataCommandReturnType), CommandReturnTypeList.Text.ToString());

                    DataConnection connection = Project.GetDataConnection(cmd);
                    IDataCommandProvider DataSource = DataCommandService.GetInstance().GetProvider(connection);
                    DataSource.RefreshSchema(connection, cmd);
                    
                    DataCommand.Save(cmd);

                    Core.Configuration.GetInstance().DataCommands.Add(cmd);
                               
              
                    this.DialogResult = DialogResult.OK;
                }
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {

        }

        private bool ValidateSaveRequest()
        {
            bool IsErrorPresent = false;
            StringBuilder errors = new StringBuilder();

            ValidateRequiredField(CommandName, "Command Name", ref IsErrorPresent, errors);
            ValidateRequiredField(CommandTypeList, "Command Type", ref IsErrorPresent, errors);
            ValidateRequiredField(CommandReturnTypeList, "Return Type", ref IsErrorPresent, errors);
            ValidateRequiredField(CommandText, "Command Text", ref IsErrorPresent, errors);

            if (CommandName.Text.Trim() != String.Empty)
            {
                ValidateExistingDataCommand(CommandName, CommandName.Text, ref IsErrorPresent, errors);
            }

            if (IsErrorPresent)
            {
                MessageBox.Show("The following error(s) occurred:\n\n" + errors.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            return (!IsErrorPresent);

        }

        private void ValidateExistingDataCommand(Control ctrl, string Caption, ref bool IsErrorPresent, StringBuilder errors)
        {
            var retVal = from cmd in Core.Configuration.GetInstance().DataCommands
                         where cmd.Name.ToLower() == ctrl.Text.ToLower()
                         select cmd;

            



          
                if (retVal.Count<DataCommand>() > 0)
                {
                    IsErrorPresent = true;
                    string errorMessage = String.Format("Data Command {0} already exists.", Caption);
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

        private void DataConnectionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                PopulateCommandTypes();
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);
            }
        }


        
    }
}
