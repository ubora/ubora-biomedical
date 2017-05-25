using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;

namespace Ubora.Web._Features.ProjectList
{
    public class ListController : UboraController
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