using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using Ubora.Domain;
using Ubora.Domain.ClinicalNeeds.Commands;
using Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Edit.Models;
using Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Overview;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Edit
{
    [Route("clinical-needs/{clinicalNeedId}/edit")]
    public class EditController : AClinicalNeedController
    {
        private readonly INodeServices _nodeServices;

        public EditController(INodeServices nodeServices)
        {
            _nodeServices = nodeServices;
        }

        public virtual async Task<IActionResult> Edit()
        {
            var isAuthorized = (await AuthorizationService.AuthorizeAsync(User, ClinicalNeed.Id, Policies.CanEditClinicalNeed)).Succeeded;
            if (!isAuthorized)
                return Forbid();

            var model = new EditClinicalNeedViewModel
            {
                Title = ClinicalNeed.Title,
                DescriptionQuillDelta = await _nodeServices.InvokeAsync<string>("./Scripts/backend/SanitizeQuillDelta.js", ClinicalNeed.Description.Value),
                AreaOfUsageTag = ClinicalNeed.AreaOfUsageTag,
                PotentialTechnologyTag = ClinicalNeed.PotentialTechnologyTag,
                ClinicalNeedTag = ClinicalNeed.ClinicalNeedTag,
                Keywords = ClinicalNeed.Keywords
            };
             
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditClinicalNeedPostModel model)
        {
            var isAuthorized = (await AuthorizationService.AuthorizeAsync(User, ClinicalNeed.Id, Policies.CanEditClinicalNeed)).Succeeded;
            if (!isAuthorized)
                return Forbid();

            if (!ModelState.IsValid)
                return await Edit();

            ExecuteUserCommand(new EditClinicalNeedCommand
            {
                ClinicalNeedId = ClinicalNeed.Id,
                Title = model.Title,
                Description = new QuillDelta(model.DescriptionQuillDelta),
                ClinicalNeedTag = model.ClinicalNeedTag,
                AreaOfUsageTag = model.AreaOfUsageTag,
                PotentialTechnologyTag = model.PotentialTechnologyTag,
                Keywords = model.Keywords
            }, Notice.Success(SuccessTexts.ClinicalNeedEdited));

            if (!ModelState.IsValid)
                return await Edit();

            return RedirectToAction(nameof(OverviewController.Overview), nameof(OverviewController).RemoveSuffix());
        }
    }
}