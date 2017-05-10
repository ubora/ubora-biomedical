using System;
using Microsoft.AspNetCore.Mvc;

namespace Ubora.Web._Features.Projects.Dashboard
{
    public class DashboardController : ProjectController
    {
        public IActionResult Dashboard(Guid id)
        {
            return RedirectToAction("StepTwo", "Workpackages", new { id });
        }
    }
}
