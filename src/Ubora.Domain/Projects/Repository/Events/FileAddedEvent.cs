using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Repository.Events
{
    public class FileAddedEvent : UboraFileEvent, IFileEvent
    {
        public FileAddedEvent(UserInfo initiatedBy, Guid projectId, Guid id, BlobLocation location, string comment, long fileSize, int revisionNumber, string fileName, string folderName)
            : base(initiatedBy, projectId, id, fileName, location, comment, fileSize, revisionNumber)
        {
            FolderName = folderName;
        }

        public string FolderName { get; private set; }

        public override string GetDescription()
        {
            return $"added file \"{FileName}\"";
        }
    }
}
