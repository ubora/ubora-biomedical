using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Events;
using Ubora.Web._Areas.ResourcesArea.ResourcePages;
using Ubora.Web._Areas.ResourcesArea.ResourcePages.Models;
using Ubora.Web._Areas.ResourcesArea._Shared;

namespace Ubora.Web._Areas.ResourcesArea.Index
{
    [Route("resources")]
    public class IndexController : ResourcesAreaController
    {
        [Route("")]
        public IActionResult Index()
        {
            IEnumerable<ResourceIndexViewModel> models =
                QueryProcessor
                    .Find(new MatchAll<ResourcePage>())
                    .OrderBy(x => x.MenuPriority)
                    .ThenBy(x => x.Title)
                    .Select(resource => new ResourceIndexViewModel
                    {
                        ResourceId = resource.Id,
                        Title = resource.Title
                    });

            return View(nameof(Index), models);
        }
    }
}
