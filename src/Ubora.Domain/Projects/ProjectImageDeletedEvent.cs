using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects
{
    internal class ProjectImageDeletedEvent : UboraEvent
    {
        public DateTime When { get; private set; }

        public ProjectImageDeletedEvent(DateTime when, UserInfo initiatedBy) : base(initiatedBy)
        {
            When = when;
        }

        public override string GetDescription() => "Deleted project image.";
    }
}
