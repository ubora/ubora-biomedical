using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.Serialization;
using Ubora.Domain.Resources.Events;

namespace Ubora.Domain.Resources
{
    public class ResourcesMenu
    {
        public const string SingletonId = "resources-menu";

        public string Id { get; private set; }

        public ImmutableList<IResourceMenuLink> Links { get; private set; } = ImmutableList<IResourceMenuLink>.Empty;

        public ResourcePageLink HighestPriorityResourcePageLink { get; private set; }

        private ResourcePageLink GetHighestPriorityResourcePageLink(IEnumerable<IResourceMenuLink> links)
        {
            foreach (var link in links.OrderByDescending(link => link.MenuPriority))
            {
                switch (link)
                {
                    case ResourceCategoryLink category:
                        var highestPriorityLink = GetHighestPriorityResourcePageLink(Links.Where(l => l.ParentCategoryId == category.Id));
                        if (highestPriorityLink == null)
                        {
                            // Keep iterating when no resource pages found from under this category tree.
                            continue;
                        }
                        return highestPriorityLink;
                    case ResourcePageLink resourcePageLink:
                        return resourcePageLink;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            // Return NULL if no resource pages found.
            return null;
        }

        [OnSerializing]
        internal void OnSerializing(StreamingContext context)
        {
            Links = Links.OrderByDescending(link => link.MenuPriority).ThenBy(link => link.Title).ToImmutableList();
            HighestPriorityResourcePageLink = GetHighestPriorityResourcePageLink(Links.Where(link => !link.ParentCategoryId.HasValue));
        }

        internal void Apply(ResourceCategoryEditedEvent @event)
        {
            var existingCategoryLink = Links.OfType<ResourceCategoryLink>().SingleOrDefault(link => link.Id == @event.CategoryId);
            if (existingCategoryLink == null)
            {
                Links = Links.Add(new ResourceCategoryLink(
                    id: @event.CategoryId,
                    parentCategoryId: @event.ParentCategoryId,
                    title: @event.Title,
                    description: @event.Description,
                    menuPriority: @event.MenuPriority
                ));
            }
            else
            {
                var newValue = existingCategoryLink.Edit(
                    title: @event.Title,
                    menuPriority: @event.MenuPriority,
                    description: @event.Description,
                    parentCategoryId: @event.ParentCategoryId);
                Links = Links.Replace(existingCategoryLink, newValue);
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

        internal void Apply(ResourcePageDeletedEvent @event)
        {
            Links = Links.Remove(Links.Single(link => link.Id == @event.ResourcePageId));
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

            Links = Links.Add(new ResourcePageLink(
                id: @event.ResourcePageId,
                parentCategoryId: @event.ParentCategoryId,
                title: @event.Title,
                menuPriority: @event.MenuPriority
            ));
        }

        internal void Apply(ResourcePageTitleChangedEvent @event)
        {
            var existingLink = Links.OfType<ResourcePageLink>().Single(link => link.Id == @event.ResourcePageId);
            var newValue = existingLink.Edit(
                title: @event.Title,
                menuPriority: existingLink.MenuPriority,
                parentCategoryId: existingLink.ParentCategoryId);
            Links = Links.Replace(existingLink, newValue);
        }

        internal void Apply(ResourcePageMenuPreferencesChangedEvent @event)
        {
            var existingLink = Links.OfType<ResourcePageLink>().Single(link => link.Id == @event.ResourcePageId);
            var newValue = existingLink.Edit(
                title: existingLink.Title,
                menuPriority: @event.MenuPriority,
                parentCategoryId: @event.ParentCategoryId);
            Links = Links.Replace(existingLink, newValue);
        }
    }
}