using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ubora.Web._Features.Projects.Workpackages.Steps.CommercialDocumentations
{
    public class IntellectualPropertyViewModel : IValidatableObject
    {
        [Required]
        public LicenseType? License { get; set; }

        public bool Attribution { get; set; }
        public bool ShareAlike { get; set; }
        public bool NonCommercial { get; set; } 
        public bool NoDerivativeWorks { get; set; }

        public bool IsCreativeCommonsLicense { get; set; }
        public bool IsUboraLicense { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (License.HasValue && License == LicenseType.CreativeCommons) 
            {
                if (!Attribution
                    && !ShareAlike
                    && !NonCommercial
                    && !NoDerivativeWorks) 
                {
                    yield return new ValidationResult("Please specify Creative Commons license further.");
                }
            }
        }
    }
}