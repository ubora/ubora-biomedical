using System.Linq;
using Marten;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Resources;
using Ubora.Web._Areas.ResourcesArea.ResourcePageCreation;
using Ubora.Web._Areas.ResourcesArea.ResourcePages;
using Ubora.Web._Areas.ResourcesArea._Shared;

namespace Ubora.Web._Areas.ResourcesArea.ResourcesMenus
{
    public class ResourcesMenusController : ResourcesAreaController
    {
        private readonly IQuerySession _querySession;

        public ResourcesMenusController(IQuerySession querySession)
        {
            _querySession = querySession;
        }

        public RedirectToActionResult HighestPriorityResourcePage()
        {
            var highestPriorityResourcePageLink = _querySession.Load<ResourcesMenu>(ResourcesMenu.SingletonId)?.HighestPriorityResourcePageLink;
            if (highestPriorityResourcePageLink == null)
            {
                return RedirectToAction(nameof(ResourcePageCreationController.Add), nameof(ResourcePageCreationController).RemoveSuffix());
            }
            return RedirectToAction(nameof(ResourcePagesController.Read), nameof(ResourcePagesController).RemoveSuffix(), new { resourcePageId = highestPriorityResourcePageLink.Id });
        }
    }
}
