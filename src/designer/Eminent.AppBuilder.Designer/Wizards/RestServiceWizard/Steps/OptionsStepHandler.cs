using CodeTorch.AI.Abstractions;
using CodeTorch.AI.Models;
using CodeTorch.Core;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeTorch.Designer.Wizards.RestServiceWizard.Steps
{
    public class OptionsStepHandler: IStepHandler
    {
        readonly ICodeTorchAIService AIService;

        public OptionsStepHandler(ICodeTorchAIService AIService)
        {
            this.AIService = AIService;
        }

        public Task<bool> OnPreviousClick(BaseWizardForm wizardForm, object stateObject)
        {
            bool retVal = true;
            return Task.FromResult(retVal);
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

            if (State.Analysis != null)
            {
                // load all rest services into dropdown
                State.RestServices = form.GetRestServicesList();
                form.FillWithRestServices(form.ExistingRestServiceDropdown, State.RestServices);

                if (State.Analysis.Recommendations.Count > 0)
                {
                    NewRestServiceRecommendation recommendation = State.Analysis.Recommendations[0];
                    form.CreateNewRestServiceNameLabel.Text = recommendation.Name;
                    form.CreateNewRestServiceResourceLabel.Text = recommendation.Resource;
                }
                else
                {
                    form.CreateNewRestServiceNameLabel.Text = "TBD";
                    form.CreateNewRestServiceResourceLabel.Text = "TBD";
                }

                form.CreateNewRestServiceRadioButton.IsChecked = State.Analysis.RecommendNewRestService;
                form.ExistingRestServiceRadioButton.IsChecked = !State.Analysis.RecommendNewRestService;

                if (!String.IsNullOrEmpty(State.Analysis.ExistingRestService))
                {
                    if (State.RestServices.Exists(x => x.Name == State.Analysis.ExistingRestService))
                    {
                        form.ExistingRestServiceDropdown.Text = State.Analysis.ExistingRestService;
                    }
                    else
                    {
                        form.ExistingRestServiceDropdown.Text = String.Empty;
                    }
                }
                else
                {
                    form.ExistingRestServiceDropdown.Text = String.Empty;
                }

                form.UpdateSelectedRestService();

                form.AnalysisReasonLabel.Text = State.Analysis.Reason;
            }

            return Task.FromResult(retVal);
        }

        public async Task<bool> OnNextClick(BaseWizardForm wizardForm, object stateObject)
        {
            var retVal = true;
            var form = wizardForm as RestServiceWizardForm;
            var State = stateObject as RestServiceWizardState;

            if (form == null || State == null)
            {
                return false;
            }

            if (form.CreateNewRestServiceRadioButton.IsChecked)
            {
                if (State.RestService == null)
                {
                    if (State.Analysis.Recommendations.Count > 0)
                    {
                        State.CreateNewService = true;
                        NewRestServiceRecommendation recommendation = State.Analysis.Recommendations[0];
                        State.RestService = new RestService
                        {
                            Name = recommendation.Name,
                            Folder = recommendation.Folder,
                            Resource = recommendation.Resource,
                            Description = recommendation.Description,
                            SupportJSON = recommendation.SupportJSON,
                            SupportXML = recommendation.SupportXML
                        };
                    }
                    else
                    {
                        State.CreateNewService = false;
                        State.RestService = new RestService();
                    }
                }
            }
            else
            {
                //get selected rest service
                if (string.IsNullOrWhiteSpace(form.ExistingRestServiceDropdown.Text))
                {
                    MessageBox.Show("Please select an existing rest service");
                    retVal = false;
                }
                else
                {
                    var restService = State.RestServices.Find(x => x.Name.Equals(form.ExistingRestServiceDropdown.Text, StringComparison.OrdinalIgnoreCase));

                    if (restService == null)
                    {
                        MessageBox.Show("Please select a valid rest service - rest service selected does not exist in configuration");
                        retVal = false;
                    }
                    else
                    {
                        State.RestService = restService;
                    }
                }
            }

            if (String.IsNullOrWhiteSpace(State.RestService.Description))
            {
                State.RestService.Description = await AIService.GenerateRestServiceDescription(State.RestService);
            }
            return retVal;
        }

        public Task<bool> PerformValidation(BaseWizardForm wizardForm, object stateObject)
        {
            return Task.FromResult(true);
        }
    }
}
