using CodeTorch.AI.Abstractions;
using CodeTorch.AI.Models;
using CodeTorch.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeTorch.AI.Services
{
    public class MockAIService : ICodeTorchAIService
    {
        public Task<string> GenerateRestServiceDescription(RestService service)
        {
            return Task.FromResult("Generated Description");
        }

        public Task<AnalyzeRequestResponse> GetRecommendationsForRestService(string request, string existingRestServicesJson)
        {
            var analyzeRequestResponse = new AnalyzeRequestResponse
            {
                RecommendNewRestService = false,
                Recommendations = new List<NewRestServiceRecommendation>
                    {
                        new NewRestServiceRecommendation
                        {
                            Name = "NewRestService",
                            Folder = "NewFolder",
                            Resource = "NewResource",
                            Description = "NewDescription",
                            SupportJSON = true,
                            SupportXML = true
                        },
                        new NewRestServiceRecommendation
                        {
                            Name = "NewRestService2",
                            Folder = "NewFolder2",
                            Resource = "NewResource2",
                            Description = "NewDescription2",
                            SupportJSON = true,
                            SupportXML = false
                        }
                    },
                ExistingRestService = "GroupMembers",
                Reason = "ExistingRestService is already available"

            };

            return Task.FromResult(analyzeRequestResponse);
        }

        public Task<DataCommand> UpdateDataCommandDescription(DataCommand command, string DatabaseProjectFolder = null)
        {
            throw new System.NotImplementedException();
        }
    }
}
