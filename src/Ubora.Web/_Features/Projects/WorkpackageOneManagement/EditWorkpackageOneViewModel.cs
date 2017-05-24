using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.Projects.WorkpackageOneManagement
{
    public class EditWorkpackageOneViewModel
    {
        public string Title { get; set; }

        [Required]
        public string Value { get; set; }

        public string PostUrl  { get; set; }

        public string EditUrl { get; set; }

        public bool IsEdit { get; set; }
    }
}