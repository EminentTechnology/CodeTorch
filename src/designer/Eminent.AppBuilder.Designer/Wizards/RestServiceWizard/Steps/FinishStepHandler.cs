using CodeTorch.Core;
using System.Threading.Tasks;

namespace CodeTorch.Designer.Wizards.RestServiceWizard.Steps
{
    public class FinishStepHandler: IStepHandler
    {
        public Task<bool> OnPreviousClick(BaseWizardForm wizardForm, object stateObject)
        {
            bool retVal = true;
            return Task.FromResult(retVal);
        }

        public Task<bool> OnPageLoad(BaseWizardForm wizardForm, object stateObject)
        {
            var form = wizardForm as RestServiceWizardForm;
            var State = stateObject as RestServiceWizardState;

            if (form == null || State == null)
            {
                return Task.FromResult(false);
            }

            if (State.RestService != null)
            {
                form.SummaryNameLabel.Text = State.RestService.Name;
                form.SummaryFolderLabel.Text = State.RestService.Folder;
                form.SummaryResourceLabel.Text = State.RestService.Resource;
            }

            return Task.FromResult(true); 
        }

        public Task<bool> PerformValidation(BaseWizardForm wizardForm, object stateObject)
        {
            bool retVal = true;
            return Task.FromResult(retVal);
        }

        public Task<bool> OnNextClick(BaseWizardForm wizardForm, object stateObject)
        {
            bool retVal = true;

            var form = wizardForm as RestServiceWizardForm;
            var State = stateObject as RestServiceWizardState;

            if (form == null || State == null)
            {
                return Task.FromResult(false);
            }

            if(State.RestService == null)
            {           
                return Task.FromResult(false);
            }

            if (State.CreateNewService)
            {
                Configuration.GetInstance().RestServices.Add(State.RestService);
                RestService.Save(State.RestService);
            }

            RestService.Save(State.RestService);

            return Task.FromResult(retVal);
        }
    }
}
