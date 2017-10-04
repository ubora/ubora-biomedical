using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Ubora.Domain.Projects.Tasks.Events;

namespace Ubora.Domain.Projects.Tasks
{
    public class ProjectTask
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }

        [JsonProperty(nameof(Assignees))]
        private readonly HashSet<TaskAssignee> _assignees = new HashSet<TaskAssignee>();
        [JsonIgnore]
        public IReadOnlyCollection<TaskAssignee> Assignees
        {
            get { return _assignees; }
            private set { }
        }

        private void Apply(TaskAddedEvent e)
        {
            Id = e.Id;
            ProjectId = e.ProjectId;
            Title = e.Title;
            Description = e.Description;

            foreach (var assigneeId in e.AssigneeIds)
            {
                _assignees.Add(new TaskAssignee(assigneeId));
            }
        }

        private void Apply(TaskEditedEvent e)
        {
            Title = e.Title;
            Description = e.Description;

            _assignees.Clear();
            foreach (var assigneeId in e.AssigneeIds)
            {
                _assignees.Add(new TaskAssignee(assigneeId));
            }
        }
    }
}