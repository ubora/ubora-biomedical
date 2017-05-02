using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Tasks
{
    public class TaskAddedEvent : UboraEvent
    {
        public TaskAddedEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public Guid ProjectId { get; set; }
        public Guid TaskId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public override string GetDescription()
        {
            throw new NotImplementedException();
        }
    }
}