using System;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.Projects.Tasks
{
    public class EditTaskViewModel
    {
        public Guid ProjectId { get; set; }
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}