using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Areas.ResourcesArea.ResourcePageCreation.Models
{
    public class AddResourcePostModel
    {
        [Required(ErrorMessage = "Please specify a title.")]
        public string Title { get; set; }

        public string Body { get; set; } // TODO required validation

        public int MenuPriority { get; set; }
    }
}