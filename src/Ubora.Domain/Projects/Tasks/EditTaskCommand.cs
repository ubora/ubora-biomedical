using System;
using Ubora.Domain.Infrastructure.Commands;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Tasks
{
    public class EditTaskCommand : ICommand
    {
        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public UserInfo InitiatedBy { get; set; }
    }
}
