using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Ubora.Web._Features.Projects.Workpackages.Steps.CommercialDocumentations
{
    public class CommercialDocumentationViewModel
    {
        public CommercialDossierViewModel CommercialDossier { get; set; }
        public IntellectualPropertyViewModel IntellectualProperty { get; set; }
    }

    public class CommercialDossierViewModel 
    {
        public string ProductName { get; set; }
        public string CommercialName { get; set; }
        public string DescriptionHtml { get; set; }
        public string DescriptionQuillDelta { get; set; }
        public bool DoesDescriptionHaveContent { get; set; }
        public IFormFile UserManual { get; set; }
        public string UserManualName { get; set; }
        public IFormFile Logo { get; set; }
        public string LogoUrl { get; set; }
        public bool HasOldLogoBeenDeleted { get; set; }
        public bool HasOldUserManualBeenRemoved { get; set; }
    }

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
    
    public enum LicenseType 
    {
        None,
        Ubora,
        CreativeCommons
    }
}