using System.Collections.Generic;
using Ubora.Web._Features.Projects.ApplicableRegulations;

namespace Ubora.Web._Features.Projects.Workpackages.Steps.PreproductionDocuments
{
    public class WP1TemplatePartialViewModel
    {
        public List<WorkpackageStepViewModel> WorkpackageStepViewModels { get; set; }
        public IEnumerable<ReviewQuestionnaireViewModel> ReviewQuestionnaireViewModels { get; set; }
    }
}