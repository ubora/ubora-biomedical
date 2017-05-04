using System;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects
{
    public class CreateProjectCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public UserInfo UserInfo { get; set; }
        public string Description { get; set; }
        public string ClinicalNeed { get; set; }
        public string AreaOfUsage { get; set; }
        public string PotentialTechnology { get; set; }
        public string GmdnTerm { get; set; }
        public string GmdnDefinition { get; set; }
        public string GmdnCode { get; set; }
    }
}