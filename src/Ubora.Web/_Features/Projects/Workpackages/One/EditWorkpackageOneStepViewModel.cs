using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.Projects.Workpackages.One
{
    public class EditWorkpackageOneStepViewModel : WorkpackageOneStepViewModel
    {
        [Required]
        public string Value { get; set; }

        public bool IsEdit { get; set; }
    }
}