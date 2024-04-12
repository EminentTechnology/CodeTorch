using CodeTorch.AI.Models;
using CodeTorch.Core;
using System.Collections.Generic;

namespace CodeTorch.Designer.Wizards.RestServiceWizard
{
    public class RestServiceWizardState
    {
        public bool CreateNewService { get; set; }
        public string Request { get; set; }
        public AnalyzeRequestResponse Analysis { get; set; } = new AnalyzeRequestResponse();
        public List<RestService> RestServices { get; set; } = new List<RestService>();
        public RestService RestService { get; set; }
    }
}