using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Marten.Events;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Agreement;
using Ubora.Domain.Infrastructure;
using Ubora.Domain.Infrastructure.Events;
using Ubora.Domain.Infrastructure.Specifications;
using Ubora.Domain.Resources;
using Ubora.Web._Features._Shared.Notices;

namespace Ubora.Web._Features.Resources
{
    public class ResourceIndexViewModel
    {
        public Guid ResourceId { get; set; }
        public string Title { get; set; }

        public class Mapper : Projection<Resource, ResourceIndexViewModel>
        {
            protected override Expression<Func<Resource, ResourceIndexViewModel>> ToSelector()
            {
                return resource => new ResourceIndexViewModel
                {
                    ResourceId = resource.Id,
                    Title = resource.Content.Title
                };
            }
        }
    }

    public class ResourceReadViewModel
    {
        public Guid ResourceId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }

    public class ResourceEditViewModel : ResourceEditPostModel
    {
        public string Title { get; set; }
    }

    public class ResourceEditPostModel
    {
        public Guid ResourceId { get; set; }
        public string Body { get; set; }
        public Guid ContentVersion { get; set; }
    }

    public class ResourceHistoryViewModel
    {
        public Guid ResourceId { get; set; }
        public IReadOnlyCollection<UboraEvent> Events { get; set; }
    }

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

        public IActionResult Index()
        {
            IEnumerable<ResourceIndexViewModel> models =
                QueryProcessor
                    .Find(new MatchAll<Resource>())
                    .Select(resource => new ResourceIndexViewModel
                    {
                        ResourceId = resource.Id,
                        Title = resource.Content.Title
                    });

            return View("Index", models);
        }

        public IActionResult Add()
        {
            return View(nameof(Add));
        }

        [HttpPost]
        public IActionResult Add(AddResourcePostModel model)
        {
            if (!ModelState.IsValid)
                return Add();

            var resourceId = Guid.NewGuid();
            ExecuteUserCommand(
                new CreateResourcePageCommand
                {
                    ResourceId = resourceId,
                    Content = new ResourceContent(
                        title: model.Title,
                        body: model.Body)
                },
                successNotice: Notice.Success("TODO"));

            if (!ModelState.IsValid)
                return Add();

            return RedirectToAction(nameof(Read), new {id = resourceId});
        }

        [HttpPost]
        public IActionResult Delete(DeleteResourcePageCommand command)
        {
            ExecuteUserCommand(command, Notice.Success("TODO"));

            return View(nameof(Delete));
        }

        public IActionResult Read(Guid id)
        {
            ResourceReadViewModel model =
                QueryProcessor
                    .FindById<Resource>(id)
                    .ThenReturn(resource => new ResourceReadViewModel
                    {
                        ResourceId = resource.Id,
                        Body = resource.Content.Body,
                        Title = resource.Content.Title
                    });

            return View(nameof(Read), model);
        }

        public IActionResult Edit(Guid id)
        {
            ResourceEditViewModel model =
                QueryProcessor
                    .FindById<Resource>(id)
                    .ThenReturn(resource => new ResourceEditViewModel
                    {
                        ResourceId = resource.Id,
                        Body = resource.Content.Body,
                        Title = resource.Content.Title,
                        ContentVersion = resource.ContentVersion
                    });

            return View(nameof(Edit), model);
        }

        [HttpPost]
        public IActionResult Edit([FromBody]ResourceEditPostModel model)
        {
            var resource = QueryProcessor.FindById<Resource>(model.ResourceId);
            if (resource == null)
                return NotFound();

            if (!ModelState.IsValid)
                return Edit(model.ResourceId);

            ExecuteUserCommand(
                new EditResourceContentCommand
                {
                    ResourceId = model.ResourceId,
                    Content = new ResourceContent(
                        title: resource.Content.Title,
                        body: model.Body),
                    PreviousContentVersion = model.ContentVersion
                },
                successNotice: Notice.Success("TODO"));

            if (!ModelState.IsValid)
                return Edit(model.ResourceId);

            return RedirectToAction(nameof(Read), new {id = model.ResourceId});
        }

        public async Task<IActionResult> History(Guid id)
        {
            var resourceEvents =
                (await _eventStore.FetchStreamAsync(
                    streamId: id))
                .OrderByDescending(martenEvent => martenEvent.Timestamp)
                .Select(martenEvent => martenEvent.Data)
                .Cast<UboraEvent>();

            ResourceHistoryViewModel model = new ResourceHistoryViewModel
            {
                ResourceId = id,
                Events = resourceEvents.ToList(),
            };
            
            return View(nameof(History), model);
        }
    }
}