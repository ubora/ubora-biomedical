using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects._Events
{
    public class ProjectDeletedEvent : ProjectEvent
    {
        public ProjectDeletedEvent(UserInfo initiatedBy, Guid projectId) : base(initiatedBy, projectId)
        {
        }

        public override string GetDescription() => "deleted project";
    }
}
