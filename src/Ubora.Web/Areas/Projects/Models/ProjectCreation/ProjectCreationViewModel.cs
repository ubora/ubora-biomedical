using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Ubora.Web.Areas.Projects.Models.ProjectCreation
{
    public class ProjectCreationViewModel : ProjectCreationPostModel
    {
    }

    public class ProjectCreationPostModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
