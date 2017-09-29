using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.Projects.Assignments
{
    public class AddAssignmentViewModel
    {
        public Guid ProjectId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public IEnumerable<TaskAssigneeViewModel> AssignmentMembers { get; set; }
        public IEnumerable<TaskAssigneeViewModel> ProjectMembers { get; set; }
    }

    public class TaskAssigneeViewModel
    {
        public Guid AssigneeId { get; set; }
        public string FullName { get; set; }
    }
}