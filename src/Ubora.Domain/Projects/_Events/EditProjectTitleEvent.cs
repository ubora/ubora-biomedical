using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects._Events
{
    public class ProjectTitleEditedEvent : ProjectEvent
    {
        public ProjectTitleEditedEvent(UserInfo initiatedBy, Guid projectId, string title) : base(initiatedBy, projectId)
        {
            Title = title;
        }

        public string Title { get; private set; }

        public override string GetDescription() => "updated project title.";
    }
}
