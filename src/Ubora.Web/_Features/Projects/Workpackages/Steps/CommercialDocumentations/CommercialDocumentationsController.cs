using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ubora.Domain;
using Ubora.Domain.Projects.CommercialDossiers;
using Ubora.Domain.Projects.CommercialDossiers.Commands;
using Ubora.Domain.Projects.IntellectualProperties;
using Ubora.Web._Features._Shared.Notices;
using Ubora.Web.Infrastructure.Storage;

namespace Ubora.Web._Features.Projects.Workpackages.Steps.CommercialDocumentations
{
    [ProjectRoute("WP5/CommercialDocumentation")]
    public class CommercialDocumentationsController : ProjectController
    {
        public const string Name = "CommercialDocumentations";
        
        private readonly IUboraStorageProvider _storageProvider;
        private readonly CommercialDossierViewModel.Factory _commercialDossierViewModelFactory;
        private readonly CommercialDossierViewModel.Helper _commercialDossierViewModelHelper;
        private readonly IntellectualPropertyViewModel.Factory _intellectualPropertyViewModelFactory;

        public CommercialDossier CommercialDossier { get; private set; }
        public IntellectualProperty IntellectualProperty { get; private set; }

        public CommercialDocumentationsController(
            IUboraStorageProvider storageProvider,
            CommercialDossierViewModel.Factory commercialDossierViewModelFactory,
            CommercialDossierViewModel.Helper commercialDossierViewModelHelper,
            IntellectualPropertyViewModel.Factory intellectualPropertyViewModelFactory) 
        {
            _storageProvider = storageProvider;
            _commercialDossierViewModelFactory = commercialDossierViewModelFactory;
            _commercialDossierViewModelHelper = commercialDossierViewModelHelper;
            _intellectualPropertyViewModelFactory = intellectualPropertyViewModelFactory;
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
            var model = new CommercialDocumentationViewModel
            {
                CommercialDossier = await _commercialDossierViewModelFactory.Create(CommercialDossier),
                IntellectualProperty = _intellectualPropertyViewModelFactory.Create(IntellectualProperty)
            };
            return View("CommercialDocumentation", model);
        }

        [HttpGet("EditIntellectualProperty")]
        public IActionResult EditIntellectualProperty() 
        {
            var model = _intellectualPropertyViewModelFactory.Create(IntellectualProperty);
            return View(model); 
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
                Attribution = model.Attribution && model.IsCreativeCommonsLicense,
                NoDerivativeWorks = model.NoDerivativeWorks && model.IsCreativeCommonsLicense,
                ShareAlike = model.ShareAlike && model.IsCreativeCommonsLicense,
                NonCommercial = model.NonCommercial && model.IsCreativeCommonsLicense,
                UboraLicense = model.IsUboraLicense
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
            var model = await _commercialDossierViewModelFactory.Create(CommercialDossier);
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
                LogoLocation = await _commercialDossierViewModelHelper.GetLogoBlobLocation(model, ProjectId, CommercialDossier.Logo),
                UserManualInfo = await _commercialDossierViewModelHelper.GetUserManualInfo(model, ProjectId, CommercialDossier.UserManual)
            }, Notice.Success(SuccessTexts.WP5CommercialDossierEdited));

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
    }
}