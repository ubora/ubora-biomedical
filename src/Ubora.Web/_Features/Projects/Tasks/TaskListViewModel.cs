using System;
using System.Collections.Generic;

namespace Ubora.Web._Features.Projects.Tasks
{
    public class TaskListViewModel
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public IEnumerable<TaskListItemViewModel> Tasks { get; set; }
    }

    public class TaskListItemViewModel
    {
        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}