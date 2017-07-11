using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects
{
    internal class ProjectImageUpdatedEvent : UboraEvent
    {
        public DateTime When { get; private set; }

        public ProjectImageUpdatedEvent(DateTime when, UserInfo initiatedBy) : base(initiatedBy)
        {
            When = when;
        }

        public override string GetDescription() => "Updated project image.";
    }
}
