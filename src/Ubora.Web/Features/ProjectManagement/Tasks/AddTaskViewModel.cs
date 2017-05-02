using System;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web.Features.ProjectManagement.Tasks
{
    public class AddTaskViewModel
    {
        public Guid ProjectId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}