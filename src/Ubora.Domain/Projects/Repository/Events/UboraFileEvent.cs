using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Projects._Events;

namespace Ubora.Domain.Projects.Repository.Events
{
    public abstract class UboraFileEvent : ProjectEvent
    {
        protected UboraFileEvent(UserInfo initiatedBy, Guid projectId, Guid id, string fileName, BlobLocation location, string comment, long fileSize, int revisionNumber) 
            : base(initiatedBy, projectId)
        {
            Id = id;
            FileName = fileName;
            Location = location;
            Comment = comment;
            FileSize = fileSize;
            RevisionNumber = revisionNumber;
        }

        public Guid Id { get; private set; }
        public string FileName { get; private set; }
        public BlobLocation Location { get; private set; }
        public string Comment { get; private set; }
        public long FileSize { get; private set; }
        public int RevisionNumber { get; private set; }

        public abstract override string GetDescription();
    }
}
