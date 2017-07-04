using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects
{
    internal class ProjectImageUpdatedEvent : UboraEvent
    {
        public string ImageName { get; set; }

        public ProjectImageUpdatedEvent(UserInfo initiatedBy) : base(initiatedBy)
        {
        }

        public override string GetDescription() => "Updated project image";
    }
}
