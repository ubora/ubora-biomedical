using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ubora.Domain;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Projects.CommercialDossiers;
using Ubora.Domain.Projects.CommercialDossiers.Commands;
using Ubora.Domain.Projects.IntellectualProperties;
using Ubora.Web._Features._Shared.Notices;
using Ubora.Web._Features.Projects._Shared;
using Ubora.Web._Features.Projects.Workpackages.Steps.CommercialDocumentations;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web.Infrastructure.Storage;

namespace Ubora.Web._Features.Projects.Workpackages.Steps.CommercialDocumentations
{
    [ProjectRoute("WP5/CommercialDocumentation")]
    public class CommercialDocumentationsController : ProjectController
    {
        public const string Name = "CommercialDocumentations";
        
        private readonly IUboraStorageProvider _storageProvider;

        public CommercialDossier CommercialDossier { get; private set; }
        public IntellectualProperty IntellectualProperty { get; private set; }

        public CommercialDocumentationsController(IUboraStorageProvider storageProvider) 
        {
            _storageProvider = storageProvider;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            CommercialDossier = QueryProcessor.FindById<CommercialDossier>(ProjectId);
            IntellectualProperty = QueryProcessor.FindById<IntellectualProperty>(ProjectId);
            ViewData[nameof(WorkpackageMenuOption)] = WorkpackageMenuOption.CommercialDocumentation;
            ViewData["MenuOption"] = ProjectMenuOption.Workpackages;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index() 
        {
            return View("CommercialDocumentation", new CommercialDocumentationViewModel 
            {
                CommercialDossier = await MapToViewModel(CommercialDossier),
                IntellectualProperty = MapToViewModel(IntellectualProperty)
            });
        }

        [HttpGet("EditIntellectualProperty")]
        public IActionResult EditIntellectualProperty() 
        {
            return View(MapToViewModel(IntellectualProperty)); 
        }

        [HttpPost("EditIntellectualProperty")]
        public async Task<IActionResult> EditIntellectualProperty(IntellectualPropertyViewModel model) 
        {
            if (!ModelState.IsValid) 
            {
                return EditIntellectualProperty();
            }

            ExecuteUserProjectCommand(new EditLicenseCommand 
            {
                Attribution = model.Attribution,
                NoDerivativeWorks = model.NoDerivativeWorks,
                ShareAlike = model.ShareAlike,
                UboraLicense = model.UboraLicense,
                NonCommercial = model.NonCommercial 
            }, Notice.Success("Successfully edited"));

            if (!ModelState.IsValid) 
            {
                return EditIntellectualProperty();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("EditCommercialDossier")]
        public async Task<IActionResult> EditCommercialDossier() 
        {
            CommercialDossierViewModel model = await MapToViewModel(CommercialDossier);
            return View(model);
        }

        [HttpPost("EditCommercialDossier")]
        public async Task<IActionResult> EditCommercialDossier(CommercialDossierViewModel model) 
        {
            if (!ModelState.IsValid) 
            {
                return await EditCommercialDossier();
            }

            ExecuteUserProjectCommand(new EditCommercialDossierCommand 
            {
                CommercialName = model.CommercialName ?? "",
                ProductName = model.ProductName ?? "",
                Description = new QuillDelta(model.DescriptionQuillDelta),
                Logo = await GetLogoBlobLocation(model.Logo)
            }, Notice.Success("Successfully edited"));

            if (!ModelState.IsValid) 
            {
                return await EditCommercialDossier();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<BlobLocation> GetLogoBlobLocation(IFormFile logoFormFile)
        {
            if (logoFormFile == null) 
            {
                return null;
            }

            using (var imageStream = logoFormFile.OpenReadStream())
            {
                var blobLocation = BlobLocations.GetCommercialDossierBlobLocation(ProjectId, logoFormFile.GetFileName());
                await _storageProvider.SavePrivate(blobLocation, imageStream);
                return blobLocation;
            }
        }

        private IntellectualPropertyViewModel MapToViewModel(IntellectualProperty intellectualProperty) 
        {
            var creativeCommonsLicense = (IntellectualProperty.License as CreativeCommonsLicense);
            
            return new IntellectualPropertyViewModel
            {
                Attribution = creativeCommonsLicense?.Attribution ?? false,
                NonCommercial = creativeCommonsLicense?.NonCommercial ?? false,
                ShareAlike = creativeCommonsLicense?.ShareAlike ?? false,
                NoDerivativeWorks = creativeCommonsLicense?.NoDerivativeWorks ?? false,
                UboraLicense = (IntellectualProperty.License as UboraLicense) != null
            };
        }
        
        private async Task<CommercialDossierViewModel> MapToViewModel(CommercialDossier commercialDossier) 
        {
            return new CommercialDossierViewModel 
            {
                ProductName = commercialDossier.ProductName,
                CommercialName = commercialDossier.CommercialName,
                DescriptionHtml = await ConvertQuillDeltaToHtml(commercialDossier.Description),
                DescriptionQuillDelta = await SanitizeQuillDeltaForEditing(commercialDossier.Description),
                LogoUrl = commercialDossier.Logo != null ? _storageProvider.GetReadUrl(commercialDossier.Logo, DateTime.UtcNow.AddSeconds(10)) : null
            };
        }
    }
}