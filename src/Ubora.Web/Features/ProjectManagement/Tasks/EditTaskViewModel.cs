using System;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web.Features.ProjectManagement.Tasks
{
    public class EditTaskViewModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}