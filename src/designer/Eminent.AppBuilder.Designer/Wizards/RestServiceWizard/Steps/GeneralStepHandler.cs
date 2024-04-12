using CodeTorch.AI.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace CodeTorch.Designer.Wizards.RestServiceWizard.Steps
{
    public class GeneralStepHandler: IStepHandler
    {
        readonly ICodeTorchAIService AIService;

        public GeneralStepHandler(ICodeTorchAIService aiService)
        { 
            this.AIService = aiService;
        }

        public Task<bool> OnPreviousClick(BaseWizardForm wizardForm, object stateObject)
        {
            bool retVal = true;
            return Task.FromResult(retVal);
        }

        public Task<bool> OnPageLoad(BaseWizardForm wizardForm, object stateObject)
        {
            bool retVal = true;
            return Task.FromResult(retVal);
        }

        public Task<bool> PerformValidation(BaseWizardForm wizardForm, object stateObject)
        {
            bool retVal = true;
            return Task.FromResult(retVal);
        }

        public async Task<bool> OnNextClick(BaseWizardForm wizardForm, object stateObject)
        {
            var form = wizardForm as RestServiceWizardForm;
            var State = stateObject as RestServiceWizardState;

            if (form == null || State == null)
            {
                return false;
            }

            if (!string.IsNullOrWhiteSpace(form.RequestTextbox.Text.Trim()))
            {
                var restservices = CodeTorch.Core.Configuration.GetInstance().RestServices;
                var restservicesHeaders = restservices.Select(x => new
                {
                    x.Name,
                    x.Folder,
                    x.Resource,
                    x.Description,
                    x.SupportJSON,
                    x.SupportXML
                }).ToList();
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(restservicesHeaders);

                State.Request = form.RequestTextbox.Text.Trim();
                State.Analysis = await AIService.GetRecommendationsForRestService(State.Request, json);
            }
            else
            {
                State.Analysis.RecommendNewRestService = true;
            }
            return true;
        }
    }
}
