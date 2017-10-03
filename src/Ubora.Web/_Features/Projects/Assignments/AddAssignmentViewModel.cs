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
        public Guid[] AssigneeIds { get; set; }
        public IEnumerable<TaskAssigneeViewModel> ProjectMembers { get; set; }
    }
}