using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects._Events
{
    public class EditProjectDescriptionEvent : UboraEvent
    {
        public Guid Id { get; set; }
        public string Description { get; set; }

        public EditProjectDescriptionEvent(UserInfo initiatedBy, Guid id, string description) : base(initiatedBy)
        {
            Id = id;
            Description = description;
        }

        public override string GetDescription() => "updated project description.";
    }
}
