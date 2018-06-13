using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.Resources.Models
{
    public class AddResourcePostModel
    {
        [Required(ErrorMessage = "Please specify a title.")]
        public string Title { get; set; }

        public string Body { get; set; } // TODO required validation
    }
}