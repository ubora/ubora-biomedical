using Microsoft.AspNetCore.Mvc;
using Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.RelatedProjects.Queries;

namespace Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.RelatedProjects
{
    public class RelatedProjectsController : AClinicalNeedController
    {
        [HttpGet("related-projects")]
        public IActionResult RelatedProjects()
        {
            return View(QueryProcessor.ExecuteQuery(new RelatedProjectsQuery { ClinicalNeedId = ClinicalNeed.Id }));
        }
    }
}
 