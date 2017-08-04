using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects
{
    public class UpdateProjectCommand : UserProjectCommand
    {
        public string Title { get; set; }
        public string ClinicalNeedTags { get; set; }
        public string AreaOfUsageTags { get; set; }
        public string PotentialTechnologyTags { get; set; }
        public string Gmdn { get; set; }
    }
}
