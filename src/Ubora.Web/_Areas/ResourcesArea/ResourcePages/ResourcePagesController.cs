using Marten.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;
using TwentyTwenty.Storage;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Commands;
using Ubora.Domain.Resources.Queries;
using Ubora.Domain.Resources.Specifications;
using Ubora.Web._Features._Shared.Notices;
using Ubora.Web._Features.Projects.History._Base;
using Ubora.Web.Infrastructure.Extensions;
using Ubora.Web._Areas.ResourcesArea.Index.Models;
using Ubora.Web._Areas.ResourcesArea.ResourcePages.Models;
using Ubora.Web._Areas.ResourcesArea._Shared;
using Ubora.Web._Areas.ResourcesArea.Index;

namespace Ubora.Web._Areas.ResourcesArea.ResourcePages
{
    [Route("resources/{slugOrId}")]
    public class ResourcePagesController : ResourcesAreaController
    {
        public string SlugOrId => RouteData.Values["slugOrId"] as string;

        public ResourcePage ResourcePage { get; private set; }

        private readonly IEventStore _eventStore;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            ResourcePage = QueryProcessor.ExecuteQuery(new FindResourcePageBySlugOrIdQuery(SlugOrId));
            if (ResourcePage == null)
            {
                context.Result = new NotFoundResult();
            }
            // Redirect if URL is not to the latest slug.
            else if (!SlugOrId.Equals(ResourcePage.ActiveSlug.Value, StringComparison.OrdinalIgnoreCase))
            {
                var test = (dynamic)context.ActionDescriptor;
                context.RouteData.Values["slugOrId"] = ResourcePage.ActiveSlug.Value;
                context.Result = new RedirectToActionResult(test.ActionName, test.ControllerName, context.RouteData.Values, permanent: true);
            }
            else
            {
                ViewData["DisableFooter"] = true;
                ViewData["ResourcePageTitle"] = ResourcePage.Content.Title;

                var urlTemplateParts = context.ActionDescriptor.AttributeRouteInfo.Template.Split("/");

                // Set "current tab" (read, edit, repository, history) from the URL part after slug.
                ViewData["CurrentTab"] = urlTemplateParts
                    .SkipWhile(urlPart => urlPart != "{slugOrId}")
                    .ElementAtOrDefault(1);
            }
        }

        [AllowAnonymous]
        [Route("")]
        public async Task<IActionResult> Read([FromServices]ResourceReadViewModel.Factory modelFactory)
        {
            var model = await modelFactory.Create(ResourcePage);
            return View(model);
        }

        [AllowAnonymous]
        [Route("repository")]
        public IActionResult Repository()
        {
            var model =
                new IndexResourceFilesViewModel(
                    files: QueryProcessor.Find(new IsFileFromResourcePageSpec(ResourcePage.Id), new ResourceFileListItemViewModel.Projection()),
                    resourcePageId: ResourcePage.Id,
                    resourcePageName: ResourcePage.Content.Title);

            return View(model);
        }

        [Authorize(Policies.CanManageResourcePages)]
        [Route("repository/add-file")]
        public IActionResult AddFile(string slugOrId)
        {
            var model = new AddResourceFileViewModel(ResourcePage.Id, ResourcePage.Content.Title);

            return View(model);
        }

        [Authorize(Policies.CanManageResourcePages)]
        [Route("repository/add-file")]
        [HttpPost]
        public IActionResult AddFile(AddResourceFilePostModel model)
        {
            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }

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
        [Route("repository/{fileId}")]
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

        [Authorize(Policies.CanManageResourcePages)]
        [Route("edit")]
        public IActionResult Edit(string slugOrId, [FromServices]ResourceEditViewModel.Factory modelFactory)
        {
            var model = modelFactory.Create(ResourcePage);
            return View(model);
        }

        [Authorize(Policies.CanManageResourcePages)]
        [HttpPost]
        [Route("edit")]
        public IActionResult Edit(ResourceEditPostModel model, [FromServices]ResourceEditViewModel.Factory modelFactory)
        {
            if (!ModelState.IsValid)
                return Edit(ResourcePage.ActiveSlug.Value, modelFactory);

            ExecuteUserCommand(
                new EditResourceContentCommand
                {
                    ResourceId = model.ResourceId,
                    Content = new ResourceContent(
                        title: model.Title,
                        body: new QuillDelta(model.Body)),
                    PreviousContentVersion = model.ContentVersion
                },
                successNotice: Notice.Success("Resource edited"));

            if (!ModelState.IsValid)
                return Edit(ResourcePage.ActiveSlug.Value, modelFactory);

            return RedirectToAction(nameof(Read), new { slugOrId = ResourcePage.ActiveSlug });
        }

        [Authorize(Policies.CanManageResourcePages)]
        [Route("edit/delete")]
        public IActionResult Delete()
        {
            return View();
        }

        [Authorize(Policies.CanManageResourcePages)]
        [HttpPost]
        [Route("edit/delete")]
        public IActionResult Delete(DeleteResourcePagePostModel postModel)
        {
            if (!ModelState.IsValid)
            {
                return Delete();
            }

            ExecuteUserCommand(new DeleteResourcePageCommand
            {
                ResourcePageId = ResourcePage.Id
            }, Notice.Success("Resource deleted"));

            if (!ModelState.IsValid)
            {
                return Delete();
            }

            return RedirectToAction(nameof(IndexController.Index), nameof(IndexController).RemoveSuffix());
        }

        [Route("history")]
        public async Task<IActionResult> History(string slugOrId, [FromServices]IEventStore eventStore, [FromServices] IEventViewModelFactoryMediator eventViewModelFactoryMediator)
        {
            var resourceEvents =
                (await eventStore.FetchStreamAsync(
                    streamId: ResourcePage.Id))
                .OrderByDescending(martenEvent => martenEvent.Timestamp);

            ResourceHistoryViewModel model = new ResourceHistoryViewModel
            {
                ResourceId = ResourcePage.Id,
                Title = ResourcePage.Content.Title,
                Events = resourceEvents.Select(x => eventViewModelFactoryMediator.Create((UboraEvent)x.Data, x.Timestamp)).ToList()
            };

            return View(model);
        }
    }
}
