using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure;

namespace Ubora.Web._Features.ProjectList
{
    public class ProjectListController : UboraController
    {
        public ProjectListController(ICommandQueryProcessor processor) : base(processor)
        {
        }

        public IActionResult MyProjects()
        {
            return View();
        }
    }
}