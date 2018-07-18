using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Resources.Events
{
    public class ResourcePageBodyEditedEvent : UboraEvent
    {
        public ResourcePageBodyEditedEvent(UserInfo initiatedBy, Guid resourcePageId, QuillDelta body, int previousBodyVersion) 
            : base(initiatedBy)
        {
            ResourcePageId = resourcePageId;
            Body = body;
            PreviousBodyVersion = previousBodyVersion;
        }

        public Guid ResourcePageId { get; set; }
        public QuillDelta Body { get; }
        public int PreviousBodyVersion { get; }

        public override string GetDescription() => "Edited resource content.";
    }
}