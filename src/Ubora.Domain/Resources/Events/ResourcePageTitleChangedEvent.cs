using Ubora.Domain.Infrastructure.Events;

namespace Ubora.Domain.Resources.Events
{
    public class ResourcePageTitleChangedEvent : UboraEvent
    {
        public ResourcePageTitleChangedEvent(UserInfo initiatedBy, string title, Slug slug) : base(initiatedBy)
        {
            Title = title;
            Slug = slug;
        }

        public string Title { get; }
        public Slug Slug { get; }

        public override string GetDescription() => "changed resource page title.";
    }
}
