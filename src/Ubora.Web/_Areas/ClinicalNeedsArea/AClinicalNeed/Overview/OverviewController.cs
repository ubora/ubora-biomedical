using Microsoft.AspNetCore.Mvc;
using Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Overview.Models;

namespace Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Overview
{
    public class OverviewController : AClinicalNeedController
    {
        public IActionResult Overview()
        {
            var model = new OverviewViewModel
            {
                Title = ClinicalNeed.Title,
                Description = ClinicalNeed.Description,
                AreaOfUsageTag = ClinicalNeed.AreaOfUsageTag,
                ClinicalNeedTag = ClinicalNeed.ClinicalNeedTag,
                PotentialTechnologyTag = ClinicalNeed.PotentialTechnologyTag,
                Keywords = ClinicalNeed.Keywords,
                IndicatedAt = ClinicalNeed.IndicatedAt
            };
            return View(model);
        }
    }
}
