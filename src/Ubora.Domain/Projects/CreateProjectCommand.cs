using System;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects
{
    public class CreateProjectCommand : UserCommand
    {
        public Guid NewProjectId { get; set; }
        public string Title { get; set; }
        public string ClinicalNeed { get; set; }
        public string AreaOfUsage { get; set; }
        public string PotentialTechnology { get; set; }
        public string Gmdn { get; set; }
    }
}