using System;
using System.Collections.Immutable;
using System.Linq;
using Marten.Events.Projections;
using Ubora.Domain.Resources.Events;

namespace Ubora.Domain.Resources
{
    // http://jasperfx.github.io/marten/documentation/events/projections/custom/
    public class ResourcesHierarchyViewProjection : ViewProjection<ResourcesHierarchy, string>
    {
        public ResourcesHierarchyViewProjection()
        {
            ProjectEvent<ResourceCategoryEditedEvent>(viewIdSelector: _ => ResourcesHierarchy.SingletonId, handler: (projection, @event) => { projection.Apply(@event); });
            ProjectEvent<ResourceCategoryDeletedEvent>(viewIdSelector: _ => ResourcesHierarchy.SingletonId, handler: (projection, @event) => { projection.Apply(@event); });
            ProjectEvent<ResourcePageCreatedEvent>(viewIdSelector: _ => ResourcesHierarchy.SingletonId, handler: (projection, @event) => { projection.Apply(@event); });
            ProjectEvent<ResourcePageTitleChangedEvent>(viewIdSelector: _ => ResourcesHierarchy.SingletonId, handler: (projection, @event) => { projection.Apply(@event); });
        }
    }

    public class ResourcesHierarchy
    {
        public const string SingletonId = "resources-hierarchy";

        public string Id { get; private set; }

        public ImmutableList<ILink> Links { get; set; } = ImmutableList<ILink>.Empty;

        internal void Apply(ResourceCategoryEditedEvent @event)
        {
            if (@event.ParentCategoryId != null)
            {
                if (Links.OfType<ResourceCategoryLink>().All(parentCategory => parentCategory.Id != @event.ParentCategoryId || parentCategory.ParentCategoryId == @event.CategoryId))
                {
                    throw new InvalidOperationException($"Parent does not exist or there is a circular reference. Parent ID: {@event.ParentCategoryId}");
                }
            }

            var existingCategoryLink = Links.SingleOrDefault(link => link.Id == @event.CategoryId);
            if (existingCategoryLink == null)
            {
                Links = Links.Add(new ResourceCategoryLink
                {
                    Id = @event.CategoryId,
                    ParentCategoryId = @event.ParentCategoryId,
                    Title = @event.Title,
                    Description = @event.Description,
                    Slug = @event.Slug
                });
            }
            else
            {
                // TODO
            }
        }

        internal void Apply(ResourceCategoryDeletedEvent @event)
        {
            if (Links.Any(link => link.ParentCategoryId == @event.CategoryId))
            {
                throw new InvalidOperationException("Can not delete a category which has children.");
            }
            Links = Links.Remove(Links.OfType<ResourceCategoryLink>().Single(l => l.Id == @event.CategoryId));
        }

        internal void Apply(ResourcePageCreatedEvent @event)
        {
            if (@event.ParentCategoryId.HasValue)
            {
                if (Links.OfType<ResourceCategoryLink>().All(category => category.Id != @event.ParentCategoryId))
                {
                    throw new InvalidOperationException($"Parent category does not exist. Parent ID: {@event.ParentCategoryId}");
                }
            }
            

            Links = Links.Add(new ResourcePageLink
            {
                Id = @event.ResourcePageId,
                ParentCategoryId = @event.ParentCategoryId,
                Title = @event.Title,
                Slug = @event.Slug
            });
        }

        internal void Apply(ResourcePageTitleChangedEvent @event)
        {
            //Links = Links.Remove(Links.OfType<ResourcePageLink>().Single(l => l.Id == @event.));
        }
    }
}