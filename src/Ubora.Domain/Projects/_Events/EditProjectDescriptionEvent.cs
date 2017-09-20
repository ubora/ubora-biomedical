using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects._Events
{
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
