using CodeTorch.AI.Models;
using CodeTorch.Core;
using System.Threading.Tasks;

namespace CodeTorch.AI.Abstractions
{
    public interface ICodeTorchAIService
    {
        Task<string> GenerateRestServiceDescription(RestService service);
        Task<AnalyzeRequestResponse> GetRecommendationsForRestService(string request, string existingRestServicesJson);
        Task<DataCommand> UpdateDataCommandDescription(DataCommand command, string DatabaseProjectFolder = null);
    }
}
