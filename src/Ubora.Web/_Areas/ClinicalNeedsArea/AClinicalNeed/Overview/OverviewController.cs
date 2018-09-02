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
                DescriptionQuillDelta = await ConvertQuillDeltaToHtml(ClinicalNeed.Description),
                AreaOfUsageTag = ClinicalNeed.AreaOfUsageTag,
                ClinicalNeedTag = ClinicalNeed.ClinicalNeedTag,
                PotentialTechnologyTag = ClinicalNeed.PotentialTechnologyTag,
                Keywords = ClinicalNeed.Keywords,
            };

            return View(model);
        }
    }
}