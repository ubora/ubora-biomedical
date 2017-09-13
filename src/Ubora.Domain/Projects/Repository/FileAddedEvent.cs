using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Repository
{
    public class FileAddedEvent : UboraFileEvent, IFileEvent
    {
        public FileAddedEvent(Guid id, Guid projectId, BlobLocation location, string comment, long fileSize, UserInfo initiatedBy, string fileName) 
            : base(id, projectId, location, comment, fileSize, initiatedBy)
        {
            FileName = fileName;
        }

        public string FileName { get; private set;  }

        public override string GetDescription()
        {
            return $"Added file \"{FileName}\"";
        }
    }
}
