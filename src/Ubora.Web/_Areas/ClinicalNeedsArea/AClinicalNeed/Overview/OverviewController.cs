using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Ubora.Web.Infrastructure;
using Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Overview.Models;

namespace Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Overview
{
    public class OverviewController : AClinicalNeedController
    {
        [RestoreModelStateFromTempData]
        public async Task<IActionResult> Overview()
        {
            var model = new OverviewViewModel
            {
                Id = ClinicalNeed.Id,
                Title = ClinicalNeed.Title,
                Description = ClinicalNeed.Description,
                AreaOfUsageTag = ClinicalNeed.AreaOfUsageTag,
                ClinicalNeedTag = ClinicalNeed.ClinicalNeedTag,
                PotentialTechnologyTag = ClinicalNeed.PotentialTechnologyTag,
                Keywords = ClinicalNeed.Keywords,
                IndicatedAt = ClinicalNeed.IndicatedAt,
                IndicatorUserId = ClinicalNeed.IndicatorUserId,
            };

            return View(model);
        }
    }
}