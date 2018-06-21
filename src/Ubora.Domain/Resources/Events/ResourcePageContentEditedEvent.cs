using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Resources.Events
{
    public class ResourcePageContentEditedEvent : UboraEvent
    {
        public ResourcePageContentEditedEvent(UserInfo initiatedBy, ResourceContent content, Guid previousContentVersion) 
            : base(initiatedBy)
        {
            Content = content;
            PreviousContentVersion = previousContentVersion;
        }
        
        public ResourceContent Content { get; }
        public Guid PreviousContentVersion { get; }

        public override string GetDescription() => "Edited resource content.";
    }
}