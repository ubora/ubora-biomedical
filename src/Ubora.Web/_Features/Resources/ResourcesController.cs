using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Extensions;
using Marten.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Commands;
using Ubora.Domain.Resources.Queries;
using Ubora.Web.Authorization;
using Ubora.Web._Features.Resources.Models;
using Ubora.Web._Features._Shared.Notices;
using Ubora.Domain.Resources.Specifications;
using Ubora.Web.Infrastructure.Extensions;
using TwentyTwenty.Storage;
using Ubora.Web._Features.Projects.History._Base;

namespace Ubora.Web._Features.Resources
{
    [Route("resources")]
    public class ResourcesController : UboraController
    {
        private readonly IEventStore _eventStore;

        // TODO: Use something less 'powerful' than EventStore
        public ResourcesController(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        [Route("")]
        public IActionResult Index()
        {
            IEnumerable<ResourceIndexViewModel> models =
                QueryProcessor
                    .Find(new MatchAll<ResourcePage>())
                    .Select(resource => new ResourceIndexViewModel
                    {
                        ResourceId = resource.Id,
                        Title = resource.Content.Title
                    });

            return View(nameof(Index), models);
        }

        [Authorize(Policies.CanManageResourcePages)]
        [Route("add")]
        public virtual IActionResult Add()
        {
            ViewData["currentTab"] = nameof(Add);

            return View(nameof(Add));
        }

        [Authorize(Policies.CanManageResourcePages)]
        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add(AddResourcePostModel model)
        {
            ViewData["currentTab"] = nameof(Add);

            if (!await AuthorizationService.IsAuthorizedAsync(User, Policies.CanManageResourcePages))
                return Unauthorized();

            if (!ModelState.IsValid)
                return Add();

            var resourceId = Guid.NewGuid();
            ExecuteUserCommand(
                new CreateResourcePageCommand
                {
                    ResourceId = resourceId,
                    Content = new ResourceContent(
                        title: model.Title,
                        body: new QuillDelta(model.Body))
                },
                successNotice: Notice.Success("TODO"));

            var resourcePage = QueryProcessor.FindById<ResourcePage>(resourceId);

            if (!ModelState.IsValid)
                return Add();

            return RedirectToAction(nameof(Read), new { slugOrId = resourcePage.ActiveSlug });
        }

        //[Route("{slugOrId}/delete")]
        //[HttpPost]
        //public IActionResult Delete(DeleteResourcePageCommand command)
        //{
        //    ExecuteUserCommand(command, Notice.Success("TODO"));

        //    return View(nameof(Delete));
        //}

        [Route("{slugOrId}")]
        public async Task<IActionResult> Read(string slugOrId, [FromServices]ResourceReadViewModel.Factory modelFactory)
        {
            ViewData["currentTab"] = nameof(Read);

            var resourcePage = QueryProcessor.ExecuteQuery(new FindResourcePageBySlugOrIdQuery(slugOrId));
            var model = await modelFactory.Create(resourcePage);
            return View(nameof(Read), model);
        }

        [Route("{slugOrId}/repository")]
        public IActionResult Repository(string slugOrId)
        {
            ViewData["currentTab"] = nameof(Repository);

            var resourcePage = QueryProcessor.ExecuteQuery(new FindResourcePageBySlugOrIdQuery(slugOrId));

            var model =
                new IndexResourceFilesViewModel(
                    files: QueryProcessor.Find(new IsFileFromResourcePageSpec(resourcePage.Id), new ResourceFileListItemViewModel.Projection()),
                    resourcePageId: resourcePage.Id,
                    resourcePageName: resourcePage.Content.Title);

            return View(nameof(Repository), model);
        }

        [Authorize(Policies.CanManageResourcePages)]
        [Route("{slugOrId}/add-file")]
        public IActionResult AddFile(string slugOrId)
        {
            ViewData["currentTab"] = nameof(Repository);

            var resourcePage = QueryProcessor.ExecuteQuery(new FindResourcePageBySlugOrIdQuery(slugOrId));

            var model = new AddResourceFileViewModel(resourcePage.Id, resourcePage.Content.Title);

            return base.View(nameof(AddFile), model);
        }

        [Authorize(Policies.CanManageResourcePages)]
        [Route("{slugOrId}/add-file")]
        [HttpPost]
        public IActionResult AddFile(AddResourceFilePostModel model)
        {
            ViewData["currentTab"] = nameof(Repository);

            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }

            foreach (var file in model.ProjectFiles)
                using (var fileStream = file.OpenReadStream())
                {
                    ExecuteUserCommand(new UploadFileToResourceRepositoryCommand
                    {
                        FileId = Guid.NewGuid(),
                        FileName = file.GetFileName(),
                        FileSize = file.Length,
                        FileStream = file.OpenReadStream(),
                        ResourcePageId = model.ResourcePageId,
                    }, Notice.Success(SuccessTexts.RepositoryFileAdded));
                }

            if (!ModelState.IsValid)
            {
                return ModelState.ToJsonResult();
            }

            return Json(new { redirect = Url.Action(nameof(Repository)) });
        }

