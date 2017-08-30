using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Repository
{
    public class FileUpdatedEvent : UboraEvent, IFileEvent
    {
        public FileUpdatedEvent(Guid id, Guid projectId, BlobLocation location, UserInfo initiatedBy) : base(initiatedBy)
        {
            Id = id;
            ProjectId = projectId;
            Location = location;
        }

        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public BlobLocation Location { get; private set; }

        public override string GetDescription()
        {
            return $"updated file [{StringTokens.File(Id)}]";
        }
    }
}
