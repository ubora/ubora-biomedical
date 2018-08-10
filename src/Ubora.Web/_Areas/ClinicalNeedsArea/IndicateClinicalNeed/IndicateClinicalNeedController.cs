using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.ClinicalNeeds.Commands;
using Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Overview;
using Ubora.Web._Areas.ClinicalNeedsArea.IndicateClinicalNeed.Models;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Areas.ClinicalNeedsArea.IndicateClinicalNeed
{
    [Authorize(Policies.CanIndicateClinicalNeeds)]
    [Route("clinical-needs/indicate-a-new-need")]
    public class IndicateClinicalNeedController : ClinicalNeedsAreaController
    {
        [AcceptVerbs("GET", "POST")]
        public IActionResult StepOne(StepOneModel model = null)
        {
            if (Request.Method == HttpMethods.Get)
            {
                ModelState.Clear();
            }

            return View(model);
        }

        [HttpPost("tags")]
        public virtual IActionResult StepTwo(StepTwoModel model, bool validate = false)
        {
            if (!validate)
            {
                ModelState.Clear();
            }

            return View(model);
        }

        [HttpPost("finalize")]
        public async Task<IActionResult> Finalize(StepTwoModel model)
        {
            if (!ModelState.IsValid)
            {
                return StepTwo(model, true);
            }

            var clinicalNeedId = Guid.NewGuid();
            ExecuteUserCommand(new IndicateClinicalNeedCommand
            {
                ClinicalNeedId = clinicalNeedId,
                Title = model.Title,
                Description = model.Description,
                ClinicalNeedTag = model.ClinicalNeedTag,
                AreaOfUsageTag = model.AreaOfUsageTag,
                PotentialTechnologyTag = model.PotentialTechnologyTag,
                Keywords = model.Keywords
            }, Notice.Success("TODO"));

            if (!ModelState.IsValid)
            {
                return StepTwo(model, true);
            }

            return RedirectToAction(nameof(OverviewController.Overview), nameof(OverviewController).RemoveSuffix(), new { clinicalNeedId });
        }
    }
}