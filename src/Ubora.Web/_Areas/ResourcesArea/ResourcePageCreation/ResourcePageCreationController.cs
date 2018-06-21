using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Commands;
using Ubora.Web._Areas.ResourcesArea.ResourcePageCreation.Models;
using Ubora.Web._Areas.ResourcesArea.ResourcePages;
using Ubora.Web._Areas.ResourcesArea._Shared;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Areas.ResourcesArea.ResourcePageCreation
{
    public class ResourcePageCreationController : ResourcesAreaController
    {
        [Authorize(Policies.CanManageResourcePages)]
        [Route("create")]
        public virtual IActionResult Add()
        {
            return View();
        }

        [Authorize(Policies.CanManageResourcePages)]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Add(AddResourcePostModel model)
        {
            if (!await AuthorizationService.IsAuthorizedAsync(User, Policies.CanManageResourcePages))
                return Unauthorized();

            if (!ModelState.IsValid)
                return Add();

            var resourcePageId = Guid.NewGuid();
            ExecuteUserCommand(
                new CreateResourcePageCommand
                {
                    ResourcePageId = resourcePageId,
                    Content = new ResourceContent(
                        title: model.Title,
                        body: new QuillDelta(model.Body)),
                    MenuPriority = model.MenuPriority
                },
                successNotice: Notice.Success("Resource page created"));

            if (!ModelState.IsValid)
                return Add();

            return RedirectToAction(nameof(ResourcePagesController.Read), nameof(ResourcePagesController).RemoveSuffix(), new { slugOrId = resourcePageId });
        }
    }
}
