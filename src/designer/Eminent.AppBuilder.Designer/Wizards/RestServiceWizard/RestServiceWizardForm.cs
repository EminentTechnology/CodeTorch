using CodeTorch.AI.Abstractions;
using CodeTorch.AI.Models;
using CodeTorch.AI.Services;
using CodeTorch.Core;
using CodeTorch.Designer.Wizards.RestServiceWizard.Steps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace CodeTorch.Designer.Wizards.RestServiceWizard
{
    public partial class RestServiceWizardForm : BaseWizardForm
    {
        const int STEP_GENERAL = 0;
        const int STEP_OPTIONS = 1;
        const int STEP_EDIT_REST_SERVICE = 2;
        const int STEP_FINISH = 3;

        readonly RestServiceWizardState State = new RestServiceWizardState();

        readonly ICodeTorchAIService AIService;

        public RestServiceWizardForm()
        {
            AIService = new ProxyAIService();

            InitializeComponent();

            //IMPORTANT: This is required to ensure that the wizard is displayed correctly
            this.Pages = Wizard.Pages;
        }

        #region Standard Wizard Events
        private async void Wizard_Previous(object sender, WizardCancelEventArgs e)
        {
            await OnWizardPrevious(sender, e, State);
            Wizard.SelectedPage = CurrentPage;
        }

        private async void Wizard_Next(object sender, WizardCancelEventArgs e)
        {
            CurrentPage = Wizard.SelectedPage;
            await OnWizardNext(sender, e, State);
            Wizard.SelectedPage = CurrentPage;
        }

        private async void Wizard_Cancel(object sender, EventArgs e)
        {
            await OnWizardCancel(sender, e);
        }

        private async void Wizard_Finish(object sender, EventArgs e)
        {
            await OnWizardFinish(sender, e, State);
        }
        #endregion

        #region Required Wizard Overrides
        public override IStepHandler GetStepHandler(int stepIndex)
        {
            switch (stepIndex)
            {
                case STEP_GENERAL:
                    return new GeneralStepHandler(AIService);
                case STEP_OPTIONS:
                    return new OptionsStepHandler(AIService);
                case STEP_EDIT_REST_SERVICE:
                    return new EditRestServiceStepHandler();
                case STEP_FINISH:
                    return new FinishStepHandler();
                default:
                    return null;
            }
        }

        public override WizardPage GetNextPage()
        {
            WizardPage nextPage = null;

            if (CurrentPage == Wizard.Pages[STEP_GENERAL])
            {
                nextPage = Wizard.Pages[STEP_OPTIONS];
            }

            if (CurrentPage == Wizard.Pages[STEP_OPTIONS])
            {
                nextPage = Wizard.Pages[STEP_EDIT_REST_SERVICE];
            }

            if (CurrentPage == Wizard.Pages[STEP_EDIT_REST_SERVICE])
            {
                nextPage = Wizard.Pages[STEP_FINISH];
            }

            if (CurrentPage == Wizard.Pages[STEP_FINISH])
            {
                nextPage = null;
            }

            return nextPage;
        }
        #endregion

        public Task UpdateRestServiceHeaderDetailsWithUserInput()
        {
            State.RestService.Name = RestServiceNameTextbox.Text;
            State.RestService.Folder = RestServiceFolderTextbox.Text;
            State.RestService.Resource = RestServiceResourceTextbox.Text;
            State.RestService.Description = RestServiceDescriptionTextbox.Text;
            State.RestService.SupportJSON = SupportJSONCheckbox.IsChecked;
            State.RestService.SupportXML = SupportXMLCheckbox.IsChecked;
            return Task.CompletedTask;
        }

        public void UpdateSelectedRestService()
        {
            if (!String.IsNullOrWhiteSpace(ExistingRestServiceDropdown.Text))
            {
                var restService = State.RestServices.Find(x => x.Name.Equals(ExistingRestServiceDropdown.Text, StringComparison.OrdinalIgnoreCase));
                if (restService != null)
                {
                    UpdateRestServiceNameLabel.Text = restService.Name;
                    UpdateRestServiceResourceLabel.Text = restService.Resource;
                }
                else
                {
                    UpdateRestServiceNameLabel.Text = "";
                    UpdateRestServiceResourceLabel.Text = "";
                }
            }
            else
            {
                ExistingRestServiceDropdown.SelectedIndex = -1;
                UpdateRestServiceNameLabel.Text = "";
                UpdateRestServiceResourceLabel.Text = "";
            }
        }

        public void FillWithRestServices(RadMultiColumnComboBox list, List<RestService> restServices)
        {
            list.DataSource = null;
            list.DisplayMember = "Name";
            list.ValueMember = "Name";
            list.DataSource = restServices;
            list.Text = String.Empty;
        }

        public void FillFolderList(RadDropDownList list, List<string> folders)
        {
            list.DataSource = null;
            list.DataSource = folders;
            list.Text = String.Empty;
            list.SelectedIndex = -1;
        }

        public List<RestService> GetRestServicesList()
        {
            List<RestService> retVal = ObjectCopier.Clone<List<RestService>>(CodeTorch.Core.Configuration.GetInstance().RestServices);
            return retVal;
        }

        public List<string> GetRestServicesFolderList()
        {
            List<RestService> retVal = ObjectCopier.Clone<List<RestService>>(CodeTorch.Core.Configuration.GetInstance().RestServices);
            
            //get distinct folders as List<string>
            List<string> folders = retVal.Select(x => x.Folder).Distinct().ToList();
            
            return folders;
        }

        private void ExistingRestServiceDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSelectedRestService();
        }

        private void ExistingRestServiceRadioButton_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            ToggleRestServiceOptions();
        }

        private void CreateNewRestServiceRadioButton_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            ToggleRestServiceOptions();
        }

        private void ToggleRestServiceOptions()
        {
            ExistingRestServiceDropdown.Enabled = !CreateNewRestServiceRadioButton.IsChecked;
            State.RestService = null;
        }

        private void NameSuggestionsListView_SelectedItemChanged(object sender, EventArgs e)
        {
            var selectedItem = NameSuggestionsListView.SelectedItem?.DataBoundItem as NewRestServiceRecommendation;
            if(selectedItem != null)
            {
                RestServiceNameTextbox.Text = selectedItem.Name;
                RestServiceFolderTextbox.Text = selectedItem.Folder;
                RestServiceResourceTextbox.Text = selectedItem.Resource;
                RestServiceDescriptionTextbox.Text = selectedItem.Description;
                SupportJSONCheckbox.IsChecked = selectedItem.SupportJSON;
                SupportXMLCheckbox.IsChecked = selectedItem.SupportXML;
            }
        }
    }
}