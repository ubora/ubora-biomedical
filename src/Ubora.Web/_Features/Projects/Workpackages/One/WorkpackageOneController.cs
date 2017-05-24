using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;

namespace Ubora.Web._Features.Projects.Workpackages.One
{
    public class WorkpackageOneController : ProjectController
    {
        public WorkpackageOneController(ICommandQueryProcessor processor) : base(processor)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
