using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Repository
{
    public abstract class UboraFileEvent : UboraEvent
    {
        public UboraFileEvent(Guid id, Guid projectId, BlobLocation location, string comment, long fileSize, UserInfo initiatedBy, int revisionNumber) 
            : base(initiatedBy)
        {
            Id = id;
            ProjectId = projectId;
            Location = location;
            Comment = comment;
            FileSize = fileSize;
            RevisionNumber = revisionNumber;
        }


        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public BlobLocation Location { get; private set; }
        public string Comment { get; private set; }
        public long FileSize { get; private set; }
        public int RevisionNumber { get; private set; }

        public override abstract string GetDescription();
    }
}
