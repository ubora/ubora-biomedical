using System;

namespace Ubora.Domain.Projects.Tasks
{
    public class ProjectTask
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }

        // TODO: Through aggregate:
        private void Apply(TaskAddedEvent e)
        {
            Id = e.TaskId;
            ProjectId = e.ProjectId;
            Title = e.Title;
            Description = e.Description;
        }
    }
}