using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTorch.Core.Interfaces
{
    public interface IWorkflowProvider
    {
        DataConnection Connection { get; set; }
        void Initialize(List<Setting> settings);

        WorkflowStep GetCurrentWorkflowStep(Workflow workflow, string entityIDValue);

        bool ChangeWorkflowStep(Workflow workflow, WorkflowNextStep nextStep, string entityIDValue, string comment);

        void SetStep(CodeTorch.Core.Workflow workflow, WorkflowStep step, string entityIDValue, string comment);

        void SetEntityStatus(CodeTorch.Core.Workflow workflow, WorkflowStep step, string entityIDValue);

        
        void Save(CodeTorch.Core.Workflow workflow);
         

        void Delete(CodeTorch.Core.Workflow workflow);
        
    }
}
