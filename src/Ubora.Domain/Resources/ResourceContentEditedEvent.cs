using System;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects.Workpackages.Events;

namespace Ubora.Domain.Resources
{
    public class ResourceContentEditedEvent : UboraEvent
    {
        public ResourceContentEditedEvent(UserInfo initiatedBy, ResourceContent content, Guid previousContentVersion) 
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