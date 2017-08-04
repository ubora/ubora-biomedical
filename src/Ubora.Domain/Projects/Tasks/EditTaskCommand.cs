using System;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.Tasks
{
    public class EditTaskCommand : UserProjectCommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
