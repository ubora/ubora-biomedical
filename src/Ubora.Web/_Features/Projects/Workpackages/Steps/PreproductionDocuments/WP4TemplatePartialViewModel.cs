using Ubora.Web._Features.Projects.Workpackages.Steps.IsoCompliances.Models;

namespace Ubora.Web._Features.Projects.Workpackages.Steps.PreproductionDocuments
{
    public class WP4TemplatePartialViewModel
    {
        public string PrototypesAndConsiderationsForSafetyAssessment { get; set; }
        public string QualityCriteria { get; set; }
        public string ResultsFromVitroOrVivo { get; set; }
        public StructuredInformationResultViewModel StructuredInformationResultViewModel { get; set; }
        public IndexViewModel IsoStandardIndexListViewModel { get; set; }
    }
}