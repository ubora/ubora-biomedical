using System;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;

namespace Ubora.Web._Features.Projects.Dashboard
{
    public class DashboardController : ProjectController
    {
        public DashboardController(ICommandQueryProcessor processor) : base(processor)
        {
        }

        public IActionResult Dashboard(Guid id)
        {
            return RedirectToAction("StepTwo", "Workpackages", new {id});
        }
    }
}