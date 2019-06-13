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
            ViewData[nameof(ProjectMenuOption)] = ProjectMenuOption.Workpackages;
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
                UboraLicense = model.IsUboraLicense,
                NonCommercial = model.NonCommercial 
            }, Notice.Success(SuccessTexts.WP5LicenseEdited));

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

            EditCommercialDossierCommand command = new EditCommercialDossierCommand
            {
                CommercialName = model.CommercialName ?? "",
                ProductName = model.ProductName ?? "",
                Description = new QuillDelta(model.DescriptionQuillDelta),
                LogoLocation = await GetLogoBlobLocation(model.Logo) ?? (model.HasOldLogoBeenDeleted ? null : CommercialDossier.Logo)
            };

            
            if (model.UserManual != null) 
            {
                BlobLocation blobLocation;
                using (var userManualStream = model.UserManual.OpenReadStream())
                {
                    blobLocation = BlobLocations.GetCommercialDossierBlobLocation(ProjectId, model.UserManual.GetFileName());
                    await _storageProvider.SavePrivate(blobLocation, userManualStream);
                }
                command.UserManualLocation = blobLocation;
                command.UserManualFileName = model.UserManual.GetFileName();
                command.UserManualFileSize = model.UserManual.Length;
            } else 
            {
                if (!model.HasOldUserManualBeenRemoved) 
                {
                    command.UserManualLocation = CommercialDossier.UserManual?.Location;
                    command.UserManualFileName = CommercialDossier.UserManual?.FileName;
                    command.UserManualFileSize = CommercialDossier.UserManual?.FileSize ?? 0;  
                }
            }

            ExecuteUserProjectCommand(command, Notice.Success(SuccessTexts.WP5CommercialDossierEdited));

            if (!ModelState.IsValid) 
            {
                return await EditCommercialDossier();
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet("DownloadUserManual")]
        public IActionResult DownloadUserManual() 
        {
            var blobSasUrl = _storageProvider.GetReadUrl(CommercialDossier.UserManual.Location, DateTime.UtcNow.AddSeconds(5));

            return Redirect(blobSasUrl);
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
            switch (IntellectualProperty.License) 
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
                        IsUboraLicense = true
                    };
                default:
                    return new IntellectualPropertyViewModel 
                    {
                        License = LicenseType.None
                    };
            }
        }
        
        private async Task<CommercialDossierViewModel> MapToViewModel(CommercialDossier commercialDossier) 
        { 
            return new CommercialDossierViewModel 
            {
                ProductName = commercialDossier.ProductName,
                CommercialName = commercialDossier.CommercialName,
                DescriptionHtml = await ConvertQuillDeltaToHtml(commercialDossier.Description),
                DescriptionQuillDelta = await SanitizeQuillDeltaForEditing(commercialDossier.Description),
                DoesDescriptionHaveContent = commercialDossier.Description != new QuillDelta(),
                LogoUrl = commercialDossier.Logo != null ? _storageProvider.GetReadUrl(commercialDossier.Logo, DateTime.UtcNow.AddSeconds(10)) : null,
                UserManualName = commercialDossier.UserManual?.FileName
            };
        }
    }
}