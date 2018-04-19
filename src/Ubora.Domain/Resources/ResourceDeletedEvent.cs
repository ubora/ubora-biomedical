using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Resources
{
    public class ResourceDeletedEvent : UboraEvent
    {
        public ResourceDeletedEvent(UserInfo initiatedBy, Guid resourceId) : base(initiatedBy)
        {
            ResourceId = resourceId;
        }
        
        public Guid ResourceId { get; }
        
        public override string GetDescription()
        {
            throw new NotImplementedException();
        }
    }
}