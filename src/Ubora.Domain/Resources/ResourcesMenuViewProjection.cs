using Marten.Events.Projections;
using Ubora.Domain.Resources.Events;

namespace Ubora.Domain.Resources
{
    // http://jasperfx.github.io/marten/documentation/events/projections/custom/
    public class ResourcesMenuViewProjection : ViewProjection<ResourcesMenu, string>
    {
        public ResourcesMenuViewProjection()
        {
            ProjectEvent<ResourceCategoryEditedEvent>(viewIdSelector: _ => ResourcesMenu.SingletonId, handler: (projection, @event) => { projection.Apply(@event); });
            ProjectEvent<ResourceCategoryDeletedEvent>(viewIdSelector: _ => ResourcesMenu.SingletonId, handler: (projection, @event) => { projection.Apply(@event); });
            ProjectEvent<ResourcePageCreatedEvent>(viewIdSelector: _ => ResourcesMenu.SingletonId, handler: (projection, @event) => { projection.Apply(@event); });
            ProjectEvent<ResourcePageTitleChangedEvent>(viewIdSelector: _ => ResourcesMenu.SingletonId, handler: (projection, @event) => { projection.Apply(@event); });
            ProjectEvent<ResourcePageDeletedEvent>(viewIdSelector: _ => ResourcesMenu.SingletonId, handler: (projection, @event) => { projection.Apply(@event); });
            ProjectEvent<ResourcePageMenuPreferencesChangedEvent>(viewIdSelector: _ => ResourcesMenu.SingletonId, handler: (projection, @event) => { projection.Apply(@event); });
        }
    }
}