using System.Collections.Generic;
using Ubora.Web._Features.Projects.ApplicableRegulations;

namespace Ubora.Web._Features.Projects.Workpackages.Steps.PreproductionDocuments
{
    public class WP1TemplatePartialViewModel
    {
        public string ClinicalNeeds { get; set; }
        public string ExistingSolutions { get; set; }
        public string IntendedUsers { get; set; }
        public string ProductRequirements { get; set; }
        public IEnumerable<ReviewQuestionnaireViewModel> ReviewQuestionnaireViewModels { get; set; }
    }
}