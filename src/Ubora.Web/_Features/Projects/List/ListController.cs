using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;

namespace Ubora.Web._Features.Projects.List
{
    public class ListController : ProjectController
    {
        public ListController(ICommandQueryProcessor processor) : base(processor)
        {
        }

        public IActionResult MyProjects()
        {
            return View();
        }
    }
}