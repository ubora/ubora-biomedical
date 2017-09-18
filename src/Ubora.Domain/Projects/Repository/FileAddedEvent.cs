using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Projects.Repository
{
    public class FileAddedEvent : UboraFileEvent, IFileEvent
    {
        public FileAddedEvent(Guid id, Guid projectId, BlobLocation location, string comment, long fileSize, UserInfo initiatedBy, string fileName, string folderName, int revisionNumber = 1) 
            : base(id, projectId, location, comment, fileSize, initiatedBy, revisionNumber)
        {
            FileName = fileName;
            FolderName = folderName;
        }

        public string FileName { get; private set;  }
        public string FolderName { get; private set; }

        public override string GetDescription()
        {
            return $"Added file \"{FileName}\"";
        }
    }
}
