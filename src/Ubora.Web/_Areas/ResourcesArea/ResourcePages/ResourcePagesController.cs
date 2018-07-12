using Marten.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;
using TwentyTwenty.Storage;
using Ubora.Domain;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Commands;
using Ubora.Domain.Resources.Specifications;
using Ubora.Web._Features._Shared.Notices;
using Ubora.Web._Features.Projects.History._Base;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web._Areas.ResourcesArea.ResourcePages.Models;
using Ubora.Web._Areas.ResourcesArea.ResourcesMenus;
using Ubora.Web._Areas.ResourcesArea._Shared;

namespace Ubora.Web._Areas.ResourcesArea.ResourcePages
{
    [Route("resources/{resourcePageId}")]
    public class ResourcePagesController : ResourcesAreaController
    {
        public Guid ResourcePageId => Guid.Parse(RouteData.Values["resourcePageId"] as string);

        public virtual ResourcePage ResourcePage { get; private set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ResourcePage = QueryProcessor.FindById<ResourcePage>(ResourcePageId);
            if (ResourcePage == null)
            {
                context.Result = new NotFoundResult();
                return;
            }

            ViewData["DisableFooter"] = true;
            ViewData["ResourcePageTitle"] = ResourcePage.Title;

            var urlTemplateParts = context.ActionDescriptor.AttributeRouteInfo.Template.Split("/");

            // Set "current tab" (read, edit, repository, history) from the URL part after slug.
            ViewData["CurrentTab"] = urlTemplateParts
                .SkipWhile(urlPart => urlPart != "{resourcePageId}")
                .ElementAtOrDefault(1);
        }

        [AllowAnonymous]
        [HttpGet("")]
        public async Task<IActionResult> Read([FromServices]ResourceReadViewModel.Factory modelFactory)
        {
            var model = await modelFactory.Create(ResourcePage);
            return View(model);
        }

        [AllowAnonymous]
        [HttpGet("repository")]
        public IActionResult Repository()
        {
            var model =
                new IndexResourceFilesViewModel(
                    files: QueryProcessor.Find(new IsFileFromResourcePageSpec(ResourcePage.Id), new ResourceFileListItemViewModel.Projection()),
                    resourcePageId: ResourcePage.Id,
                    resourcePageName: ResourcePage.Title);

            return View(model);
        }

        [Authorize(Policies.CanManageResources)]
        [HttpGet("repository/add-file")]
        public IActionResult AddFile()
        {
            var model = new AddResourceFileViewModel(ResourcePage.Id, ResourcePage.Title);

            return View(model);
        }

        [HttpPost("repository/add-file")]
        public async Task<IActionResult> AddFile(AddResourceFilePostModel model)
        {
            if (!await AuthorizationService.IsAuthorizedAsync(User, Policies.CanManageResources))
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
                return ModelState.ToJsonResult();

            foreach (var file in model.ProjectFiles)
                using (var fileStream = file.OpenReadStream())
                {
                    ExecuteUserCommand(new UploadResourceFileCommand
                    {
                        FileId = Guid.NewGuid(),
                        FileName = file.GetFileName(),
                        FileSize = file.Length,
                        FileStream = fileStream,
                        ResourcePageId = ResourcePage.Id,
                    }, Notice.Success(SuccessTexts.RepositoryFileAdded));
                }

            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }

            return Json(new { redirect = Url.Action(nameof(Repository)) });
        }

        [AllowAnonymous]
        [HttpGet("repository/{fileId}")]
        public IActionResult DownloadFile(Guid fileId, [FromServices] IStorageProvider storageProvider)
        {
            var file = QueryProcessor.FindById<ResourceFile>(fileId);
            if (file == null)
                return NotFound();

            if (file.ResourcePageId != ResourcePage.Id)
                return NotFound();

            var blobSasUrl = storageProvider.GetBlobUrl(file.BlobLocation.ContainerName, file.BlobLocation.BlobPath);

            return Redirect(blobSasUrl);
        }

        [HttpGet("edit")]
        [Authorize(Policies.CanManageResources)]
        public IActionResult Edit([FromServices]ResourceEditViewModel.Factory modelFactory)
        {
            var model = modelFactory.Create(ResourcePage);
            return View(model);
        }

        [HttpPost("edit")]
        public async Task<IActionResult> Edit(ResourceEditPostModel model, [FromServices]ResourceEditViewModel.Factory modelFactory)
        {
            if (!await AuthorizationService.IsAuthorizedAsync(User, Policies.CanManageResources))
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
                return Edit(modelFactory);

            ExecuteUserCommand(
                new EditResourcePageCommand
                {
                    ResourcePageId = model.ResourceId,
                    Title = model.Title,
                    Body = new QuillDelta(model.Body),
                    PreviousContentVersion = model.ContentVersion,
                    MenuPriority = model.MenuPriority,
                    ParentCategoryId = model.ParentCategoryId
                },
                successNotice: Notice.Success("Resource edited"));

            if (!ModelState.IsValid)
                return Edit(modelFactory);

            return RedirectToAction(nameof(Read));
        }

        // TODO: Modal?
        [HttpGet("edit/delete")]
        [Authorize(Policies.CanManageResources)]
        public IActionResult Delete()
        {
            return View();
        }

        [HttpPost("edit/delete")]
        public async Task<IActionResult> Delete(DeleteResourcePagePostModel postModel)
        {
            if (!await AuthorizationService.IsAuthorizedAsync(User, Policies.CanManageResources))
            {
                return Forbid();
            }

            if (!ModelState.IsValid)
                return Delete();

            ExecuteUserCommand(new DeleteResourcePageCommand
            {
                ResourcePageId = ResourcePage.Id
            }, Notice.Success("Resource page deleted"));

            if (!ModelState.IsValid)
                return Delete();

            return RedirectToAction(nameof(ResourcesMenusController.HighestPriorityResourcePage), nameof(ResourcesMenusController).RemoveSuffix());
        }

        [HttpGet("history")]
        [Authorize(Policies.CanManageResources)]
        public async Task<IActionResult> History(string resourcePageId, [FromServices]IEventStore eventStore, [FromServices] IEventViewModelFactoryMediator eventViewModelFactoryMediator)
        {
            var resourceEvents =
                (await eventStore.FetchStreamAsync(
                    streamId: ResourcePage.Id))
                .OrderByDescending(martenEvent => martenEvent.Timestamp);

            ResourceHistoryViewModel model = new ResourceHistoryViewModel
            {
                ResourceId = ResourcePage.Id,
                Title = ResourcePage.Title,
                Events = resourceEvents.Select(x => eventViewModelFactoryMediator.Create((UboraEvent)x.Data, x.Timestamp)).ToList()
            };

            return View(model);
        }
    }
}
