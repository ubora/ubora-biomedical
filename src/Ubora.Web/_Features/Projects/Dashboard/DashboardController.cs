using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;

namespace Ubora.Web._Features.Projects.Dashboard
{
    public class DashboardController : ProjectController
    {
        public DashboardController(ICommandQueryProcessor processor) : base(processor)
        {
        }

        [Route(nameof(Dashboard))]
        public IActionResult Dashboard()
        {
            return RedirectToAction("Index", "WorkpackageOne", new { ProjectId });
        }
    }
}