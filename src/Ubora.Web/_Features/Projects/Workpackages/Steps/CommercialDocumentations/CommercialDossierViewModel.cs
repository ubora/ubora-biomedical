using Microsoft.AspNetCore.Http;

namespace Ubora.Web._Features.Projects.Workpackages.Steps.CommercialDocumentations
{
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
}