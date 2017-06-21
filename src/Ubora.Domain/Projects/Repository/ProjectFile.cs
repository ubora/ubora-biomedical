using System;

namespace Ubora.Domain.Projects.Repository
{
    public class ProjectFile
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public string Title { get; private set; }

        private void Apply(FileAddedEvent e)
        {
            Id = e.Id;
            ProjectId = e.ProjectId;
            Title = e.FileName;
        }
    }
}
