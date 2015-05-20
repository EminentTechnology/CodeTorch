using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CodeTorch.Core;


namespace CodeTorch.Core.Design
{
    public partial class ScreenDataCommandCollectionForm : Form
    {

        List<ScreenDataCommand> _DataCommands = new List<ScreenDataCommand>();

        public List<ScreenDataCommand> DataCommands
        {
            get { return _DataCommands; }
            set { _DataCommands = value; }
        }


        public ScreenDataCommandCollectionForm()
        {
            InitializeComponent();
        }

        private void ScreenDataCommandCollectionForm_Load(object sender, EventArgs e)
        {

               //fill both lists
                FillLists();
            
        }

        

        private void AddDataCommand_Click(object sender, EventArgs e)
        {

            if (AvailableCommandList.SelectedIndex < 0)
            {
                MessageBox.Show("Please select an available data command to move");
                return;
            }

            DataCommand command = (DataCommand)AvailableCommandList.SelectedItem;
            ScreenDataCommand c = new ScreenDataCommand();
            List<ScreenDataCommandParameter> parameters = new List<ScreenDataCommandParameter>();

            c.Name = command.Name;

            foreach (DataCommandParameter parameter in command.Parameters)
            {
                ScreenDataCommandParameter p = new ScreenDataCommandParameter();

                p.InputType = ScreenInputType.Control;
                p.Name = parameter.Name;
                p.InputKey = parameter.Name.Replace("@", "").Replace("'", "").Replace(" ", "");

                c.Parameters.Add(p);

            }

            DataCommands.Add(c);
        

            FillLists();

 
        }

        private void RemoveDataCommand_Click(object sender, EventArgs e)
        {
   
            if (this.PageCommandList.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a data command tied to this page");
                return;
            }

            ScreenDataCommand item = (ScreenDataCommand)PageCommandList.SelectedItem;
            DataCommands.Remove(item);

            FillLists();

        }


        //private void Close_Click(object sender, EventArgs e)
        //{
   
        //        this.DialogResult = DialogResult.OK;
          
        //}

  
        

        private void PageCommandList_SelectedIndexChanged(object sender, EventArgs e)
        {
  
                DisableParameterGrid();

                ScreenDataCommand item = (ScreenDataCommand)PageCommandList.SelectedItem;

                RefreshDataCommandParameters(item);

                FillParameterList(item.Parameters);
                this.SelectedDataCommandLabel.Text = this.PageCommandList.Text + " Parameter(s)";

                EnableParameterGrid();
          
        }

        private void RefreshDataCommandParameters(ScreenDataCommand item)
        {
            //get data command
            DataCommand c = DataCommand.GetDataCommand(item.Name);

            if (c != null)
            {

                //loop through screen data command parameters
                List<ScreenDataCommandParameter> remove = new List<ScreenDataCommandParameter>();
                foreach (ScreenDataCommandParameter p in item.Parameters)
                {
                    ScreenDataCommandParameter localP = p;
                    if (!c.Parameters.Exists(x => x.Name == localP.Name))
                    {
                        remove.Add(localP);
                    }
                }

                //are there any missing from data command - if so remove
                foreach(ScreenDataCommandParameter p in remove)
                {
                    item.Parameters.Remove(p);
                }


                //loop through data command parameters
                foreach (DataCommandParameter p in c.Parameters)
                {
                    DataCommandParameter localP = p;
                    if (!item.Parameters.Exists(x => x.Name == localP.Name))
                    {
                        //are there any missing from screen command parameters - if so add with control as default

                        ScreenDataCommandParameter sp = new ScreenDataCommandParameter();

                        sp.InputType = ScreenInputType.Control;
                        sp.Name = p.Name;
                        sp.InputKey = p.Name.Replace("@", "").Replace("'", "").Replace(" ", "");

                        item.Parameters.Add(sp);
                   
                    }
                }
                

            }
        }

        private void FillParameterList(List<ScreenDataCommandParameter> items)
        {
            this.ParameterList.DisplayMember = "Name";
            this.ParameterList.ValueMember = "Name";
            this.ParameterList.DataSource = items;
            this.ParameterList.Refresh();
        }

        private void ParameterList_SelectedIndexChanged(object sender, EventArgs e)
        {

            ScreenDataCommandParameter item = (ScreenDataCommandParameter)ParameterList.SelectedItem;
            parameterPropertyGrid.SelectedObject = item;
            SelectedParameterLabel.Text = item.Name + " " + "Parameter";
        }

        

        private void FillLists()
        {
            FillAvailableDataCommandsList();
            FillPageDataCommandsList();
        }

        private void FillAvailableDataCommandsList()
        {
            var retVal = from availableCommand in Configuration.GetInstance().DataCommands
                         join selectedCommand in DataCommands
                            on availableCommand.Name equals selectedCommand.Name into c
                         from selectedCommand in c.DefaultIfEmpty()
                         where selectedCommand == null
                         select availableCommand;

            this.AvailableCommandList.DisplayMember = "Name";
            this.AvailableCommandList.ValueMember = "Name";
            this.AvailableCommandList.DataSource = retVal.ToList<DataCommand>();
            this.AvailableCommandList.Refresh();
        }

        private void FillPageDataCommandsList()
        {
            var retVal = from item in DataCommands
                         select item;

            this.PageCommandList.DisplayMember = "Name";
            this.PageCommandList.ValueMember = "Name";
            this.PageCommandList.DataSource = retVal.ToList<ScreenDataCommand>();
            this.PageCommandList.Refresh();
        }


        

        

        private void EnableParameterGrid()
        {
            
            this.parameterPropertyGrid.Enabled = true;

        }

        private void DisableParameterGrid()
        {
            this.SelectedParameterLabel.Text = String.Empty;
            this.parameterPropertyGrid.Enabled = false;
            this.parameterPropertyGrid.SelectedObject = null;
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        

       

      
    }
}