        [Route("{resourcePageId}/repository/{fileId}")]
        public IActionResult DownloadFile(Guid resourcePageId, Guid fileId, [FromServices] IStorageProvider storageProvider)
        {
            var file = QueryProcessor.FindById<ResourceFile>(fileId);
            if (file == null)
                return NotFound();

            if (file.ResourcePageId != resourcePageId)
                return NotFound();

            var blobSasUrl = storageProvider.GetBlobUrl(file.BlobLocation.ContainerName, file.BlobLocation.BlobPath);

            return Redirect(blobSasUrl);
        }

        [Authorize(Policies.CanManageResourcePages)]
        [Route("{slugOrId}/edit")]
        public IActionResult Edit(string slugOrId, [FromServices]ResourceEditViewModel.Factory modelFactory)
        {
            ViewData["currentTab"] = nameof(Edit);

            var resourcePage = QueryProcessor.ExecuteQuery(new FindResourcePageBySlugOrIdQuery(slugOrId));
            var model = modelFactory.Create(resourcePage);
            return View(nameof(Edit), model);
        }

        [Authorize(Policies.CanManageResourcePages)]
        [HttpPost]
        [Route("{slugOrId}/edit")]
        public IActionResult Edit(ResourceEditPostModel model, [FromServices]ResourceEditViewModel.Factory modelFactory)
        {
            ViewData["currentTab"] = nameof(Edit);

            var resource = QueryProcessor.FindById<ResourcePage>(model.ResourceId);
            if (resource == null)
                return NotFound();

            if (!ModelState.IsValid)
                return Edit(resource.ActiveSlug.Value, modelFactory);

            ExecuteUserCommand(
                new EditResourceContentCommand
                {
                    ResourceId = model.ResourceId,
                    Content = new ResourceContent(
                        title: resource.Content.Title,
                        body: new QuillDelta(model.Body)),
                    PreviousContentVersion = model.ContentVersion
                },
                successNotice: Notice.Success("Resource edited"));

            if (!ModelState.IsValid)
                return Edit(resource.ActiveSlug.Value, modelFactory);

            return RedirectToAction(nameof(Read), new { slugOrId = resource.ActiveSlug });
        }

        [Route("{slugOrId}/history")]
        public async Task<IActionResult> History(string slugOrId, [FromServices] IEventViewModelFactoryMediator eventViewModelFactoryMediator)
        {
            ViewData["currentTab"] = nameof(History);

            var resourcePage = QueryProcessor.ExecuteQuery(new FindResourcePageBySlugOrIdQuery(slugOrId));

            var resourceEvents =
                (await _eventStore.FetchStreamAsync(
                    streamId: resourcePage.Id))
                .OrderByDescending(martenEvent => martenEvent.Timestamp);

            ResourceHistoryViewModel model = new ResourceHistoryViewModel
            {
                ResourceId = resourcePage.Id,
                Title = resourcePage.Content.Title,
                Events = resourceEvents.Select(x => eventViewModelFactoryMediator.Create((UboraEvent)x.Data, x.Timestamp)).ToList()
            };

            return View(nameof(History), model);
        }

        [Route("slugify")]
        public string Slugify(string text)
        {
            return Url.Action(nameof(Read), "Resources", new { slugOrId = Slug.Generate(text).Value }, protocol: HttpContext.Request.Scheme);
        }
    }
}