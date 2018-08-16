using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Ubora.Web._Areas.ClinicalNeedsArea.AClinicalNeed.RelatedProjects
{
    public class RelatedProjectsController : AClinicalNeedController
    {
        [HttpGet("related-projects")]
        public IActionResult RelatedProjects()
        {
            return View();
        }
    }
}
 