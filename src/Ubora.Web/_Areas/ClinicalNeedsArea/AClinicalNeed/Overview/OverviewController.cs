using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Overview.Models;

namespace Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.Overview
{
    public class OverviewController : AClinicalNeedController
    {
        public async Task<IActionResult> Overview([FromServices]INodeServices nodeServices)
        {
            var model = new OverviewViewModel
            {
                Id = ClinicalNeed.Id,
                Title = ClinicalNeed.Title,
                Description = await nodeServices.InvokeAsync<string>("./Scripts/backend/ConvertQuillDeltaToHtml.js", ClinicalNeed.Description.Value),
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