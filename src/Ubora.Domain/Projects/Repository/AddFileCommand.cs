using System;
using Ubora.Domain.Infrastructure.Commands;

namespace Ubora.Domain.Projects.Repository
{
    public class AddFileCommand : UserProjectCommand
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
