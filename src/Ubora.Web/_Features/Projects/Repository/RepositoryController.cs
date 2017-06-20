using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ubora.Domain.Infrastructure;

namespace Ubora.Web._Features.Projects.Repository
{
    public class RepositoryController : ProjectController
    {
        public RepositoryController(ICommandQueryProcessor processor) : base(processor)
        {
        }

        [Route(nameof(Repository))]
        public IActionResult Repository()
        {
            var model = new ProjectRepositoryViewModel();
            return View(model);
        }
    }
}
