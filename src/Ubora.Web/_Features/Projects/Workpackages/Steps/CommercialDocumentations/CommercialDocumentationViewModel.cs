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
        public IFormFile Logo { get; set; }
        public string LogoUrl { get; set; }
    }

    public class IntellectualPropertyViewModel 
    {
        public LicenseType License { get; set; }
        public bool Attribution { get; set; }
        public bool ShareAlike { get; set; }
        public bool NonCommercial { get; set; } 
        public bool NoDerivativeWorks { get; set; }
        public bool UboraLicense { get; set; }
    }
    
    public enum LicenseType 
    {
        None,
        Ubora,
        CreativeCommons
    }
}