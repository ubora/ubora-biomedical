using System;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.Repository.Events;

namespace Ubora.Domain.Projects.Repository
{
    public class ProjectFile
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }

        public string FileName { get; private set; }
        public BlobLocation Location { get; private set; }
        public string Comment { get; private set; }
        public long FileSize { get; private set; }
        public string FolderName { get; private set; }
        public int RevisionNumber { get; private set; }

        public bool IsHidden { get; private set; }

        private void Apply(FileAddedEvent e)
        {
            Id = e.Id;
            ProjectId = e.ProjectId;
            FileName = e.FileName;
            Location = e.Location;
            Comment = e.Comment;
            FileSize = e.FileSize;
            FolderName = e.FolderName;
            RevisionNumber = e.RevisionNumber;
        }

        private void Apply(FileHiddenEvent e)
        {
            Id = e.Id;
            IsHidden = true;
        }

        private void Apply(FileUpdatedEvent e)
        {
            FileName = e.FileName;
            Location = e.Location;
            FileSize = e.FileSize;
            Comment = e.Comment;
            RevisionNumber = e.RevisionNumber;
        }
    }
}
