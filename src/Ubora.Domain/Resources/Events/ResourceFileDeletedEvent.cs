using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Resources.Events
{
    public class ResourceFileDeletedEvent : UboraEvent
    {
        public ResourceFileDeletedEvent(Guid fileId, UserInfo initiatedBy) : base(initiatedBy)
        {
            FileId = fileId;
        }
        
        public Guid FileId { get; }

        public override string GetDescription()
        {
            throw new NotImplementedException();
        }
    }
}