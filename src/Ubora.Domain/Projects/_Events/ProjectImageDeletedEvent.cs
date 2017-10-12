using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects._Events
{
    internal class ProjectImageDeletedEvent : ProjectEvent
    {
        public ProjectImageDeletedEvent(UserInfo initiatedBy, Guid projectId, DateTime @when) : base(initiatedBy, projectId)
        {
            When = when;
        }

        public DateTime When { get; private set; }

        public override string GetDescription() => "deleted project image.";
    }
}
