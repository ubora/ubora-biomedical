using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;

namespace Ubora.Web._Features.Projects.Workpackages
{
    [ProjectRoute("[controller]")]
    public class WorkpackagesController : ProjectController
    {
        public WorkpackagesController(ICommandQueryProcessor processor) : base(processor)
        {
        }

        public IActionResult OverviewOfAll()
        {
            return View();
        }
    }
}