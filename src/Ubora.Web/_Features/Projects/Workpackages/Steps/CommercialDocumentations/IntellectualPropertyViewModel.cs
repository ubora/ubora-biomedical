using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Ubora.Domain.Projects.IntellectualProperties;

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

        public bool IsCreativeCommonsLicense => License == LicenseType.CreativeCommons;
        public bool IsUboraLicense => License == LicenseType.Ubora;

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

        public class Factory
        {
            public IntellectualPropertyViewModel Create(IntellectualProperty intellectualProperty)
            {
                switch (intellectualProperty.License)
                {
                    case CreativeCommonsLicense creativeCommonsLicense:
                        return new IntellectualPropertyViewModel
                        {
                            License = LicenseType.CreativeCommons,
                            Attribution = creativeCommonsLicense.Attribution,
                            NonCommercial = creativeCommonsLicense.NonCommercial,
                            ShareAlike = creativeCommonsLicense.ShareAlike,
                            NoDerivativeWorks = creativeCommonsLicense.NoDerivativeWorks,
                        };
                    case UboraLicense uboraLicense:
                        return new IntellectualPropertyViewModel
                        {
                            License = LicenseType.Ubora,
                        };
                    default:
                        return new IntellectualPropertyViewModel
                        {
                            License = LicenseType.None
                        };
                }
            }
        }
    }
}