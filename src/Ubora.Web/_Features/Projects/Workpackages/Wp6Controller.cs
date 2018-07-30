using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ubora.Web._Features.Projects._Shared;

namespace Ubora.Web._Features.Projects.Workpackages
{
    [ProjectRoute("wp6")]
    public class Wp6Controller : ProjectController
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            ViewData["MenuOption"] = ProjectMenuOption.Workpackages;
            ViewData["WorkpackageMenuOption"] = WorkpackageMenuOption.WP4StructuredInformationOnTheDevice;
        }

        [Route("proto")]
        public IActionResult Proto()
        {
            return View();
        }
    }
}
