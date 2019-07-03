using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain;
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
        // NOTE: We are using HttpPost to move data in request body.

        public IActionResult StepOne()
        {
            return View(new StepOneModel());
        }

        [HttpPost]
        public IActionResult StepOne(StepOneModel model)
        {
            return View(model);
        }

        [HttpPost("tags")]
        public virtual IActionResult StepTwo(StepOneModel stepOneModel)
        {
            if (!ModelState.IsValid)
            {
                stepOneModel.RestoreStepOneUrl = true;
                return View("StepOne", stepOneModel);
            }

            var stepTwoModel = new StepTwoModel
            {
                Title = stepOneModel.Title,
                Description = stepOneModel.Description,
                AreaOfUsageTag = stepOneModel.AreaOfUsageTag,
                ClinicalNeedTag = stepOneModel.ClinicalNeedTag,
                PotentialTechnologyTag = stepOneModel.PotentialTechnologyTag,
                Keywords = stepOneModel.Keywords
            };

            return View(stepTwoModel);
        }

        [HttpPost("finalize")]
        public IActionResult Finalize(StepTwoModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("StepTwo", model);
            }

            var clinicalNeedId = Guid.NewGuid();
            ExecuteUserCommand(new IndicateClinicalNeedCommand
            {
                ClinicalNeedId = clinicalNeedId,
                Title = model.Title,
                Description = new QuillDelta(model.Description),
                ClinicalNeedTag = model.ClinicalNeedTag,
                AreaOfUsageTag = model.AreaOfUsageTag,
                PotentialTechnologyTag = model.PotentialTechnologyTag,
                Keywords = model.Keywords
            }, Notice.None("Visual feedback obvious enough."));

            if (!ModelState.IsValid)
            {
                return View("StepTwo", model);
            }

            return RedirectToAction(nameof(OverviewController.Overview), nameof(OverviewController).RemoveSuffix(), new { clinicalNeedId });
        }
    }
}