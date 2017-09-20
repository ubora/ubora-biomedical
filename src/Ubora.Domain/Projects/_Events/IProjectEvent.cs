using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects._Events
{
    public interface IProjectEvent
    {
        Guid ProjectId { get; }
    }

    public abstract class ProjectEvent : UboraEvent, IProjectEvent
    {
        protected ProjectEvent(UserInfo initiatedBy, Guid projectId) : base(initiatedBy)
        {
            ProjectId = projectId;
        }

        public Guid ProjectId { get; }
    }
}
