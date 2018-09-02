using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects._Events
{
    public class ProjectDescriptionEditedEventV2 : ProjectEvent
    {
        public ProjectDescriptionEditedEventV2(UserInfo initiatedBy, Guid projectId, QuillDelta description) : base(initiatedBy, projectId)
        {
            Description = description;
        }

        public QuillDelta Description { get; }

        public override string GetDescription() => "updated project description.";
    }

    public class EditProjectDescriptionEvent : ProjectEvent
    {
        public EditProjectDescriptionEvent(UserInfo initiatedBy, Guid projectId, string description) : base(initiatedBy, projectId)
        {
            Description = description;
        }

        public string Description { get; private set; }

        public override string GetDescription() => "updated project description.";
    }
}
