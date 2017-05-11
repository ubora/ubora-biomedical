using System;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects
{
    public class CreateProjectCommand : UserCommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string ClinicalNeed { get; set; }
        public string AreaOfUsage { get; set; }
        public string PotentialTechnology { get; set; }
        public string GmdnTerm { get; set; }
        public string GmdnDefinition { get; set; }
        public string GmdnCode { get; set; }
    }
}