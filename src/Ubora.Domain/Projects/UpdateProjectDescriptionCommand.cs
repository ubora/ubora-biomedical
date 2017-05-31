using System;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects
{
    public class UpdateProjectDescriptionCommand : UserProjectCommand
    {
        public string Description { get; set; }
    }
}
