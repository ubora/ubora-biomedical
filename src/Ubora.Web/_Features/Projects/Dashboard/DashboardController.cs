using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;

namespace Ubora.Web._Features.Projects.Dashboard
{
    [Authorize(Policy = nameof(Policies.FromProject))]
    public class DashboardController : ProjectController
    {
        public DashboardController(ICommandQueryProcessor processor) : base(processor)
        {
        }

        [Route("dashboard")]
        public IActionResult Dashboard()
        {
            return RedirectToAction("StepTwo", "Workpackages", new { ProjectId });
        }
    }
}