using System;
using Ubora.Domain.Infrastructure;

namespace Ubora.Domain.Projects.Repository
{
    public class ProjectFile
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public string FileName { get; private set; }
        public BlobLocation Location { get; private set; }
        public bool IsHidden { get; private set; }

        private void Apply(FileAddedEvent e)
        {
            Id = e.Id;
            ProjectId = e.ProjectId;
            FileName = e.FileName;
            Location = e.Location;
        }

        private void Apply(FileHidEvent e)
        {
            Id = e.Id;
            IsHidden = true; 
        }

        private void Apply(FileUpdatedEvent e)
        {
            Location = e.Location;
        }
    }
}
