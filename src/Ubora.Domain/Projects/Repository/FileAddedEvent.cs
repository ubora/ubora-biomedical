using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Repository
{
    public class FileAddedEvent : UboraEvent, IFileEvent
    {
        public FileAddedEvent(UserInfo initiatedBy, Guid projectId, Guid id, string fileName, BlobLocation location) : base(initiatedBy)
        {
            ProjectId = projectId;
            Id = id;
            FileName = fileName;
            Location = location;
        }

        public Guid ProjectId { get; private set; }
        public Guid Id { get; private set; }
        public string FileName { get; private set;  }
        public BlobLocation Location { get; private set; }

        public override string GetDescription()
        {
            return $"Added file \"{Location}\"";
        }
    }
}
