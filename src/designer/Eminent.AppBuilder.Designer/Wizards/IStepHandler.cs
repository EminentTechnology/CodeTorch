using System.Threading.Tasks;

namespace CodeTorch.Designer.Wizards
{
    public interface IStepHandler
    {
        Task<bool> OnPreviousClick(BaseWizardForm wizardForm, object stateObject);
        Task<bool> OnPageLoad(BaseWizardForm wizardForm, object stateObject);
        Task<bool> PerformValidation(BaseWizardForm wizardForm, object stateObject);
        Task<bool> OnNextClick(BaseWizardForm wizardForm, object stateObject);
    }
}
