using System.Collections.Generic;

namespace CodeTorch.AI.Models
{
    public class AnalyzeRequestResponse
    {
        public bool RecommendNewRestService { get; set; }

        public List<NewRestServiceRecommendation> Recommendations { get; set; } = new List<NewRestServiceRecommendation>();

        public string ExistingRestService { get; set; }
        public string Reason { get; set; }
    }
}
