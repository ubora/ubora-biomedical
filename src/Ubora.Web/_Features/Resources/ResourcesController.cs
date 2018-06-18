using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Extensions;
using Marten.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Resources;
using Ubora.Domain.Resources.Commands;
using Ubora.Domain.Resources.Queries;
using Ubora.Web.Authorization;
using Ubora.Web._Features.Resources.Models;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features.Resources
{
    public class ResourcesController : UboraController
    {
        private readonly IEventStore _eventStore;

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewData["IsResourcesArea"] = true;
        }

        // TODO: Use something less 'powerful' than EventStore
        public ResourcesController(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        [Route("resources")]
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

            return View("Index", models);
        }

        public virtual IActionResult Add()
        {
            return View(nameof(Add));
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddResourcePostModel model)
        {
            if (!await AuthorizationService.IsAuthorizedAsync(User, Policies.CanAddResourcePage))
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

            return RedirectToAction(nameof(Read), new { slug = resourcePage.Slug });
        }

        [HttpPost]
        public IActionResult Delete(DeleteResourcePageCommand command)
        {
            ExecuteUserCommand(command, Notice.Success("TODO"));

            return View(nameof(Delete));
        }

        [Route("r/{slug}")]
        public async Task<IActionResult> Read(string slug, [FromServices]ResourceReadViewModel.Factory modelFactory)
        {
            var resourcePage = QueryProcessor.ExecuteQuery(new FindResourcePageBySlugQuery(slug));
            var model = await modelFactory.Create(resourcePage);
            return View(nameof(Read), model);
        }

        [Route("r/{slug}/edit")]
        public IActionResult Edit(string slug, [FromServices]ResourceEditViewModel.Factory modelFactory)
        {
            var resourcePage = QueryProcessor.ExecuteQuery(new FindResourcePageBySlugQuery(slug));
            var model = modelFactory.Create(resourcePage);
            return View(nameof(Edit), model);
        }

        [HttpPost]
        [Route("r/{slug}/edit")]
        public IActionResult Edit(ResourceEditPostModel model, [FromServices]ResourceEditViewModel.Factory modelFactory)
        {
            var resource = QueryProcessor.FindById<ResourcePage>(model.ResourceId);
            if (resource == null)
                return NotFound();

            if (!ModelState.IsValid)
                return Edit(resource.Slug.Value, modelFactory);

            ExecuteUserCommand(
                new EditResourceContentCommand
                {
                    ResourceId = model.ResourceId,
                    Content = new ResourceContent(
                        title: resource.Content.Title,
                        body: new QuillDelta(model.Body)),
                    PreviousContentVersion = model.ContentVersion
                },
                successNotice: Notice.Success("TODO"));

            if (!ModelState.IsValid)
                return Edit(resource.Slug.Value, modelFactory);

            return RedirectToAction(nameof(Read), new { slug = resource.Slug });
        }

        [Route("resources/{slug}/history")]
        public async Task<IActionResult> History(string slug)
        {
            var resourcePage = QueryProcessor.ExecuteQuery(new FindResourcePageBySlugQuery(slug)); 

            var resourceEvents =
                (await _eventStore.FetchStreamAsync(
                    streamId: resourcePage.Id))
                .OrderByDescending(martenEvent => martenEvent.Timestamp)
                .Select(martenEvent => martenEvent.Data)
                .Cast<UboraEvent>();

            ResourceHistoryViewModel model = new ResourceHistoryViewModel
            {
                ResourceId = resourcePage.Id,
                Title = resourcePage.Content.Title,
                Events = resourceEvents.ToList(),
            };

            return View(nameof(History), model);
        }

        [Route("resources/slugify")]
        public string Slugify(string text)
        {
            return Url.Action(nameof(Read), "Resources", new { slug = Slug.Generate(text).Value }, protocol: HttpContext.Request.Scheme);
        }
    }
}