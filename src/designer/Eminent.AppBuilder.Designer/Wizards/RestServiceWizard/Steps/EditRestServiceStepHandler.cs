using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using static CodeTorch.Designer.Wizards.RestServiceWizard.RestServiceWizardForm;

namespace CodeTorch.Designer.Wizards.RestServiceWizard.Steps
{
    public class EditRestServiceStepHandler: IStepHandler
    {
        public async Task<bool> OnPreviousClick(BaseWizardForm wizardForm, object stateObject)
        {
            bool retVal = true;

            var form = wizardForm as RestServiceWizardForm;

            if (form != null)
            {
                await form.UpdateRestServiceHeaderDetailsWithUserInput();
            }

            return retVal;
        }

        public Task<bool> OnPageLoad(BaseWizardForm wizardForm, object stateObject)
        {
            var retVal = true;
            var form = wizardForm as RestServiceWizardForm;
            var State = stateObject as RestServiceWizardState;

            if (form == null || State == null)
            {
                return Task.FromResult(false);
            }

            var folders = form.GetRestServicesFolderList();
            form.FillFolderList(form.RestServiceFolderTextbox, folders);

            if (State.RestService != null)
            {
                form.RestServiceNameTextbox.Text = State.RestService.Name;
                form.RestServiceFolderTextbox.Text = State.RestService.Folder;
                form.RestServiceResourceTextbox.Text = State.RestService.Resource;
                form.RestServiceDescriptionTextbox.Text = State.RestService.Description;
                form.SupportJSONCheckbox.IsChecked = State.RestService.SupportJSON;
                form.SupportXMLCheckbox.IsChecked = State.RestService.SupportXML;
            }

            if (State.CreateNewService && (State.Analysis?.Recommendations?.Count ?? 0) > 0)
            {
                form.NameSuggestionsListView.DataSource = State.Analysis.Recommendations;
                form.NameSuggestionsListView.SelectedIndex = -1;
                form.SuggestionsLabel.Visible = true;
                form.NameSuggestionsListView.Visible = true;
            }
            else
            {
                form.SuggestionsLabel.Visible = false;
                form.NameSuggestionsListView.Visible = false;
                form.NameSuggestionsListView.Items.Clear();
                form.NameSuggestionsListView.DataSource = null;
            }

            return Task.FromResult(retVal);
        }

        public Task<bool> PerformValidation(BaseWizardForm wizardForm, object stateObject)
        {
            var form = wizardForm as RestServiceWizardForm;
            var State = stateObject as RestServiceWizardState;
            var failed = Task.FromResult(false);

            if (form != null && State != null)
            {
                if (string.IsNullOrWhiteSpace(form.RestServiceNameTextbox.Text))
                {
                    MessageBox.Show("Please enter a valid name for the rest service");
                    return failed;
                }

                if (string.IsNullOrWhiteSpace(form.RestServiceFolderTextbox.Text))
                {
                    MessageBox.Show("Please enter a valid folder for the rest service");
                    return failed;
                }

                if (string.IsNullOrWhiteSpace(form.RestServiceResourceTextbox.Text))
                {
                    MessageBox.Show("Please enter a valid resource for the rest service");
                    return failed;
                }

                if (string.IsNullOrWhiteSpace(form.RestServiceDescriptionTextbox.Text))
                {
                    MessageBox.Show("Please enter a valid description for the rest service");
                    return failed;
                }

                if (!form.SupportJSONCheckbox.IsChecked && !form.SupportXMLCheckbox.IsChecked)
                {
                    MessageBox.Show("Please select at least one format for the rest service");
                    return failed;
                }

                //check if rest service name already exists - only if we are creating a new rest service
                if (State.CreateNewService && State.RestServices.Exists(x => x.Name.Equals(form.RestServiceNameTextbox.Text, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("Rest service with the same name already exists. Please enter a different name");
                    return failed;
                }
            }

            return Task.FromResult(true);
        }

        public async Task<bool> OnNextClick(BaseWizardForm wizardForm, object stateObject)
        {
            bool retVal = true;

            var form = wizardForm as RestServiceWizardForm;

            if (form != null)
            {
                await form.UpdateRestServiceHeaderDetailsWithUserInput();
            }

            return retVal;
        }
    }
}
