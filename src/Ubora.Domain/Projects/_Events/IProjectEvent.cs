using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects._Events
{
    public abstract class ProjectEvent : UboraEvent
    {
        protected ProjectEvent(UserInfo initiatedBy, Guid projectId) : base(initiatedBy)
        {
            ProjectId = projectId;
        }

        public Guid ProjectId { get; }
    }
}
