using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CodeTorch.Core.Design
{
    public partial class ScreenDataCommandCollectionForm : Form
    {
        List<ScreenDataCommand> _DataCommands = new List<ScreenDataCommand>();

        public object Instance { get; set; }
        public string InstancePropertyName { get; set; }

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
                ScreenDataCommandParameter p = DetermineLikelyScreenInputType(parameter);

                c.Parameters.Add(p);
            }

            DataCommands.Add(c);
        
            FillLists();

            if (Instance != null && Instance is BaseRestServiceMethod)
            {
                if(Instance is GetRestServiceMethod)
                {
                    var getRestServiceMethod = (GetRestServiceMethod)Instance;  
                    if(String.IsNullOrEmpty(getRestServiceMethod.RequestDataCommand))
                    {
                        getRestServiceMethod.RequestDataCommand = c.Name;
                    }
                }

                if (Instance is PostRestServiceMethod)
                {
                    var postRestServiceMethod = (PostRestServiceMethod)Instance;

                    if (command.ReturnType == DataCommandReturnType.Integer)
                    {
                        if (String.IsNullOrEmpty(postRestServiceMethod.RequestDataCommand))
                        {
                            postRestServiceMethod.RequestDataCommand = c.Name;
                        }
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(postRestServiceMethod.ResponseDataCommand))
                        {
                            postRestServiceMethod.ResponseDataCommand = c.Name;
                        }
                    }
                }

                if (Instance is PutRestServiceMethod)
                {
                    var putRestServiceMethod = (PutRestServiceMethod)Instance;

                    if (command.ReturnType == DataCommandReturnType.Integer)
                    {
                        if (String.IsNullOrEmpty(putRestServiceMethod.RequestDataCommand))
                        {
                            putRestServiceMethod.RequestDataCommand = c.Name;
                        }
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(putRestServiceMethod.ResponseDataCommand))
                        {
                            putRestServiceMethod.ResponseDataCommand = c.Name;
                        }
                    }
                }

                if (Instance is DeleteRestServiceMethod)
                {
                    var deleteRestServiceMethod = (DeleteRestServiceMethod)Instance;
                    if (String.IsNullOrEmpty(deleteRestServiceMethod.RequestDataCommand))
                    {
                        deleteRestServiceMethod.RequestDataCommand = c.Name;
                    }
                }
            }
        }

        private ScreenDataCommandParameter DetermineLikelyScreenInputType(DataCommandParameter parameter)
        {
            ScreenDataCommandParameter p = new ScreenDataCommandParameter();

            p.Name = parameter.Name;
            p.InputKey = parameter.Name.Replace("@", "").Replace("'", "").Replace(" ", "");
            p.InputType = ScreenInputType.Control;

            if(Instance == null)
            {
                return p;
            }

            if (Instance is BaseRestServiceMethod)
            {
                return DetermineLikelyScreenInputTypeForRestServices(parameter, p) ?? p;
            }

           return p;
        }

        private ScreenDataCommandParameter DetermineLikelyScreenInputTypeForRestServices(DataCommandParameter parameter, ScreenDataCommandParameter p)
        {
            if (Instance == null || !(Instance is BaseRestServiceMethod))
            {
                return p;
            }

            var methodInstance = Instance as BaseRestServiceMethod;
            if (methodInstance.ParentService != null &&
                methodInstance.ParentService.Resource != null &&
                methodInstance.ParentService.Resource.IndexOf("{" + p.InputKey + "}", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                p.InputType = ScreenInputType.Special;
                p.Default = p.InputKey;
                p.InputKey = "UrlSegment";
                
                return p;
            }

            // Set input type based on REST service method type
            if (Instance is GetRestServiceMethod || Instance is DeleteRestServiceMethod)
            {
                p.InputType = ScreenInputType.QueryString;
            }
            else
            {
                p.InputType = ScreenInputType.Form;
            }

            var defaultInputType = p.InputType;
            var restServiceParameters = new List<RestServiceMethodParameterInput>();

            // Collect all parameters from all REST services into a list
            foreach (var restService in Configuration.GetInstance().RestServices)
            {
                foreach (var method in restService.Methods)
                {
                    foreach (var dataCommand in method.DataCommands)
                    {
                        restServiceParameters.AddRange(dataCommand.Parameters.Select(dataCommandParameter => new RestServiceMethodParameterInput
                        {
                            RestServiceName = restService.Name,
                            MethodName = method.GetType().Name,
                            DataCommandName = dataCommand.Name,
                            ParameterName = dataCommandParameter.Name,
                            InputType = dataCommandParameter.InputType,
                            InputKey = dataCommandParameter.InputKey,
                            InputDefault = dataCommandParameter.Default
                        }));
                    }
                }
            }

            // Determine high frequency input type and key, considering method specificity first
            var highFrequencyInputBasedOnMethodType = GetHighFrequencyInput(p.Name, ((BaseRestServiceMethod)Instance).GetType().Name, restServiceParameters);
            var highFrequencyInput = GetHighFrequencyInput(p.Name, null, restServiceParameters);

            // Prefer the specific method's high frequency input if it exists with more than one record
            var selectedHighFrequencyInput = (highFrequencyInputBasedOnMethodType != null && highFrequencyInputBasedOnMethodType.Count() > 1)
                ? highFrequencyInputBasedOnMethodType : highFrequencyInput;

            // Update parameter if a suitable high frequency input type and key are found
            if (selectedHighFrequencyInput?.Count() > 1 && selectedHighFrequencyInput.Key.InputType != defaultInputType)
            {
                var item = selectedHighFrequencyInput.First();
                p.InputType = item.InputType;
                p.InputKey = item.InputKey;
                p.Default = item.InputDefault;
            }

            if(Instance is GetRestServiceMethod)
            {
                if(p.InputType == ScreenInputType.Form)
                {
                    p.InputType = ScreenInputType.QueryString;
                }
            }

            return p;
        }

        // Helper method to extract high frequency input based on the parameter name and optional method name
        private static IGrouping<dynamic, RestServiceMethodParameterInput> GetHighFrequencyInput(string parameterName, string methodName, List<RestServiceMethodParameterInput> parameters)
        {
            return parameters
                .Where(x => x.ParameterName == parameterName && (methodName == null || x.MethodName.Equals(methodName, StringComparison.OrdinalIgnoreCase)))
                // Exclude URL segment input type from high frequency input type
                .Where(x => !x.InputKey.Equals("UrlSegment", StringComparison.OrdinalIgnoreCase))
                .GroupBy(x => new { x.InputType, x.InputKey })
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();
        }

        public class RestServiceMethodParameterInput
        {
            public string RestServiceName { get; set; }
            public string MethodName { get; set; }
            public string DataCommandName { get; set; }
            public string ParameterName { get; set; }

            public ScreenInputType InputType { get; set; }
            public string InputKey { get; set; }
            public string InputDefault { get; set; }
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
                        //are there any missing from screen command parameters - if so add them
                        ScreenDataCommandParameter sp = DetermineLikelyScreenInputType(p);
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
