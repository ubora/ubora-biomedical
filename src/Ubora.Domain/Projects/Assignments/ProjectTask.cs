using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Ubora.Domain.Projects.Assignments.Events;

namespace Ubora.Domain.Projects.Assignments
{
    public class Assignment : IProjectEntity
    {
        public Guid Id { get; private set; }
        public Guid ProjectId { get; private set; }
        public Guid CreatedByUserId { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }

        [JsonProperty(nameof(Assignees))]
        private readonly HashSet<TaskAssignee> _assignees = new HashSet<TaskAssignee>();
        [JsonIgnore]
        // Virtual for testing.
        public virtual IReadOnlyCollection<TaskAssignee> Assignees
        {
            get { return _assignees; }
            private set { }
        }

        private void Apply(AssignmentAddedEvent e)
        {
            Id = e.Id;
            ProjectId = e.ProjectId;
            CreatedByUserId = e.InitiatedBy.UserId;
            Title = e.Title;
            Description = e.Description;

            foreach (var assigneeId in e.AssigneeIds)
            {
                _assignees.Add(new TaskAssignee(assigneeId));
            }
        }

        private void Apply(AssignmentEditedEvent e)
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