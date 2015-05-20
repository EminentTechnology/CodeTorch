using CodeTorch.Core;
using CodeTorch.Designer.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeTorch.Designer.Forms
{
    public partial class ProjectConnections : Form
    {

        public Project Project { get; set; }

        public ProjectConnections()
        {
            InitializeComponent();
        }

        private void ProjectConnections_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateConnections();
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);

            }
        }

        private void ConnectionsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                SelectConnection();
            }
            catch (Exception ex)
            {
                ErrorManager.HandleError(ex);

            }
        }

        private void PopulateConnections()
        {

            if (this.Project != null)
            {
                List<DataConnection> remove = new List<DataConnection>();
                List<DataConnection> availableConnections = Configuration.GetInstance().DataConnections;

                //see if there are connections in project file that no longer exist in project
                foreach (DataConnection connection in this.Project.Connections)
                {
                    //if conneciton in project file no longer exists in project then remove it
                    if (!availableConnections.Exists(x => x.Name == connection.Name))
                    {
                        remove.Add(connection);
                    }

                    //if connection in project file exists in project but connection type has changed then lets remove it
                    if (availableConnections.Exists
                        (
                            x => 
                            ((x.Name == connection.Name) && (x.DataConnectionType != connection.DataConnectionType))
                        )
                    )
                    {
                        remove.Add(connection);
                    }
                    
                }

                //remove missing connections from project file
                foreach (DataConnection connection in remove)
                {
                    Project.Connections.Remove(connection);
                }


                //loop through data command parameters
                foreach (DataConnection connection in availableConnections)
                {

                    if (!this.Project.Connections.Exists(x => x.Name == connection.Name))
                    {
                        //add the missing connection
                        DataConnection copy = ObjectCopier.Clone<DataConnection>(connection);
                        Project.Connections.Add(copy);

                    }
                }

                FillConnectionsList();

            }
        }

        private void FillConnectionsList()
        {
        

            this.ConnectionsList.DisplayMember = "Name";
            this.ConnectionsList.ValueMember = "Name";
            this.ConnectionsList.DataSource = Project.Connections;
            this.ConnectionsList.Refresh();
        }

        private void SelectConnection()
        {
            if (this.ConnectionsList.SelectedItem != null)
            {
                DataConnection item = (DataConnection)ConnectionsList.SelectedItem;
                this.PropertyGrid.Enabled = true;
                this.PropertyGrid.SelectedObject = item;
                this.DataConnectionLabel.Text = String.Format("Connection - {0}:", item.Name);
            }
            else
            {
                this.PropertyGrid.Enabled = false;
                this.PropertyGrid.SelectedObject = null;
                this.DataConnectionLabel.Text = String.Empty;
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        
    }
}
