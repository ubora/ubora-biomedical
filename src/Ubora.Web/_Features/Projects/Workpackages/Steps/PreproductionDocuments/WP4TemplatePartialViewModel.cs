using System.Collections.Generic;
using Ubora.Web._Features.Projects.Workpackages.Steps.IsoCompliances.Models;

namespace Ubora.Web._Features.Projects.Workpackages.Steps.PreproductionDocuments
{
    public class WP4TemplatePartialViewModel
    {
        public List<WorkpackageStepViewModel> WorkpackageStepViewModels { get; set; }
        public StructuredInformationResultViewModel StructuredInformationResultViewModel { get; set; }
        public IndexViewModel IsoStandardIndexListViewModel { get; set; }
    }
}