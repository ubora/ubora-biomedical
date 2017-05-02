using System.ComponentModel.DataAnnotations;

namespace Ubora.Web.Areas.Projects.Views.Create
{
    public class CreatePostModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}