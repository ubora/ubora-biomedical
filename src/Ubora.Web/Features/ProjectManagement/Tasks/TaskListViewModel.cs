using System;
using System.Collections.Generic;

namespace Ubora.Web.Features.ProjectManagement.Tasks
{
    public class TaskListViewModel
    {
        public Guid ProjectId { get; set; }
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