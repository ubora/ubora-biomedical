using System;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects
{
    public class UpdateProjectCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ClinicalNeedTags { get; set; }
        public string AreaOfUsageTags { get; set; }
        public string PotentialTechnologyTags { get; set; }
        public string DescriptionOfNeed { get; set; }
        public string DescriptionOfExistingSolutionsAndAnalysis { get; set; }
        public string ProductFunctionality { get; set; }
        public string ProductPerformance { get; set; }
        public string ProductUsability { get; set; }
        public string ProductSafety { get; set; }
        public string PatientPopulationStudy { get; set; }
        public string UserRequirementStudy { get; set; }
        public string AdditionalInformation { get; set; }
        public string GmdnTerm { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
