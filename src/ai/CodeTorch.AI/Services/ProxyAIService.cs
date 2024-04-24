using CodeTorch.AI.Abstractions;
using CodeTorch.AI.Models;
using CodeTorch.Core;
using System.Threading.Tasks;

namespace CodeTorch.AI.Services
{
    public class ProxyAIService : ICodeTorchAIService
    {
        bool InTestMode = false;
        readonly ICodeTorchAIService _aiService;

        public ProxyAIService() {
            if(InTestMode)
            {
                _aiService = new MockAIService();
            }
            else
            {
                _aiService = new CodeTorchAIService();
            }
        }

        public Task<string> GenerateRestServiceDescription(RestService service)
        {
            return _aiService.GenerateRestServiceDescription(service);
        }

        public Task<AnalyzeRequestResponse> GetRecommendationsForRestService(string request, string existingRestServicesJson)
        {
            return _aiService.GetRecommendationsForRestService(request, existingRestServicesJson);
        }

        public Task<DataCommand> UpdateDataCommandDescription(DataCommand command, string DatabaseProjectFolder = null)
        {
            return _aiService.UpdateDataCommandDescription(command, DatabaseProjectFolder);
        }
    }
}
