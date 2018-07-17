using System;
using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Resources.Events
{
    public class ResourcePageMenuPreferencesChangedEvent : UboraEvent
    {
        public ResourcePageMenuPreferencesChangedEvent(UserInfo initiatedBy, Guid resourcePageId, int menuPriority, Guid? parentCategoryId) : base(initiatedBy)
        {
            ResourcePageId = resourcePageId;
            MenuPriority = menuPriority;
            ParentCategoryId = parentCategoryId;
        }

        public Guid ResourcePageId { get; }
        public int MenuPriority { get; }
        public Guid? ParentCategoryId { get; }

        public override string GetDescription() => "changed menu preferences.";
    }
}