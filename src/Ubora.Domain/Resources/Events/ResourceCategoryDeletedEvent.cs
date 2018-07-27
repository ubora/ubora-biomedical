using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Resources.Events
{
    public class ResourceCategoryDeletedEvent : UboraEvent
    {
        public Guid CategoryId { get; }

        public ResourceCategoryDeletedEvent(UserInfo initiatedBy, Guid categoryId) : base(initiatedBy)
        {
            CategoryId = categoryId;
        }

        public override string GetDescription() => "deleted category.";
    }
}